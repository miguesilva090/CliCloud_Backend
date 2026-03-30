using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using HorarioTecnicoEntity = CliCloud.Domain.Entities.Tecnicos.HorarioTecnico;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.Filters;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.Specifications;

// After creating this service:
// -- 1. Create a HorarioTecnico domain entity in CliCloud.Domain/Entities/Tecnicos
// -- 2. Add DbSet<HorarioTecnico> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a HorarioTecnico api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoService
{
    public class HorarioTecnicoService : IHorarioTecnicoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public HorarioTecnicoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<HorarioTecnicoDTO>>> GetHorarioTecnicoAsync(string keyword = "")
        {
            HorarioTecnicoSearchList specification = new(keyword);
            IEnumerable<HorarioTecnicoDTO> list = await _repository.GetListAsync<HorarioTecnicoEntity, HorarioTecnicoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HorarioTecnicoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<HorarioTecnicoLightDTO>>> GetHorarioTecnicoLightAsync(string keyword = "")
        {
            HorarioTecnicoSearchList specification = new(keyword);
            IEnumerable<HorarioTecnicoLightDTO> list = await _repository.GetListAsync<HorarioTecnicoEntity, HorarioTecnicoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HorarioTecnicoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<HorarioTecnicoTableDTO>> GetHorarioTecnicoPaginatedAsync(HorarioTecnicoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            HorarioTecnicoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<HorarioTecnicoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<HorarioTecnicoEntity, HorarioTecnicoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all HorarioTecnicos (non-paginated)
        public async Task<Response<IEnumerable<HorarioTecnicoTableDTO>>> GetAllHorarioTecnicoAsync(HorarioTecnicoAllFilter filter)
        {
            try
            {
                filter ??= new HorarioTecnicoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                HorarioTecnicoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<HorarioTecnicoTableDTO> list = await _repository.GetListAsync<HorarioTecnicoEntity, HorarioTecnicoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<HorarioTecnicoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<HorarioTecnicoTableDTO>>(ex.Message);
            }
        }

        // get single HorarioTecnico by Id 
        public async Task<Response<HorarioTecnicoDTO>> GetHorarioTecnicoAsync(Guid id)
        {
            try
            {
                HorarioTecnicoDTO dto = await _repository.GetByIdAsync<HorarioTecnicoEntity, HorarioTecnicoDTO, Guid>(id);
                return ResponseFactory.Success<HorarioTecnicoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<HorarioTecnicoDTO>(ex.Message);
            }
        }

        // get multiple HorarioTecnicos by TecnicoId 
        public async Task<Response<IEnumerable<HorarioTecnicoDTO>>> GetHorarioTecnicoByTecnicoIdAsync(Guid tecnicoId)
        {
          try
          {
            HorarioTecnicoSearchByTecnicoId specification = new(tecnicoId);
            IEnumerable<HorarioTecnicoDTO> results = await _repository.GetListAsync<HorarioTecnicoEntity, HorarioTecnicoDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<HorarioTecnicoDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<HorarioTecnicoDTO>>(ex.Message);
          }
        }

        // create new HorarioTecnico
        public async Task<Response<Guid>> CreateHorarioTecnicoAsync(CreateHorarioTecnicoRequest request)
        {
            if(!Guid.TryParse(request.TecnicoId, out Guid tecnicoGuid))
            {
                return ResponseFactory.Fail<Guid>("TecnicoId inválido");
            }

            HorarioTecnicoEntity newHorarioTecnico = _mapper.Map(request, new HorarioTecnicoEntity());
            newHorarioTecnico.TecnicoId = tecnicoGuid;

            try
            {
                HorarioTecnicoEntity response = await _repository.CreateAsync<HorarioTecnicoEntity, Guid>(newHorarioTecnico);
                _ = await _repository.SaveChangesAsync();

                // TODO: Se HorarioTecnicoDiaService existir, criar os horários aqui
                // if(request.Horarios != null && request.Horarios.Any())
                // {
                //   var horariosResult = await _horarioTecnicoDiaService.CreateHorarioTecnicoDiaBulkAsync(...);
                // }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update HorarioTecnico
        public async Task<Response<Guid>> UpdateHorarioTecnicoAsync(UpdateHorarioTecnicoRequest request, Guid id)
        {
            HorarioTecnicoEntity HorarioTecnicoInDb = await _repository.GetByIdAsync<HorarioTecnicoEntity, Guid>(id);
            if (HorarioTecnicoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("HorarioTecnico não encontrado");
            }

            if(!Guid.TryParse(request.TecnicoId, out Guid tecnicoGuid))
            {
                return ResponseFactory.Fail<Guid>("TecnicoId inválido");
            }

            HorarioTecnicoEntity updatedHorarioTecnico = _mapper.Map(request, HorarioTecnicoInDb);
            updatedHorarioTecnico.TecnicoId = tecnicoGuid;

            try
            {
                HorarioTecnicoEntity response = await _repository.UpdateAsync<HorarioTecnicoEntity, Guid>(updatedHorarioTecnico);
                _ = await _repository.SaveChangesAsync();

                // TODO: Se HorarioTecnicoDiaService existir, atualizar os horários aqui
                // if(request.Horarios != null)
                // {
                //   var horariosResult = await _horarioTecnicoDiaService.UpsertHorarioTecnicoDiaBulkAsync(...);
                // }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete HorarioTecnico
        public async Task<Response<Guid>> DeleteHorarioTecnicoAsync(Guid id)
        {
            try
            {
                // TODO: Se HorarioTecnicoDiaService existir, deletar os horários primeiro
                // var horarios = await _repository.GetListAsync<HorarioTecnicoDia, Guid>(...);
                // foreach(var horario in horarios) { await _horarioTecnicoDiaService.DeleteHorarioTecnicoDiaAsync(...); }

                HorarioTecnicoEntity? HorarioTecnico = await _repository.RemoveByIdAsync<HorarioTecnicoEntity, Guid>(id);
                if (HorarioTecnico == null)
                {
                    return ResponseFactory.Fail<Guid>("HorarioTecnico não encontrado");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(HorarioTecnico.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple HorarioTecnicos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleHorarioTecnicoAsync(IEnumerable<Guid> ids)
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
                HorarioTecnicoEntity? entity = await _repository.GetByIdAsync<HorarioTecnicoEntity, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"HorarioTecnico com ID {id} não encontrado.");
                  continue;
                }

                // TODO: Se HorarioTecnicoDiaService existir, deletar os horários primeiro

                HorarioTecnicoEntity? deletedEntity = await _repository.RemoveByIdAsync<HorarioTecnicoEntity, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"HorarioTecnico com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"HorarioTecnico com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} horários de técnico.";
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
