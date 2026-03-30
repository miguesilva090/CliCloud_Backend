using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.Specifications
{
    public class FichaClinicaSecaoCampoSearchTable : Specification<FichaClinicaSecaoCampo>
    {
        public FichaClinicaSecaoCampoSearchTable(Guid? separadorId, string? keyword = "", string? dynamicOrder = "")
        {
            if (separadorId.HasValue && separadorId.Value != Guid.Empty)
            {
                _ = Query.Where(x => x.SeparadorId == separadorId.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.Nome);
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder);
            }
        }
    }
}

