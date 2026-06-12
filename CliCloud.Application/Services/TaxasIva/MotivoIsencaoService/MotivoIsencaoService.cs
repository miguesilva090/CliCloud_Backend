using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.TaxasIva;
using CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs;
using CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.Filters;
using CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.Specifications;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService
{
    public class MotivoIsencaoService : IMotivoIsencaoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public MotivoIsencaoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<MotivoIsencaoDTO>>> GetMotivoIsencaoAsync(string keyword = "")
        {
            var spec = new MotivoIsencaoSearchList(keyword);
            var list = await _repository.GetListAsync<MotivoIsencao, MotivoIsencaoDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<MotivoIsencaoLightDTO>>> GetMotivoIsencaoLightAsync(string keyword = "")
        {
            var spec = new MotivoIsencaoSearchList(keyword);
            var list = await _repository.GetListAsync<MotivoIsencao, MotivoIsencaoLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<MotivoIsencaoTableDTO>> GetMotivoIsencaoPaginatedAsync(MotivoIsencaoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new MotivoIsencaoSearchTable(filter.Filters ?? [], order);
            return await _repository.GetPaginatedResultsAsync<MotivoIsencao, MotivoIsencaoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<MotivoIsencaoTableDTO>>> GetAllMotivoIsencaoAsync(MotivoIsencaoAllFilter? filter)
        {
            try
            {
                filter ??= new MotivoIsencaoAllFilter();
                var order = filter.GetOrderByString();
                var spec = new MotivoIsencaoSearchTable(filter.Filters ?? [], order);
                var list = await _repository.GetListAsync<MotivoIsencao, MotivoIsencaoTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<MotivoIsencaoTableDTO>>(ex.Message); }
        }

        public async Task<Response<MotivoIsencaoDTO>> GetMotivoIsencaoAsync(Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync<MotivoIsencao, MotivoIsencaoDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex) { return ResponseFactory.Fail<MotivoIsencaoDTO>(ex.Message); }
        }

        public async Task<Response<Guid>> CreateMotivoIsencaoAsync(CreateMotivoIsencaoRequest request)
        {
            request.Codigo = request.Codigo.Trim();
            request.CodigoSaft = request.CodigoSaft.Trim();
            request.Descricao = request.Descricao.Trim();
            request.Norma = string.IsNullOrWhiteSpace(request.Norma) ? null : request.Norma.Trim();
            request.Mencao = string.IsNullOrWhiteSpace(request.Mencao) ? null : request.Mencao.Trim();

            var entity = _mapper.Map<MotivoIsencao>(request);
            try
            {
                var created = await _repository.CreateAsync<MotivoIsencao, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> UpdateMotivoIsencaoAsync(UpdateMotivoIsencaoRequest request, Guid id)
        {
            request.Codigo = request.Codigo.Trim();
            request.CodigoSaft = request.CodigoSaft.Trim();
            request.Descricao = request.Descricao.Trim();
            request.Norma = string.IsNullOrWhiteSpace(request.Norma) ? null : request.Norma.Trim();
            request.Mencao = string.IsNullOrWhiteSpace(request.Mencao) ? null : request.Mencao.Trim();

            var existing = await _repository.GetByIdAsync<MotivoIsencao, Guid>(id);
            _mapper.Map(request, existing);
            try
            {
                _ = await _repository.UpdateAsync<MotivoIsencao, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(existing.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> DeleteMotivoIsencaoAsync(Guid id)
        {
            try
            {
                var entity = await _repository.RemoveByIdAsync<MotivoIsencao, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMotivoIsencaoAsync(IEnumerable<Guid> ids)
        {
            var ok = new List<Guid>();
            foreach (var id in ids.ToList())
            {
                try
                {
                    var e = await _repository.RemoveByIdAsync<MotivoIsencao, Guid>(id);
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
