using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;


namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.Specifications
{
    public class TiposTratamentoDentarioSearchList : Specification<TipoTratamentoDentario>
    {
        public TiposTratamentoDentarioSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                Query.Where(x =>
                    x.Codigo.Contains(keyword) ||
                    x.Descricao.Contains(keyword));
            }

            Query.OrderBy(x => x.Codigo);
        }
    }
}
