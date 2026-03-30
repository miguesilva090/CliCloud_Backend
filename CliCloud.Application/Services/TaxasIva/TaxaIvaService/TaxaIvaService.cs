using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using TaxaIvaEntity = CliCloud.Domain.Entities.TaxasIva.TaxaIva;
using CliCloud.Application.Services.TaxasIva.TaxaIvaService.DTOs;
using CliCloud.Application.Services.TaxasIva.TaxaIvaService.Filters;
using CliCloud.Application.Services.TaxasIva.TaxaIvaService.Specifications;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService
{
    public class TaxaIvaService : ITaxaIvaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public TaxaIvaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<TaxaIvaDTO>>> GetTaxaIvaAsync(string keyword = "")
        {
            TaxaIvaSearchList specification = new(keyword);
            IEnumerable<TaxaIvaDTO> list = await _repository.GetListAsync<TaxaIvaEntity, TaxaIvaDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TaxaIvaDTO>>(list);
        }

        public async Task<Response<IEnumerable<TaxaIvaLightDTO>>> GetTaxaIvaLightAsync(string keyword = "")
        {
            TaxaIvaSearchList specification = new(keyword);
            IEnumerable<TaxaIvaLightDTO> list = await _repository.GetListAsync<TaxaIvaEntity, TaxaIvaLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TaxaIvaLightDTO>>(list);
        }

        public async Task<PaginatedResponse<TaxaIvaTableDTO>> GetTaxaIvaPaginatedAsync(TaxaIvaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            TaxaIvaSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<TaxaIvaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<TaxaIvaEntity, TaxaIvaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<TaxaIvaTableDTO>>> GetAllTaxaIvaAsync(TaxaIvaAllFilter filter)
        {
            try
            {
                filter ??= new TaxaIvaAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                TaxaIvaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<TaxaIvaTableDTO> list = await _repository.GetListAsync<TaxaIvaEntity, TaxaIvaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<TaxaIvaTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<TaxaIvaTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<TaxaIvaDTO>> GetTaxaIvaAsync(Guid id)
        {
            try
            {
                TaxaIvaDTO dto = await _repository.GetByIdAsync<TaxaIvaEntity, TaxaIvaDTO, Guid>(id);
                return ResponseFactory.Success<TaxaIvaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TaxaIvaDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateTaxaIvaAsync(CreateTaxaIvaRequest request)
        {
            TaxaIvaMatchDescricao specification = new(request.Descricao ?? "");
            bool exists = await _repository.ExistsAsync<TaxaIvaEntity, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Taxa IVA com esta descrição já existe");

            TaxaIvaEntity newEntity = _mapper.Map(request, new TaxaIvaEntity());
            try
            {
                TaxaIvaEntity response = await _repository.CreateAsync<TaxaIvaEntity, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateTaxaIvaAsync(UpdateTaxaIvaRequest request, Guid id)
        {
            TaxaIvaEntity entityInDb = await _repository.GetByIdAsync<TaxaIvaEntity, Guid>(id);
            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                TaxaIvaMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<TaxaIvaEntity, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe uma Taxa IVA com esta descrição");
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

        public async Task<Response<Guid>> DeleteTaxaIvaAsync(Guid id)
        {
            try
            {
                TaxaIvaEntity? entity = await _repository.RemoveByIdAsync<TaxaIvaEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Taxa IVA não encontrada");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTaxaIvaAsync(IEnumerable<Guid> ids)
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
                        TaxaIvaEntity? entity = await _repository.GetByIdAsync<TaxaIvaEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Taxa IVA com ID {id} não encontrada.");
                            continue;
                        }
                        TaxaIvaEntity? deletedEntity = await _repository.RemoveByIdAsync<TaxaIvaEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"Taxa IVA com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Taxa IVA com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} taxas IVA.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
