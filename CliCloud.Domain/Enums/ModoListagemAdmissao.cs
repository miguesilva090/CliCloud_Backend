#nullable enable

namespace CliCloud.Domain.Enums;

public enum ModoListagemAdmissao
{
  Dia = 0,
  Pendentes = 1,
  /// <summary>Área financeira: admissões do utente por faturar, sem filtro à data do documento.</summary>
  ParaFaturacao = 2,
}
