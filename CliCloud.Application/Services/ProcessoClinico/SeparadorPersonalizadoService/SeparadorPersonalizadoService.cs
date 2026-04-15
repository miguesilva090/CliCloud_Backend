using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Filters;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService;

public class SeparadorPersonalizadoService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentTenantUserService currentTenantUserService
)
    : ISeparadorPersonalizadoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentTenantUserService _currentTenantUserService = currentTenantUserService;

    private Guid GetCurrentUserId()
    {
        _currentTenantUserService.SetUser();
        if (Guid.TryParse(_currentTenantUserService.UserId, out Guid userId))
        {
            return userId;
        }

        throw new InvalidOperationException("Utilizador atual inválido.");
    }

    public async Task<Response<IEnumerable<SeparadorPersonalizadoDTO>>> GetSeparadorPersonalizadoAsync(
        Guid clinicaId,
        string keyword = ""
    )
    {
        try
        {
            Guid userId = GetCurrentUserId();
            SeparadorPersonalizadoSearchList specification = new(clinicaId, userId, keyword);
            IEnumerable<SeparadorPersonalizadoDTO> list =
                await _repository.GetListAsync<SeparadorPersonalizado, SeparadorPersonalizadoDTO, Guid>(
                    specification
                );
            return ResponseFactory.Success<IEnumerable<SeparadorPersonalizadoDTO>>(list);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<SeparadorPersonalizadoDTO>>(ex.Message);
        }
    }

    public async Task<PaginatedResponse<SeparadorPersonalizadoDTO>> GetSeparadorPersonalizadoPaginatedAsync(
        Guid clinicaId,
        SeparadorPersonalizadoTableFilter filter
    )
    {
        Guid userId = GetCurrentUserId();
        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            filter.PageNumber = 1;
        }

        string dynamicOrder =
            filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
        SeparadorPersonalizadoSearchTable specification = new(
            clinicaId,
            userId,
            filter.Keyword,
            dynamicOrder
        );

        return await _repository.GetPaginatedResultsAsync<
            SeparadorPersonalizado,
            SeparadorPersonalizadoDTO,
            Guid
        >(filter.PageNumber, filter.PageSize, specification);
    }

    public async Task<Response<SeparadorPersonalizadoDTO>> GetSeparadorPersonalizadoAsync(
        Guid clinicaId,
        Guid id
    )
    {
        try
        {
            Guid userId = GetCurrentUserId();
            SeparadorPersonalizadoByClinicaAndIdSpec specification = new(clinicaId, userId, id);
            SeparadorPersonalizadoDTO? dto =
                (
                    await _repository.GetListAsync<
                        SeparadorPersonalizado,
                        SeparadorPersonalizadoDTO,
                        Guid
                    >(specification)
                ).FirstOrDefault();

            return dto == null
                ? ResponseFactory.Fail<SeparadorPersonalizadoDTO>("Separador personalizado não encontrado.")
                : ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<SeparadorPersonalizadoDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CreateSeparadorPersonalizadoAsync(
        Guid clinicaId,
        CreateSeparadorPersonalizadoRequest request
    )
    {
        try
        {
            Guid userId = GetCurrentUserId();
            SeparadorPersonalizado entity = _mapper.Map<SeparadorPersonalizado>(request);
            entity.ClinicaId = clinicaId;
            entity.UtilizadorId = userId;
            SeparadorPersonalizado response =
                await _repository.CreateAsync<SeparadorPersonalizado, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(response.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateSeparadorPersonalizadoAsync(
        Guid clinicaId,
        UpdateSeparadorPersonalizadoRequest request,
        Guid id
    )
    {
        try
        {
            Guid userId = GetCurrentUserId();
            SeparadorPersonalizadoByClinicaAndIdSpec entitySpec = new(clinicaId, userId, id);
            SeparadorPersonalizado? entityInDb =
                (await _repository.GetListAsync<SeparadorPersonalizado, Guid>(entitySpec)).FirstOrDefault();

            if (entityInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Separador personalizado não encontrado.");
            }

            _ = _mapper.Map(request, entityInDb);
            entityInDb.ClinicaId = clinicaId;
            entityInDb.UtilizadorId = userId;
            SeparadorPersonalizado response =
                await _repository.UpdateAsync<SeparadorPersonalizado, Guid>(entityInDb);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(response.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> DeleteSeparadorPersonalizadoAsync(Guid clinicaId, Guid id)
    {
        try
        {
            Guid userId = GetCurrentUserId();
            SeparadorPersonalizadoByClinicaAndIdSpec entitySpec = new(clinicaId, userId, id);
            SeparadorPersonalizado? entity =
                (await _repository.GetListAsync<SeparadorPersonalizado, Guid>(entitySpec)).FirstOrDefault();

            if (entity == null)
            {
                return ResponseFactory.Fail<Guid>("Separador personalizado não encontrado.");
            }

            _ = await _repository.RemoveByIdAsync<SeparadorPersonalizado, Guid>(entity.Id);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(entity.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleSeparadorPersonalizadoAsync(
        Guid clinicaId,
        IEnumerable<Guid> ids
    )
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
                    SeparadorPersonalizadoByClinicaAndIdSpec entitySpec = new(clinicaId, userId, id);
                    SeparadorPersonalizado? entity =
                        (await _repository.GetListAsync<SeparadorPersonalizado, Guid>(entitySpec))
                        .FirstOrDefault();

                    if (entity == null)
                    {
                        failedDeletions.Add($"Separador personalizado com ID {id} não encontrado.");
                        continue;
                    }

                    _ = await _repository.RemoveByIdAsync<SeparadorPersonalizado, Guid>(entity.Id);
                    _ = await _repository.SaveChangesAsync();
                    successfullyDeletedIds.Add(id);
                }
                catch (Exception)
                {
                    failedDeletions.Add($"Erro ao eliminar separador personalizado com ID {id}.");
                    _repository.ClearChangeTracker();
                }
            }

            if (successfullyDeletedIds.Count == idsList.Count)
            {
                return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }

            if (successfullyDeletedIds.Count > 0)
            {
                return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(
                    successfullyDeletedIds,
                    $"Eliminados {successfullyDeletedIds.Count} de {idsList.Count} separadores personalizados."
                );
            }

            return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
        }
    }
}
