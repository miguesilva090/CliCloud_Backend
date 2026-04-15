using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Filters;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Specifications;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using System.Globalization;
using System.Text;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService
{
    public class FichaClinicaSecaoTemplateService : IFichaClinicaSecaoTemplateService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentTenantUserService _currentTenantUserService;

        public FichaClinicaSecaoTemplateService(
            IRepositoryAsync repository,
            IMapper mapper,
            ICurrentTenantUserService currentTenantUserService
        )
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

        private static string NormalizeCodigoBase(string? source)
        {
            string raw = string.IsNullOrWhiteSpace(source) ? "FORMULARIO" : source.Trim();
            string normalized = raw.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new();
            foreach (char c in normalized)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc == UnicodeCategory.NonSpacingMark)
                {
                    continue;
                }

                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(char.ToUpperInvariant(c));
                }
                else
                {
                    sb.Append('_');
                }
            }

            string compact = sb.ToString().Trim('_');
            while (compact.Contains("__"))
            {
                compact = compact.Replace("__", "_");
            }

            return string.IsNullOrWhiteSpace(compact) ? "FORMULARIO" : compact;
        }

        private async Task<string> GenerateUniqueCodigoAsync(Guid userId, string nome)
        {
            const int maxLength = 100;
            string baseCodigo = NormalizeCodigoBase(nome);
            if (baseCodigo.Length > maxLength)
            {
                baseCodigo = baseCodigo[..maxLength];
            }

            string codigo = baseCodigo;
            int sequence = 2;

            while (await _repository.ExistsAsync<FichaClinicaSecaoTemplate, Guid>(
                new FichaClinicaSecaoTemplateMatchCodigo(userId, codigo)
            ))
            {
                string suffix = $"_{sequence}";
                int allowedBaseLength = Math.Max(1, maxLength - suffix.Length);
                string truncatedBase = baseCodigo.Length > allowedBaseLength
                    ? baseCodigo[..allowedBaseLength]
                    : baseCodigo;
                codigo = $"{truncatedBase}{suffix}";
                sequence++;
            }

            return codigo;
        }

        // get full List
        public async Task<Response<IEnumerable<FichaClinicaSecaoTemplateDTO>>> GetFichaClinicaSecaoTemplateAsync(string keyword = "")
        {
            try
            {
                Guid userId = GetCurrentUserId();
                FichaClinicaSecaoTemplateSearchList specification = new(userId, keyword);
                IEnumerable<FichaClinicaSecaoTemplateDTO> list =
                    await _repository.GetListAsync<FichaClinicaSecaoTemplate, FichaClinicaSecaoTemplateDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<FichaClinicaSecaoTemplateDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<FichaClinicaSecaoTemplateDTO>>(ex.Message);
            }
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<FichaClinicaSecaoTemplateDTO>> GetFichaClinicaSecaoTemplatePaginatedAsync(
            FichaClinicaSecaoTemplateTableFilter filter
        )
        {
            Guid userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = (filter.Sorting != null) ? GSHelpers.GenerateOrderByString(filter) : "";
            FichaClinicaSecaoTemplateSearchTable specification =
                new(userId, filter.Keyword, dynamicOrder);

            PaginatedResponse<FichaClinicaSecaoTemplateDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<FichaClinicaSecaoTemplate, FichaClinicaSecaoTemplateDTO, Guid>(
                    filter.PageNumber,
                    filter.PageSize,
                    specification
                );

            return pagedResponse;
        }

        // get single by Id
        public async Task<Response<FichaClinicaSecaoTemplateDTO>> GetFichaClinicaSecaoTemplateAsync(Guid id)
        {
            try
            {
                Guid userId = GetCurrentUserId();
                FichaClinicaSecaoTemplateDTO dto =
                    await _repository.GetByIdAsync<FichaClinicaSecaoTemplate, FichaClinicaSecaoTemplateDTO, Guid>(id);
                if (dto == null)
                {
                    return ResponseFactory.Fail<FichaClinicaSecaoTemplateDTO>("Template de secção de ficha clínica não encontrado.");
                }
                FichaClinicaSecaoTemplate? entity =
                    await _repository.GetByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
                if (entity == null || entity.UtilizadorId != userId)
                {
                    return ResponseFactory.Fail<FichaClinicaSecaoTemplateDTO>("Template de secção de ficha clínica não encontrado.");
                }
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FichaClinicaSecaoTemplateDTO>(ex.Message);
            }
        }

        // create new
        public async Task<Response<Guid>> CreateFichaClinicaSecaoTemplateAsync(CreateFichaClinicaSecaoTemplateRequest request)
        {
            try
            {
                Guid userId = GetCurrentUserId();
                string codigoInput = (request.Codigo ?? string.Empty).Trim();
                string codigo = string.IsNullOrWhiteSpace(codigoInput)
                    ? await GenerateUniqueCodigoAsync(userId, request.Nome)
                    : NormalizeCodigoBase(codigoInput);

                if (codigo.Length > 100)
                {
                    codigo = codigo[..100];
                }

                request.Codigo = codigo;

                FichaClinicaSecaoTemplateMatchCodigo specification = new(userId, request.Codigo);
                bool exists = await _repository.ExistsAsync<FichaClinicaSecaoTemplate, Guid>(specification);
                if (exists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe um template com este código.");
                }

                FichaClinicaSecaoTemplate entity = _mapper.Map<FichaClinicaSecaoTemplate>(request);
                entity.UtilizadorId = userId;
                FichaClinicaSecaoTemplate response =
                    await _repository.CreateAsync<FichaClinicaSecaoTemplate, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update
        public async Task<Response<Guid>> UpdateFichaClinicaSecaoTemplateAsync(
            UpdateFichaClinicaSecaoTemplateRequest request,
            Guid id
        )
        {
            try
            {
                Guid userId = GetCurrentUserId();
                FichaClinicaSecaoTemplate entityInDb =
                    await _repository.GetByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
                if (entityInDb == null || entityInDb.UtilizadorId != userId)
                {
                    return ResponseFactory.Fail<Guid>("Template de secção de ficha clínica não encontrado.");
                }

                _ = _mapper.Map(request, entityInDb);
                entityInDb.UtilizadorId = userId;

                FichaClinicaSecaoTemplate response =
                    await _repository.UpdateAsync<FichaClinicaSecaoTemplate, Guid>(entityInDb);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete single
        public async Task<Response<Guid>> DeleteFichaClinicaSecaoTemplateAsync(Guid id)
        {
            try
            {
                Guid userId = GetCurrentUserId();
                FichaClinicaSecaoTemplate? existing =
                    await _repository.GetByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
                if (existing == null || existing.UtilizadorId != userId)
                {
                    return ResponseFactory.Fail<Guid>("Template de secção de ficha clínica não encontrado.");
                }
                FichaClinicaSecaoTemplate? entity =
                    await _repository.RemoveByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleFichaClinicaSecaoTemplateAsync(IEnumerable<Guid> ids)
        {
            try
            {
                Guid userId = GetCurrentUserId();
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = new();
                List<string> failedDeletions = new();

                foreach (Guid id in idsList)
                {
                    try
                    {
                        FichaClinicaSecaoTemplate? entity =
                            await _repository.GetByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
                        if (entity == null || entity.UtilizadorId != userId)
                        {
                            failedDeletions.Add($"Template de ficha clínica com ID {id} não encontrado.");
                            continue;
                        }

                        FichaClinicaSecaoTemplate? deletedEntity =
                            await _repository.RemoveByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar template de ficha clínica com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Erro ao eliminar template de ficha clínica com ID {id}.");
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
                        $"Eliminados {successfullyDeletedIds.Count} de {idsList.Count} templates de ficha clínica."
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
    }
}

