namespace CliCloud.WebApi.HostedServices;

public class SmsAutomaticoOptions
{
    public bool Ativo { get; set; }
    public string HoraExecucao { get; set; } = "11:05";
}