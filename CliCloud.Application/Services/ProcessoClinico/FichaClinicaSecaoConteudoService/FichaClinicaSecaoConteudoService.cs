using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.Filters;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.Specifications;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService
{
    public class FichaClinicaSecaoConteudoService : IFichaClinicaSecaoConteudoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        private readonly ICurrentTenantUserService _currentTenantUserService;

        public FichaClinicaSecaoConteudoService(IRepositoryAsync repository, IMapper mapper, ICurrentTenantUserService currentTenantUserService)
        {
            _repository = repository;
            _mapper = mapper; 
            _currentTenantUserService = currentTenantUserService;
        }

        private Guid GetCurrentUserId()
        {
            _currentTenantUserService.SetUser();
            if (Guid.TryParse(_currentTenantUserService.UserId, out Guid userId))
            {
                return userId;
            }

            throw new InvalidOperationException("Utilizador atual inválido.");
        }

        private async Task<bool> UserOwnsTemplateAsync(Guid separadorId, Guid userId)
        {
            FichaClinicaSecaoTemplate? template = 
                await _repository.GetByIdAsync<FichaClinicaSecaoTemplate, Guid>(separadorId);

            return template != null && template.UtilizadorId == userId;
        }

        // get full List
        public async Task<Response<IEnumerable<FichaClinicaSecaoConteudoDTO>>> GetFichaClinicaSecaoConteudoAsync(string keyword = "")
        {
            FichaClinicaSecaoConteudoSearchList specification = new(keyword); // ardalis specification
            IEnumerable<FichaClinicaSecaoConteudoDTO> list = await _repository.GetListAsync<FichaClinicaSecaoConteudo, FichaClinicaSecaoConteudoDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<FichaClinicaSecaoConteudoDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<FichaClinicaSecaoConteudoDTO>> GetFichaClinicaSecaoConteudoPaginatedAsync(FichaClinicaSecaoConteudoTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = (filter.Sorting != null) ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            FichaClinicaSecaoConteudoSearchTable specification = new(filter?.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<FichaClinicaSecaoConteudoDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<FichaClinicaSecaoConteudo, FichaClinicaSecaoConteudoDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single FichaClinicaSecaoConteudo by Id 
        public async Task<Response<FichaClinicaSecaoConteudoDTO>> GetFichaClinicaSecaoConteudoAsync(Guid id)
        {
            try
            {
                FichaClinicaSecaoConteudoDTO dto = await _repository.GetByIdAsync<FichaClinicaSecaoConteudo, FichaClinicaSecaoConteudoDTO, Guid>(id);
                return ResponseFactory.Success<FichaClinicaSecaoConteudoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FichaClinicaSecaoConteudoDTO>(ex.Message);
            }
        }

        // create new FichaClinicaSecaoConteudo
        public async Task<Response<Guid>> CreateFichaClinicaSecaoConteudoAsync(CreateFichaClinicaSecaoConteudoRequest request)
        {
            FichaClinicaSecaoConteudoMatchName specification = new(request.UtenteId, request.CampoId); // ardalis specification 
            bool FichaClinicaSecaoConteudoExists = await _repository.ExistsAsync<FichaClinicaSecaoConteudo, Guid>(specification);
            if (FichaClinicaSecaoConteudoExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe conteúdo para este campo e utente.");
            }

            FichaClinicaSecaoConteudo newFichaClinicaSecaoConteudo = _mapper.Map(request, new FichaClinicaSecaoConteudo()); // map dto to domain entity

            try
            {
                FichaClinicaSecaoConteudo response = await _repository.CreateAsync<FichaClinicaSecaoConteudo, Guid>(newFichaClinicaSecaoConteudo); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update FichaClinicaSecaoConteudo
        public async Task<Response<Guid>> UpdateFichaClinicaSecaoConteudoAsync(UpdateFichaClinicaSecaoConteudoRequest request, Guid id)
        {
            FichaClinicaSecaoConteudo FichaClinicaSecaoConteudoInDb = await _repository.GetByIdAsync<FichaClinicaSecaoConteudo, Guid>(id); // get existing entity
            if (FichaClinicaSecaoConteudoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            FichaClinicaSecaoConteudo updatedFichaClinicaSecaoConteudo = _mapper.Map(request, FichaClinicaSecaoConteudoInDb); // map dto to domain entity

            try
            {
                FichaClinicaSecaoConteudo response = await _repository.UpdateAsync<FichaClinicaSecaoConteudo, Guid>(updatedFichaClinicaSecaoConteudo);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete FichaClinicaSecaoConteudo
        public async Task<Response<Guid>> DeleteFichaClinicaSecaoConteudoAsync(Guid id)
        {
            try
            {
                FichaClinicaSecaoConteudo? entity = await _repository.RemoveByIdAsync<FichaClinicaSecaoConteudo, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple FichaClinicaSecaoConteudo
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleFichaClinicaSecaoConteudoAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = new();
                List<string> failedDeletions = new();

                foreach (Guid id in idsList)
                {
                    try
                    {
                        FichaClinicaSecaoConteudo? entity = await _repository.GetByIdAsync<FichaClinicaSecaoConteudo, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Conteúdo de ficha clínica com ID {id} não encontrado.");
                            continue;
                        }

                        FichaClinicaSecaoConteudo? deletedEntity = await _repository.RemoveByIdAsync<FichaClinicaSecaoConteudo, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar conteúdo de ficha clínica com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Erro ao eliminar conteúdo de ficha clínica com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(
                        successfullyDeletedIds,
                        $"Eliminados {successfullyDeletedIds.Count} de {idsList.Count} conteúdos de ficha clínica."
                    );
                }
                else
                {
                    return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<FichaClinicaSecaoConteudoDTO>>> GetByUtenteAndSeparadorAsync(Guid utenteId, Guid separadorId)
        {
            try
            {
                Guid userId = GetCurrentUserId();

                bool OwnsTemplate = await UserOwnsTemplateAsync(separadorId, userId);
                if(!OwnsTemplate)
                {
                    return ResponseFactory.Fail<IEnumerable<FichaClinicaSecaoConteudoDTO>>("Separador não encontrado");
                }

                FichaClinicaSecaoConteudoByUtenteAndSeparadorSpec spec = 
                    new(utenteId, separadorId);

                IEnumerable<FichaClinicaSecaoConteudoDTO> list = 
                    await _repository.GetListAsync<FichaClinicaSecaoConteudo, FichaClinicaSecaoConteudoDTO, Guid>(spec);

                return ResponseFactory.Success<IEnumerable<FichaClinicaSecaoConteudoDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<FichaClinicaSecaoConteudoDTO>>(ex.Message);

            }
        }

        public async Task<Response<IEnumerable<Guid>>> UpsertLoteAsync(
            UpsertFichaClinicaSecaoConteudoLoteRequest request
        )
        {
            try
            {
                Guid userId = GetCurrentUserId();

                bool ownsTemplate = await UserOwnsTemplateAsync(request.SeparadorId, userId);
                if(!ownsTemplate)
                {
                    return ResponseFactory.Fail<IEnumerable<Guid>>("Separador não encontrado");
                }

                IEnumerable<FichaClinicaSecaoCampo> camposDoSeparador = 
                    await _repository.GetListAsync<FichaClinicaSecaoCampo, Guid>();

                HashSet<Guid> campoIdsPermitidos = camposDoSeparador
                    .Where(c => c.SeparadorId == request.SeparadorId)
                    .Select(c => c.Id)
                    .ToHashSet();

                List<Guid> idsAfetados = new();

                foreach ( UpsertFichaClinicaSecaoConteudoLoteItemRequest item in request.Itens)
                {
                    if(!campoIdsPermitidos.Contains(item.CampoId))
                    {
                        return ResponseFactory.Fail<IEnumerable<Guid>>(
                            $"Campo {item.CampoId} não pertence ao separador informado."
                        );
                    }

                    FichaClinicaSecaoConteudoMatchName matchSpec = 
                        new(request.UtenteId, item.CampoId);

                    FichaClinicaSecaoConteudo? existente = 
                        (await _repository.GetListAsync<FichaClinicaSecaoConteudo, Guid>(matchSpec)).FirstOrDefault();

                    if(existente == null)
                    {
                        FichaClinicaSecaoConteudo novo = new()
                        {
                            UtenteId = request.UtenteId,
                            CampoId = item.CampoId,
                            Texto = item.Texto ?? string.Empty
                        };

                        FichaClinicaSecaoConteudo created = 
                            await _repository.CreateAsync<FichaClinicaSecaoConteudo, Guid>(novo);

                        idsAfetados.Add(created.Id);
                    }
                    else 
                    {
                        existente.Texto = item.Texto ?? string.Empty;

                        FichaClinicaSecaoConteudo updated = 
                            await _repository.UpdateAsync<FichaClinicaSecaoConteudo, Guid>(existente);

                        idsAfetados.Add(updated.Id);
                    }
                }

                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<IEnumerable<Guid>>(idsAfetados);
            }
            catch ( Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

