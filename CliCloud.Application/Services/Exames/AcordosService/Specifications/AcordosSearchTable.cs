using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.AcordosService.Specifications
{
    public class AcordosSearchTable : Specification<Acordos>
    {
        public AcordosSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query.Include(x => x.TipoExame).Include(x => x.Organismo);
            if (filters != null && filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    switch (f.Id.ToLowerInvariant())
                    {
                        case "codigosubsistema":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.CodigoSubsistema != null && x.CodigoSubsistema.Contains(f.Value));
                            break;
                        case "tipoexamedesignacao":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.TipoExame != null && x.TipoExame.Designacao != null && x.TipoExame.Designacao.Contains(f.Value));
                            break;
                        case "organismonome":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Organismo != null && x.Organismo.Nome != null && x.Organismo.Nome.Contains(f.Value));
                            break;
                        case "inactivo":
                            if (!string.IsNullOrWhiteSpace(f.Value) && bool.TryParse(f.Value, out var inactivo))
                                _ = Query.Where(x => x.Inactivo == inactivo);
                            break;
                    }
                }
            }
            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderBy(x => x.CodigoSubsistema);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
