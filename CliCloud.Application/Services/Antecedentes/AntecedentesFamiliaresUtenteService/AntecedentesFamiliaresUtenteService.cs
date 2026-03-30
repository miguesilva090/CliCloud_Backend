using AutoMapper;
using CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Antecedentes;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.Filters;
using CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.Specifications;

// After creating this service:
// -- 1. Create a AntecedentesFamiliaresUtente domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AntecedentesFamiliaresUtente> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AntecedentesFamiliaresUtente api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService
{
    public class AntecedentesFamiliaresUtenteService : IAntecedentesFamiliaresUtenteService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AntecedentesFamiliaresUtenteService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<AntecedentesFamiliaresUtenteDTO>>> GetAntecedentesFamiliaresUtenteAsync(string keyword = "")
        {
            AntecedentesFamiliaresUtenteSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AntecedentesFamiliaresUtenteDTO> list = await _repository.GetListAsync<AntecedentesFamiliaresUtente, AntecedentesFamiliaresUtenteDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<AntecedentesFamiliaresUtenteDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<AntecedentesFamiliaresUtenteLightDTO>>> GetAntecedentesFamiliaresUtenteLightAsync(string keyword = "")
        {
            AntecedentesFamiliaresUtenteSearchList specification = new(keyword);
            IEnumerable<AntecedentesFamiliaresUtenteLightDTO> list = await _repository.GetListAsync<AntecedentesFamiliaresUtente, AntecedentesFamiliaresUtenteLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<AntecedentesFamiliaresUtenteLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AntecedentesFamiliaresUtenteTableDTO>> GetAntecedentesFamiliaresUtentePaginatedAsync(AntecedentesFamiliaresUtenteTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
            AntecedentesFamiliaresUtenteSearchTable specification = new(tableFilters, dynamicOrder); // ardalis specification
            PaginatedResponse<AntecedentesFamiliaresUtenteTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AntecedentesFamiliaresUtente, AntecedentesFamiliaresUtenteTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all AntecedentesFamiliaresUtente (non-paginated)
        public async Task<Response<IEnumerable<AntecedentesFamiliaresUtenteTableDTO>>> GetAllAntecedentesFamiliaresUtenteAsync(AntecedentesFamiliaresUtenteAllFilter filter)
        {
            try
            {
                filter ??= new AntecedentesFamiliaresUtenteAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                AntecedentesFamiliaresUtenteSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<AntecedentesFamiliaresUtenteTableDTO> list = await _repository.GetListAsync<AntecedentesFamiliaresUtente, AntecedentesFamiliaresUtenteTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<AntecedentesFamiliaresUtenteTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<AntecedentesFamiliaresUtenteTableDTO>>(ex.Message);
            }
        }


        // get single AntecedentesFamiliaresUtente by Id 
        public async Task<Response<AntecedentesFamiliaresUtenteDTO>> GetAntecedentesFamiliaresUtenteAsync(Guid id)
        {
            try
            {
                AntecedentesFamiliaresUtenteDTO dto = await _repository.GetByIdAsync<AntecedentesFamiliaresUtente, AntecedentesFamiliaresUtenteDTO, Guid>(id);
                return ResponseFactory.Success<AntecedentesFamiliaresUtenteDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AntecedentesFamiliaresUtenteDTO>(ex.Message);
            }
        }

        // get single AntecedentesFamiliaresUtente by NomeDoenca 
        public async Task<Response<AntecedentesFamiliaresUtenteDTO>> GetAntecedentesFamiliaresUtenteByNomeDoencaAsync(string nomeDoenca)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(nomeDoenca))
                {
                    return ResponseFactory.Fail<AntecedentesFamiliaresUtenteDTO>("Nome da Doença não pode ser vazio");
                }

                AntecedentesFamiliaresUtenteMatchNameDoenca specification = new(nomeDoenca);
                IEnumerable<AntecedentesFamiliaresUtenteDTO> result = await _repository.GetListAsync<AntecedentesFamiliaresUtente, AntecedentesFamiliaresUtenteDTO, Guid>(specification);
                AntecedentesFamiliaresUtenteDTO? antecedentesFamiliaresUtente = result.FirstOrDefault();

                if(antecedentesFamiliaresUtente == null)
                {
                    return ResponseFactory.Fail<AntecedentesFamiliaresUtenteDTO>("Não foi encontrado nenhum Antecedente Familiar com o Nome da Doença fornecido");
                }

                return ResponseFactory.Success<AntecedentesFamiliaresUtenteDTO>(antecedentesFamiliaresUtente);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<AntecedentesFamiliaresUtenteDTO>(ex.Message);
            }
        }

        // create new AntecedentesFamiliaresUtente
        public async Task<Response<Guid>> CreateAntecedentesFamiliaresUtenteAsync(CreateAntecedentesFamiliaresUtenteRequest request)
        {
            AntecedentesFamiliaresUtente newAntecedentesFamiliaresUtente = _mapper.Map(request, new AntecedentesFamiliaresUtente()); // map dto to domain entity

            try
            {
                AntecedentesFamiliaresUtente response = await _repository.CreateAsync<AntecedentesFamiliaresUtente, Guid>(newAntecedentesFamiliaresUtente); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AntecedentesFamiliaresUtente
        public async Task<Response<Guid>> UpdateAntecedentesFamiliaresUtenteAsync(UpdateAntecedentesFamiliaresUtenteRequest request, Guid id)
        {
            AntecedentesFamiliaresUtente AntecedentesFamiliaresUtenteInDb = await _repository.GetByIdAsync<AntecedentesFamiliaresUtente, Guid>(id); // get existing entity
            if (AntecedentesFamiliaresUtenteInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            AntecedentesFamiliaresUtente updatedAntecedentesFamiliaresUtente = _mapper.Map(request, AntecedentesFamiliaresUtenteInDb); // map dto to domain entity

            try
            {
                AntecedentesFamiliaresUtente response = await _repository.UpdateAsync<AntecedentesFamiliaresUtente, Guid>(updatedAntecedentesFamiliaresUtente);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AntecedentesFamiliaresUtente
        public async Task<Response<Guid>> DeleteAntecedentesFamiliaresUtenteAsync(Guid id)
        {
            try
            {
                AntecedentesFamiliaresUtente? AntecedentesFamiliaresUtente = await _repository.RemoveByIdAsync<AntecedentesFamiliaresUtente, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(AntecedentesFamiliaresUtente.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple AntecedentesFamiliaresUtente
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAntecedentesFamiliaresUtenteAsync(IEnumerable<Guid> ids)
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
                        AntecedentesFamiliaresUtente? deletedEntity = await _repository.RemoveByIdAsync<AntecedentesFamiliaresUtente, Guid>(id);
                        if(deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"AntecedentesFamiliaresUtente com ID {id} não encontrado.");
                        }
                    }
                    catch(Exception)
                    {
                        failedDeletions.Add($"Erro ao eliminar Antecedentes Familiares com ID {id}.");
                    }

                }

                if(successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if(successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} Antecedentes Familiares.");
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

