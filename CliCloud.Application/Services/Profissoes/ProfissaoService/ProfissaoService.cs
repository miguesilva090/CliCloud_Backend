using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using ProfissaoEntity = CliCloud.Domain.Entities.Profissoes.Profissao;
using CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs;
using CliCloud.Application.Services.Profissoes.ProfissaoService.Filters;
using CliCloud.Application.Services.Profissoes.ProfissaoService.Specifications;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService
{
    public class ProfissaoService : IProfissaoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public ProfissaoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ProfissaoDTO>>> GetProfissaoAsync(string keyword = "")
        {
            ProfissaoSearchList specification = new(keyword);
            IEnumerable<ProfissaoDTO> list = await _repository.GetListAsync<ProfissaoEntity, ProfissaoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<ProfissaoDTO>>(list);
        }

        public async Task<Response<IEnumerable<ProfissaoLightDTO>>> GetProfissaoLightAsync(string keyword = "")
        {
            ProfissaoSearchList specification = new(keyword);
            IEnumerable<ProfissaoLightDTO> list = await _repository.GetListAsync<ProfissaoEntity, ProfissaoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<ProfissaoLightDTO>>(list);
        }

        public async Task<PaginatedResponse<ProfissaoTableDTO>> GetProfissaoPaginatedAsync(ProfissaoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            ProfissaoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<ProfissaoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<ProfissaoEntity, ProfissaoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<ProfissaoTableDTO>>> GetAllProfissaoAsync(ProfissaoAllFilter filter)
        {
            try
            {
                filter ??= new ProfissaoAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                ProfissaoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<ProfissaoTableDTO> list = await _repository.GetListAsync<ProfissaoEntity, ProfissaoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<ProfissaoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<ProfissaoTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<ProfissaoDTO>> GetProfissaoAsync(Guid id)
        {
            try
            {
                ProfissaoDTO dto = await _repository.GetByIdAsync<ProfissaoEntity, ProfissaoDTO, Guid>(id);
                return ResponseFactory.Success<ProfissaoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<ProfissaoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateProfissaoAsync(CreateProfissaoRequest request)
        {
            ProfissaoMatchDescricao specification = new(request.Descricao ?? "");
            bool exists = await _repository.ExistsAsync<ProfissaoEntity, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Profissão com esta descrição já existe");

            ProfissaoEntity newEntity = _mapper.Map(request, new ProfissaoEntity());
            try
            {
                ProfissaoEntity response = await _repository.CreateAsync<ProfissaoEntity, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateProfissaoAsync(UpdateProfissaoRequest request, Guid id)
        {
            ProfissaoEntity entityInDb = await _repository.GetByIdAsync<ProfissaoEntity, Guid>(id);
            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                ProfissaoMatchDescricao specification = new(request.Descricao ?? "");
                bool descricaoExists = await _repository.ExistsAsync<ProfissaoEntity, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe uma Profissão com esta descrição");
            }

            _ = _mapper.Map(request, entityInDb);
            try
            {
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entityInDb.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteProfissaoAsync(Guid id)
        {
            try
            {
                ProfissaoEntity? entity = await _repository.RemoveByIdAsync<ProfissaoEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Profissão não encontrada");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleProfissaoAsync(IEnumerable<Guid> ids)
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
                        ProfissaoEntity? entity = await _repository.GetByIdAsync<ProfissaoEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Profissão com ID {id} não encontrada.");
                            continue;
                        }
                        ProfissaoEntity? deletedEntity = await _repository.RemoveByIdAsync<ProfissaoEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"Profissão com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Profissão com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} profissões.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
