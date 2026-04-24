#nullable enable 
namespace CliCloud.Domain.Common
{
    public interface ISoftDelete
    {
        DateTime? DeletedOn { get; set; }
        string? DeletedBy { get; set; }
    }
}