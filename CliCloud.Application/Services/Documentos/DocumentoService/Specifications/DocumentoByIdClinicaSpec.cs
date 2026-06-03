using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public class DocumentoByIdClinicaSpec : Specification<Documento>
    {
        public DocumentoByIdClinicaSpec(Guid id, Guid clinicaId)
        {
            _ = Query
                .Where(x => x.Id == id && x.ClinicaId == clinicaId)
                .Include(x => x.TipoDocumento)
                .Include(x => x.Utente)
                .Include(x => x.Organismo)
                .Include(x => x.CodigoPostal)
                .Include(x => x.MotivoIsencao)
                .Include(x => x.Linhas.OrderBy(l => l.NumeroLinha))
                .Include(x => x.OrigemClinica);
        }
    }
}
