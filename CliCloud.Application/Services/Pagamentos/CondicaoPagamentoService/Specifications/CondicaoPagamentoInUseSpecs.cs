using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Empresas;
using CliCloud.Domain.Entities.Fornecedores;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Specifications;

public class DocumentoByCondicaoPagamentoSpec : Specification<Documento>
{
    public DocumentoByCondicaoPagamentoSpec(Guid condicaoPagamentoId)
    {
        _ = Query.Where(x => x.CondicaoPagamentoId == condicaoPagamentoId);
    }
}

public class OrganismoByCondicaoPagamentoSpec : Specification<Organismo>
{
    public OrganismoByCondicaoPagamentoSpec(Guid condicaoPagamentoId)
    {
        _ = Query.Where(x => x.CondicaoPagamentoId == condicaoPagamentoId);
    }
}

public class FornecedorByCondicaoPagamentoSpec : Specification<Fornecedor>
{
    public FornecedorByCondicaoPagamentoSpec(Guid condicaoPagamentoId)
    {
        _ = Query.Where(x => x.CondicaoPagamentoId == condicaoPagamentoId);
    }
}

public class EmpresaByCondicaoPagamentoSpec : Specification<Empresa>
{
    public EmpresaByCondicaoPagamentoSpec(Guid condicaoPagamentoId)
    {
        _ = Query.Where(x => x.CondicaoPagamentoId == condicaoPagamentoId);
    }
}
