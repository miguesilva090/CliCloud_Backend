using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using CategoriaEspecialidadeEntity = CliCloud.Domain.Entities.Especialidades.CategoriaEspecialidade;
using CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs;
using CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.Filters;
using CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.Specifications;

// After creating this service:
// -- 1. Create a CategoriaEspecialidade domain entity in CliCloud.Domain/Entities/Especialidades
// -- 2. Add DbSet<CategoriaEspecialidade> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a CategoriaEspecialidade api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService
{
    public class CategoriaEspecialidadeService : ICategoriaEspecialidadeService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public CategoriaEspecialidadeService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<CategoriaEspecialidadeDTO>>> GetCategoriaEspecialidadeAsync(string keyword = "")
        {
            CategoriaEspecialidadeSearchList specification = new(keyword);
            IEnumerable<CategoriaEspecialidadeDTO> list = await _repository.GetListAsync<CategoriaEspecialidadeEntity, CategoriaEspecialidadeDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<CategoriaEspecialidadeDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<CategoriaEspecialidadeLightDTO>>> GetCategoriaEspecialidadeLightAsync(string keyword = "")
        {
            CategoriaEspecialidadeSearchList specification = new(keyword);
            IEnumerable<CategoriaEspecialidadeLightDTO> list = await _repository.GetListAsync<CategoriaEspecialidadeEntity, CategoriaEspecialidadeLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<CategoriaEspecialidadeLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<CategoriaEspecialidadeTableDTO>> GetCategoriaEspecialidadePaginatedAsync(CategoriaEspecialidadeTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            CategoriaEspecialidadeSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<CategoriaEspecialidadeTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<CategoriaEspecialidadeEntity, CategoriaEspecialidadeTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all CategoriaEspecialidades (non-paginated)
        public async Task<Response<IEnumerable<CategoriaEspecialidadeTableDTO>>> GetAllCategoriaEspecialidadeAsync(CategoriaEspecialidadeAllFilter filter)
        {
            try
            {
                filter ??= new CategoriaEspecialidadeAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                CategoriaEspecialidadeSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<CategoriaEspecialidadeTableDTO> list = await _repository.GetListAsync<CategoriaEspecialidadeEntity, CategoriaEspecialidadeTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<CategoriaEspecialidadeTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<CategoriaEspecialidadeTableDTO>>(ex.Message);
            }
        }

        // get single CategoriaEspecialidade by Id 
        public async Task<Response<CategoriaEspecialidadeDTO>> GetCategoriaEspecialidadeAsync(Guid id)
        {
            try
            {
                CategoriaEspecialidadeDTO dto = await _repository.GetByIdAsync<CategoriaEspecialidadeEntity, CategoriaEspecialidadeDTO, Guid>(id);
                return ResponseFactory.Success<CategoriaEspecialidadeDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<CategoriaEspecialidadeDTO>(ex.Message);
            }
        }

        // create new CategoriaEspecialidade
        public async Task<Response<Guid>> CreateCategoriaEspecialidadeAsync(CreateCategoriaEspecialidadeRequest request)
        {
            CategoriaEspecialidadeMatchDescricao specification = new(request.Descricao ?? "");
            bool CategoriaEspecialidadeExists = await _repository.ExistsAsync<CategoriaEspecialidadeEntity, Guid>(specification);
            if (CategoriaEspecialidadeExists)
            {
                return ResponseFactory.Fail<Guid>("CategoriaEspecialidade com esta Descrição já existe");
            }

            CategoriaEspecialidadeEntity newCategoriaEspecialidade = _mapper.Map(request, new CategoriaEspecialidadeEntity());

            try
            {
                CategoriaEspecialidadeEntity response = await _repository.CreateAsync<CategoriaEspecialidadeEntity, Guid>(newCategoriaEspecialidade);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update CategoriaEspecialidade
        public async Task<Response<Guid>> UpdateCategoriaEspecialidadeAsync(UpdateCategoriaEspecialidadeRequest request, Guid id)
        {
            // Carrega a entidade rastreada pelo DbContext
            CategoriaEspecialidadeEntity CategoriaEspecialidadeInDb = await _repository.GetByIdAsync<CategoriaEspecialidadeEntity, Guid>(id);

            string currentDescricao = CategoriaEspecialidadeInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                CategoriaEspecialidadeMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<CategoriaEspecialidadeEntity, Guid>(specification);
                if (descricaoExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe uma CategoriaEspecialidade com esta Descrição");
                }
            }

            // Atualiza os campos da entidade rastreada (não precisamos de a voltar a carregar/atualizar via repositório)
            _ = _mapper.Map(request, CategoriaEspecialidadeInDb);

            try
            {
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(CategoriaEspecialidadeInDb.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete CategoriaEspecialidade
        public async Task<Response<Guid>> DeleteCategoriaEspecialidadeAsync(Guid id)
        {
            try
            {
                CategoriaEspecialidadeEntity? CategoriaEspecialidade = await _repository.RemoveByIdAsync<CategoriaEspecialidadeEntity, Guid>(id);
                if (CategoriaEspecialidade == null)
                {
                    return ResponseFactory.Fail<Guid>("CategoriaEspecialidade não encontrada");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(CategoriaEspecialidade.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple CategoriaEspecialidades
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleCategoriaEspecialidadeAsync(IEnumerable<Guid> ids)
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
                CategoriaEspecialidadeEntity? entity = await _repository.GetByIdAsync<CategoriaEspecialidadeEntity, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"CategoriaEspecialidade com ID {id} não encontrada.");
                  continue;
                }

                CategoriaEspecialidadeEntity? deletedEntity = await _repository.RemoveByIdAsync<CategoriaEspecialidadeEntity, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"CategoriaEspecialidade com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"CategoriaEspecialidade com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} categorias de especialidade.";
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
