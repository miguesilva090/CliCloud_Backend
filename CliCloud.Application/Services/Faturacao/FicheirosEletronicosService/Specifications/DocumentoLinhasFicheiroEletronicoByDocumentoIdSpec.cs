using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.Specifications;

public sealed class DocumentoLinhasFicheiroEletronicoByDocumentoIdSpec : Specification<DocumentoLinha>
{
    public DocumentoLinhasFicheiroEletronicoByDocumentoIdSpec(Guid documentoId)
    {
        _ = Query
            .Where(x => x.DocumentoId == documentoId && x.AdmissaoServicoId != null && x.DeletedOn == null)
            .Include(x => x.AdmissaoServico!)
                .ThenInclude(a => a.Admissao)
            .Include(x => x.Servico);
    }
}