using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.DTOs;
using CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.Filters;
using CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.Specifications;
using CliCloud.Domain.Entities.RegioesCorpo;
using IRegiaoCorpoService = CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.IRegiaoCorpoService;

// After creating this service:
// -- 1. Create a RegiaoCorpo domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<RegiaoCorpo> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a RegiaoCorpo api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.RegiaoCorpoService
{
    public class RegiaoCorpoService : IRegiaoCorpoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public RegiaoCorpoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<RegiaoCorpoDTO>>> GetRegiaoCorpoAsync(string keyword = "")
        {
            RegiaoCorpoSearchList specification = new(keyword);
            IEnumerable<RegiaoCorpoDTO> list = await _repository.GetListAsync<RegiaoCorpo, RegiaoCorpoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<RegiaoCorpoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<RegiaoCorpoLightDTO>>> GetRegiaoCorpoLightAsync(string keyword = "")
        {
            RegiaoCorpoSearchList specification = new(keyword);
            IEnumerable<RegiaoCorpoLightDTO> list = await _repository.GetListAsync<RegiaoCorpo, RegiaoCorpoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<RegiaoCorpoLightDTO>>(list);
        }
        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<RegiaoCorpoTableDTO>> GetRegiaoCorpoPaginatedAsync(RegiaoCorpoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            RegiaoCorpoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<RegiaoCorpoTableDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<RegiaoCorpo, RegiaoCorpoTableDTO, Guid>(
                    filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all RegiaoCorpos (non-paginated)
        public async Task<Response<IEnumerable<RegiaoCorpoTableDTO>>> GetAllRegiaoCorpoAsync(RegiaoCorpoAllFilter filter)
        {
            try
            {
                filter ??= new RegiaoCorpoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                RegiaoCorpoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<RegiaoCorpoTableDTO> list = await _repository.GetListAsync<RegiaoCorpo, RegiaoCorpoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<RegiaoCorpoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<RegiaoCorpoTableDTO>>(ex.Message);
            }
        }
        // get single RegiaoCorpo by Id 
        public async Task<Response<RegiaoCorpoDTO>> GetRegiaoCorpoAsync(Guid id)
        {
            try
            {
                RegiaoCorpoDTO dto = await _repository.GetByIdAsync<RegiaoCorpo, RegiaoCorpoDTO, Guid>(id);
                return ResponseFactory.Success<RegiaoCorpoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<RegiaoCorpoDTO>(ex.Message);
            }
        }

        // get single RegiaoCorpo by Descricao (exact match)
        public async Task<Response<RegiaoCorpoDTO>> GetRegiaoCorpoByDescricaoAsync(string descricao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    return ResponseFactory.Fail<RegiaoCorpoDTO>("Descricao não pode ser vazia");
                }

                RegiaoCorpoMatchDescricao specification = new(descricao);
                IEnumerable<RegiaoCorpoDTO> results =
                    await _repository.GetListAsync<RegiaoCorpo, RegiaoCorpoDTO, Guid>(specification);

                RegiaoCorpoDTO? regiao = results.FirstOrDefault();
                if (regiao == null)
                {
                    return ResponseFactory.Fail<RegiaoCorpoDTO>("Não foi encontrada nenhuma Região do Corpo com a descrição fornecida");
                }

                return ResponseFactory.Success(regiao);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<RegiaoCorpoDTO>(ex.Message);
            }
        }

        // create new RegiaoCorpo
        public async Task<Response<Guid>> CreateRegiaoCorpoAsync(CreateRegiaoCorpoRequest request)
        {
            RegiaoCorpoMatchDescricao specification = new(request.Descricao);
            bool regiaoCorpoExists = await _repository.ExistsAsync<RegiaoCorpo, Guid>(specification);
            if (regiaoCorpoExists)
            {
                return ResponseFactory.Fail<Guid>("Região do Corpo já existe");
            }

            RegiaoCorpo newRegiaoCorpo = _mapper.Map(request, new RegiaoCorpo());

            try
            {
                RegiaoCorpo response = await _repository.CreateAsync<RegiaoCorpo, Guid>(newRegiaoCorpo);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update RegiaoCorpo
        public async Task<Response<Guid>> UpdateRegiaoCorpoAsync(UpdateRegiaoCorpoRequest request, Guid id)
        {
            RegiaoCorpo regiaoCorpoInDb = await _repository.GetByIdAsync<RegiaoCorpo, Guid>(id);
            if (regiaoCorpoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            string currentDescricao = regiaoCorpoInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao.Trim();

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                RegiaoCorpoMatchDescricao specification = new(request.Descricao);
                bool descricaoExists = await _repository.ExistsAsync<RegiaoCorpo, Guid>(specification);
                if (descricaoExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe uma Região do Corpo com esta descrição");
                }
            }

            _ = _mapper.Map(request, regiaoCorpoInDb);

            try
            {
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(regiaoCorpoInDb.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete RegiaoCorpo
        public async Task<Response<Guid>> DeleteRegiaoCorpoAsync(Guid id)
        {
            try
            {
                RegiaoCorpo? regiaoCorpo = await _repository.RemoveByIdAsync<RegiaoCorpo, Guid>(id);
                if (regiaoCorpo == null)
                {
                    return ResponseFactory.Fail<Guid>("Região do Corpo não encontrada");
                }

                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(regiaoCorpo.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleRegiaoCorpoAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach (Guid id in idsList)
                {
                    try
                    {
                        RegiaoCorpo? entity = await _repository.GetByIdAsync<RegiaoCorpo, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Região do Corpo com ID {id} não encontrada.");
                            continue;
                        }

                        RegiaoCorpo? deletedEntity = await _repository.RemoveByIdAsync<RegiaoCorpo, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar Região do Corpo com ID {id}.");
                        }
                    }
                    catch
                    {
                        failedDeletions.Add($"Falha ao eliminar Região do Corpo com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }

                if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} regiões do corpo.");
                }

                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

