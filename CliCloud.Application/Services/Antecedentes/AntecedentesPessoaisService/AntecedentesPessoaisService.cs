using AutoMapper;
using CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Antecedentes;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.Filters;
using CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.Specifications;

// After creating this service:
// -- 1. Create a AntecedentesPessoais domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AntecedentesPessoais> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AntecedentesPessoais api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService
{
    public class AntecedentesPessoaisService : IAntecedentesPessoaisService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AntecedentesPessoaisService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<AntecedentesPessoaisDTO>>> GetAntecedentesPessoaisAsync(string keyword = "")
        {
            AntecedentesPessoaisSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AntecedentesPessoaisDTO> list = await _repository.GetListAsync<AntecedentesPessoais, AntecedentesPessoaisDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<AntecedentesPessoaisDTO>>(list);
        }

        //get lightweight list 
        public async Task<Response<IEnumerable<AntecedentesPessoaisLightDTO>>> GetAntecedentesPessoaisLightAsync(string keyword = "")
        {
            AntecedentesPessoaisSearchList specification = new(keyword);
            IEnumerable<AntecedentesPessoaisLightDTO> list = await _repository.GetListAsync<AntecedentesPessoais, AntecedentesPessoaisLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<AntecedentesPessoaisLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AntecedentesPessoaisTableDTO>> GetAntecedentesPessoaisPaginatedAsync(AntecedentesPessoaisTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            AntecedentesPessoaisSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<AntecedentesPessoaisTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AntecedentesPessoais, AntecedentesPessoaisTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all AntecedentesPessoais (non-paginated)
        public async Task<Response<IEnumerable<AntecedentesPessoaisTableDTO>>> GetAllAntecedentesPessoaisAsync(AntecedentesPessoaisAllFilter filter)
        {
            try
            {
                filter ??= new AntecedentesPessoaisAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                AntecedentesPessoaisSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<AntecedentesPessoaisTableDTO> list = await _repository.GetListAsync<AntecedentesPessoais, AntecedentesPessoaisTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<AntecedentesPessoaisTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<AntecedentesPessoaisTableDTO>>(ex.Message);
            }
        }


        // get single AntecedentesPessoais by Id 
        public async Task<Response<AntecedentesPessoaisDTO>> GetAntecedentesPessoaisAsync(Guid id)
        {
            try
            {
                AntecedentesPessoaisDTO dto = await _repository.GetByIdAsync<AntecedentesPessoais, AntecedentesPessoaisDTO, Guid>(id);
                return ResponseFactory.Success<AntecedentesPessoaisDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AntecedentesPessoaisDTO>(ex.Message);
            }
        }

        //get single AntecedentesPessoais by Nome Doenca 
        public async Task<Response<AntecedentesPessoaisDTO>> GetAntecedentesPessoaisByNomeDoencaAsync(string nomeDoenca)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(nomeDoenca))
                {
                    return ResponseFactory.Fail<AntecedentesPessoaisDTO>("Nome da Doença não pode ser vazio");
                }

                AntecedentesPessoaisMatchNameDoenca specification = new(nomeDoenca);
                IEnumerable<AntecedentesPessoaisDTO> results = await _repository.GetListAsync<AntecedentesPessoais, AntecedentesPessoaisDTO, Guid>(specification);

                AntecedentesPessoaisDTO? antecedentesPessoais = results.FirstOrDefault();
                if(antecedentesPessoais == null)
                {
                    return ResponseFactory.Fail<AntecedentesPessoaisDTO>("Não foi encontrado nenhum Antecedente Pessoal com o Nome da Doença fornecido");
                }

                return ResponseFactory.Success<AntecedentesPessoaisDTO>(antecedentesPessoais);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<AntecedentesPessoaisDTO>(ex.Message);
            }
        }

        // create new AntecedentesPessoais
        public async Task<Response<Guid>> CreateAntecedentesPessoaisAsync(CreateAntecedentesPessoaisRequest request)
        {
            AntecedentesPessoais newAntecedentesPessoais = _mapper.Map(request, new AntecedentesPessoais()); // map dto to domain entity

            try
            {
                AntecedentesPessoais response = await _repository.CreateAsync<AntecedentesPessoais, Guid>(newAntecedentesPessoais); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AntecedentesPessoais
        public async Task<Response<Guid>> UpdateAntecedentesPessoaisAsync(UpdateAntecedentesPessoaisRequest request, Guid id)
        {
            AntecedentesPessoais AntecedentesPessoaisInDb = await _repository.GetByIdAsync<AntecedentesPessoais, Guid>(id); // get existing entity
            if (AntecedentesPessoaisInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            AntecedentesPessoais updatedAntecedentesPessoais = _mapper.Map(request, AntecedentesPessoaisInDb); // map dto to domain entity

            try
            {
                AntecedentesPessoais response = await _repository.UpdateAsync<AntecedentesPessoais, Guid>(updatedAntecedentesPessoais);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AntecedentesPessoais
        public async Task<Response<Guid>> DeleteAntecedentesPessoaisAsync(Guid id)
        {
            try
            {
                AntecedentesPessoais? AntecedentesPessoais = await _repository.RemoveByIdAsync<AntecedentesPessoais, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(AntecedentesPessoais.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple AntecedentesPessoais 
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAntecedentesPessoaisAsync(IEnumerable<Guid> ids)
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
                        AntecedentesPessoais? deletedEntity = await _repository.RemoveByIdAsync<AntecedentesPessoais, Guid>(id);
                        if(deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else 
                        {
                            failedDeletions.Add($"AntecedentesPessoais com ID {id} não encontrado.");
                        }
                    }
                    catch(Exception)
                    {
                        failedDeletions.Add($"Erro ao eliminar Antecedentes Pessoais com ID {id}.");
                    }
                }

                if(successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if(successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} Antecedentes Pessoais.");
                }
                else 
                {
                    return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
                }
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

