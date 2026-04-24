using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Utility;

public class Feriado : AuditableEntityWithSoftDelete
{
    
    public Guid ClinicaId {get;set;}
    public DateTime Data {get;set;}
    public string Designacao {get;set;} = string.Empty;
    public bool Ativo {get;set;} = true;

}