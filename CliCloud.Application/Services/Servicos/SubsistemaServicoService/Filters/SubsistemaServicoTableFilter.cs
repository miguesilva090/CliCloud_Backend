using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Servicos.SubsistemaServicoService.Filters
{
  public class SubsistemaServicoTableFilter : PaginationFilter
  {
    public Guid? ServicoId { get; set; }
    public Guid? OrganismoId { get; set; }
  }
}
