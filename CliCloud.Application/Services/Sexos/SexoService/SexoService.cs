using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using SexoEntity = CliCloud.Domain.Entities.Sexos.Sexo;
using CliCloud.Application.Services.Sexos.SexoService.DTOs;
using CliCloud.Application.Services.Sexos.SexoService.Filters;
using CliCloud.Application.Services.Sexos.SexoService.Specifications;

namespace CliCloud.Application.Services.Sexos.SexoService
{
    public class SexoService : ISexoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public SexoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<SexoDTO>>> GetSexoAsync(string keyword = "")
        {
            SexoSearchList specification = new(keyword);
            IEnumerable<SexoDTO> list = await _repository.GetListAsync<SexoEntity, SexoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<SexoDTO>>(list);
        }

        public async Task<Response<IEnumerable<SexoLightDTO>>> GetSexoLightAsync(string keyword = "")
        {
            SexoSearchList specification = new(keyword);
            IEnumerable<SexoLightDTO> list = await _repository.GetListAsync<SexoEntity, SexoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<SexoLightDTO>>(list);
        }

        public async Task<PaginatedResponse<SexoTableDTO>> GetSexoPaginatedAsync(SexoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            SexoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<SexoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<SexoEntity, SexoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<SexoTableDTO>>> GetAllSexoAsync(SexoAllFilter filter)
        {
            try
            {
                filter ??= new SexoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                SexoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<SexoTableDTO> list = await _repository.GetListAsync<SexoEntity, SexoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<SexoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<SexoTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<SexoDTO>> GetSexoAsync(Guid id)
        {
            try
            {
                SexoDTO dto = await _repository.GetByIdAsync<SexoEntity, SexoDTO, Guid>(id);
                return ResponseFactory.Success<SexoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<SexoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateSexoAsync(CreateSexoRequest request)
        {
            SexoMatchDescricao specification = new(request.Descricao ?? "");
            bool exists = await _repository.ExistsAsync<SexoEntity, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Sexo com esta descrição já existe");

            SexoEntity newEntity = _mapper.Map(request, new SexoEntity());
            try
            {
                SexoEntity response = await _repository.CreateAsync<SexoEntity, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateSexoAsync(UpdateSexoRequest request, Guid id)
        {
            SexoEntity entityInDb = await _repository.GetByIdAsync<SexoEntity, Guid>(id);
            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                SexoMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<SexoEntity, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe um Sexo com esta descrição");
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

        public async Task<Response<Guid>> DeleteSexoAsync(Guid id)
        {
            try
            {
                SexoEntity? entity = await _repository.RemoveByIdAsync<SexoEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Sexo não encontrado");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleSexoAsync(IEnumerable<Guid> ids)
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
                        SexoEntity? entity = await _repository.GetByIdAsync<SexoEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Sexo com ID {id} não encontrado.");
                            continue;
                        }
                        SexoEntity? deletedEntity = await _repository.RemoveByIdAsync<SexoEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"Sexo com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Sexo com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} sexos.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

