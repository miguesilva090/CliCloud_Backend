using AutoMapper;
using System.Linq;
using CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.Filters;
using CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.Specifications;

// After creating this service:
// -- 1. Create a PeriocidadeTratamento domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<PeriocidadeTratamento> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a PeriocidadeTratamento api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService
{
    public class PeriocidadeTratamentoService : IPeriocidadeTratamentoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public PeriocidadeTratamentoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<PeriocidadeTratamentoDTO>>> GetPeriocidadeTratamentoAsync(string keyword = "")
        {
            PeriocidadeTratamentoSearchList specification = new(keyword); // ardalis specification
            IEnumerable<PeriocidadeTratamentoDTO> list = await _repository.GetListAsync<PeriocidadeTratamento, PeriocidadeTratamentoDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<PeriocidadeTratamentoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<PeriocidadeTratamentoLightDTO>>> GetPeriocidadeTratamentoLightAsync(string keyword = "")
        {
            PeriocidadeTratamentoSearchList specification = new(keyword);
            IEnumerable<PeriocidadeTratamentoLightDTO> list = await _repository.GetListAsync<PeriocidadeTratamento, PeriocidadeTratamentoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<PeriocidadeTratamentoLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<PeriocidadeTratamentoTableDTO>> GetPeriocidadeTratamentoPaginatedAsync(PeriocidadeTratamentoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            PeriocidadeTratamentoSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<PeriocidadeTratamentoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<PeriocidadeTratamento, PeriocidadeTratamentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all PeriocidadeTratamento (non-paginated)
        public async Task<Response<IEnumerable<PeriocidadeTratamentoTableDTO>>> GetAllPeriocidadeTratamentoAsync(PeriocidadeTratamentoAllFilter filter)
        {
            try
            {
                filter ??= new PeriocidadeTratamentoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                PeriocidadeTratamentoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<PeriocidadeTratamentoTableDTO> list = await _repository.GetListAsync<PeriocidadeTratamento, PeriocidadeTratamentoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<PeriocidadeTratamentoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<PeriocidadeTratamentoTableDTO>>(ex.Message);
            }
        }


        // get single PeriocidadeTratamento by Id 
        public async Task<Response<PeriocidadeTratamentoDTO>> GetPeriocidadeTratamentoAsync(Guid id)
        {
            try
            {
                PeriocidadeTratamentoDTO dto = await _repository.GetByIdAsync<PeriocidadeTratamento, PeriocidadeTratamentoDTO, Guid>(id);
                return ResponseFactory.Success<PeriocidadeTratamentoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<PeriocidadeTratamentoDTO>(ex.Message);
            }
        }

        // get single PeriocidadeTratamento by Descricao (exact match)
        public async Task<Response<PeriocidadeTratamentoDTO>> GetPeriocidadeTratamentoByDescricaoAsync(string descricao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    return ResponseFactory.Fail<PeriocidadeTratamentoDTO>("Descricao não pode ser vazia");
                }

                PeriocidadeTratamentoMatchDescricao specification = new(descricao);
                IEnumerable<PeriocidadeTratamentoDTO> results =
                    await _repository.GetListAsync<PeriocidadeTratamento, PeriocidadeTratamentoDTO, Guid>(specification);

                PeriocidadeTratamentoDTO? periocidadeTratamento = results.FirstOrDefault();
                if (periocidadeTratamento == null)
                {
                    return ResponseFactory.Fail<PeriocidadeTratamentoDTO>(
                        "Não foi encontrada nenhuma periodicidade de tratamento com a descrição fornecida.");
                }

                return ResponseFactory.Success(periocidadeTratamento);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<PeriocidadeTratamentoDTO>(ex.Message);
            }
        }

        // create new PeriocidadeTratamento
        public async Task<Response<Guid>> CreatePeriocidadeTratamentoAsync(CreatePeriocidadeTratamentoRequest request)
        {
            PeriocidadeTratamentoMatchDescricao specification = new(request.Descricao); // ardalis specification 
            bool PeriocidadeTratamentoExists = await _repository.ExistsAsync<PeriocidadeTratamento, Guid>(specification);
            if (PeriocidadeTratamentoExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe uma periodicidade de tratamento com essa descrição.");
            }

            PeriocidadeTratamento newPeriocidadeTratamento = _mapper.Map(request, new PeriocidadeTratamento()); // map dto to domain entity

            try
            {
                PeriocidadeTratamento response = await _repository.CreateAsync<PeriocidadeTratamento, Guid>(newPeriocidadeTratamento); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update PeriocidadeTratamento
        public async Task<Response<Guid>> UpdatePeriocidadeTratamentoAsync(UpdatePeriocidadeTratamentoRequest request, Guid id)
        {
            PeriocidadeTratamento PeriocidadeTratamentoInDb = await _repository.GetByIdAsync<PeriocidadeTratamento, Guid>(id); // get existing entity
            if (PeriocidadeTratamentoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            PeriocidadeTratamento updatedPeriocidadeTratamento = _mapper.Map(request, PeriocidadeTratamentoInDb); // map dto to domain entity

            try
            {
                PeriocidadeTratamento response = await _repository.UpdateAsync<PeriocidadeTratamento, Guid>(updatedPeriocidadeTratamento);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete PeriocidadeTratamento
        public async Task<Response<Guid>> DeletePeriocidadeTratamentoAsync(Guid id)
        {
            try
            {
                PeriocidadeTratamento? PeriocidadeTratamento = await _repository.RemoveByIdAsync<PeriocidadeTratamento, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(PeriocidadeTratamento.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple PeriocidadeTratamento 
        public async Task<Response<IEnumerable<Guid>>> DeleteMultiplePeriocidadeTratamentoAsync(IEnumerable<Guid> ids)
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
                        PeriocidadeTratamento? entity = await _repository.GetByIdAsync<PeriocidadeTratamento, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Periocidade de Tratamento com ID {id} não encontrado.");
                            continue;
                        }

                        PeriocidadeTratamento? deletedEntity = await _repository.RemoveByIdAsync<PeriocidadeTratamento, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else 
                        {
                            failedDeletions.Add($"Falha ao eliminar Periocidade de Tratamento com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Falha ao eliminar Periocidade de Tratamento com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }
                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} Periocidades de Tratamento.");
                }
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

