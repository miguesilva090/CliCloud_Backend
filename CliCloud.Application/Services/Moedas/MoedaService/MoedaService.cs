using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using MoedaEntity = CliCloud.Domain.Entities.Moedas.Moeda;
using CliCloud.Application.Services.Moedas.MoedaService.DTOs;
using CliCloud.Application.Services.Moedas.MoedaService.Filters;
using CliCloud.Application.Services.Moedas.MoedaService.Specifications;

namespace CliCloud.Application.Services.Moedas.MoedaService
{
    public class MoedaService : IMoedaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public MoedaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<MoedaDTO>>> GetMoedaAsync(string keyword = "")
        {
            MoedaSearchList specification = new(keyword);
            IEnumerable<MoedaDTO> list = await _repository.GetListAsync<MoedaEntity, MoedaDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MoedaDTO>>(list);
        }

        public async Task<Response<IEnumerable<MoedaLightDTO>>> GetMoedaLightAsync(string keyword = "")
        {
            MoedaSearchList specification = new(keyword);
            IEnumerable<MoedaLightDTO> list = await _repository.GetListAsync<MoedaEntity, MoedaLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MoedaLightDTO>>(list);
        }

        public async Task<PaginatedResponse<MoedaTableDTO>> GetMoedaPaginatedAsync(MoedaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            MoedaSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<MoedaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<MoedaEntity, MoedaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<MoedaTableDTO>>> GetAllMoedaAsync(MoedaAllFilter filter)
        {
            try
            {
                filter ??= new MoedaAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                MoedaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<MoedaTableDTO> list = await _repository.GetListAsync<MoedaEntity, MoedaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<MoedaTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MoedaTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<MoedaDTO>> GetMoedaAsync(Guid id)
        {
            try
            {
                MoedaDTO dto = await _repository.GetByIdAsync<MoedaEntity, MoedaDTO, Guid>(id);
                return ResponseFactory.Success<MoedaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MoedaDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateMoedaAsync(CreateMoedaRequest request)
        {
            MoedaMatchDescricao specification = new(request.Descricao ?? "");
            bool exists = await _repository.ExistsAsync<MoedaEntity, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Moeda com esta descrição já existe");

            MoedaEntity newEntity = _mapper.Map<CreateMoedaRequest, MoedaEntity>(request);
            try
            {
                MoedaEntity response = await _repository.CreateAsync<MoedaEntity, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateMoedaAsync(UpdateMoedaRequest request, Guid id)
        {
            MoedaEntity entityInDb = await _repository.GetByIdAsync<MoedaEntity, Guid>(id);
            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                MoedaMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<MoedaEntity, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe uma Moeda com esta descrição");
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

        public async Task<Response<Guid>> DeleteMoedaAsync(Guid id)
        {
            try
            {
                MoedaEntity? entity = await _repository.RemoveByIdAsync<MoedaEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Moeda não encontrada");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMoedaAsync(IEnumerable<Guid> ids)
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
                        MoedaEntity? entity = await _repository.GetByIdAsync<MoedaEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Moeda com ID {id} não encontrada.");
                            continue;
                        }
                        MoedaEntity? deletedEntity = await _repository.RemoveByIdAsync<MoedaEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"Moeda com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Moeda com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} moedas.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
