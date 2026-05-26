using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;

public partial class MarcacoesAdministrativoService
{
  public async Task<Response<List<DisponibilidadeMedicoDiaEventoDTO>>> GetDisponibilidadeMedicosMesAsync(
    DisponibilidadeMedicosMesRequest request
  )
  {
    if (request.EspecialidadeId == Guid.Empty)
    {
      return ResponseFactory.Fail<List<DisponibilidadeMedicoDiaEventoDTO>>(
        "Especialidade é obrigatória."
      );
    }

    if (request.Mes is < 1 or > 12)
    {
      return ResponseFactory.Fail<List<DisponibilidadeMedicoDiaEventoDTO>>("Mês inválido.");
    }

    if (request.Ano < 2000 || request.Ano > 2100)
    {
      return ResponseFactory.Fail<List<DisponibilidadeMedicoDiaEventoDTO>>("Ano inválido.");
    }

    List<Medico> medicos = (
      await _repository.GetListAsync<Medico, Guid>(
        new MedicosPorEspecialidadeSpec(request.EspecialidadeId)
      )
    ).ToList();

    if (medicos.Count == 0)
    {
      return ResponseFactory.Success(new List<DisponibilidadeMedicoDiaEventoDTO>());
    }

    Clinica? clinica = await ObterClinicaAtualAsync();
    DateTime dataInicio = new(request.Ano, request.Mes, 1);
    DateTime dataFim = new(request.Ano, request.Mes, DateTime.DaysInMonth(request.Ano, request.Mes));
    List<Feriado> feriadosClinica = await ObterFeriadosClinicaAsync(clinica?.Id, dataInicio, dataFim);

    List<DisponibilidadeMedicoDiaEventoDTO> eventos = [];
    int seq = 1;

    for (DateTime day = dataInicio; day.Date <= dataFim.Date; day = day.AddDays(1))
    {
      if (IsDiaFolgaClinica(clinica, day) || feriadosClinica.Any(f => f.Ativo && f.Data.Date == day.Date))
      {
        continue;
      }

      foreach (Medico medico in medicos)
      {
        HorarioMedico? horario = await ObterHorarioMedicoAsync(medico.Id);
        List<FolgasMedico> folgas = await ObterFolgasMedicoAsync(medico.Id);
        if (MarcacoesCalendarioBuilder.IsFolgaTodoDia(folgas, day))
        {
          continue;
        }

        List<HorarioMedicoVariavel> horariosVariaveis =
          await ObterHorariosVariaveisMedicoAsync(medico.Id, day, day);
        List<Feriado> feriados = feriadosClinica.Where(f => f.Data.Date == day.Date).ToList();

        var calRequest = new MarcacaoCalendarioRequest
        {
          MedicoId = medico.Id,
          EspecialidadeId = request.EspecialidadeId,
          DataDe = day,
          DataAte = day,
        };

        var spec = new MarcacaoAdministrativoCalendarioSpec(
          medico.Id,
          day,
          day,
          null,
          request.EspecialidadeId
        );
        List<ConsultaMarcacao> marcacoes =
          (await _repository.GetListAsync<ConsultaMarcacao, Guid>(spec)).ToList();

        MarcacaoCalendarioDTO calendario = MarcacoesCalendarioBuilder.Build(
          calRequest,
          clinica,
          horario,
          folgas,
          feriados,
          horariosVariaveis,
          marcacoes
        );

        if (!MarcacoesCalendarioBuilder.TemSlotLivreNoDia(calendario))
        {
          continue;
        }

        string dataIso = day.ToString("yyyy-MM-dd");
        eventos.Add(
          new DisponibilidadeMedicoDiaEventoDTO
          {
            Id = seq++,
            Title = medico.Nome ?? string.Empty,
            MedicoId = medico.Id,
            Start = $"{dataIso}T08:00:00",
            End = $"{dataIso}T08:30:00",
          }
        );
      }
    }

    return ResponseFactory.Success(eventos);
  }

  private static bool IsDiaFolgaClinica(Clinica? clinica, DateTime day)
  {
    if (clinica == null)
    {
      return false;
    }

    int dow = (int)day.DayOfWeek;
    return dow switch
    {
      0 => clinica.FolgaDom == true,
      1 => clinica.FolgaSeg == true,
      2 => clinica.FolgaTer == true,
      3 => clinica.FolgaQua == true,
      4 => clinica.FolgaQui == true,
      5 => clinica.FolgaSex == true,
      6 => clinica.FolgaSab == true,
      _ => false,
    };
  }
}
