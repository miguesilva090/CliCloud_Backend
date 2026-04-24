namespace CliCloud.Domain.Entities.Common
{
  public abstract class AuditableEntityWithSoftDelete : AuditableEntity, ISoftDelete
  {
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
  }
}
