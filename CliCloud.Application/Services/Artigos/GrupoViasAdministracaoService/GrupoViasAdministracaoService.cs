using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs;
using CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.Filters;
using CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Artigos;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService
{
    public class GrupoViasAdministracaoService : IGrupoViasAdministracaoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public GrupoViasAdministracaoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GrupoViasAdministracaoDTO>>> GetGrupoViasAdministracaoAsync(string keyword = "")
        {
            var specification = new GrupoViasAdministracaoSearchList(keyword);
            IEnumerable<GrupoViasAdministracaoDTO> list =
                await _repository.GetListAsync<GrupoViasAdministracao, GrupoViasAdministracaoDTO, Guid>(specification);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<GrupoViasAdministracaoTableDTO>> GetGrupoViasAdministracaoPaginatedAsync(
            GrupoViasAdministracaoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null
                ? GSHelpers.GenerateOrderByString(filter)
                : string.Empty;

            var specification = new GrupoViasAdministracaoSearchTable(filter.Filters ?? new List<TableFilter>(), dynamicOrder);
            PaginatedResponse<GrupoViasAdministracaoTableDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<GrupoViasAdministracao, GrupoViasAdministracaoTableDTO, Guid>(
                    filter.PageNumber,
                    filter.PageSize,
                    specification);

            return pagedResponse;
        }

        public async Task<Response<GrupoViasAdministracaoDTO>> GetGrupoViasAdministracaoAsync(Guid id)
        {
            try
            {
                var spec = new GrupoViasAdministracaoByIdWithVias();
                GrupoViasAdministracaoDTO dto =
                    await _repository.GetByIdAsync<GrupoViasAdministracao, GrupoViasAdministracaoDTO, Guid>(id, spec);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<GrupoViasAdministracaoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateGrupoViasAdministracaoAsync(CreateGrupoViasAdministracaoRequest request)
        {
            var specification = new GrupoViasAdministracaoMatchName(request.Descricao);
            bool exists = await _repository.ExistsAsync<GrupoViasAdministracao, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Já existe um Grupo de Vias de Administração com esta descrição.");

            GrupoViasAdministracao newEntity = _mapper.Map(request, new GrupoViasAdministracao());

            try
            {
                GrupoViasAdministracao response =
                    await _repository.CreateAsync<GrupoViasAdministracao, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();

                if (request.Linhas != null && request.Linhas.Count > 0)
                {
                    int linha = 0;
                    foreach (var item in request.Linhas)
                    {
                        var linhaEntity = new GrupoViasAdministracaoLinha
                        {
                            Id = Guid.NewGuid(),
                            GrupoId = response.Id,
                            ViaId = item.ViaId,
                            Linha = linha++,
                            Descricao = item.Descricao,
                            Quantidade = item.Quantidade
                        };
                        _ = await _repository.CreateAsync<GrupoViasAdministracaoLinha, Guid>(linhaEntity);
                    }
                    _ = await _repository.SaveChangesAsync();
                }

                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateGrupoViasAdministracaoAsync(Guid id, UpdateGrupoViasAdministracaoRequest request)
        {
            var spec = new GrupoViasAdministracaoByIdWithVias();
            GrupoViasAdministracao entityInDb =
                await _repository.GetByIdAsync<GrupoViasAdministracao, Guid>(id, spec);
            if (entityInDb == null)
                return ResponseFactory.Fail<Guid>("Grupo de Vias de Administração não encontrado");

            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                var specification = new GrupoViasAdministracaoMatchName(request.Descricao);
                bool descricaoExists = await _repository.ExistsAsync<GrupoViasAdministracao, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe um Grupo de Vias de Administração com esta descrição.");
            }

            entityInDb.Descricao = request.Descricao ?? entityInDb.Descricao ?? "";
            _ = await _repository.UpdateAsync<GrupoViasAdministracao, Guid>(entityInDb);

            if (entityInDb.Vias != null && entityInDb.Vias.Count > 0)
            {
                foreach (var linha in entityInDb.Vias.ToList())
                {
                    await _repository.RemoveAsync<GrupoViasAdministracaoLinha, Guid>(linha);
                }
            }

            if (request.Linhas != null && request.Linhas.Count > 0)
            {
                int linha = 0;
                foreach (var item in request.Linhas)
                {
                    var linhaEntity = new GrupoViasAdministracaoLinha
                    {
                        Id = Guid.NewGuid(),
                        GrupoId = id,
                        ViaId = item.ViaId,
                        Linha = linha++,
                        Descricao = item.Descricao,
                        Quantidade = item.Quantidade
                    };
                    _ = await _repository.CreateAsync<GrupoViasAdministracaoLinha, Guid>(linhaEntity);
                }
            }

            try
            {
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entityInDb.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteGrupoViasAdministracaoAsync(Guid id)
        {
            try
            {
                GrupoViasAdministracao? entity =
                    await _repository.RemoveByIdAsync<GrupoViasAdministracao, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Grupo de Vias de Administração não encontrado");

                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleGrupoViasAdministracaoAsync(IEnumerable<Guid> ids)
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
                        GrupoViasAdministracao? entity =
                            await _repository.GetByIdAsync<GrupoViasAdministracao, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Grupo de Vias de Administração com ID {id} não encontrado.");
                            continue;
                        }

                        GrupoViasAdministracao? deletedEntity =
                            await _repository.RemoveByIdAsync<GrupoViasAdministracao, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Grupo de Vias de Administração com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Grupo de Vias de Administração com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);

                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(
                        successfullyDeletedIds,
                        $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} grupos de vias.");

                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
