using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using EspecialidadeEntity = CliCloud.Domain.Entities.Especialidades.Especialidade;
using CliCloud.Application.Services.Especialidades.EspecialidadeService.DTOs;
using CliCloud.Application.Services.Especialidades.EspecialidadeService.Filters;
using CliCloud.Application.Services.Especialidades.EspecialidadeService.Specifications;

// After creating this service:
// -- 1. Create a Especialidade domain entity in CliCloud.Domain/Entities/Especialidades
// -- 2. Add DbSet<Especialidade> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a Especialidade api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Especialidades.EspecialidadeService
{
    public class EspecialidadeService : IEspecialidadeService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public EspecialidadeService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<EspecialidadeDTO>>> GetEspecialidadeAsync(string keyword = "")
        {
            EspecialidadeSearchList specification = new(keyword);
            IEnumerable<EspecialidadeDTO> list = await _repository.GetListAsync<EspecialidadeEntity, EspecialidadeDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<EspecialidadeDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<EspecialidadeLightDTO>>> GetEspecialidadeLightAsync(string keyword = "")
        {
            EspecialidadeSearchList specification = new(keyword);
            IEnumerable<EspecialidadeLightDTO> list = await _repository.GetListAsync<EspecialidadeEntity, EspecialidadeLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<EspecialidadeLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<EspecialidadeTableDTO>> GetEspecialidadePaginatedAsync(EspecialidadeTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            EspecialidadeSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<EspecialidadeTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<EspecialidadeEntity, EspecialidadeTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Especialidades (non-paginated)
        public async Task<Response<IEnumerable<EspecialidadeTableDTO>>> GetAllEspecialidadeAsync(EspecialidadeAllFilter filter)
        {
            try
            {
                filter ??= new EspecialidadeAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                EspecialidadeSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<EspecialidadeTableDTO> list = await _repository.GetListAsync<EspecialidadeEntity, EspecialidadeTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<EspecialidadeTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<EspecialidadeTableDTO>>(ex.Message);
            }
        }

        // get single Especialidade by Id 
        public async Task<Response<EspecialidadeDTO>> GetEspecialidadeAsync(Guid id)
        {
            try
            {
                EspecialidadeDTO dto = await _repository.GetByIdAsync<EspecialidadeEntity, EspecialidadeDTO, Guid>(id);
                return ResponseFactory.Success<EspecialidadeDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EspecialidadeDTO>(ex.Message);
            }
        }

        // get multiple Especialidades by Nome 
        public async Task<Response<IEnumerable<EspecialidadeDTO>>> GetEspecialidadeByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<EspecialidadeDTO>>("Nome não pode ser vazio");
            }

            EspecialidadeSearchByName specification = new(nome);
            IEnumerable<EspecialidadeDTO> results = await _repository.GetListAsync<EspecialidadeEntity, EspecialidadeDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<EspecialidadeDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<EspecialidadeDTO>>(ex.Message);
          }
        }

        // create new Especialidade
        public async Task<Response<Guid>> CreateEspecialidadeAsync(CreateEspecialidadeRequest request)
        {
            EspecialidadeEntity newEspecialidade = _mapper.Map(request, new EspecialidadeEntity());

            // Mapear CategoriaEspecialidadeId se fornecido
            if(!string.IsNullOrWhiteSpace(request.CategoriaEspecialidadeId) && Guid.TryParse(request.CategoriaEspecialidadeId, out Guid categoriaEspecialidadeGuid))
            {
              newEspecialidade.CategoriaEspecialidadeId = categoriaEspecialidadeGuid;
            }

            try
            {
                EspecialidadeEntity response = await _repository.CreateAsync<EspecialidadeEntity, Guid>(newEspecialidade);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Especialidade (entidade rastreada: map + SaveChanges, sem UpdateAsync)
        public async Task<Response<Guid>> UpdateEspecialidadeAsync(UpdateEspecialidadeRequest request, Guid id)
        {
            EspecialidadeEntity EspecialidadeInDb = await _repository.GetByIdAsync<EspecialidadeEntity, Guid>(id);

            _ = _mapper.Map(request, EspecialidadeInDb);

            if (!string.IsNullOrWhiteSpace(request.CategoriaEspecialidadeId) && Guid.TryParse(request.CategoriaEspecialidadeId, out Guid categoriaEspecialidadeGuid))
            {
                EspecialidadeInDb.CategoriaEspecialidadeId = categoriaEspecialidadeGuid;
            }
            else if (string.IsNullOrWhiteSpace(request.CategoriaEspecialidadeId))
            {
                EspecialidadeInDb.CategoriaEspecialidadeId = null;
            }

            try
            {
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(EspecialidadeInDb.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Especialidade
        public async Task<Response<Guid>> DeleteEspecialidadeAsync(Guid id)
        {
            try
            {
                EspecialidadeEntity? Especialidade = await _repository.RemoveByIdAsync<EspecialidadeEntity, Guid>(id);
                if (Especialidade == null)
                {
                    return ResponseFactory.Fail<Guid>("Especialidade não encontrada");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(Especialidade.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Especialidades
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleEspecialidadeAsync(IEnumerable<Guid> ids)
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
                EspecialidadeEntity? entity = await _repository.GetByIdAsync<EspecialidadeEntity, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Especialidade com ID {id} não encontrada.");
                  continue;
                }

                EspecialidadeEntity? deletedEntity = await _repository.RemoveByIdAsync<EspecialidadeEntity, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"Especialidade com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Especialidade com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} especialidades.";
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
