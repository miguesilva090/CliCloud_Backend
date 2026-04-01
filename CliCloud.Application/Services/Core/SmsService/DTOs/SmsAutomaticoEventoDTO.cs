using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.SmsService.DTOs;

public class SmsAutomaticoEventoDTO : IDto 
{
    public string CodigoRegra { get; set; } = string.Empty;
    public Guid ClinicaId { get; set; }
    public string NumeroDestino {get;set;} = string.Empty;
    public string MensagemTemplate {get;set;} = string.Empty;
    public Dictionary<string, string> Placeholders {get;set;} = [];

    public string Modulo {get;set;} = "SMSAutomatico";
    public int? CodigoUtente {get;set;}
    public string? CodigoMedico {get;set;}
    public int? CodigoFisioterapeuta {get;set;}
    public int? CodigoConsulta {get;set;}
    public int? CodigoTratamento {get;set;}
    public int? CodigoAula {get;set;}
}