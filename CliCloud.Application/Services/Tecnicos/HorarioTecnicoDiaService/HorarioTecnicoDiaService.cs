using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using HorarioTecnicoDiaEntity = CliCloud.Domain.Entities.Tecnicos.HorarioTecnicoDia;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.DTOs;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.Filters;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.Specifications;

// After creating this service:
// -- 1. Create a HorarioTecnicoDia domain entity in CliCloud.Domain/Entities/Tecnicos
// -- 2. Add DbSet<HorarioTecnicoDia> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a HorarioTecnicoDia api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService
{
    public class HorarioTecnicoDiaService : IHorarioTecnicoDiaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public HorarioTecnicoDiaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<HorarioTecnicoDiaDTO>>> GetHorarioTecnicoDiaAsync(string keyword = "")
        {
            HorarioTecnicoDiaSearchList specification = new(keyword);
            IEnumerable<HorarioTecnicoDiaDTO> list = await _repository.GetListAsync<HorarioTecnicoDiaEntity, HorarioTecnicoDiaDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HorarioTecnicoDiaDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<HorarioTecnicoDiaLightDTO>>> GetHorarioTecnicoDiaLightAsync(string keyword = "")
        {
            HorarioTecnicoDiaSearchList specification = new(keyword);
            IEnumerable<HorarioTecnicoDiaLightDTO> list = await _repository.GetListAsync<HorarioTecnicoDiaEntity, HorarioTecnicoDiaLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HorarioTecnicoDiaLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<HorarioTecnicoDiaTableDTO>> GetHorarioTecnicoDiaPaginatedAsync(HorarioTecnicoDiaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            HorarioTecnicoDiaSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<HorarioTecnicoDiaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<HorarioTecnicoDiaEntity, HorarioTecnicoDiaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all HorarioTecnicoDias (non-paginated)
        public async Task<Response<IEnumerable<HorarioTecnicoDiaTableDTO>>> GetAllHorarioTecnicoDiaAsync(HorarioTecnicoDiaAllFilter filter)
        {
            try
            {
                filter ??= new HorarioTecnicoDiaAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                HorarioTecnicoDiaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<HorarioTecnicoDiaTableDTO> list = await _repository.GetListAsync<HorarioTecnicoDiaEntity, HorarioTecnicoDiaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<HorarioTecnicoDiaTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<HorarioTecnicoDiaTableDTO>>(ex.Message);
            }
        }

        // get single HorarioTecnicoDia by Id 
        public async Task<Response<HorarioTecnicoDiaDTO>> GetHorarioTecnicoDiaAsync(Guid id)
        {
            try
            {
                HorarioTecnicoDiaDTO dto = await _repository.GetByIdAsync<HorarioTecnicoDiaEntity, HorarioTecnicoDiaDTO, Guid>(id);
                return ResponseFactory.Success<HorarioTecnicoDiaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<HorarioTecnicoDiaDTO>(ex.Message);
            }
        }

        // get multiple HorarioTecnicoDias by HorarioTecnicoId 
        public async Task<Response<IEnumerable<HorarioTecnicoDiaDTO>>> GetHorarioTecnicoDiaByHorarioTecnicoIdAsync(Guid horarioTecnicoId)
        {
          try
          {
            HorarioTecnicoDiaSearchByHorarioTecnicoId specification = new(horarioTecnicoId);
            IEnumerable<HorarioTecnicoDiaDTO> results = await _repository.GetListAsync<HorarioTecnicoDiaEntity, HorarioTecnicoDiaDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<HorarioTecnicoDiaDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<HorarioTecnicoDiaDTO>>(ex.Message);
          }
        }

        // create new HorarioTecnicoDia
        public async Task<Response<Guid>> CreateHorarioTecnicoDiaAsync(CreateHorarioTecnicoDiaRequest request)
        {
            if(!Guid.TryParse(request.HorarioTecnicoId, out Guid horarioTecnicoGuid))
            {
                return ResponseFactory.Fail<Guid>("HorarioTecnicoId inválido");
            }

            HorarioTecnicoDiaEntity newHorarioTecnicoDia = _mapper.Map(request, new HorarioTecnicoDiaEntity());
            newHorarioTecnicoDia.HorarioTecnicoId = horarioTecnicoGuid;

            try
            {
                HorarioTecnicoDiaEntity response = await _repository.CreateAsync<HorarioTecnicoDiaEntity, Guid>(newHorarioTecnicoDia);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update HorarioTecnicoDia
        public async Task<Response<Guid>> UpdateHorarioTecnicoDiaAsync(UpdateHorarioTecnicoDiaRequest request, Guid id)
        {
            HorarioTecnicoDiaEntity HorarioTecnicoDiaInDb = await _repository.GetByIdAsync<HorarioTecnicoDiaEntity, Guid>(id);
            if (HorarioTecnicoDiaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("HorarioTecnicoDia não encontrado");
            }

            if(!Guid.TryParse(request.HorarioTecnicoId, out Guid horarioTecnicoGuid))
            {
                return ResponseFactory.Fail<Guid>("HorarioTecnicoId inválido");
            }

            HorarioTecnicoDiaEntity updatedHorarioTecnicoDia = _mapper.Map(request, HorarioTecnicoDiaInDb);
            updatedHorarioTecnicoDia.HorarioTecnicoId = horarioTecnicoGuid;

            try
            {
                HorarioTecnicoDiaEntity response = await _repository.UpdateAsync<HorarioTecnicoDiaEntity, Guid>(updatedHorarioTecnicoDia);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete HorarioTecnicoDia
        public async Task<Response<Guid>> DeleteHorarioTecnicoDiaAsync(Guid id)
        {
            try
            {
                HorarioTecnicoDiaEntity? HorarioTecnicoDia = await _repository.RemoveByIdAsync<HorarioTecnicoDiaEntity, Guid>(id);
                if (HorarioTecnicoDia == null)
                {
                    return ResponseFactory.Fail<Guid>("HorarioTecnicoDia não encontrado");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(HorarioTecnicoDia.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple HorarioTecnicoDias
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleHorarioTecnicoDiaAsync(IEnumerable<Guid> ids)
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
                HorarioTecnicoDiaEntity? entity = await _repository.GetByIdAsync<HorarioTecnicoDiaEntity, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"HorarioTecnicoDia com ID {id} não encontrado.");
                  continue;
                }

                HorarioTecnicoDiaEntity? deletedEntity = await _repository.RemoveByIdAsync<HorarioTecnicoDiaEntity, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"HorarioTecnicoDia com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"HorarioTecnicoDia com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} horários de técnico (dia).";
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
