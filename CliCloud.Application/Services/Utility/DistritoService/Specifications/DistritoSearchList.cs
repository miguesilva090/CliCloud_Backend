using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;


namespace CliCloud.Application.Services.Utility.DistritoService.Specifications
{
    public class DistritoSearchList : Specification<Distrito>
    {
        public DistritoSearchList(string? keyword = "", Guid? paisId = null)
        {

            if(paisId.HasValue)
            {
              _ = Query.Where(x => x.PaisId == paisId);
            }
            
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Nome); // ordem alfabética

        }
    }
}
