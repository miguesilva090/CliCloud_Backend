using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications
{
    public class TipoDocumentoSearchList : Specification<TipoDocumento>
    {
        public TipoDocumentoSearchList(string? keyword = "", Guid clinicaId = default)
        {
            _ = Query.Where(x => x.ClinicaId == clinicaId);
            _ = Query.Where(x => !x.Inactivo);
            _ = Query.Where(x => x.PermiteMovimento == null || x.PermiteMovimento == 1);
            _ = Query.Where(x =>
                string.IsNullOrEmpty(x.TipoSerie)
                || x.TipoSerie == "N");

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    x.Descricao.Contains(keyword)
                    || x.Abreviatura.Contains(keyword)
                    || (x.NumeroSerie != null && x.NumeroSerie.Contains(keyword)));
            }

            _ = Query.OrderBy(x => x.Abreviatura).ThenBy(x => x.NumeroSerie);
        }
    }
}
