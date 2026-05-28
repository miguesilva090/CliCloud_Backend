using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications
{
    public class TipoDocumentoMatchAbreviatura : Specification<TipoDocumento>
    {
        public TipoDocumentoMatchAbreviatura(string abreviatura, Guid clinicaId = default)
        {
            _ = Query.Where(x => x.Abreviatura == abreviatura && x.ClinicaId == clinicaId);
        }
    }
}
