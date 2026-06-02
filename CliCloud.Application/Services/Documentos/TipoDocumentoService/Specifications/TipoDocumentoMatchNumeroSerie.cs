using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications
{
    public class TipoDocumentoMatchNumeroSerie : Specification<TipoDocumento>
    {
        public TipoDocumentoMatchNumeroSerie(string numeroSerie, Guid clinicaId = default)
        {
            _ = Query.Where(x => x.NumeroSerie == numeroSerie && x.ClinicaId == clinicaId);
        }
    }
}
