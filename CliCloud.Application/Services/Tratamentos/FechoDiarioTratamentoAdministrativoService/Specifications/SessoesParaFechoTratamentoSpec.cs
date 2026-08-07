using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;


namespace CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.Specifications;

public sealed class SessoesParaFechoTratamentoSpec : Specification<SessaoTratamento>
{
    public SessoesParaFechoTratamentoSpec(DateTime data)
    {
        DateTime dia = data.Date;
        _ = Query
            .Where(x => x.DeletedOn == null)
            .Where(x => x.Data.HasValue && x.Data.Value.Date == dia)
            .Where(x => x.HistSess == null || x.HistSess == 0)
            .Include(x => x.Tratamento)
            .Include(x => x.Servicos);
    }
}