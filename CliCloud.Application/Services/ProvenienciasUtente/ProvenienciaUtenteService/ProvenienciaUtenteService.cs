using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using ProvenienciaUtenteEntity = CliCloud.Domain.Entities.ProvenienciasUtente.ProvenienciaUtente;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.Filters;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.Specifications;

namespace CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService
{
    public class ProvenienciaUtenteService : IProvenienciaUtenteService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public ProvenienciaUtenteService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ProvenienciaUtenteDTO>>> GetProvenienciaUtenteAsync(string keyword = "")
        {
            ProvenienciaUtenteSearchList specification = new(keyword);
            IEnumerable<ProvenienciaUtenteDTO> list = await _repository.GetListAsync<ProvenienciaUtenteEntity, ProvenienciaUtenteDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<ProvenienciaUtenteDTO>>(list);
        }

        public async Task<Response<IEnumerable<ProvenienciaUtenteLightDTO>>> GetProvenienciaUtenteLightAsync(string keyword = "")
        {
            ProvenienciaUtenteSearchList specification = new(keyword);
            IEnumerable<ProvenienciaUtenteLightDTO> list = await _repository.GetListAsync<ProvenienciaUtenteEntity, ProvenienciaUtenteLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<ProvenienciaUtenteLightDTO>>(list);
        }

        public async Task<PaginatedResponse<ProvenienciaUtenteTableDTO>> GetProvenienciaUtentePaginatedAsync(ProvenienciaUtenteTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            ProvenienciaUtenteSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<ProvenienciaUtenteTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<ProvenienciaUtenteEntity, ProvenienciaUtenteTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<ProvenienciaUtenteTableDTO>>> GetAllProvenienciaUtenteAsync(ProvenienciaUtenteAllFilter filter)
        {
            try
            {
                filter ??= new ProvenienciaUtenteAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                ProvenienciaUtenteSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<ProvenienciaUtenteTableDTO> list = await _repository.GetListAsync<ProvenienciaUtenteEntity, ProvenienciaUtenteTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<ProvenienciaUtenteTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<ProvenienciaUtenteTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<ProvenienciaUtenteDTO>> GetProvenienciaUtenteAsync(Guid id)
        {
            try
            {
                ProvenienciaUtenteDTO dto = await _repository.GetByIdAsync<ProvenienciaUtenteEntity, ProvenienciaUtenteDTO, Guid>(id);
                return ResponseFactory.Success<ProvenienciaUtenteDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<ProvenienciaUtenteDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateProvenienciaUtenteAsync(CreateProvenienciaUtenteRequest request)
        {
            ProvenienciaUtenteMatchDescricao specification = new(request.Descricao ?? "");
            bool exists = await _repository.ExistsAsync<ProvenienciaUtenteEntity, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Proveniência Utente com esta descrição já existe");

            ProvenienciaUtenteEntity newEntity = _mapper.Map(request, new ProvenienciaUtenteEntity());
            try
            {
                ProvenienciaUtenteEntity response = await _repository.CreateAsync<ProvenienciaUtenteEntity, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateProvenienciaUtenteAsync(UpdateProvenienciaUtenteRequest request, Guid id)
        {
            ProvenienciaUtenteEntity entityInDb = await _repository.GetByIdAsync<ProvenienciaUtenteEntity, Guid>(id);
            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                ProvenienciaUtenteMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<ProvenienciaUtenteEntity, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe uma Proveniência Utente com esta descrição");
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

        public async Task<Response<Guid>> DeleteProvenienciaUtenteAsync(Guid id)
        {
            try
            {
                ProvenienciaUtenteEntity? entity = await _repository.RemoveByIdAsync<ProvenienciaUtenteEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Proveniência Utente não encontrada");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleProvenienciaUtenteAsync(IEnumerable<Guid> ids)
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
                        ProvenienciaUtenteEntity? entity = await _repository.GetByIdAsync<ProvenienciaUtenteEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Proveniência Utente com ID {id} não encontrada.");
                            continue;
                        }
                        ProvenienciaUtenteEntity? deletedEntity = await _repository.RemoveByIdAsync<ProvenienciaUtenteEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"Proveniência Utente com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Proveniência Utente com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} proveniências utente.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
