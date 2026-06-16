using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Empresas;
using CliCloud.Domain.Entities.Fornecedores;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Specifications;

public class DocumentoByModoPagamentoSpec : Specification<Documento>
{
    public DocumentoByModoPagamentoSpec(Guid modoPagamentoId)
    {
        _ = Query.Where(x => x.ModoPagamentoId == modoPagamentoId);
    }
}

public class OrganismoByModoPagamentoSpec : Specification<Organismo>
{
    public OrganismoByModoPagamentoSpec(Guid modoPagamentoId)
    {
        _ = Query.Where(x => x.ModoPagamentoId == modoPagamentoId);
    }
}

public class FornecedorByModoPagamentoSpec : Specification<Fornecedor>
{
    public FornecedorByModoPagamentoSpec(Guid modoPagamentoId)
    {
        _ = Query.Where(x => x.ModoPagamentoId == modoPagamentoId);
    }
}

public class EmpresaByModoPagamentoSpec : Specification<Empresa>
{
    public EmpresaByModoPagamentoSpec(Guid modoPagamentoId)
    {
        _ = Query.Where(x => x.ModoPagamentoId == modoPagamentoId);
    }
}
