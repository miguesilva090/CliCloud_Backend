using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.FundirUtentesService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Utility.FundirUtentesService;

public class FundirUtentesService(
    IRepositoryAsync repository,
    IFundirUtentesFusaoRunner fusaoRunner,
    IUtenteService utenteService,
    ITransactionalExecutor transactionalExecutor
) : IFundirUtentesService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IFundirUtentesFusaoRunner _fusaoRunner = fusaoRunner;
    private readonly IUtenteService _utenteService = utenteService;
    private readonly ITransactionalExecutor _transactionalExecutor = transactionalExecutor;

    public async Task<Response<FundirUtentesResponse>> FundirUtentesAsync(
        FundirUtentesRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.UtenteOrigemId == request.UtenteApagarId)
            return ResponseFactory.Fail<FundirUtentesResponse>("Está a inserir o mesmo Utente");

        Utente origem;
        try
        {
            origem = await _repository.GetByIdAsync<Utente, Guid>(
                request.UtenteOrigemId,
                new UtenteByIdWithIncludes(request.UtenteOrigemId),
                cancellationToken);
        }
        catch (InvalidOperationException)
        {
            return ResponseFactory.Fail<FundirUtentesResponse>("Utente de origem não encontrado");
        }

        Utente apagar;
        try
        {
            apagar = await _repository.GetByIdAsync<Utente, Guid>(
                request.UtenteApagarId,
                new UtenteByIdWithIncludes(request.UtenteApagarId),
                cancellationToken);
        }
        catch (InvalidOperationException)
        {
            return ResponseFactory.Fail<FundirUtentesResponse>("Utente a apagar não encontrado");
        }

        try
        {
            await _transactionalExecutor.ExecuteAsync(async token =>
            {
                await MergeSubsistemasAsync(origem, apagar, token);
                await _fusaoRunner.ExecutarFusaoReferenciasAsync(
                    request.UtenteOrigemId,
                    request.UtenteApagarId,
                    token);

                Response<Guid> deleteResult =
                    await _utenteService.DeleteUtenteAsync(request.UtenteApagarId);

                if (deleteResult.Status != ResponseStatus.Success)
                {
                    string msg = deleteResult.Messages.TryGetValue("$", out List<string>? msgs) && msgs.Count > 0
                        ? msgs[0]
                        : "Falha ao eliminar o utente após fusão";
                    throw new InvalidOperationException(msg);
                }
            }, cancellationToken);

            return ResponseFactory.Success(new FundirUtentesResponse
            {
                UtenteOrigemId = request.UtenteOrigemId,
                UtenteApagadoId = request.UtenteApagarId,
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<FundirUtentesResponse>(ex.Message);
        }
    }

    private async Task MergeSubsistemasAsync(
        Utente origem,
        Utente apagar,
        CancellationToken cancellationToken)
    {
        HashSet<Guid> organismosOrigem = CollectOrganismoIds(origem);
        HashSet<Guid> organismosApagar = CollectOrganismoIds(apagar);

        foreach (Guid orgId in organismosApagar)
        {
            if (organismosOrigem.Contains(orgId))
                continue;

            UtenteSubsistemaLinha? linhaFonte =
                apagar.SubsistemaLinhas.FirstOrDefault(l => l.OrganismoId == orgId);

            var novaLinha = new UtenteSubsistemaLinha
            {
                Id = Guid.NewGuid(),
                UtenteId = origem.Id,
                OrganismoId = orgId,
                Designacao = linhaFonte?.Designacao,
                NumeroBeneficiario = linhaFonte?.NumeroBeneficiario,
                Sigla = linhaFonte?.Sigla,
                NomeBeneficiario = linhaFonte?.NomeBeneficiario,
                DataCartao = linhaFonte?.DataCartao,
                NumeroApolice = linhaFonte?.NumeroApolice,
                EmpresaId = linhaFonte?.EmpresaId,
            };

            _ = await _repository.CreateAsync<UtenteSubsistemaLinha, Guid>(novaLinha);
            _ = organismosOrigem.Add(orgId);
        }

        foreach (UtenteSubsistemaLinha linha in apagar.SubsistemaLinhas.ToList())
            _ = await _repository.RemoveByIdAsync<UtenteSubsistemaLinha, Guid>(linha.Id);

        await _repository.SaveChangesAsync();
    }

    private static HashSet<Guid> CollectOrganismoIds(Utente utente)
    {
        var ids = new HashSet<Guid>();

        if (utente.OrganismoId is Guid organismoId)
            _ = ids.Add(organismoId);

        if (utente.SeguradoraId is Guid seguradoraId)
            _ = ids.Add(seguradoraId);

        foreach (UtenteSubsistemaLinha linha in utente.SubsistemaLinhas)
        {
            if (linha.OrganismoId is Guid linhaOrgId)
                _ = ids.Add(linhaOrgId);
        }

        return ids;
    }
}
