using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.Specifications
{
    public class TiposTratamentoDentarioSearchTable : Specification<TipoTratamentoDentario>
    {
        public TiposTratamentoDentarioSearchTable(string? keyword = "", string? dynamicOrder = "")
        {

            if(!string.IsNullOrWhiteSpace(keyword))
            {
                Query.Where( x => 
                    x.Codigo.Contains(keyword) ||
                    x.Descricao.Contains(keyword));
            }

            if(string.IsNullOrEmpty(dynamicOrder))
            {
                Query.OrderBy(x => x.Codigo);
            }
            else
            {
                Query.OrderBy(dynamicOrder);
            }


        }
    }
}
