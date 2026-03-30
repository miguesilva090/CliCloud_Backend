using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using HorarioMedicoEntity = CliCloud.Domain.Entities.Medicos.HorarioMedico;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.DTOs;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.Filters;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.Specifications;

// After creating this service:
// -- 1. Create a HorarioMedico domain entity in CliCloud.Domain/Entities/Medicos
// -- 2. Add DbSet<HorarioMedico> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a HorarioMedico api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Medicos.HorarioMedicoService
{
    public class HorarioMedicoService : IHorarioMedicoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public HorarioMedicoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<HorarioMedicoDTO>>> GetHorarioMedicoAsync(string keyword = "")
        {
            HorarioMedicoSearchList specification = new(keyword);
            IEnumerable<HorarioMedicoDTO> list = await _repository.GetListAsync<HorarioMedicoEntity, HorarioMedicoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HorarioMedicoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<HorarioMedicoLightDTO>>> GetHorarioMedicoLightAsync(string keyword = "")
        {
            HorarioMedicoSearchList specification = new(keyword);
            IEnumerable<HorarioMedicoLightDTO> list = await _repository.GetListAsync<HorarioMedicoEntity, HorarioMedicoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HorarioMedicoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<HorarioMedicoTableDTO>> GetHorarioMedicoPaginatedAsync(HorarioMedicoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            HorarioMedicoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<HorarioMedicoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<HorarioMedicoEntity, HorarioMedicoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all HorarioMedicos (non-paginated)
        public async Task<Response<IEnumerable<HorarioMedicoTableDTO>>> GetAllHorarioMedicoAsync(HorarioMedicoAllFilter filter)
        {
            try
            {
                filter ??= new HorarioMedicoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                HorarioMedicoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<HorarioMedicoTableDTO> list = await _repository.GetListAsync<HorarioMedicoEntity, HorarioMedicoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<HorarioMedicoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<HorarioMedicoTableDTO>>(ex.Message);
            }
        }

        // get single HorarioMedico by Id 
        public async Task<Response<HorarioMedicoDTO>> GetHorarioMedicoAsync(Guid id)
        {
            try
            {
                HorarioMedicoDTO dto = await _repository.GetByIdAsync<HorarioMedicoEntity, HorarioMedicoDTO, Guid>(id);
                return ResponseFactory.Success<HorarioMedicoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<HorarioMedicoDTO>(ex.Message);
            }
        }

        // get multiple HorarioMedicos by MedicoId 
        public async Task<Response<IEnumerable<HorarioMedicoDTO>>> GetHorarioMedicoByMedicoIdAsync(Guid medicoId)
        {
          try
          {
            HorarioMedicoSearchByMedicoId specification = new(medicoId);
            IEnumerable<HorarioMedicoDTO> results = await _repository.GetListAsync<HorarioMedicoEntity, HorarioMedicoDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<HorarioMedicoDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<HorarioMedicoDTO>>(ex.Message);
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

        // create new HorarioMedico (upsert: se já existir para o MedicoId, atualiza)
        public async Task<Response<Guid>> CreateHorarioMedicoAsync(CreateHorarioMedicoRequest request)
        {
            if(!Guid.TryParse(request.MedicoId, out Guid medicoGuid))
            {
                return ResponseFactory.Fail<Guid>("MedicoId inválido");
            }

            try
            {
                // Verificar se já existe HorarioMedico para este médico (índice único MedicoId)
                HorarioMedicoSearchByMedicoId spec = new(medicoGuid);
                IEnumerable<HorarioMedicoEntity> existentes = await _repository.GetListAsync<HorarioMedicoEntity, Guid>(spec);
                HorarioMedicoEntity? existente = existentes.FirstOrDefault();

                if (existente != null)
                {
                    // Já existe: atualizar a entidade já carregada (evita GetByIdAsync que pode falhar)
                    _mapper.Map(request, existente);
                    existente.MedicoId = medicoGuid;
                    existente.MinMarcacao = ParseTimeSpan(request.MinMarcacao);
                    existente.PrimeiraConsulta = ParseTimeSpan(request.PrimeiraConsulta);

                    HorarioMedicoEntity updated = await _repository.UpdateAsync<HorarioMedicoEntity, Guid>(existente);
                    _ = await _repository.SaveChangesAsync();
                    return ResponseFactory.Success<Guid>(updated.Id);
                }

                HorarioMedicoEntity newHorarioMedico = _mapper.Map(request, new HorarioMedicoEntity());
                newHorarioMedico.MedicoId = medicoGuid;
                newHorarioMedico.MinMarcacao = ParseTimeSpan(request.MinMarcacao);
                newHorarioMedico.PrimeiraConsulta = ParseTimeSpan(request.PrimeiraConsulta);

                HorarioMedicoEntity response = await _repository.CreateAsync<HorarioMedicoEntity, Guid>(newHorarioMedico);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update HorarioMedico
        public async Task<Response<Guid>> UpdateHorarioMedicoAsync(UpdateHorarioMedicoRequest request, Guid id)
        {
            HorarioMedicoEntity HorarioMedicoInDb = await _repository.GetByIdAsync<HorarioMedicoEntity, Guid>(id);
            if (HorarioMedicoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("HorarioMedico não encontrado");
            }

            if(!Guid.TryParse(request.MedicoId, out Guid medicoGuid))
            {
                return ResponseFactory.Fail<Guid>("MedicoId inválido");
            }

            HorarioMedicoEntity updatedHorarioMedico = _mapper.Map(request, HorarioMedicoInDb);
            updatedHorarioMedico.MedicoId = medicoGuid;
            updatedHorarioMedico.MinMarcacao = ParseTimeSpan(request.MinMarcacao);
            updatedHorarioMedico.PrimeiraConsulta = ParseTimeSpan(request.PrimeiraConsulta);

            try
            {
                HorarioMedicoEntity response = await _repository.UpdateAsync<HorarioMedicoEntity, Guid>(updatedHorarioMedico);
                _ = await _repository.SaveChangesAsync();

                // TODO: Se HorarioMedicoDiaService existir, atualizar os horários aqui
                // if(request.Horarios != null)
                // {
                //   var horariosResult = await _horarioMedicoDiaService.UpsertHorarioMedicoDiaBulkAsync(...);
                // }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete HorarioMedico
        public async Task<Response<Guid>> DeleteHorarioMedicoAsync(Guid id)
        {
            try
            {
                // TODO: Se HorarioMedicoDiaService existir, deletar os horários primeiro
                // var horarios = await _repository.GetListAsync<HorarioMedicoDia, Guid>(...);
                // foreach(var horario in horarios) { await _horarioMedicoDiaService.DeleteHorarioMedicoDiaAsync(...); }

                HorarioMedicoEntity? HorarioMedico = await _repository.RemoveByIdAsync<HorarioMedicoEntity, Guid>(id);
                if (HorarioMedico == null)
                {
                    return ResponseFactory.Fail<Guid>("HorarioMedico não encontrado");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(HorarioMedico.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple HorarioMedicos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleHorarioMedicoAsync(IEnumerable<Guid> ids)
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
                HorarioMedicoEntity? entity = await _repository.GetByIdAsync<HorarioMedicoEntity, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"HorarioMedico com ID {id} não encontrado.");
                  continue;
                }

                // TODO: Se HorarioMedicoDiaService existir, deletar os horários primeiro

                HorarioMedicoEntity? deletedEntity = await _repository.RemoveByIdAsync<HorarioMedicoEntity, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"HorarioMedico com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"HorarioMedico com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} horários de médico.";
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
