using System.Linq;
using AutoMapper;
using CliCloud.Application.Services.GrauAlergiaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Alergias;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.GrauAlergiaService.Filters;
using CliCloud.Application.Services.GrauAlergiaService.Specifications;

namespace CliCloud.Application.Services.GrauAlergiaService
{
    public class GrauAlergiaService : IGrauAlergiaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public GrauAlergiaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GrauAlergiaDTO>>> GetGrauAlergiaAsync(string keyword = "")
        {
            GrauAlergiaSearchList specification = new(keyword);
            IEnumerable<GrauAlergiaDTO> list = await _repository.GetListAsync<GrauAlergia, GrauAlergiaDTO, Guid>(specification);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<GrauAlergiaLightDTO>>> GetGrauAlergiaLightAsync(string keyword = "")
        {
            GrauAlergiaSearchList specification = new(keyword);
            IEnumerable<GrauAlergiaLightDTO> list = await _repository.GetListAsync<GrauAlergia, GrauAlergiaLightDTO, Guid>(specification);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<GrauAlergiaTableDTO>> GetGrauAlergiaPaginatedAsync(GrauAlergiaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            GrauAlergiaSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<GrauAlergiaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<GrauAlergia, GrauAlergiaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<GrauAlergiaTableDTO>>> GetAllGrauAlergiaAsync(GrauAlergiaAllFilter filter)
        {
            try
            {
                filter ??= new GrauAlergiaAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                GrauAlergiaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<GrauAlergiaTableDTO> list = await _repository.GetListAsync<GrauAlergia, GrauAlergiaTableDTO, Guid>(specification);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<GrauAlergiaTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<GrauAlergiaDTO>> GetGrauAlergiaAsync(Guid id)
        {
            try
            {
                GrauAlergiaDTO dto = await _repository.GetByIdAsync<GrauAlergia, GrauAlergiaDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<GrauAlergiaDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateGrauAlergiaAsync(CreateGrauAlergiaRequest request)
        {
            GrauAlergiaMatchDescricao specification = new(request.Descricao);
            bool exists = await _repository.ExistsAsync<GrauAlergia, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Grau de Alergia com esta descrição já existe");

            GrauAlergia newEntity = _mapper.Map(request, new GrauAlergia());
            try
            {
                GrauAlergia response = await _repository.CreateAsync<GrauAlergia, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateGrauAlergiaAsync(UpdateGrauAlergiaRequest request, Guid id)
        {
            GrauAlergia entityInDb = await _repository.GetByIdAsync<GrauAlergia, Guid>(id);
            if (entityInDb == null)
                return ResponseFactory.Fail<Guid>("Grau de Alergia não encontrado");

            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                GrauAlergiaMatchDescricao specification = new(request.Descricao);
                bool descricaoExists = await _repository.ExistsAsync<GrauAlergia, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe um Grau de Alergia com esta descrição");
            }

            _ = _mapper.Map(request, entityInDb);
            try
            {
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entityInDb.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteGrauAlergiaAsync(Guid id)
        {
            try
            {
                GrauAlergia? entity = await _repository.RemoveByIdAsync<GrauAlergia, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Grau de Alergia não encontrado");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleGrauAlergiaAsync(IEnumerable<Guid> ids)
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
                        GrauAlergia? entity = await _repository.GetByIdAsync<GrauAlergia, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Grau de Alergia com ID {id} não encontrado.");
                            continue;
                        }
                        GrauAlergia? deletedEntity = await _repository.RemoveByIdAsync<GrauAlergia, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"Grau de Alergia com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Grau de Alergia com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} graus de alergia.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
