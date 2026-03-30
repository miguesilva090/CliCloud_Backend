namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class GuardarMedicosSmsRequest 
    {
        public string CodigoConfiguracao { get; set; } = "1";
        public List<string> CodigosMedicos {get;set;} = [];
    }
}