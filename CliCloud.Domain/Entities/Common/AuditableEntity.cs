namespace CliCloud.Domain.Entities.Common
{
  public abstract class AuditableEntity : BaseEntity<Guid>, IAuditableEntity, ISoftDelete
  {
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
  }
}
