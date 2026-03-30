using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.Specifications
{
    public class FichaClinicaSecaoCampoSearchList : Specification<FichaClinicaSecaoCampo>
    {
        public FichaClinicaSecaoCampoSearchList(Guid? separadorId, string? keyword = "")
        {
            if (separadorId.HasValue && separadorId.Value != Guid.Empty)
            {
                _ = Query.Where(x => x.SeparadorId == separadorId.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.Nome);
        }
    }
}

