using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Specifications;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.Specifications;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService;

public class SeparadorPersonalizadoVinculoService(
    IRepositoryAsync repository,
    ICurrentTenantUserService currentTenantUserService
)
    : ISeparadorPersonalizadoVinculoService
{
    private readonly IRepositoryAsync _repository = repository;
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

    public async Task<Response<IEnumerable<SeparadorPersonalizadoVinculoDTO>>> GetBySeparadorAsync(
        Guid clinicaId,
        Guid separadorPersonalizadoId
    )
    {
        Guid userId = GetCurrentUserId();
        SeparadorPersonalizadoByClinicaAndIdSpec separadorSpec = new(
            clinicaId,
            userId,
            separadorPersonalizadoId
        );
        bool separadorExiste =
            (await _repository.GetListAsync<SeparadorPersonalizado, Guid>(separadorSpec)).FirstOrDefault()
            != null;

        if (!separadorExiste)
        {
            return ResponseFactory.Fail<IEnumerable<SeparadorPersonalizadoVinculoDTO>>(
                "Separador personalizado não encontrado."
            );
        }

        SeparadorPersonalizadoVinculoBySeparadorSpec spec = new(separadorPersonalizadoId);
        IEnumerable<SeparadorPersonalizadoVinculoDTO> result =
            (
                await _repository.GetListAsync<
                    SeparadorPersonalizadoVinculo,
                    SeparadorPersonalizadoVinculoDTO,
                    Guid
                >(spec)
            ).OrderBy(x => x.Tipo).ThenBy(x => x.EntidadeId);

        return ResponseFactory.Success(result);
    }

    public async Task<Response<Guid>> CreateAsync(
        Guid clinicaId,
        CreateSeparadorPersonalizadoVinculoRequest request
    )
    {
        Guid userId = GetCurrentUserId();
        SeparadorPersonalizadoByClinicaAndIdSpec separadorSpec = new(
            clinicaId,
            userId,
            request.SeparadorPersonalizadoId
        );
        bool separadorExiste =
            (await _repository.GetListAsync<SeparadorPersonalizado, Guid>(separadorSpec)).FirstOrDefault()
            != null;

        if (!separadorExiste)
        {
            return ResponseFactory.Fail<Guid>("Separador personalizado não encontrado.");
        }

        SeparadorPersonalizadoVinculoMatchSpec dupSpec = new(
            request.SeparadorPersonalizadoId,
            request.Tipo,
            request.EntidadeId
        );
        bool exists = await _repository.ExistsAsync<SeparadorPersonalizadoVinculo, Guid>(dupSpec);
        if (exists)
        {
            return ResponseFactory.Fail<Guid>("Este vínculo já existe.");
        }

        SeparadorPersonalizadoVinculo entity = new()
        {
            SeparadorPersonalizadoId = request.SeparadorPersonalizadoId,
            Tipo = request.Tipo,
            EntidadeId = request.EntidadeId
        };

        try
        {
            SeparadorPersonalizadoVinculo created =
                await _repository.CreateAsync<SeparadorPersonalizadoVinculo, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> DeleteAsync(Guid clinicaId, Guid id)
    {
        try
        {
            Guid userId = GetCurrentUserId();
            SeparadorPersonalizadoVinculoByIdSpec vinculoSpec = new(id);
            SeparadorPersonalizadoVinculo? vinculo =
                (await _repository.GetListAsync<SeparadorPersonalizadoVinculo, Guid>(vinculoSpec))
                .FirstOrDefault();

            if (vinculo == null)
            {
                return ResponseFactory.Fail<Guid>("Vínculo não encontrado.");
            }

            SeparadorPersonalizadoByClinicaAndIdSpec separadorSpec = new(
                clinicaId,
                userId,
                vinculo.SeparadorPersonalizadoId
            );
            bool permitido =
                (await _repository.GetListAsync<SeparadorPersonalizado, Guid>(separadorSpec)).FirstOrDefault()
                != null;

            if (!permitido)
            {
                return ResponseFactory.Fail<Guid>("Vínculo não pertence à clínica atual.");
            }

            _ = await _repository.RemoveByIdAsync<SeparadorPersonalizadoVinculo, Guid>(id);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }
}
