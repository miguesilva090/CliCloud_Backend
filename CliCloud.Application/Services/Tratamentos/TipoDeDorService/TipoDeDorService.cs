using AutoMapper;
using CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Utility;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Services.Tratamentos.TipoDeDorService.Filters;
using CliCloud.Application.Services.Tratamentos.TipoDeDorService.Specifications;
namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService

// After creating this service:
// -- 1. Create a TipoDeDor domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<TipoDeDor> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a TipoDeDor api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
{
    public class TipoDeDorService : ITipoDeDorService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public TipoDeDorService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<TipoDeDorDTO>>> GetTipoDeDorAsync(string keyword = "")
        {
            TipoDeDorSearchList specification = new(keyword); // ardalis specification
            IEnumerable<TipoDeDorDTO> list = await _repository.GetListAsync<TipoDeDor, TipoDeDorDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<TipoDeDorDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<TipoDeDorLightDTO>>> GetTipoDeDorLightAsync(string keyword = "")
        {
            TipoDeDorSearchList specification = new(keyword);
            IEnumerable<TipoDeDorLightDTO> list = await _repository.GetListAsync<TipoDeDor, TipoDeDorLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TipoDeDorLightDTO>>(list);
        }


        // get Tanstack Table paginated list 
        public async Task<PaginatedResponse<TipoDeDorTableDTO>> GetTipoDeDorPaginatedAsync(TipoDeDorTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            TipoDeDorSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<TipoDeDorTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<TipoDeDor, TipoDeDorTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all TipoDeDor (non-paginated)
        public async Task<Response<IEnumerable<TipoDeDorTableDTO>>> GetAllTipoDeDorAsync(TipoDeDorAllFilter filter)
        {
            try
            {
                filter ??= new TipoDeDorAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                TipoDeDorSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<TipoDeDorTableDTO> list = await _repository.GetListAsync<TipoDeDor, TipoDeDorTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<TipoDeDorTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<TipoDeDorTableDTO>>(ex.Message);
            }
        }


        // get single TipoDeDor by Id 
        public async Task<Response<TipoDeDorDTO>> GetTipoDeDorAsync(Guid id)
        {
            try
            {
                TipoDeDorDTO dto = await _repository.GetByIdAsync<TipoDeDor, TipoDeDorDTO, Guid>(id);
                return ResponseFactory.Success<TipoDeDorDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TipoDeDorDTO>(ex.Message);
            }
        }

        // get single TipoDeDor by Descricao (exact match)
        public async Task<Response<TipoDeDorDTO>> GetTipoDeDorByDescricaoAsync(string descricao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    return ResponseFactory.Fail<TipoDeDorDTO>("Descricao não pode ser vazia");
                }
            

            TipoDeDorMatchDescricao specification = new(descricao);
            IEnumerable<TipoDeDorDTO> results = 
                await _repository.GetListAsync<TipoDeDor, TipoDeDorDTO, Guid>(specification);
            
            TipoDeDorDTO? tipoDeDor = results.FirstOrDefault();
            if (tipoDeDor == null)
            {
                return ResponseFactory.Fail<TipoDeDorDTO>("Não foi encontrado nenhum TipoDeDor com a Descricao fornecida");
            }

            return ResponseFactory.Success<TipoDeDorDTO>(tipoDeDor);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TipoDeDorDTO>(ex.Message);
            }
        }

        // create new TipoDeDor
        public async Task<Response<Guid>> CreateTipoDeDorAsync(CreateTipoDeDorRequest request)
        {
            TipoDeDorMatchDescricao specification = new(request.Descricao); 
            bool tipoDeDorExists = await _repository.ExistsAsync<TipoDeDor, Guid>(specification);
            if (tipoDeDorExists)
            {
                return ResponseFactory.Fail<Guid>("Tipo de dor já existe.");
            }

            TipoDeDor newTipoDeDor = _mapper.Map(request, new TipoDeDor()); // map dto to domain entity

            try
            {
                TipoDeDor response = await _repository.CreateAsync<TipoDeDor, Guid>(newTipoDeDor); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update TipoDeDor
        public async Task<Response<Guid>> UpdateTipoDeDorAsync(UpdateTipoDeDorRequest request, Guid id)
        {
            TipoDeDor TipoDeDorInDb = await _repository.GetByIdAsync<TipoDeDor, Guid>(id); // get existing entity
            if (TipoDeDorInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não foi encontrado nenhum TipoDeDor com o Id fornecido");
            }

            TipoDeDor updatedTipoDeDor = _mapper.Map(request, TipoDeDorInDb); // map dto to domain entity

            try
            {
                TipoDeDor response = await _repository.UpdateAsync<TipoDeDor, Guid>(updatedTipoDeDor);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete TipoDeDor
        public async Task<Response<Guid>> DeleteTipoDeDorAsync(Guid id)
        {
            try
            {
                TipoDeDor? TipoDeDor = await _repository.RemoveByIdAsync<TipoDeDor, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(TipoDeDor.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple TipoDeDor
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoDeDorAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach(Guid id in idsList)
                {
                    try
                    {
                        TipoDeDor? entity = await _repository.GetByIdAsync<TipoDeDor, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"TipoDeDor com ID {id} não encontrado.");
                            continue;
                        }

                        TipoDeDor? deletedEntity = await _repository.RemoveByIdAsync<TipoDeDor, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar TipoDeDor com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Falha ao eliminar TipoDeDor com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }

                if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} tipos de dor.");
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

