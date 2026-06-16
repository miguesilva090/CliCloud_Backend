using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Pagamentos.TipoPagamentoService.DTOs;
using CliCloud.Application.Services.Pagamentos.TipoPagamentoService.Specifications;
using TipoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.TipoPagamento;

namespace CliCloud.Application.Services.Pagamentos.TipoPagamentoService;

public class TipoPagamentoService(IRepositoryAsync repository) : ITipoPagamentoService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<IEnumerable<TipoPagamentoLightDTO>>> GetLightAsync(string keyword = "")
    {
        var spec = new TipoPagamentoSearchList(keyword);
        var list = await _repository.GetListAsync<TipoPagamentoEntity, TipoPagamentoLightDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }
}
