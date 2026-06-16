using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.TaxasIva;
using CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.DTOs;
using CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.Filters;
using CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.Specifications;

namespace CliCloud.Application.Services.TaxasIva.MotivoRetencaoService
{
    public class MotivoRetencaoService : IMotivoRetencaoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public MotivoRetencaoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<MotivoRetencaoDTO>>> GetMotivoRetencaoAsync(string keyword = "")
        {
            var spec = new MotivoRetencaoSearchList(keyword);
            var list = await _repository.GetListAsync<MotivoRetencao, MotivoRetencaoDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<MotivoRetencaoLightDTO>>> GetMotivoRetencaoLightAsync(string keyword = "", string? tipoImposto = null)
        {
            var spec = new MotivoRetencaoSearchList(keyword, tipoImposto);
            var list = await _repository.GetListAsync<MotivoRetencao, MotivoRetencaoLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<MotivoRetencaoTableDTO>> GetMotivoRetencaoPaginatedAsync(MotivoRetencaoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new MotivoRetencaoSearchTable(filter.Filters ?? [], order);
            return await _repository.GetPaginatedResultsAsync<MotivoRetencao, MotivoRetencaoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<MotivoRetencaoTableDTO>>> GetAllMotivoRetencaoAsync(MotivoRetencaoAllFilter? filter)
        {
            try
            {
                filter ??= new MotivoRetencaoAllFilter();
                var order = filter.GetOrderByString();
                var spec = new MotivoRetencaoSearchTable(filter.Filters ?? [], order);
                var list = await _repository.GetListAsync<MotivoRetencao, MotivoRetencaoTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<MotivoRetencaoTableDTO>>(ex.Message); }
        }

        public async Task<Response<MotivoRetencaoDTO>> GetMotivoRetencaoAsync(Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync<MotivoRetencao, MotivoRetencaoDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex) { return ResponseFactory.Fail<MotivoRetencaoDTO>(ex.Message); }
        }

        public async Task<Response<Guid>> CreateMotivoRetencaoAsync(CreateMotivoRetencaoRequest request)
        {
            request.Descricao = request.Descricao.Trim();
            request.TipoImposto = request.TipoImposto.Trim().ToUpperInvariant();

            var entity = _mapper.Map<MotivoRetencao>(request);
            try
            {
                var created = await _repository.CreateAsync<MotivoRetencao, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> UpdateMotivoRetencaoAsync(UpdateMotivoRetencaoRequest request, Guid id)
        {
            request.Descricao = request.Descricao.Trim();
            request.TipoImposto = request.TipoImposto.Trim().ToUpperInvariant();

            var existing = await _repository.GetByIdAsync<MotivoRetencao, Guid>(id);
            _mapper.Map(request, existing);
            try
            {
                _ = await _repository.UpdateAsync<MotivoRetencao, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(existing.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> DeleteMotivoRetencaoAsync(Guid id)
        {
            try
            {
                var entity = await _repository.RemoveByIdAsync<MotivoRetencao, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMotivoRetencaoAsync(IEnumerable<Guid> ids)
        {
            var ok = new List<Guid>();
            foreach (var id in ids.ToList())
            {
                try
                {
                    var e = await _repository.RemoveByIdAsync<MotivoRetencao, Guid>(id);
                    if (e != null) { _ = await _repository.SaveChangesAsync(); ok.Add(id); }
                }
                catch { _repository.ClearChangeTracker(); }
            }
            if (ok.Count == ids.Count()) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
            if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {ids.Count()}.");
            return ResponseFactory.Fail<IEnumerable<Guid>>("Nenhum eliminado.");
        }
    }
}
