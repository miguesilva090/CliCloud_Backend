namespace CliCloud.WebApi.HostedServices;

public class EmailAutomaticoOptions
{
    public bool Ativo { get; set; } = false;
    public string HoraExecucao { get; set; } = "11:05:00";
}
