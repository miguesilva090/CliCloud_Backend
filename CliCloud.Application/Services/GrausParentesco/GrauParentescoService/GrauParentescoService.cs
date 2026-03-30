using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using GrauParentescoEntity = CliCloud.Domain.Entities.GrausParentesco.GrauParentesco;
using CliCloud.Application.Services.GrausParentesco.GrauParentescoService.DTOs;
using CliCloud.Application.Services.GrausParentesco.GrauParentescoService.Filters;
using CliCloud.Application.Services.GrausParentesco.GrauParentescoService.Specifications;

namespace CliCloud.Application.Services.GrausParentesco.GrauParentescoService
{
    public class GrauParentescoService : IGrauParentescoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public GrauParentescoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GrauParentescoDTO>>> GetGrauParentescoAsync(string keyword = "")
        {
            GrauParentescoSearchList specification = new(keyword);
            IEnumerable<GrauParentescoDTO> list = await _repository.GetListAsync<GrauParentescoEntity, GrauParentescoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<GrauParentescoDTO>>(list);
        }

        public async Task<Response<IEnumerable<GrauParentescoLightDTO>>> GetGrauParentescoLightAsync(string keyword = "")
        {
            GrauParentescoSearchList specification = new(keyword);
            IEnumerable<GrauParentescoLightDTO> list = await _repository.GetListAsync<GrauParentescoEntity, GrauParentescoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<GrauParentescoLightDTO>>(list);
        }

        public async Task<PaginatedResponse<GrauParentescoTableDTO>> GetGrauParentescoPaginatedAsync(GrauParentescoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            GrauParentescoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<GrauParentescoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<GrauParentescoEntity, GrauParentescoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<GrauParentescoTableDTO>>> GetAllGrauParentescoAsync(GrauParentescoAllFilter filter)
        {
            try
            {
                filter ??= new GrauParentescoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                GrauParentescoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<GrauParentescoTableDTO> list = await _repository.GetListAsync<GrauParentescoEntity, GrauParentescoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<GrauParentescoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<GrauParentescoTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<GrauParentescoDTO>> GetGrauParentescoAsync(Guid id)
        {
            try
            {
                GrauParentescoDTO dto = await _repository.GetByIdAsync<GrauParentescoEntity, GrauParentescoDTO, Guid>(id);
                return ResponseFactory.Success<GrauParentescoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<GrauParentescoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateGrauParentescoAsync(CreateGrauParentescoRequest request)
        {
            GrauParentescoMatchDescricao specification = new(request.Descricao ?? "");
            bool exists = await _repository.ExistsAsync<GrauParentescoEntity, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Grau Parentesco com esta descrição já existe");

            GrauParentescoEntity newEntity = _mapper.Map(request, new GrauParentescoEntity());
            try
            {
                GrauParentescoEntity response = await _repository.CreateAsync<GrauParentescoEntity, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateGrauParentescoAsync(UpdateGrauParentescoRequest request, Guid id)
        {
            GrauParentescoEntity entityInDb = await _repository.GetByIdAsync<GrauParentescoEntity, Guid>(id);
            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                GrauParentescoMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<GrauParentescoEntity, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe um Grau Parentesco com esta descrição");
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

        public async Task<Response<Guid>> DeleteGrauParentescoAsync(Guid id)
        {
            try
            {
                GrauParentescoEntity? entity = await _repository.RemoveByIdAsync<GrauParentescoEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Grau Parentesco não encontrado");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleGrauParentescoAsync(IEnumerable<Guid> ids)
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
                        GrauParentescoEntity? entity = await _repository.GetByIdAsync<GrauParentescoEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Grau Parentesco com ID {id} não encontrado.");
                            continue;
                        }
                        GrauParentescoEntity? deletedEntity = await _repository.RemoveByIdAsync<GrauParentescoEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"Grau Parentesco com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Grau Parentesco com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} graus parentesco.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
