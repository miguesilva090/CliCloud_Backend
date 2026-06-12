using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Specifications
{
    public class NaturezaDocumentoMatchSigla : Specification<NaturezaDocumento>
    {
        public NaturezaDocumentoMatchSigla(string sigla, Guid? excludedId = null)
        {
            _ = Query.Where(x => x.Sigla == sigla);
            if (excludedId.HasValue)
                _ = Query.Where(x => x.Id != excludedId.Value);
        }
    }
}