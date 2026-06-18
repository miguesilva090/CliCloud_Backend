using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;

public class DeleteMultipleFamiliaArtigoRequest : IDto 
{
    public IEnumerable<Guid> Ids {get; set;} = [];
}