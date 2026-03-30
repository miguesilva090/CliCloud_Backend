using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Specifications
{
    // Mantido apenas para compatibilidade, não está a ser usado neste momento.
    public class FichaClinicaSecaoTemplateMatchName : Specification<FichaClinicaSecaoTemplate>
    {
        public FichaClinicaSecaoTemplateMatchName(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Nome == name);
            }

            _ = Query.OrderBy(h => h.Nome);
        }
    }
}
