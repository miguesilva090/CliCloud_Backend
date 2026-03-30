using Ardalis.Specification;
using CentroSaudeEntity = CliCloud.Domain.Entities.CentroSaude.CentroSaude;

namespace CliCloud.Application.Services.CentroSaude.CentroSaudeService.Specifications
{
    public class CentroSaudeMatchNContrib : Specification<CentroSaudeEntity>
    {
        public CentroSaudeMatchNContrib(string ncontrib)
        {
            _ = Query.Where(x => x.NumeroContribuinte != null && x.NumeroContribuinte == ncontrib);
        }
    }
}
