using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Sms;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;

namespace CliCloud.Application.Services.Core.SmsService.Specifications
{
    public class HistoricoSmsTabelaSpec : Specification<HistoricoSms>
    {
        public HistoricoSmsTabelaSpec(Guid clinicaId,List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query.Where(x => x.ClinicaId == clinicaId);

            if (filters != null && filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    switch (f.Id.ToLowerInvariant())
                    {
                        case "status":
                            if (!string.IsNullOrWhiteSpace(f.Value)) 
                                _ = Query.Where(x => x.Status.Contains(f.Value));
                            break;
                        case "modulo":
                            if (!string.IsNullOrWhiteSpace(f.Value)) 
                                _ = Query.Where(x => x.Modulo.Contains(f.Value));
                            break;
                        case "numerodestinatario": 
                            if (!string.IsNullOrWhiteSpace(f.Value)) 
                                _ = Query.Where(x => x.NumeroDestinatario.Contains(f.Value));
                            break;
                        case "codigoutente":
                            if (int.TryParse(f.Value, out var codigoUtente)) 
                                _ = Query.Where(x => x.CodigoUtente == codigoUtente);
                            break;
                    }
                }
            }

            if ( string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderByDescending(x => x.DataHoraCriacao);
            else
                _ = Query.OrderBy(dynamicOrder);

        }
    }
}