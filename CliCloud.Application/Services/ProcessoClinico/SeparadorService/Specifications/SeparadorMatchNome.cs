using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService.Specifications;

public class SeparadorMatchNome : Specification<Separador>
{
    public SeparadorMatchNome(string nome, Guid? excludeId = null)
    {
        if (!string.IsNullOrWhiteSpace(nome))
        {
            if (excludeId.HasValue)
            {
                _ = Query.Where(x => x.Nome == nome && x.Id != excludeId.Value);
            }
            else
            {
                _ = Query.Where(x => x.Nome == nome);
            }
        }
    }
}
