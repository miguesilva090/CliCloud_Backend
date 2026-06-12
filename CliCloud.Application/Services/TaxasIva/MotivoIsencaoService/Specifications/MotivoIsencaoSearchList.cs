using Ardalis.Specification;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.Specifications
{
    public class MotivoIsencaoSearchList : Specification<MotivoIsencao>
    {
        public MotivoIsencaoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    x.Codigo.Contains(keyword) ||
                    (x.CodigoSaft != null && x.CodigoSaft.Contains(keyword)) ||
                    x.Descricao.Contains(keyword) ||
                    (x.Norma != null && x.Norma.Contains(keyword)) ||
                    (x.Mencao != null && x.Mencao.Contains(keyword)));
            }
            _ = Query.OrderBy(x => x.Codigo);
        }
    }
}
