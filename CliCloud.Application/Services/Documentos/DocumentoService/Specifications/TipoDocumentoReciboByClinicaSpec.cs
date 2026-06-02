using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public sealed class TipoDocumentoReciboByClinicaSpec : Specification<TipoDocumento>
    {
        public TipoDocumentoReciboByClinicaSpec(Guid clinicaId)
        {
            _ = Query
                .Where(x =>
                    x.ClinicaId == clinicaId &&
                    !x.Inactivo &&
                    (
                        x.Abreviatura == "RC" ||
                        (x.Descricao != null && x.Descricao.Contains("recibo"))
                    )
                )
                .OrderByDescending(x => x.Abreviatura == "RC")
                .ThenBy(x => x.Descricao);
        }
    }
}
