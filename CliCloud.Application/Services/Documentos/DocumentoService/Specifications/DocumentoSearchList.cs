using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;
namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public class DocumentoSearchList : Specification<Documento>
    {
        public DocumentoSearchList(string? keyword = "", Guid? clinicaId = null)
        {
            _ = Query
                .Include(x => x.TipoDocumento)
                .Where(d =>
                    d.TipoDocumento == null
                    || (
                        (d.TipoDocumento.Abreviatura == null
                            || (
                                d.TipoDocumento.Abreviatura.ToUpper() != "RC"
                                && d.TipoDocumento.Abreviatura.ToUpper() != "FR"
                                && d.TipoDocumento.Abreviatura.ToUpper() != "REC"))
                        && (d.TipoDocumento.Descricao == null
                            || !d.TipoDocumento.Descricao.ToLower().Contains("recibo"))));

            if (clinicaId.HasValue)
            {
                _ = Query.Where(x => x.ClinicaId == clinicaId.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    (x.NomeCliente != null && x.NomeCliente.Contains(keyword))
                    || (x.NumeroContribuinteCliente != null && x.NumeroContribuinteCliente.Contains(keyword))
                    || (x.NumeroExibicao != null && x.NumeroExibicao.Contains(keyword))
                    || x.NumeroDocumento.ToString().Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.Data ?? x.CreatedOn);
        }
    }
}