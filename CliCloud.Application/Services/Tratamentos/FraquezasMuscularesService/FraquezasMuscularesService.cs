using AutoMapper;
using CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.Filters;
using CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.Specifications;

// After creating this service:
// -- 1. Create a FraquezasMusculares domain entity in CliCloud.Domain/Entities/Tratamentos
// -- 2. Add DbSet<FraquezasMusculares> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a FraquezasMusculares api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService
{
    public class FraquezasMuscularesService : IFraquezasMuscularesService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public FraquezasMuscularesService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<FraquezasMuscularesDTO>>> GetFraquezasMuscularesAsync(string keyword = "")
        {
            FraquezasMuscularesSearchList specification = new(keyword); // ardalis specification
            IEnumerable<FraquezasMuscularesDTO> list = await _repository.GetListAsync<FraquezasMusculares, FraquezasMuscularesDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<FraquezasMuscularesDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<FraquezasMuscularesLightDTO>>> GetFraquezasMuscularesLightAsync(string keyword = "")
        {
            FraquezasMuscularesSearchList specification = new(keyword); // ardalis specification
            IEnumerable<FraquezasMuscularesLightDTO> list = await _repository.GetListAsync<FraquezasMusculares, FraquezasMuscularesLightDTO, Guid>(specification); // lightweight list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<FraquezasMuscularesLightDTO>>(list);
        }


        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<FraquezasMuscularesTableDTO>> GetFraquezasMuscularesPaginatedAsync(FraquezasMuscularesTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            FraquezasMuscularesSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<FraquezasMuscularesTableDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<FraquezasMusculares, FraquezasMuscularesTableDTO, Guid>(
                    filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all FraquezasMusculares (non-paginated)
        public async Task<Response<IEnumerable<FraquezasMuscularesTableDTO>>> GetAllFraquezasMuscularesAsync(FraquezasMuscularesAllFilter filter)
        {
            try
            {
                filter ??= new FraquezasMuscularesAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                FraquezasMuscularesSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<FraquezasMuscularesTableDTO> list = await _repository.GetListAsync<FraquezasMusculares, FraquezasMuscularesTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<FraquezasMuscularesTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<FraquezasMuscularesTableDTO>>(ex.Message);
            }
        }


        // get single FraquezasMusculares by Id
        public async Task<Response<FraquezasMuscularesDTO>> GetFraquezasMuscularesAsync(Guid id)
        {
            try
            {
                FraquezasMuscularesDTO dto = await _repository.GetByIdAsync<FraquezasMusculares, FraquezasMuscularesDTO, Guid>(id);
                return ResponseFactory.Success<FraquezasMuscularesDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FraquezasMuscularesDTO>(ex.Message);
            }
        }

        // get single FraquezasMusculares by Descricao (exact match)
        public async Task<Response<FraquezasMuscularesDTO>> GetFraquezasMuscularesByDescricaoAsync(string descricao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    return ResponseFactory.Fail<FraquezasMuscularesDTO>("Descricao não pode ser vazia");
                }

                FraquezasMuscularesMatchDescricao specification = new(descricao); // ardalis specification
                IEnumerable<FraquezasMuscularesDTO> results = 
                    await _repository.GetListAsync<FraquezasMusculares, FraquezasMuscularesDTO, Guid>(specification);

                FraquezasMuscularesDTO? fraquezasMusculares = results.FirstOrDefault();
                if (fraquezasMusculares == null)
                {
                    return ResponseFactory.Fail<FraquezasMuscularesDTO>("Não foi encontrada nenhuma FraquezasMusculares com a descrição fornecida");
                }

                return ResponseFactory.Success<FraquezasMuscularesDTO>(fraquezasMusculares);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FraquezasMuscularesDTO>(ex.Message);
            }
        }

        // create new FraquezasMusculares
        public async Task<Response<Guid>> CreateFraquezasMuscularesAsync(CreateFraquezasMuscularesRequest request)
        {
            FraquezasMuscularesMatchDescricao specification = new(request.Descricao); // ardalis specification 
            bool FraquezasMuscularesExists = await _repository.ExistsAsync<FraquezasMusculares, Guid>(specification);
            if (FraquezasMuscularesExists)
            {
                return ResponseFactory.Fail<Guid>("FraquezasMusculares já existe");
            }

            FraquezasMusculares newFraquezasMusculares = _mapper.Map(request, new FraquezasMusculares()); // map dto to domain entity

            try
            {
                FraquezasMusculares response = await _repository.CreateAsync<FraquezasMusculares, Guid>(newFraquezasMusculares); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update FraquezasMusculares
        public async Task<Response<Guid>> UpdateFraquezasMuscularesAsync(UpdateFraquezasMuscularesRequest request, Guid id)
        {
            FraquezasMusculares FraquezasMuscularesInDb = await _repository.GetByIdAsync<FraquezasMusculares, Guid>(id); // get existing entity
            if (FraquezasMuscularesInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            FraquezasMusculares updatedFraquezasMusculares = _mapper.Map(request, FraquezasMuscularesInDb); // map dto to domain entity

            try
            {
                FraquezasMusculares response = await _repository.UpdateAsync<FraquezasMusculares, Guid>(updatedFraquezasMusculares);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete FraquezasMusculares
        public async Task<Response<Guid>> DeleteFraquezasMuscularesAsync(Guid id)
        {
            try
            {
                FraquezasMusculares? FraquezasMusculares = await _repository.RemoveByIdAsync<FraquezasMusculares, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(FraquezasMusculares.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple FraquezasMusculares
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleFraquezasMuscularesAsync(IEnumerable<Guid> ids)
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
                        FraquezasMusculares? entity = await _repository.GetByIdAsync<FraquezasMusculares, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"FraquezasMusculares com ID {id} não encontrada.");
                            continue;
                        }

                        FraquezasMusculares? deletedEntity = await _repository.RemoveByIdAsync<FraquezasMusculares, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar FraquezasMusculares com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Falha ao eliminar FraquezasMusculares com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }
                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} FraquezasMusculares.");
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

