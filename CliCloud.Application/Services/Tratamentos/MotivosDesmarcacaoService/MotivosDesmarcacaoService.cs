using AutoMapper;
using CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.Filters;
using CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.Specifications;

// After creating this service:
// -- 1. Create a MotivosDesmarcacao domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<MotivosDesmarcacao> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a MotivosDesmarcacao api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService
{
    public class MotivosDesmarcacaoService(IRepositoryAsync repository, IMapper mapper) : IMotivosDesmarcacaoService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper;

        // get full List
        public async Task<Response<IEnumerable<MotivosDesmarcacaoDTO>>> GetMotivosDesmarcacaoAsync(string keyword = "")
        {
            MotivosDesmarcacaoSearchList specification = new(keyword);
            IEnumerable<MotivosDesmarcacaoDTO> list = await _repository.GetListAsync<MotivosDesmarcacao, MotivosDesmarcacaoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MotivosDesmarcacaoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<MotivosDesmarcacaoLightDTO>>> GetMotivosDesmarcacaoLightAsync(string keyword = "")
        {
            MotivosDesmarcacaoSearchList specification = new(keyword);
            IEnumerable<MotivosDesmarcacaoLightDTO> list = await _repository.GetListAsync<MotivosDesmarcacao, MotivosDesmarcacaoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MotivosDesmarcacaoLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<MotivosDesmarcacaoTableDTO>> GetMotivosDesmarcacaoPaginatedAsync(MotivosDesmarcacaoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            MotivosDesmarcacaoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<MotivosDesmarcacaoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<MotivosDesmarcacao, MotivosDesmarcacaoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all MotivosDesmarcacao (non-paginated)
        public async Task<Response<IEnumerable<MotivosDesmarcacaoTableDTO>>> GetAllMotivosDesmarcacaoAsync(MotivosDesmarcacaoAllFilter filter)
        {
            try
            {
                filter ??= new MotivosDesmarcacaoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                MotivosDesmarcacaoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<MotivosDesmarcacaoTableDTO> list = await _repository.GetListAsync<MotivosDesmarcacao, MotivosDesmarcacaoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<MotivosDesmarcacaoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MotivosDesmarcacaoTableDTO>>(ex.Message);
            }
        }


        // get single MotivosDesmarcacao by Id 
        public async Task<Response<MotivosDesmarcacaoDTO>> GetMotivosDesmarcacaoAsync(Guid id)
        {
            try
            {
                MotivosDesmarcacaoDTO dto = await _repository.GetByIdAsync<MotivosDesmarcacao, MotivosDesmarcacaoDTO, Guid>(id);
                return ResponseFactory.Success<MotivosDesmarcacaoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MotivosDesmarcacaoDTO>(ex.Message);
            }
        }

        // get single MotivosDesmarcacao by Descricao (exact match)
        public async Task<Response<MotivosDesmarcacaoDTO>> GetMotivosDesmarcacaoByDescricaoAsync(string descricao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    return ResponseFactory.Fail<MotivosDesmarcacaoDTO>("Descricao não pode ser vazia");
                }

                MotivosDesmarcacaoMatchDescricao specification = new(descricao);
                IEnumerable<MotivosDesmarcacaoDTO> results = await _repository.GetListAsync<MotivosDesmarcacao, MotivosDesmarcacaoDTO, Guid>(specification);

                MotivosDesmarcacaoDTO? motivosDesmarcacao = results.FirstOrDefault();
                if (motivosDesmarcacao == null)
                {
                    return ResponseFactory.Fail<MotivosDesmarcacaoDTO>("Não foi encontrado nenhum Motivo de Desmarcacao com a descrição fornecida");
                }

                return ResponseFactory.Success<MotivosDesmarcacaoDTO>(motivosDesmarcacao);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MotivosDesmarcacaoDTO>(ex.Message);
            }
        }

        // create new MotivosDesmarcacao
        public async Task<Response<Guid>> CreateMotivosDesmarcacaoAsync(CreateMotivosDesmarcacaoRequest request)
        {
            MotivosDesmarcacaoMatchDescricao specification = new(request.Descricao); // ardalis specification 
            bool MotivosDesmarcacaoExists = await _repository.ExistsAsync<MotivosDesmarcacao, Guid>(specification);
            if (MotivosDesmarcacaoExists)
            {
                return ResponseFactory.Fail<Guid>("MotivosDesmarcacao já existe");
            }

            MotivosDesmarcacao newMotivosDesmarcacao = _mapper.Map(request, new MotivosDesmarcacao()); // map dto to domain entity

            try
            {
                MotivosDesmarcacao response = await _repository.CreateAsync<MotivosDesmarcacao, Guid>(newMotivosDesmarcacao); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update MotivosDesmarcacao
        public async Task<Response<Guid>> UpdateMotivosDesmarcacaoAsync(UpdateMotivosDesmarcacaoRequest request, Guid id)
        {
            MotivosDesmarcacao MotivosDesmarcacaoInDb = await _repository.GetByIdAsync<MotivosDesmarcacao, Guid>(id); // get existing entity
            if (MotivosDesmarcacaoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            MotivosDesmarcacao updatedMotivosDesmarcacao = _mapper.Map(request, MotivosDesmarcacaoInDb); // map dto to domain entity

            try
            {
                MotivosDesmarcacao response = await _repository.UpdateAsync<MotivosDesmarcacao, Guid>(updatedMotivosDesmarcacao);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete MotivosDesmarcacao
        public async Task<Response<Guid>> DeleteMotivosDesmarcacaoAsync(Guid id)
        {
            try
            {
                MotivosDesmarcacao? MotivosDesmarcacao = await _repository.RemoveByIdAsync<MotivosDesmarcacao, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(MotivosDesmarcacao.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple MotivosDesmarcacao
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMotivosDesmarcacaoAsync(IEnumerable<Guid> ids)
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
                        MotivosDesmarcacao? entity = await _repository.GetByIdAsync<MotivosDesmarcacao, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Motivo de Desmarcacao com ID {id} não encontrado.");
                            continue;
                        }

                        MotivosDesmarcacao? deletedEntity = await _repository.RemoveByIdAsync<MotivosDesmarcacao, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar Motivo de Desmarcacao com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Falha ao eliminar Motivo de Desmarcacao com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }
                
                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }

                if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} motivos de desmarcacao.");
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

