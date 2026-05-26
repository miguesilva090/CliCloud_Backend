using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Specifications;
using CliCloud.Application.Services.Medicos.FolgasMedicoService.Specifications;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.Specifications;
using CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;

public partial class MarcacoesAdministrativoService
{
  public async Task<Response<MarcacaoCalendarioDTO>> GetCalendarioAsync(
    MarcacaoCalendarioRequest request
  )
  {
    if (request.MedicoId == Guid.Empty)
    {
      return ResponseFactory.Fail<MarcacaoCalendarioDTO>("Médico é obrigatório.");
    }

    DateTime dataDe = request.DataDe.Date;
    DateTime dataAte = request.DataAte.Date;
    if (dataAte < dataDe)
    {
      return ResponseFactory.Fail<MarcacaoCalendarioDTO>(
        "Intervalo de datas inválido (data até anterior à data de)."
      );
    }

    Clinica? clinica = await ObterClinicaAtualAsync();
    HorarioMedico? horario = await ObterHorarioMedicoAsync(request.MedicoId);
    List<FolgasMedico> folgas = await ObterFolgasMedicoAsync(request.MedicoId);
    List<Feriado> feriados = await ObterFeriadosClinicaAsync(clinica?.Id, dataDe, dataAte);
    List<HorarioMedicoVariavel> horariosVariaveis =
      await ObterHorariosVariaveisMedicoAsync(request.MedicoId, dataDe, dataAte);

    var spec = new MarcacaoAdministrativoCalendarioSpec(
      request.MedicoId,
      dataDe,
      dataAte,
      request.SalaId,
      request.EspecialidadeId
    );
    List<ConsultaMarcacao> marcacoes =
      (await _repository.GetListAsync<ConsultaMarcacao, Guid>(spec)).ToList();

    MarcacaoCalendarioDTO calendario = MarcacoesCalendarioBuilder.Build(
      request,
      clinica,
      horario,
      folgas,
      feriados,
      horariosVariaveis,
      marcacoes
    );

    return ResponseFactory.Success(calendario);
  }

  private async Task<Clinica?> ObterClinicaAtualAsync()
  {
    if (_currentClinicaService == null
      || string.IsNullOrWhiteSpace(_currentClinicaService.ClinicaId)
      || !Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId))
    {
      return null;
    }

    return await _repository.GetByIdAsync<Clinica, Guid>(clinicaId);
  }

  private async Task<List<Feriado>> ObterFeriadosClinicaAsync(
    Guid? clinicaId,
    DateTime dataDe,
    DateTime dataAte
  )
  {
    if (!clinicaId.HasValue)
    {
      return [];
    }

    IEnumerable<Feriado> todos = await _repository.GetListAsync<Feriado, Guid>();
    return todos
      .Where(x =>
        x.DeletedOn == null
        && x.ClinicaId == clinicaId.Value
        && x.Ativo
        && x.Data.Date >= dataDe.Date
        && x.Data.Date <= dataAte.Date
      )
      .ToList();
  }

  private async Task<List<HorarioMedicoVariavel>> ObterHorariosVariaveisMedicoAsync(
    Guid medicoId,
    DateTime dataDe,
    DateTime dataAte
  )
  {
    var spec = new HorarioMedicoVariavelSearchByMedicoId(medicoId);
    IEnumerable<HorarioMedicoVariavel> list =
      await _repository.GetListAsync<HorarioMedicoVariavel, Guid>(spec);
    return list
      .Where(x =>
        x.DeletedOn == null
        && x.Data.Date >= dataDe.Date
        && x.Data.Date <= dataAte.Date
      )
      .ToList();
  }
}
