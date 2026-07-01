using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.WebserviceAdseService.DTOs;
using CliCloud.Application.Services.Faturacao.WebserviceAdseService.Specifications;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Faturacao;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Faturacao.WebserviceAdseService;

public class WebserviceAdseService(IRepositoryAsync repository) : IWebserviceAdseService
{
    private const string SecretMask = "********";

    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<WebserviceAdseDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
    {
        WebserviceAdse? entity = await ObterPorClinicaAsync(clinicaId).ConfigureAwait(false);

        if (entity is null)
        {
            return ResponseFactory.Success(new WebserviceAdseDTO
            {
                Id = Guid.Empty,
                ClinicaId = clinicaId,
            });
        }

        return ResponseFactory.Success(MapToDto(entity));
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(
        Guid clinicaId,
        AtualizarWebserviceAdseRequest request)
    {
        Organismo? organismo = await _repository
            .GetByIdAsync<Organismo, Guid>(request.OrganismoId)
            .ConfigureAwait(false);

        if (organismo is null || organismo.DeletedOn is not null || !OrganismoAdseAtivoSpec.EstaAtivo(organismo.Status))
            return ResponseFactory.Fail<Guid>("Organismo ADSE inválido.");

        Clinica? clinicaFisio = await _repository
            .GetByIdAsync<Clinica, Guid>(request.ClinicaFisioterapiaId)
            .ConfigureAwait(false);

        if (clinicaFisio is null || clinicaFisio.DeletedOn is not null)
            return ResponseFactory.Fail<Guid>("Entidade fisioterapia inválida.");

        WebserviceAdse entity = await ObterPorClinicaAsync(clinicaId).ConfigureAwait(false)
            ?? new WebserviceAdse { ClinicaId = clinicaId };

        entity.OrganismoId = request.OrganismoId;
        entity.ClinicaFisioterapiaId = request.ClinicaFisioterapiaId;
        entity.UrlAdse = request.UrlAdse.Trim();
        entity.DominioUserAdse = request.DominioUserAdse.Trim();
        entity.UserAdse = request.UserAdse.Trim();
        entity.PasswordAdse = ResolveSecret(request.PasswordAdse, entity.PasswordAdse) ?? string.Empty;
        entity.PasslocalAdse = ResolveSecret(request.PasslocalAdse, entity.PasslocalAdse) ?? string.Empty;
        entity.NumlocalAdse = request.NumlocalAdse;
        entity.PastaPdfAdse = request.PastaPdfAdse.Trim();
        entity.NomelocalAdse = clinicaFisio.Nome.Trim();

        if (entity.Id == Guid.Empty)
            _ = await _repository.CreateAsync<WebserviceAdse, Guid>(entity).ConfigureAwait(false);
        else
            _ = await _repository.UpdateAsync<WebserviceAdse, Guid>(entity).ConfigureAwait(false);

        _ = await _repository.SaveChangesAsync().ConfigureAwait(false);
        return ResponseFactory.Success(entity.Id);
    }

    public async Task<Response<IReadOnlyList<AdseOrganismoLookupDTO>>> ListarOrganismosAsync()
    {
        IEnumerable<Organismo> rows = await _repository
            .GetListAsync<Organismo, Guid>(new OrganismosAdseLookupSpec())
            .ConfigureAwait(false);

        IReadOnlyList<AdseOrganismoLookupDTO> list = rows
            .Where(x => !string.IsNullOrWhiteSpace(x.Nome))
            .Select(x => new AdseOrganismoLookupDTO { Id = x.Id, Nome = x.Nome.Trim() })
            .ToList();

        return ResponseFactory.Success(list);
    }

    private async Task<WebserviceAdse?> ObterPorClinicaAsync(Guid clinicaId)
    {
        IEnumerable<WebserviceAdse> rows = await _repository
            .GetListAsync<WebserviceAdse, Guid>(new WebserviceAdsePorClinicaSpec(clinicaId))
            .ConfigureAwait(false);

        return rows.FirstOrDefault();
    }

    private static WebserviceAdseDTO MapToDto(WebserviceAdse entity) => new()
    {
        Id = entity.Id,
        ClinicaId = entity.ClinicaId,
        OrganismoId = entity.OrganismoId,
        ClinicaFisioterapiaId = entity.ClinicaFisioterapiaId,
        UrlAdse = entity.UrlAdse,
        DominioUserAdse = entity.DominioUserAdse,
        UserAdse = entity.UserAdse,
        PasswordAdse = MaskSecret(entity.PasswordAdse),
        PasslocalAdse = MaskSecret(entity.PasslocalAdse),
        NumlocalAdse = entity.NumlocalAdse,
        NomelocalAdse = entity.NomelocalAdse,
        PastaPdfAdse = entity.PastaPdfAdse,
    };

    private static string? MaskSecret(string? value) =>
        string.IsNullOrWhiteSpace(value) ? value : SecretMask;

    private static string? ResolveSecret(string? incoming, string? existing)
    {
        if (string.IsNullOrWhiteSpace(incoming))
            return existing;

        string trimmed = incoming.Trim();
        return trimmed == SecretMask ? existing : trimmed;
    }
}
