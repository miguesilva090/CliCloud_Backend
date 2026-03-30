using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Artigos.ViaAdministracaoService.DTOs;
using CliCloud.Application.Services.Artigos.ViaAdministracaoService.Filters;
using CliCloud.Application.Services.Artigos.ViaAdministracaoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Artigos;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService
{
    public class ViaAdministracaoService : IViaAdministracaoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public ViaAdministracaoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ViaAdministracaoDTO>>> GetViaAdministracaoAsync(string keyword = "")
        {
            var specification = new ViaAdministracaoSearchList(keyword);
            IEnumerable<ViaAdministracaoDTO> list =
                await _repository.GetListAsync<ViaAdministracao, ViaAdministracaoDTO, Guid>(specification);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<ViaAdministracaoTableDTO>> GetViaAdministracaoPaginatedAsync(
            ViaAdministracaoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null
                ? GSHelpers.GenerateOrderByString(filter)
                : string.Empty;

            var specification = new ViaAdministracaoSearchTable(filter.Filters ?? new List<TableFilter>(), dynamicOrder);
            PaginatedResponse<ViaAdministracaoTableDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<ViaAdministracao, ViaAdministracaoTableDTO, Guid>(
                    filter.PageNumber,
                    filter.PageSize,
                    specification);

            return pagedResponse;
        }

        public async Task<Response<ViaAdministracaoDTO>> GetViaAdministracaoAsync(Guid id)
        {
            try
            {
                ViaAdministracaoDTO dto =
                    await _repository.GetByIdAsync<ViaAdministracao, ViaAdministracaoDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<ViaAdministracaoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateViaAdministracaoAsync(CreateViaAdministracaoRequest request)
        {
            var specification = new ViaAdministracaoMatchName(request.Descricao);
            bool exists = await _repository.ExistsAsync<ViaAdministracao, Guid>(specification);
            if (exists)
                return ResponseFactory.Fail<Guid>("Já existe uma Via de Administração com esta descrição.");

            ViaAdministracao newEntity = _mapper.Map(request, new ViaAdministracao());

            try
            {
                ViaAdministracao response =
                    await _repository.CreateAsync<ViaAdministracao, Guid>(newEntity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateViaAdministracaoAsync(UpdateViaAdministracaoRequest request, Guid id)
        {
            ViaAdministracao entityInDb =
                await _repository.GetByIdAsync<ViaAdministracao, Guid>(id);
            if (entityInDb == null)
                return ResponseFactory.Fail<Guid>("Via de Administração não encontrada");

            string currentDescricao = entityInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                var specification = new ViaAdministracaoMatchName(request.Descricao);
                bool descricaoExists = await _repository.ExistsAsync<ViaAdministracao, Guid>(specification);
                if (descricaoExists)
                    return ResponseFactory.Fail<Guid>("Já existe uma Via de Administração com esta descrição.");
            }

            _ = _mapper.Map(request, entityInDb);
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

        public async Task<Response<Guid>> DeleteViaAdministracaoAsync(Guid id)
        {
            try
            {
                ViaAdministracao? entity =
                    await _repository.RemoveByIdAsync<ViaAdministracao, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Via de Administração não encontrada");

                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleViaAdministracaoAsync(IEnumerable<Guid> ids)
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
                        ViaAdministracao? entity =
                            await _repository.GetByIdAsync<ViaAdministracao, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Via de Administração com ID {id} não encontrada.");
                            continue;
                        }

                        ViaAdministracao? deletedEntity =
                            await _repository.RemoveByIdAsync<ViaAdministracao, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Via de Administração com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Via de Administração com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);

                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(
                        successfullyDeletedIds,
                        $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} vias de administração.");

                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
