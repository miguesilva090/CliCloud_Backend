using CliCloud.Application.Common;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public sealed class LoteDirectSaveValidator(IRepositoryAsync repository) : ILoteDirectSaveValidator
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<string?> ValidateAsync(CreateLoteDirectRequest request, Guid? excludeId = null)
    {
        if (request.CodigoOrganismo is null or <= 0)
            return "Organismo em falta.";

        if (string.IsNullOrWhiteSpace(request.Credencial))
            return "Nº credencial em falta.";

        bool credencialDuplicada = await _repository
            .ExistsAsync<LoteDirect, Guid>(new LoteDirectByCredencialSpec(request.Credencial.Trim(), excludeId))
            .ConfigureAwait(false);

        if (credencialDuplicada)
            return "Já existe uma credencial com este número.";

        if (request is { Mes: >= 1 and <= 12, Ano: >= 1900, CodigoOrganismo: int org })
        {
            bool mesEmHistorico = await _repository
                .ExistsAsync<LoteDirect, Guid>(new LoteDirectOrganismoMesAnoHistoricoSpec(org, request.Mes.Value, request.Ano.Value))
                .ConfigureAwait(false);

            if (mesEmHistorico)
                return "O mês/ano deste organismo já foi transferido para histórico.";
        }

        return null;
    }
}
