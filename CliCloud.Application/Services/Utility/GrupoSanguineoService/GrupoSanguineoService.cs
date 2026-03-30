using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using GrupoSanguineoEntity = CliCloud.Domain.Entities.GruposSanguineos.GrupoSanguineo;
using CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs;
using CliCloud.Application.Services.Utility.GrupoSanguineoService.Filters;
using CliCloud.Application.Services.Utility.GrupoSanguineoService.Specifications;

namespace CliCloud.Application.Services.Utility.GrupoSanguineoService
{
    public class GrupoSanguineoService : IGrupoSanguineoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public GrupoSanguineoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GrupoSanguineoDTO>>> GetGrupoSanguineoAsync(string keyword = "")
        {
            GrupoSanguineoSearchList specification = new(keyword);
            IEnumerable<GrupoSanguineoDTO> list = await _repository.GetListAsync<GrupoSanguineoEntity, GrupoSanguineoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<GrupoSanguineoDTO>>(list);
        }

        public async Task<Response<IEnumerable<GrupoSanguineoLightDTO>>> GetGrupoSanguineoLightAsync(string keyword = "")
        {
            GrupoSanguineoSearchList specification = new(keyword);
            IEnumerable<GrupoSanguineoLightDTO> list = await _repository.GetListAsync<GrupoSanguineoEntity, GrupoSanguineoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<GrupoSanguineoLightDTO>>(list);
        }

        public async Task<PaginatedResponse<GrupoSanguineoTableDTO>> GetGrupoSanguineoPaginatedAsync(GrupoSanguineoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            GrupoSanguineoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<GrupoSanguineoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<GrupoSanguineoEntity, GrupoSanguineoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<GrupoSanguineoTableDTO>>> GetAllGrupoSanguineoAsync(GrupoSanguineoAllFilter filter)
        {
            try
            {
                filter ??= new GrupoSanguineoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                GrupoSanguineoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<GrupoSanguineoTableDTO> list = await _repository.GetListAsync<GrupoSanguineoEntity, GrupoSanguineoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<GrupoSanguineoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<GrupoSanguineoTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<GrupoSanguineoDTO>> GetGrupoSanguineoAsync(Guid id)
        {
            try
            {
                GrupoSanguineoDTO dto = await _repository.GetByIdAsync<GrupoSanguineoEntity, GrupoSanguineoDTO, Guid>(id);
                return ResponseFactory.Success<GrupoSanguineoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<GrupoSanguineoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateGrupoSanguineoAsync(CreateGrupoSanguineoRequest request)
        {
            GrupoSanguineoMatchDescricao specification = new(request.Descricao ?? "");
            bool exists = await _repository.ExistsAsync<GrupoSanguineoEntity, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Grupo Sanguíneo com esta Descrição já existe");

            GrupoSanguineoEntity newEntity = _mapper.Map(request, new GrupoSanguineoEntity());
            try
            {
                GrupoSanguineoEntity response = await _repository.CreateAsync<GrupoSanguineoEntity, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateGrupoSanguineoAsync(UpdateGrupoSanguineoRequest request, Guid id)
        {
            GrupoSanguineoEntity entityInDb = await _repository.GetByIdAsync<GrupoSanguineoEntity, Guid>(id);
            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                GrupoSanguineoMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<GrupoSanguineoEntity, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe um Grupo Sanguíneo com esta Descrição");
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

        public async Task<Response<Guid>> DeleteGrupoSanguineoAsync(Guid id)
        {
            try
            {
                GrupoSanguineoEntity? entity = await _repository.RemoveByIdAsync<GrupoSanguineoEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Grupo Sanguíneo não encontrado");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleGrupoSanguineoAsync(IEnumerable<Guid> ids)
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
                        GrupoSanguineoEntity? entity = await _repository.GetByIdAsync<GrupoSanguineoEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Grupo Sanguíneo com ID {id} não encontrado.");
                            continue;
                        }
                        GrupoSanguineoEntity? deletedEntity = await _repository.RemoveByIdAsync<GrupoSanguineoEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"Grupo Sanguíneo com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Grupo Sanguíneo com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} grupos sanguíneos.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
