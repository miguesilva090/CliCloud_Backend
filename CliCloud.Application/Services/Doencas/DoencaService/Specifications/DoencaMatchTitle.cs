using Ardalis.Specification;
using CliCloud.Domain.Entities.Doencas;

namespace CliCloud.Application.Services.Doencas.DoencaService.Specifications
{
    public class DoencaMatchTitle : Specification<Doenca>
    {
        public DoencaMatchTitle(string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                _ = Query.Where(h => h.Title == title);
            }
            _ = Query.OrderBy(h => h.Title);
        }
    }
}
