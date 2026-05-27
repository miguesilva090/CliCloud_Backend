using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ConsultaService.Specifications;

public sealed class ConsultaFaturacaoByDocumentoIdSpec : Specification<ConsultaFaturacao>
{
    public ConsultaFaturacaoByDocumentoIdSpec(Guid documentoId)
    {
        _ = Query.Where(x => x.DocumentoId == documentoId && x.DeletedOn == null);
    }
}
