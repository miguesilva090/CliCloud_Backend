using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using HorarioMedicoDiaEntity = CliCloud.Domain.Entities.Medicos.HorarioMedicoDia;
using CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs;
using CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.Filters;
using CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.Specifications;

// After creating this service:
// -- 1. Create a HorarioMedicoDia domain entity in CliCloud.Domain/Entities/Medicos
// -- 2. Add DbSet<HorarioMedicoDia> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a HorarioMedicoDia api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService
{
    public class HorarioMedicoDiaService : IHorarioMedicoDiaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public HorarioMedicoDiaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<HorarioMedicoDiaDTO>>> GetHorarioMedicoDiaAsync(string keyword = "")
        {
            HorarioMedicoDiaSearchList specification = new(keyword);
            IEnumerable<HorarioMedicoDiaDTO> list = await _repository.GetListAsync<HorarioMedicoDiaEntity, HorarioMedicoDiaDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HorarioMedicoDiaDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<HorarioMedicoDiaLightDTO>>> GetHorarioMedicoDiaLightAsync(string keyword = "")
        {
            HorarioMedicoDiaSearchList specification = new(keyword);
            IEnumerable<HorarioMedicoDiaLightDTO> list = await _repository.GetListAsync<HorarioMedicoDiaEntity, HorarioMedicoDiaLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HorarioMedicoDiaLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<HorarioMedicoDiaTableDTO>> GetHorarioMedicoDiaPaginatedAsync(HorarioMedicoDiaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            HorarioMedicoDiaSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<HorarioMedicoDiaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<HorarioMedicoDiaEntity, HorarioMedicoDiaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all HorarioMedicoDias (non-paginated)
        public async Task<Response<IEnumerable<HorarioMedicoDiaTableDTO>>> GetAllHorarioMedicoDiaAsync(HorarioMedicoDiaAllFilter filter)
        {
            try
            {
                filter ??= new HorarioMedicoDiaAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                HorarioMedicoDiaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<HorarioMedicoDiaTableDTO> list = await _repository.GetListAsync<HorarioMedicoDiaEntity, HorarioMedicoDiaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<HorarioMedicoDiaTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<HorarioMedicoDiaTableDTO>>(ex.Message);
            }
        }

        // get single HorarioMedicoDia by Id 
        public async Task<Response<HorarioMedicoDiaDTO>> GetHorarioMedicoDiaAsync(Guid id)
        {
            try
            {
                HorarioMedicoDiaDTO dto = await _repository.GetByIdAsync<HorarioMedicoDiaEntity, HorarioMedicoDiaDTO, Guid>(id);
                return ResponseFactory.Success<HorarioMedicoDiaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<HorarioMedicoDiaDTO>(ex.Message);
            }
        }

        // get multiple HorarioMedicoDias by HorarioMedicoId 
        public async Task<Response<IEnumerable<HorarioMedicoDiaDTO>>> GetHorarioMedicoDiaByHorarioMedicoIdAsync(Guid horarioMedicoId)
        {
          try
          {
            HorarioMedicoDiaSearchByHorarioMedicoId specification = new(horarioMedicoId);
            IEnumerable<HorarioMedicoDiaDTO> results = await _repository.GetListAsync<HorarioMedicoDiaEntity, HorarioMedicoDiaDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<HorarioMedicoDiaDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<HorarioMedicoDiaDTO>>(ex.Message);
          }
        }

        // Helper method to parse TimeSpan from string
        private static TimeSpan? ParseTimeSpan(string? timeString)
        {
            if(string.IsNullOrWhiteSpace(timeString))
            {
                return null;
            }

            if(TimeSpan.TryParse(timeString, out TimeSpan result))
            {
                return result;
            }

            return null;
        }

        // create new HorarioMedicoDia (upsert: se já existir para HorarioMedicoId+DiaSemana+Periodo, atualiza)
        public async Task<Response<Guid>> CreateHorarioMedicoDiaAsync(CreateHorarioMedicoDiaRequest request)
        {
            if(!Guid.TryParse(request.HorarioMedicoId, out Guid horarioMedicoGuid))
            {
                return ResponseFactory.Fail<Guid>("HorarioMedicoId inválido");
            }

            try
            {
                // Verificar se já existe HorarioMedicoDia para este HorarioMedicoId + DiaSemana + Periodo (índice único)
                HorarioMedicoDiaSearchByHorarioMedicoId spec = new(horarioMedicoGuid);
                IEnumerable<HorarioMedicoDiaEntity> existentes = await _repository.GetListAsync<HorarioMedicoDiaEntity, Guid>(spec);
                HorarioMedicoDiaEntity? existente = existentes.FirstOrDefault(x =>
                    x.DiaSemana == request.DiaSemana && x.Periodo == request.Periodo);

                if (existente != null)
                {
                    // Já existe: atualizar a entidade já carregada (evita GetByIdAsync que pode falhar)
                    _mapper.Map(request, existente);
                    existente.HorarioMedicoId = horarioMedicoGuid;
                    existente.Inicio = ParseTimeSpan(request.Inicio);
                    existente.Fim = ParseTimeSpan(request.Fim);

                    HorarioMedicoDiaEntity updated = await _repository.UpdateAsync<HorarioMedicoDiaEntity, Guid>(existente);
                    _ = await _repository.SaveChangesAsync();
                    return ResponseFactory.Success<Guid>(updated.Id);
                }

                HorarioMedicoDiaEntity newHorarioMedicoDia = _mapper.Map(request, new HorarioMedicoDiaEntity());
                newHorarioMedicoDia.HorarioMedicoId = horarioMedicoGuid;
                newHorarioMedicoDia.Inicio = ParseTimeSpan(request.Inicio);
                newHorarioMedicoDia.Fim = ParseTimeSpan(request.Fim);

                HorarioMedicoDiaEntity response = await _repository.CreateAsync<HorarioMedicoDiaEntity, Guid>(newHorarioMedicoDia);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update HorarioMedicoDia
        public async Task<Response<Guid>> UpdateHorarioMedicoDiaAsync(UpdateHorarioMedicoDiaRequest request, Guid id)
        {
            HorarioMedicoDiaEntity HorarioMedicoDiaInDb = await _repository.GetByIdAsync<HorarioMedicoDiaEntity, Guid>(id);
            if (HorarioMedicoDiaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("HorarioMedicoDia não encontrado");
            }

            if(!Guid.TryParse(request.HorarioMedicoId, out Guid horarioMedicoGuid))
            {
                return ResponseFactory.Fail<Guid>("HorarioMedicoId inválido");
            }

            HorarioMedicoDiaEntity updatedHorarioMedicoDia = _mapper.Map(request, HorarioMedicoDiaInDb);
            updatedHorarioMedicoDia.HorarioMedicoId = horarioMedicoGuid;
            updatedHorarioMedicoDia.Inicio = ParseTimeSpan(request.Inicio);
            updatedHorarioMedicoDia.Fim = ParseTimeSpan(request.Fim);

            try
            {
                HorarioMedicoDiaEntity response = await _repository.UpdateAsync<HorarioMedicoDiaEntity, Guid>(updatedHorarioMedicoDia);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete HorarioMedicoDia
        public async Task<Response<Guid>> DeleteHorarioMedicoDiaAsync(Guid id)
        {
            try
            {
                HorarioMedicoDiaEntity? HorarioMedicoDia = await _repository.RemoveByIdAsync<HorarioMedicoDiaEntity, Guid>(id);
                if (HorarioMedicoDia == null)
                {
                    return ResponseFactory.Fail<Guid>("HorarioMedicoDia não encontrado");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(HorarioMedicoDia.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple HorarioMedicoDias
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleHorarioMedicoDiaAsync(IEnumerable<Guid> ids)
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
                HorarioMedicoDiaEntity? entity = await _repository.GetByIdAsync<HorarioMedicoDiaEntity, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"HorarioMedicoDia com ID {id} não encontrado.");
                  continue;
                }

                HorarioMedicoDiaEntity? deletedEntity = await _repository.RemoveByIdAsync<HorarioMedicoDiaEntity, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"HorarioMedicoDia com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"HorarioMedicoDia com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} horários de médico (dia).";
              return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, message);
            }
            else
            {
              return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ",failedDeletions));
            }
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
          }
        }
    }
}
