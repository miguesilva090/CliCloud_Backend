using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Application.Services.Documentos.ReciboService.DTOs;
using CliCloud.Application.Services.Documentos.ReciboService.Filters;
using CliCloud.Application.Services.Documentos.ReciboService.Specifications;

namespace CliCloud.Application.Services.Documentos.ReciboService
{
  public class ReciboService : IReciboService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public ReciboService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<ReciboDTO>>> GetReciboAsync(string keyword = "")
    {
      var spec = new ReciboSearchList(keyword);
      var list = await _repository.GetListAsync<Recibo, ReciboDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ReciboLightDTO>>> GetReciboLightAsync(string keyword = "")
    {
      var spec = new ReciboSearchList(keyword);
      var list = await _repository.GetListAsync<Recibo, ReciboLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ReciboTableDTO>> GetReciboPaginatedAsync(ReciboTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new ReciboSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Recibo, ReciboTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ReciboTableDTO>>> GetAllReciboAsync(ReciboAllFilter filter)
    {
      try
      {
        filter ??= new ReciboAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new ReciboSearchTable(filters, order);
        var list = await _repository.GetListAsync<Recibo, ReciboTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<ReciboTableDTO>>(ex.Message); }
    }

    public async Task<Response<ReciboDTO>> GetReciboAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<Recibo, ReciboDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex) { return ResponseFactory.Fail<ReciboDTO>(ex.Message); }
    }
  }
}
