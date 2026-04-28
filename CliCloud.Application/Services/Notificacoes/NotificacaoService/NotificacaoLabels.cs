namespace CliCloud.Application.Services.Notificacoes.NotificacaoService;

/// <summary>Mapeamento de códigos numéricos para texto (equivalente a EstadoDescr / PrioridadeDescr no ASP legado).</summary>
public static class NotificacaoLabels
{
  public static string EstadoPt(int estado)
  {
    return estado switch
    {
      0 => "Pendente",
      1 => "Lida",
      2 => "Em progresso",
      3 => "Atualização clínica",
      _ => $"Estado {estado}",
    };
  }

  /// <summary>Prioridade por convenção CliCloud (add legado pré-seleciona 2).</summary>
  public static string PrioridadePt(int prioridade)
  {
    return prioridade switch
    {
      0 => "Baixa",
      1 => "Normal",
      2 => "Alta",
      3 => "Urgente",
      _ => $"Prioridade {prioridade}",
    };
  }
}
