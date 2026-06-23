using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Stocks.SubsistemaArtigoService.Filters;
using CliCloud.Domain.Entities.Stocks;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService.Specifications;

public class SubsistemaArtigoSearchTable : Specification<SubsistemaArtigo>
{
    public SubsistemaArtigoSearchTable(SubsistemaArtigoTableFilter filter, Guid clinicaId, string? dynamicOrder = "")
    {
        _ = Query
            .Where(x => x.ClinicaId == clinicaId)
            .Include(x => x.Artigo)
            .Include(x => x.Organismo);


        if(filter.ArtigoId.HasValue)
            _ = Query.Where(x => x.ArtigoId == filter.ArtigoId.Value);
        
        if(filter.OrganismoId.HasValue)
            _ = Query.Where(x => x.OrganismoId == filter.OrganismoId.Value);
        
        if(filter.Inativo.HasValue)
            _ = Query.Where(x => x.Inativo == filter.Inativo.Value);

        if(!string.IsNullOrWhiteSpace(filter.FiltroBox))
        {
            string fb = filter.FiltroBox.Trim();
            _ = Query.Where(x => 
                x.CodigoCartaoInstituicao.Contains(fb) || 
                x.Artigo.Descricao.Contains(fb) || 
                x.Artigo.NumeroArtigo.Contains(fb) || 
                x.Organismo.Nome.Contains(fb));
        }

        if(filter.Filters != null )
        {
            foreach(var f in filter.Filters)
            {
                switch((f.Id ?? "").ToLowerInvariant())
                {
                    case "artigoid": 
                        if(Guid.TryParse(f.Value, out Guid artigoId))
                            _ = Query.Where(x => x.ArtigoId == artigoId);
                        break;
                    case "organismoid":
                        if(Guid.TryParse(f.Value, out Guid organismoId))
                            _ = Query.Where(x => x.OrganismoId == organismoId);
                        break;
                    case "inativo": 
                        if(bool.TryParse(f.Value, out bool inativo))
                            _ = Query.Where(x => x.Inativo == inativo);
                        break;
                    case "codigocartaoinstituicao":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.CodigoCartaoInstituicao.Contains(f.Value));
                        break;
                    case "artigodescricao":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x =>
                                x.Artigo.Descricao.Contains(f.Value) ||
                                x.Artigo.NumeroArtigo.Contains(f.Value) ||
                                x.Organismo.Nome.Contains(f.Value) ||
                                x.CodigoCartaoInstituicao.Contains(f.Value));
                        break;
                    case "organismonome":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Organismo.Nome.Contains(f.Value));
                        break;
                }
            }
        }

        _ = string.IsNullOrEmpty(dynamicOrder)
            ? Query.OrderBy(x => x.CodigoCartaoInstituicao)
            : Query.OrderBy(dynamicOrder);
    }
}