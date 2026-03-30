using AutoMapper;
using CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.Filters;
using CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.Specifications;

// After creating this service:
// -- 1. Create a ModeloAparelho domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<ModeloAparelho> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a ModeloAparelho api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Tratamentos.ModeloAparelhoService
{
    public class ModeloAparelhoService : IModeloAparelhoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public ModeloAparelhoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<ModeloAparelhoDTO>>> GetModeloAparelhoAsync(string keyword = "")
        {
            ModeloAparelhoSearchList specification = new(keyword); // ardalis specification
            IEnumerable<ModeloAparelhoDTO> list = await _repository.GetListAsync<ModeloAparelho, ModeloAparelhoDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<ModeloAparelhoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<ModeloAparelhoLightDTO>>> GetModeloAparelhoLightAsync(string keyword = "")
        {
            ModeloAparelhoSearchList specification = new(keyword);
            IEnumerable<ModeloAparelhoLightDTO> list = await _repository.GetListAsync<ModeloAparelho, ModeloAparelhoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<ModeloAparelhoLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<ModeloAparelhoTableDTO>> GetModeloAparelhoPaginatedAsync(ModeloAparelhoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            ModeloAparelhoSearchTable specification = new(filter.Filters ?? new List<TableFilter>(), dynamicOrder); // ardalis specification
            PaginatedResponse<ModeloAparelhoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<ModeloAparelho, ModeloAparelhoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all ModeloAparelho (non-paginated)
        public async Task<Response<IEnumerable<ModeloAparelhoTableDTO>>> GetAllModeloAparelhoAsync(ModeloAparelhoAllFilter filter)
        {
            try
            {
                filter ??= new ModeloAparelhoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                ModeloAparelhoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<ModeloAparelhoTableDTO> list = await _repository.GetListAsync<ModeloAparelho, ModeloAparelhoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<ModeloAparelhoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<ModeloAparelhoTableDTO>>(ex.Message);
            }
        }


        // get single ModeloAparelho by Id 
        public async Task<Response<ModeloAparelhoDTO>> GetModeloAparelhoAsync(Guid id)
        {
            try
            {
                ModeloAparelhoDTO dto = await _repository.GetByIdAsync<ModeloAparelho, ModeloAparelhoDTO, Guid>(id);
                return ResponseFactory.Success<ModeloAparelhoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<ModeloAparelhoDTO>(ex.Message);
            }
        }

        // get single ModeloAparelho by Designacao (exact match)
        public async Task<Response<ModeloAparelhoDTO>> GetModeloAparelhoByDesignacaoAsync(string designacao)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(designacao))
                {
                    return ResponseFactory.Fail<ModeloAparelhoDTO>("Designação não pode ser vazia");
                }

                ModeloAparelhoMatchDesignacao specification = new(designacao);
                IEnumerable<ModeloAparelhoDTO> results = await _repository.GetListAsync<ModeloAparelho, ModeloAparelhoDTO, Guid>(specification);
                
                ModeloAparelhoDTO? modeloAparelho = results.FirstOrDefault();
                if(modeloAparelho == null)
                {
                    return ResponseFactory.Fail<ModeloAparelhoDTO>("Não foi encontrado nenhum ModeloAparelho com a designação fornecida");
                }
                return ResponseFactory.Success<ModeloAparelhoDTO>(modeloAparelho);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<ModeloAparelhoDTO>(ex.Message);
            }
        }

        // create new ModeloAparelho
        public async Task<Response<Guid>> CreateModeloAparelhoAsync(CreateModeloAparelhoRequest request)
        {
            ModeloAparelhoMatchDesignacao specification = new(request.Designacao); // ardalis specification 
            bool ModeloAparelhoExists = await _repository.ExistsAsync<ModeloAparelho, Guid>(specification);
            if (ModeloAparelhoExists)
            {
                return ResponseFactory.Fail<Guid>("ModeloAparelho already exists");
            }

            ModeloAparelho newModeloAparelho = _mapper.Map(request, new ModeloAparelho()); // map dto to domain entity

            try
            {
                ModeloAparelho response = await _repository.CreateAsync<ModeloAparelho, Guid>(newModeloAparelho); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update ModeloAparelho
        public async Task<Response<Guid>> UpdateModeloAparelhoAsync(UpdateModeloAparelhoRequest request, Guid id)
        {
            ModeloAparelho ModeloAparelhoInDb = await _repository.GetByIdAsync<ModeloAparelho, Guid>(id); // get existing entity
            if (ModeloAparelhoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            ModeloAparelho updatedModeloAparelho = _mapper.Map(request, ModeloAparelhoInDb); // map dto to domain entity

            try
            {
                ModeloAparelho response = await _repository.UpdateAsync<ModeloAparelho, Guid>(updatedModeloAparelho);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete ModeloAparelho
        public async Task<Response<Guid>> DeleteModeloAparelhoAsync(Guid id)
        {
            try
            {
                ModeloAparelho? ModeloAparelho = await _repository.RemoveByIdAsync<ModeloAparelho, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(ModeloAparelho.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple ModeloAparelho
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleModeloAparelhoAsync(IEnumerable<Guid> ids)
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
                        ModeloAparelho? entity = await _repository.GetByIdAsync<ModeloAparelho, Guid>(id);
                        if(entity == null)
                        {
                            failedDeletions.Add($"ModeloAparelho com ID {id} não encontrado.");
                            continue;
                        }

                        ModeloAparelho? deletedEntity = await _repository.RemoveByIdAsync<ModeloAparelho, Guid>(id);
                        if(deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar ModeloAparelho com ID {id}.");
                        }
                    }
                    catch(Exception)
                    {
                        failedDeletions.Add($"Falha ao eliminar ModeloAparelho com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }
                if(successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                if(successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} ModeloAparelhos.");
                }
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

