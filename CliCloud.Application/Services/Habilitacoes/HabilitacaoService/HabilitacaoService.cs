using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using HabilitacaoEntity = CliCloud.Domain.Entities.Habilitacoes.Habilitacao;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService.Filters;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService.Specifications;

namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService
{
    public class HabilitacaoService : IHabilitacaoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public HabilitacaoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<HabilitacaoDTO>>> GetHabilitacaoAsync(string keyword = "")
        {
            HabilitacaoSearchList specification = new(keyword);
            IEnumerable<HabilitacaoDTO> list = await _repository.GetListAsync<HabilitacaoEntity, HabilitacaoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HabilitacaoDTO>>(list);
        }

        public async Task<Response<IEnumerable<HabilitacaoLightDTO>>> GetHabilitacaoLightAsync(string keyword = "")
        {
            HabilitacaoSearchList specification = new(keyword);
            IEnumerable<HabilitacaoLightDTO> list = await _repository.GetListAsync<HabilitacaoEntity, HabilitacaoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HabilitacaoLightDTO>>(list);
        }

        public async Task<PaginatedResponse<HabilitacaoTableDTO>> GetHabilitacaoPaginatedAsync(HabilitacaoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            HabilitacaoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<HabilitacaoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<HabilitacaoEntity, HabilitacaoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<HabilitacaoTableDTO>>> GetAllHabilitacaoAsync(HabilitacaoAllFilter filter)
        {
            try
            {
                filter ??= new HabilitacaoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                HabilitacaoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<HabilitacaoTableDTO> list = await _repository.GetListAsync<HabilitacaoEntity, HabilitacaoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<HabilitacaoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<HabilitacaoTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<HabilitacaoDTO>> GetHabilitacaoAsync(Guid id)
        {
            try
            {
                HabilitacaoDTO dto = await _repository.GetByIdAsync<HabilitacaoEntity, HabilitacaoDTO, Guid>(id);
                return ResponseFactory.Success<HabilitacaoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<HabilitacaoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateHabilitacaoAsync(CreateHabilitacaoRequest request)
        {
            HabilitacaoMatchDescricao specification = new(request.Descricao ?? "");
            bool exists = await _repository.ExistsAsync<HabilitacaoEntity, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Habilitação com esta Descrição já existe");

            HabilitacaoEntity newEntity = _mapper.Map(request, new HabilitacaoEntity());
            try
            {
                HabilitacaoEntity response = await _repository.CreateAsync<HabilitacaoEntity, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateHabilitacaoAsync(UpdateHabilitacaoRequest request, Guid id)
        {
            HabilitacaoEntity entityInDb = await _repository.GetByIdAsync<HabilitacaoEntity, Guid>(id);
            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                HabilitacaoMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<HabilitacaoEntity, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe uma Habilitação com esta Descrição");
            }

            _ = _mapper.Map(request, entityInDb);
            try
            {
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entityInDb.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteHabilitacaoAsync(Guid id)
        {
            try
            {
                HabilitacaoEntity? entity = await _repository.RemoveByIdAsync<HabilitacaoEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Habilitação não encontrada");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleHabilitacaoAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach (Guid id in idsList)
                {
                    try
                    {
                        HabilitacaoEntity? entity = await _repository.GetByIdAsync<HabilitacaoEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Habilitação com ID {id} não encontrada.");
                            continue;
                        }
                        HabilitacaoEntity? deletedEntity = await _repository.RemoveByIdAsync<HabilitacaoEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"Habilitação com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Habilitação com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} habilitações.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
