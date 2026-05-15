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
      StatusConsulta = admissao.StatusConsulta ?? StatusConsulta.Concluida,
      OrganismoId = admissao.OrganismoId,
      Credencial = admissao.Credencial,
      CredencialExterna = admissao.CredencialExterna,
      SeguradoraId = admissao.SeguradoraId,
      Sinistrado = admissao.Sinistrado,
      Justificacao = admissao.Justificacao,
      MotivoJustificacao = admissao.MotivoJustificacao,
      TratamentoId = admissao.TratamentoId,
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

  public static List<ServicoConsulta> MapearServicos(Admissao admissao, Guid consultaId)
  {
    return admissao.Servicos
      .OrderBy(s => s.Linha)
      .Select(s => new ServicoConsulta
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
      })
      .ToList();
  }
}
