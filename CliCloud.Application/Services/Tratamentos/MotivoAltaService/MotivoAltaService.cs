using AutoMapper;
using CliCloud.Application.Services.Tratamentos.MotivoAltaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Tratamentos.MotivoAltaService.Filters;
using CliCloud.Application.Services.Tratamentos.MotivoAltaService.Specifications;

// After creating this service:
// -- 1. Create a MotivoAlta domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<MotivoAlta> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a MotivoAlta api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Tratamentos.MotivoAltaService
{
    public class MotivoAltaService : IMotivoAltaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public MotivoAltaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<MotivoAltaDTO>>> GetMotivoAltaAsync(string keyword = "")
        {
            MotivoAltaSearchList specification = new(keyword); // ardalis specification
            IEnumerable<MotivoAltaDTO> list = await _repository.GetListAsync<MotivoAlta, MotivoAltaDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<MotivoAltaDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<MotivoAltaLightDTO>>> GetMotivoAltaLightAsync(string keyword = "") 
        {
            MotivoAltaSearchList specification = new(keyword);
            IEnumerable<MotivoAltaLightDTO> list = await _repository.GetListAsync<MotivoAlta, MotivoAltaLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MotivoAltaLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<MotivoAltaTableDTO>> GetMotivoAltaPaginatedAsync(MotivoAltaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            MotivoAltaSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<MotivoAltaTableDTO> pagedResponse = 
                await _repository.GetPaginatedResultsAsync<MotivoAlta, MotivoAltaTableDTO, Guid>(
                    filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all MotivoAlta (non-paginated)
        public async Task<Response<IEnumerable<MotivoAltaTableDTO>>> GetAllMotivoAltaAsync(MotivoAltaAllFilter filter)
        {
            try
            {
                filter ??= new MotivoAltaAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                MotivoAltaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<MotivoAltaTableDTO> list = await _repository.GetListAsync<MotivoAlta, MotivoAltaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<MotivoAltaTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MotivoAltaTableDTO>>(ex.Message);
            }
        }


        // get single MotivoAlta by Id 
        public async Task<Response<MotivoAltaDTO>> GetMotivoAltaAsync(Guid id)
        {
            try
            {
                MotivoAltaDTO dto = await _repository.GetByIdAsync<MotivoAlta, MotivoAltaDTO, Guid>(id);
                return ResponseFactory.Success<MotivoAltaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MotivoAltaDTO>(ex.Message);
            }
        }

        //get single MotivoAlta by Descricao (exact match)
        public async Task<Response<MotivoAltaDTO>> GetMotivoAltaByDescricaoAsync(string descricao)
        {
            try
            {
               if(string.IsNullOrWhiteSpace(descricao))
               {
                return ResponseFactory.Fail<MotivoAltaDTO>("Descrição não pode ser vazia");
               }
               
               MotivoAltaMatchDescricao specification = new(descricao);
               IEnumerable<MotivoAltaDTO> results = await _repository.GetListAsync<MotivoAlta, MotivoAltaDTO, Guid>(specification);

               MotivoAltaDTO? motivoAlta = results.FirstOrDefault();
               if(motivoAlta == null)
               {
                    return ResponseFactory.Fail<MotivoAltaDTO>("Não foi encontrado nenhum Motivo de Alta com a descrição fornecida");
               }

               return ResponseFactory.Success<MotivoAltaDTO>(motivoAlta);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MotivoAltaDTO>(ex.Message);
            }
        }

        // create new MotivoAlta
        public async Task<Response<Guid>> CreateMotivoAltaAsync(CreateMotivoAltaRequest request)
        {
            MotivoAltaMatchDescricao specification = new(request.Descricao); // ardalis specification 
            bool MotivoAltaExists = await _repository.ExistsAsync<MotivoAlta, Guid>(specification);
            if (MotivoAltaExists)
            {
                return ResponseFactory.Fail<Guid>("MotivoAlta já existe");
            }

            MotivoAlta newMotivoAlta = _mapper.Map(request, new MotivoAlta()); // map dto to domain entity

            try
            {
                MotivoAlta response = await _repository.CreateAsync<MotivoAlta, Guid>(newMotivoAlta); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update MotivoAlta
        public async Task<Response<Guid>> UpdateMotivoAltaAsync(UpdateMotivoAltaRequest request, Guid id)
        {
            MotivoAlta MotivoAltaInDb = await _repository.GetByIdAsync<MotivoAlta, Guid>(id); // get existing entity
            if (MotivoAltaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            MotivoAlta updatedMotivoAlta = _mapper.Map(request, MotivoAltaInDb); // map dto to domain entity

            try
            {
                MotivoAlta response = await _repository.UpdateAsync<MotivoAlta, Guid>(updatedMotivoAlta);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete MotivoAlta
        public async Task<Response<Guid>> DeleteMotivoAltaAsync(Guid id)
        {
            try
            {
                MotivoAlta? MotivoAlta = await _repository.RemoveByIdAsync<MotivoAlta, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(MotivoAlta.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple MotivoAlta 
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMotivoAltaAsync(IEnumerable<Guid> ids)
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
                        MotivoAlta? entity = await _repository.GetByIdAsync<MotivoAlta, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Motivo de Alta com ID {id} não encontrado.");
                            continue;
                        }

                        MotivoAlta? deletedEntity = await _repository.RemoveByIdAsync<MotivoAlta, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else 
                        {
                            failedDeletions.Add($"Falha ao eliminar Motivo de Alta com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Falha ao eliminar Motivo de Alta com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }
                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} Motivos de Alta.");
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
