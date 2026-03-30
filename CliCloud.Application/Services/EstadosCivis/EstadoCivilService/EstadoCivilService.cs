using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using EstadoCivilEntity = CliCloud.Domain.Entities.EstadosCivis.EstadoCivil;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.Filters;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.Specifications;

namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService
{
    public class EstadoCivilService : IEstadoCivilService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public EstadoCivilService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<EstadoCivilDTO>>> GetEstadoCivilAsync(string keyword = "")
        {
            EstadoCivilSearchList specification = new(keyword);
            IEnumerable<EstadoCivilDTO> list = await _repository.GetListAsync<EstadoCivilEntity, EstadoCivilDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<EstadoCivilDTO>>(list);
        }

        public async Task<Response<IEnumerable<EstadoCivilLightDTO>>> GetEstadoCivilLightAsync(string keyword = "")
        {
            EstadoCivilSearchList specification = new(keyword);
            IEnumerable<EstadoCivilLightDTO> list = await _repository.GetListAsync<EstadoCivilEntity, EstadoCivilLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<EstadoCivilLightDTO>>(list);
        }

        public async Task<PaginatedResponse<EstadoCivilTableDTO>> GetEstadoCivilPaginatedAsync(EstadoCivilTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            EstadoCivilSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<EstadoCivilTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<EstadoCivilEntity, EstadoCivilTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<EstadoCivilTableDTO>>> GetAllEstadoCivilAsync(EstadoCivilAllFilter filter)
        {
            try
            {
                filter ??= new EstadoCivilAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                EstadoCivilSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<EstadoCivilTableDTO> list = await _repository.GetListAsync<EstadoCivilEntity, EstadoCivilTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<EstadoCivilTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<EstadoCivilTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<EstadoCivilDTO>> GetEstadoCivilAsync(Guid id)
        {
            try
            {
                EstadoCivilDTO dto = await _repository.GetByIdAsync<EstadoCivilEntity, EstadoCivilDTO, Guid>(id);
                return ResponseFactory.Success<EstadoCivilDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EstadoCivilDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateEstadoCivilAsync(CreateEstadoCivilRequest request)
        {
            EstadoCivilMatchDescricao specification = new(request.Descricao ?? "");
            bool exists = await _repository.ExistsAsync<EstadoCivilEntity, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Estado Civil com esta descrição já existe");

            EstadoCivilEntity newEntity = _mapper.Map(request, new EstadoCivilEntity());
            try
            {
                EstadoCivilEntity response = await _repository.CreateAsync<EstadoCivilEntity, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateEstadoCivilAsync(UpdateEstadoCivilRequest request, Guid id)
        {
            EstadoCivilEntity entityInDb = await _repository.GetByIdAsync<EstadoCivilEntity, Guid>(id);
            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                EstadoCivilMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<EstadoCivilEntity, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe um Estado Civil com esta descrição");
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

        public async Task<Response<Guid>> DeleteEstadoCivilAsync(Guid id)
        {
            try
            {
                EstadoCivilEntity? entity = await _repository.RemoveByIdAsync<EstadoCivilEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("EstadoCivil não encontrado");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleEstadoCivilAsync(IEnumerable<Guid> ids)
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
                        EstadoCivilEntity? entity = await _repository.GetByIdAsync<EstadoCivilEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"EstadoCivil com ID {id} não encontrado.");
                            continue;
                        }
                        EstadoCivilEntity? deletedEntity = await _repository.RemoveByIdAsync<EstadoCivilEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"EstadoCivil com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"EstadoCivil com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} estados civis.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
