using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;

/// <summary>
/// Mantém ADMISS ligada à marcação (legado: MarcarConsulta / OrdemEntradaMarcacoesEdtSave).
/// </summary>
public static class MarcacaoAdmissaoSyncHelper
{
  public sealed class MarcacaoAdmissaoFields
  {
    public Guid? OrganismoId { get; init; }
    public string? Credencial { get; init; }
    public TimeSpan? HoraFim { get; init; }
    public Guid? SalaId { get; init; }
  }

  public static async Task<Admissao> SyncAdmissaoFromMarcacaoAsync(
    IRepositoryAsync repository,
    ConsultaMarcacao marcacao,
    MarcacaoAdmissaoFields fields,
    Admissao? admissaoExistente = null
  )
  {
    Admissao? admissao = admissaoExistente;
    if (admissao == null)
    {
      List<Admissao> existentes = (
        await repository.GetListAsync<Admissao, Guid>(new AdmissaoByConsultaMarcacaoSpec(marcacao.Id))
      ).ToList();
      admissao = existentes.FirstOrDefault();
    }

    if (admissao == null)
    {
      admissao = new Admissao
      {
        Id = Guid.NewGuid(),
        UtenteId = marcacao.UtenteId,
        ConsultaMarcacaoId = marcacao.Id,
        MedicoId = marcacao.MedicoId,
        EspecialidadeId = marcacao.EspecialidadeId,
        TecnicoId = marcacao.TecnicoId,
        FuncionarioId = marcacao.FuncionarioId,
        MedicoExternoId = marcacao.MedicoExternoId,
        SalaId = fields.SalaId ?? marcacao.SalaId,
        Data = marcacao.Data,
        HoraInicio = marcacao.HoraMarcacao,
        HoraFim = fields.HoraFim,
        OrganismoId = fields.OrganismoId,
        MotivoConsultaId = marcacao.MotivoConsultaId,
        TipoConsultaId = marcacao.TipoConsultaId,
        TipoAdmissaoId = marcacao.TipoAdmissaoId,
        Credencial = fields.Credencial,
        NumDestacavel = marcacao.NumDestacavel,
        Obs = marcacao.Obs,
        Origem = OrigemAdmissao.Marcacao,
        DataHoraMarcacao = DateTime.UtcNow,
        Pago = false,
        Faturado = false,
        EmTratamento = marcacao.EmTratamento,
        StatusConsulta = marcacao.StatusConsulta,
      };
      await AdmissaoHoraCalculoHelper.AplicarHoraFimAsync(admissao, repository);
      _ = await repository.CreateAsync<Admissao, Guid>(admissao);
      return admissao;
    }

    admissao.ConsultaMarcacaoId = marcacao.Id;
    admissao.UtenteId = marcacao.UtenteId;
    admissao.MedicoId = marcacao.MedicoId;
    admissao.EspecialidadeId = marcacao.EspecialidadeId;
    admissao.TecnicoId = marcacao.TecnicoId;
    admissao.FuncionarioId = marcacao.FuncionarioId;
    admissao.MedicoExternoId = marcacao.MedicoExternoId;
    admissao.SalaId = fields.SalaId ?? marcacao.SalaId;
    admissao.Data = marcacao.Data;
    admissao.HoraInicio = marcacao.HoraMarcacao;
    if (fields.HoraFim.HasValue)
    {
      admissao.HoraFim = fields.HoraFim;
    }

    admissao.OrganismoId = fields.OrganismoId;
    admissao.MotivoConsultaId = marcacao.MotivoConsultaId;
    admissao.TipoConsultaId = marcacao.TipoConsultaId;
    admissao.TipoAdmissaoId = marcacao.TipoAdmissaoId;
    admissao.Credencial = fields.Credencial;
    admissao.NumDestacavel = marcacao.NumDestacavel;
    admissao.Obs = marcacao.Obs;
    admissao.EmTratamento = marcacao.EmTratamento;
    admissao.StatusConsulta = marcacao.StatusConsulta;
    await AdmissaoHoraCalculoHelper.AplicarHoraFimAsync(admissao, repository);
    _ = await repository.UpdateAsync<Admissao, Guid>(admissao);
    return admissao;
  }

  public static async Task SyncDesmarcarFromMarcacaoAsync(
    IRepositoryAsync repository,
    ConsultaMarcacao marcacao
  )
  {
    Admissao? admissao = (
      await repository.GetListAsync<Admissao, Guid>(new AdmissaoByConsultaMarcacaoSpec(marcacao.Id))
    ).FirstOrDefault();
    if (admissao == null)
    {
      return;
    }

    admissao.StatusConsulta = StatusConsulta.Desmarcada;
    admissao.Confirmado = false;
    admissao.Obs = marcacao.Obs;
    admissao.DataHoraMarcacao = DateTime.UtcNow;
    _ = await repository.UpdateAsync<Admissao, Guid>(admissao);
  }
}
