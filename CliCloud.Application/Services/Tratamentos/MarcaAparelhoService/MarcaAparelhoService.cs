using AutoMapper;
using CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.Filters;
using CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.Specifications;

// After creating this service:
// -- 1. Create a MarcaAparelho domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<MarcaAparelho> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a MarcaAparelho api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService
{
    public class MarcaAparelhoService : IMarcaAparelhoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public MarcaAparelhoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<MarcaAparelhoDTO>>> GetMarcaAparelhoAsync(string keyword = "")
        {
            MarcaAparelhoSearchList specification = new(keyword); // ardalis specification
            IEnumerable<MarcaAparelhoDTO> list = await _repository.GetListAsync<MarcaAparelho, MarcaAparelhoDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<MarcaAparelhoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<MarcaAparelhoLightDTO>>> GetMarcaAparelhoLightAsync(string keyword = "")
        {
            MarcaAparelhoSearchList specification = new(keyword);
            IEnumerable<MarcaAparelhoLightDTO> list = await _repository.GetListAsync<MarcaAparelho, MarcaAparelhoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MarcaAparelhoLightDTO>>(list);
        }


        // get Tanstack Table paginated list (table view)
        public async Task<PaginatedResponse<MarcaAparelhoTableDTO>> GetMarcaAparelhoPaginatedAsync(MarcaAparelhoTableFilter filter)
        {
            // Se existirem filtros, força a ir para a primeira página
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            // Ordenação dinâmica vinda da tabela (TanStack)
            string dynamicOrder = filter.Sorting != null
                ? GSHelpers.GenerateOrderByString(filter)
                : string.Empty;

            MarcaAparelhoSearchTable specification =
                new(filter.Filters ?? new List<TableFilter>(), dynamicOrder);

            PaginatedResponse<MarcaAparelhoTableDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<MarcaAparelho, MarcaAparelhoTableDTO, Guid>(
                    filter.PageNumber,
                    filter.PageSize,
                    specification
                );

            return pagedResponse;
        }

        // get all MarcaAparelho (non-paginated)
        public async Task<Response<IEnumerable<MarcaAparelhoTableDTO>>> GetAllMarcaAparelhoAsync(MarcaAparelhoAllFilter filter)
        {
            try
            {
                filter ??= new MarcaAparelhoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                MarcaAparelhoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<MarcaAparelhoTableDTO> list = await _repository.GetListAsync<MarcaAparelho, MarcaAparelhoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<MarcaAparelhoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MarcaAparelhoTableDTO>>(ex.Message);
            }
        }


        // get single MarcaAparelho by Id 
        public async Task<Response<MarcaAparelhoDTO>> GetMarcaAparelhoAsync(Guid id)
        {
            try
            {
                MarcaAparelhoDTO dto = await _repository.GetByIdAsync<MarcaAparelho, MarcaAparelhoDTO, Guid>(id);
                return ResponseFactory.Success<MarcaAparelhoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MarcaAparelhoDTO>(ex.Message);
            }
        }

        // get single MarcaAparelho by Designacao (exact match)
        public async Task<Response<MarcaAparelhoDTO>> GetMarcaAparelhoByDesignacaoAsync(string designacao)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(designacao))
                {
                    return ResponseFactory.Fail<MarcaAparelhoDTO>("Designação não pode ser vazia");
                }

                MarcaAparelhoMatchDesignacao specification = new(designacao);
                IEnumerable<MarcaAparelhoDTO> results = await _repository.GetListAsync<MarcaAparelho, MarcaAparelhoDTO, Guid>(specification);
                
                MarcaAparelhoDTO? marcaAparelho = results.FirstOrDefault();
                if(marcaAparelho == null)
                {
                    return ResponseFactory.Fail<MarcaAparelhoDTO>("Não foi encontrado nenhum MarcaAparelho com a designação fornecida");
                }
                return ResponseFactory.Success<MarcaAparelhoDTO>(marcaAparelho);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MarcaAparelhoDTO>(ex.Message);
            }
        }

        // create new MarcaAparelho
        public async Task<Response<Guid>> CreateMarcaAparelhoAsync(CreateMarcaAparelhoRequest request)
        {
            MarcaAparelhoMatchDesignacao specification = new(request.Designacao); // ardalis specification 
            bool MarcaAparelhoExists = await _repository.ExistsAsync<MarcaAparelho, Guid>(specification);
            if (MarcaAparelhoExists)
            {
                return ResponseFactory.Fail<Guid>("MarcaAparelho já existe");
            }

            MarcaAparelho newMarcaAparelho = _mapper.Map(request, new MarcaAparelho()); // map dto to domain entity

            try
            {
                MarcaAparelho response = await _repository.CreateAsync<MarcaAparelho, Guid>(newMarcaAparelho); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update MarcaAparelho
        public async Task<Response<Guid>> UpdateMarcaAparelhoAsync(UpdateMarcaAparelhoRequest request, Guid id)
        {
            MarcaAparelho MarcaAparelhoInDb = await _repository.GetByIdAsync<MarcaAparelho, Guid>(id); // get existing entity
            if (MarcaAparelhoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            MarcaAparelho updatedMarcaAparelho = _mapper.Map(request, MarcaAparelhoInDb); // map dto to domain entity

            try
            {
                MarcaAparelho response = await _repository.UpdateAsync<MarcaAparelho, Guid>(updatedMarcaAparelho);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete MarcaAparelho
        public async Task<Response<Guid>> DeleteMarcaAparelhoAsync(Guid id)
        {
            try
            {
                MarcaAparelho? MarcaAparelho = await _repository.RemoveByIdAsync<MarcaAparelho, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(MarcaAparelho.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple MarcaAparelho
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMarcaAparelhoAsync(IEnumerable<Guid> ids)
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
                        MarcaAparelho? entity = await _repository.GetByIdAsync<MarcaAparelho, Guid>(id);
                        if(entity == null)
                        {
                            failedDeletions.Add($"MarcaAparelho com ID {id} não encontrado.");
                            continue;
                        }
                        MarcaAparelho? deletedEntity = await _repository.RemoveByIdAsync<MarcaAparelho, Guid>(id);
                        if(deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar MarcaAparelho com ID {id}.");
                        }
                    }
                    catch(Exception)
                    {
                        failedDeletions.Add($"Falha ao eliminar MarcaAparelho com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }
                if(successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                if(successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} MarcaAparelhos.");
                }
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

