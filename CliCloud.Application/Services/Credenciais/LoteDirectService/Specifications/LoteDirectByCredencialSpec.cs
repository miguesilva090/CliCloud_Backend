using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public sealed class LoteDirectByCredencialSpec : Specification<LoteDirect>
{
    public LoteDirectByCredencialSpec(string credencial, Guid? excludeId = null)
    {
        string trimmed = credencial.Trim();
        Query.Where(x => x.Credencial != null && x.Credencial == trimmed);
        if (excludeId.HasValue)
            Query.Where(x => x.Id != excludeId.Value);
    }
}
