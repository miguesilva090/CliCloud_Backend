using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

internal static class AdmissaoPromocaoHelper
{
  public static Consulta CriarConsultaDesdeAdmissao(Admissao admissao)
  {
    return new Consulta
    {
      UtenteId = admissao.UtenteId,
      MedicoId = admissao.MedicoId,
      EspecialidadeId = admissao.EspecialidadeId,
      TecnicoId = admissao.TecnicoId,
      FuncionarioId = admissao.FuncionarioId,
      MedicoExternoId = admissao.MedicoExternoId,
      SalaId = admissao.SalaId,
      Data = admissao.Data,
      HoraInicio = admissao.HoraInicio,
      HoraFim = admissao.HoraFim,
      StatusConsulta = AdmissaoPromocaoEstadoHelper.ResolverStatusConsultaHistorico(admissao),
      Confirmado = admissao.Confirmado,
      ConfirmaConsulta = admissao.ConfirmaConsulta,
      Efetuado = admissao.Efetuado,
      Faltou = AdmissaoPromocaoEstadoHelper.DerivarFaltou(admissao),
      OrganismoId = admissao.OrganismoId,
      Credencial = admissao.Credencial,
      CredencialExterna = admissao.CredencialExterna,
      NumDestacavel = admissao.NumDestacavel,
      SeguradoraId = admissao.SeguradoraId,
      Sinistrado = admissao.Sinistrado ?? 0,
      Justificacao = admissao.Justificacao ?? 0,
      MotivoJustificacao = admissao.MotivoJustificacao,
      TratamentoId = admissao.TratamentoId,
      MotivoConsultaId = admissao.MotivoConsultaId,
      HoraChegada = admissao.HoraChegada,
      Ordem = admissao.Ordem,
      DataHoraMarcacao = admissao.DataHoraMarcacao,
      Obs = admissao.Obs,
      Diagnostico = admissao.Diagnostico,
      TipoConsultaId = admissao.TipoConsultaId,
      ConsultaMarcacaoId = admissao.ConsultaMarcacaoId,
      AdmissaoId = admissao.Id,
      TipoAdmissaoId = admissao.TipoAdmissaoId,
      DoencaPrincipalId = admissao.DoencaPrincipalId,
      DoencaSecundariaId = admissao.DoencaSecundariaId,
    };
  }

  /// <summary>
  /// Cenário A: consulta clínica já existe (marcação → atendimento); fecho funde dados da receção.
  /// Não altera o estado clínico já definido (Em atendimento / Concluída).
  /// </summary>
  public static void MesclarAdmissaoEmConsultaExistente(Consulta consulta, Admissao admissao)
  {
    consulta.AdmissaoId ??= admissao.Id;
    consulta.ConsultaMarcacaoId ??= admissao.ConsultaMarcacaoId;

    consulta.OrganismoId ??= admissao.OrganismoId;
    consulta.SeguradoraId ??= admissao.SeguradoraId;
    consulta.TratamentoId ??= admissao.TratamentoId;
    consulta.TipoAdmissaoId ??= admissao.TipoAdmissaoId;
    consulta.TipoConsultaId ??= admissao.TipoConsultaId;
    consulta.MotivoConsultaId ??= admissao.MotivoConsultaId;
    consulta.DoencaPrincipalId ??= admissao.DoencaPrincipalId;
    consulta.DoencaSecundariaId ??= admissao.DoencaSecundariaId;
    consulta.HoraChegada ??= admissao.HoraChegada;
    consulta.Ordem ??= admissao.Ordem;
    consulta.DataHoraMarcacao ??= admissao.DataHoraMarcacao;

    if (string.IsNullOrWhiteSpace(consulta.Credencial) && !string.IsNullOrWhiteSpace(admissao.Credencial))
    {
      consulta.Credencial = admissao.Credencial;
    }

    if (string.IsNullOrWhiteSpace(consulta.NumDestacavel) && !string.IsNullOrWhiteSpace(admissao.NumDestacavel))
    {
      consulta.NumDestacavel = admissao.NumDestacavel;
    }

    if (!consulta.CredencialExterna.HasValue && admissao.CredencialExterna.HasValue)
    {
      consulta.CredencialExterna = admissao.CredencialExterna;
    }

    if (!consulta.Sinistrado.HasValue || consulta.Sinistrado == 0)
    {
      consulta.Sinistrado = admissao.Sinistrado ?? 0;
    }

    if (!consulta.Justificacao.HasValue || consulta.Justificacao == 0)
    {
      consulta.Justificacao = admissao.Justificacao ?? 0;
    }

    if (string.IsNullOrWhiteSpace(consulta.MotivoJustificacao))
    {
      consulta.MotivoJustificacao = admissao.MotivoJustificacao;
    }

    if (string.IsNullOrWhiteSpace(consulta.Obs))
    {
      consulta.Obs = admissao.Obs;
    }

    if (string.IsNullOrWhiteSpace(consulta.Diagnostico))
    {
      consulta.Diagnostico = admissao.Diagnostico;
    }

    consulta.MedicoId ??= admissao.MedicoId;
    consulta.EspecialidadeId ??= admissao.EspecialidadeId;
    consulta.TecnicoId ??= admissao.TecnicoId;
    consulta.FuncionarioId ??= admissao.FuncionarioId;
    consulta.MedicoExternoId ??= admissao.MedicoExternoId;
    consulta.SalaId ??= admissao.SalaId;
    consulta.Data ??= admissao.Data;
    consulta.HoraInicio ??= admissao.HoraInicio;
    consulta.HoraFim ??= admissao.HoraFim;

    AdmissaoPromocaoEstadoHelper.AplicarEstadosRecepcao(consulta, admissao, preservarExistentes: true);

    if (!consulta.StatusConsulta.HasValue)
    {
      StatusConsulta? resolvido = AdmissaoPromocaoEstadoHelper.ResolverStatusConsultaHistorico(admissao);
      if (resolvido.HasValue)
      {
        consulta.StatusConsulta = resolvido;
      }
    }
  }

  public static List<ServicoConsulta> MapearServicos(Admissao admissao, Guid consultaId)
  {
    return admissao.Servicos
      .OrderBy(s => s.Linha)
      .Select(s => MapearServico(s, consultaId))
      .ToList();
  }

  public static List<ServicoConsulta> MapearServicosNovos(
    Admissao admissao,
    Guid consultaId,
    IReadOnlyCollection<int> linhasExistentes
  )
  {
    HashSet<int> linhas = linhasExistentes.ToHashSet();
    return admissao.Servicos
      .Where(s => !linhas.Contains(s.Linha))
      .OrderBy(s => s.Linha)
      .Select(s => MapearServico(s, consultaId))
      .ToList();
  }

  private static ServicoConsulta MapearServico(AdmissaoServico s, Guid consultaId)
  {
    return new ServicoConsulta
    {
      ConsultaId = consultaId,
      ServicoId = s.ServicoId,
      ValorServico = s.ValorServico,
      CodigoArtigo = s.CodigoArtigo,
      NomeArtigo = s.NomeArtigo,
      ValorArtigo = s.ValorArtigo,
      Quantidade = s.Quantidade,
      MargemMed = s.MargemMed,
      MargemIns = s.MargemIns,
      RecMed = s.RecMed,
      RecInst = s.RecInst,
      DescInst = s.DescInst,
      DescCli = s.DescCli,
      ValorDesc = s.ValorDesc,
      Ordem = s.Ordem,
      Dente = s.Dente,
      ExameId = s.ExameId,
      Linha = s.Linha,
      NCheque = s.NCheque,
      Electrocardiograma = s.Electrocardiograma,
      ValorUt = s.ValorUt,
    };
  }
}
