using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.Specifications;

public sealed class DocumentoOrganismoParaFicheiroEletronicoSpec : Specification<Documento>
{
    public DocumentoOrganismoParaFicheiroEletronicoSpec(Guid documentoId, Guid clinicaId)
    {
        _ = Query.Where(x => 
            x.Id == documentoId &&
            x.ClinicaId == clinicaId &&
            x.OrganismoId != null &&
            x.DeletedOn == null);
    }
}