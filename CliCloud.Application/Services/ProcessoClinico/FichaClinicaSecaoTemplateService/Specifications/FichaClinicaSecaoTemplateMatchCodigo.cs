using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Specifications
{
    public class FichaClinicaSecaoTemplateMatchCodigo : Specification<FichaClinicaSecaoTemplate>
    {
        public FichaClinicaSecaoTemplateMatchCodigo(
            Guid utilizadorId,
            string codigo,
            Guid? excludeId = null
        )
        {
            if (!string.IsNullOrWhiteSpace(codigo))
            {
                if (excludeId.HasValue)
                {
                    _ = Query.Where(t =>
                        t.UtilizadorId == utilizadorId
                        && t.Codigo == codigo
                        && t.Id != excludeId.Value
                    );
                }
                else
                {
                    _ = Query.Where(t => t.UtilizadorId == utilizadorId && t.Codigo == codigo);
                }
            }

            _ = Query.OrderBy(t => t.Codigo);
        }
    }
}

