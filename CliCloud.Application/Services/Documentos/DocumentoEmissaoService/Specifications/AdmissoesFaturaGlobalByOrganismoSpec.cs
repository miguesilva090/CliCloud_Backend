using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class AdmissoesFaturaGlobalByOrganismoSpec : Specification<Admissao>
{
    public AdmissoesFaturaGlobalByOrganismoSpec(
        Guid organismoId,
        DateTime dataDe,
        DateTime dataAte,
        Guid? utenteId,
        bool exigirReciboPago
    )
    {
        dataDe = dataDe.Date;
        dataAte = dataAte.Date;

        _ = Query
            .Include(x => x.Utente)
            .Include(x => x.Organismo)
            .Include(x => x.Servicos)
                .ThenInclude(s => s.Servico)
                    .ThenInclude(s => s!.TaxaIva)
            .Where(x => x.OrganismoId == organismoId )
            .Where(x => x.Data.HasValue && x.Data.Value.Date >= dataDe && x.Data.Value.Date <= dataAte);

            if(utenteId.HasValue)
                _ = Query.Where(x => x.UtenteId == utenteId.Value);
            
            if(exigirReciboPago)
                _ = Query.Where(x => x.Pago == true);
    }
}