using System.Linq;
using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.HistoriaClinica;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.Filters;
using CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.Specifications;
using CliCloud.Domain.Entities.Medicos;

// After creating this service:
// -- 1. Create a HistoriaClinica domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<HistoriaClinica> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a HistoriaClinica api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService
{
    public class HistoriaClinicaService : IHistoriaClinicaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public HistoriaClinicaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<HistoriaClinicaDTO>>> GetHistoriaClinicaAsync(string keyword = "")
        {
            HistoriaClinicaSearchList specification = new(keyword); // ardalis specification
            IEnumerable<HistoriaClinicaDTO> list = await _repository.GetListAsync<HistoriaClinica, HistoriaClinicaDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<HistoriaClinicaDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<HistoriaClinicaLightDTO>>> GetHistoriaClinicaLightAsync(string keyword = "")
        {
            HistoriaClinicaSearchList specification = new(keyword);
            IEnumerable<HistoriaClinicaLightDTO> list = await _repository.GetListAsync<HistoriaClinica, HistoriaClinicaLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HistoriaClinicaLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<HistoriaClinicaTableDTO>> GetHistoriaClinicaPaginatedAsync(HistoriaClinicaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            HistoriaClinicaSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<HistoriaClinicaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<HistoriaClinica, HistoriaClinicaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all HistoriaClinica (non-paginated)
        public async Task<Response<IEnumerable<HistoriaClinicaTableDTO>>> GetAllHistoriaClinicaAsync(HistoriaClinicaAllFilter filter)
        {
            try
            {
                filter ??= new HistoriaClinicaAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                HistoriaClinicaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<HistoriaClinicaTableDTO> list = await _repository.GetListAsync<HistoriaClinica, HistoriaClinicaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<HistoriaClinicaTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<HistoriaClinicaTableDTO>>(ex.Message);
            }
        }


        // get single HistoriaClinica by Id 
        public async Task<Response<HistoriaClinicaDTO>> GetHistoriaClinicaAsync(Guid id)
        {
            try
            {
                HistoriaClinicaDTO dto = await _repository.GetByIdAsync<HistoriaClinica, HistoriaClinicaDTO, Guid>(id);
                return ResponseFactory.Success<HistoriaClinicaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<HistoriaClinicaDTO>(ex.Message);
            }
        }

        // create new HistoriaClinica
        public async Task<Response<Guid>> CreateHistoriaClinicaAsync(CreateHistoriaClinicaRequest request)
        {
            HistoriaClinica newHistoriaClinica = _mapper.Map(request, new HistoriaClinica()); // map dto to domain entity

            // Se a especialidade não vier preenchida, usar a especialidade do médico associado
            if (newHistoriaClinica.EspecialidadeId == null && request.MedicoId != Guid.Empty)
            {
                try
                {
                    Medico? medico = await _repository.GetByIdAsync<Medico, Guid>(request.MedicoId);
                    if (medico != null)
                    {
                        newHistoriaClinica.EspecialidadeId = medico.EspecialidadeId;
                    }
                }
                catch
                {
                    // Em caso de erro ao obter o médico, continua sem especialidade explícita
                }
            }

            try
            {
                HistoriaClinica response = await _repository.CreateAsync<HistoriaClinica, Guid>(newHistoriaClinica); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update HistoriaClinica
        public async Task<Response<Guid>> UpdateHistoriaClinicaAsync(UpdateHistoriaClinicaRequest request, Guid id)
        {
            HistoriaClinica historiaClinicaInDb = await _repository.GetByIdAsync<HistoriaClinica, Guid>(id); // get existing entity
            if (historiaClinicaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("História clínica não encontrada.");
            }

            // Regra: só é possível alterar a ÚLTIMA entrada da história clínica
            // e apenas até 24h após a sua criação.
            UltimaHistoriaClinicaPorUtenteSpec ultimaSpec = new(historiaClinicaInDb.UtenteId);
            IEnumerable<HistoriaClinica> historicoUtente =
                await _repository.GetListAsync<HistoriaClinica, Guid>(ultimaSpec);

            HistoriaClinica? ultimaEntrada = historicoUtente.FirstOrDefault();
            if (ultimaEntrada == null || ultimaEntrada.Id != historiaClinicaInDb.Id)
            {
                return ResponseFactory.Fail<Guid>(
                    "Apenas é possível alterar a última entrada da história clínica do utente."
                );
            }

            DateTime limiteEdicao = historiaClinicaInDb.CreatedOn.AddHours(24);
            if (DateTime.UtcNow > limiteEdicao)
            {
                return ResponseFactory.Fail<Guid>(
                    "O prazo de 24 horas para editar esta entrada de história clínica já expirou."
                );
            }

            HistoriaClinica updatedHistoriaClinica = _mapper.Map(request, historiaClinicaInDb); // map dto to domain entity

            try
            {
                HistoriaClinica response = await _repository.UpdateAsync<HistoriaClinica, Guid>(updatedHistoriaClinica);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete HistoriaClinica
        public async Task<Response<Guid>> DeleteHistoriaClinicaAsync(Guid id)
        {
            try
            {
                HistoriaClinica? historiaClinicaInDb = await _repository.GetByIdAsync<HistoriaClinica, Guid>(id);
                if (historiaClinicaInDb == null)
                {
                    return ResponseFactory.Fail<Guid>("História clínica não encontrada.");
                }

                // Mesmo critério de fecho de registo: só permite apagar a última entrada dentro de 24h
                UltimaHistoriaClinicaPorUtenteSpec ultimaSpec = new(historiaClinicaInDb.UtenteId);
                IEnumerable<HistoriaClinica> historicoUtente =
                    await _repository.GetListAsync<HistoriaClinica, Guid>(ultimaSpec);

                HistoriaClinica? ultimaEntrada = historicoUtente.FirstOrDefault();
                if (ultimaEntrada == null || ultimaEntrada.Id != historiaClinicaInDb.Id)
                {
                    return ResponseFactory.Fail<Guid>(
                        "Apenas é possível eliminar a última entrada da história clínica do utente."
                    );
                }

                DateTime limiteEdicao = historiaClinicaInDb.CreatedOn.AddHours(24);
                if (DateTime.UtcNow > limiteEdicao)
                {
                    return ResponseFactory.Fail<Guid>(
                        "O prazo de 24 horas para eliminar esta entrada de história clínica já expirou."
                    );
                }

                _ = await _repository.RemoveByIdAsync<HistoriaClinica, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(historiaClinicaInDb.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple HistoriaClinica
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleHistoriaClinicaAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = new();
                List<string> failedDeletions = new();

                foreach(Guid id in idsList)
                {
                    try
                    {
                        HistoriaClinica? entity = await _repository.GetByIdAsync<HistoriaClinica, Guid>(id);
                        if(entity == null)
                        {
                            failedDeletions.Add($"História clínica com ID {id} não encontrada.");
                            continue;
                        }

                        HistoriaClinica? deletedEntity = await _repository.RemoveByIdAsync<HistoriaClinica, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar história clínica com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Erro ao eliminar história clínica com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    string message =
                        $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} registos de história clínica.";
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, message);
                }
                else
                {
                    return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

