using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.ReplicarSubsistemasService.DTOs;
using CliCloud.Application.Services.Utility.ReplicarSubsistemasService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Utility.ReplicarSubsistemasService;

public class ReplicarSubsistemasService(IRepositoryAsync repository) : IReplicarSubsistemasService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ReplicarSubsistemasResponse>> ReplicarSubsistemasAsync(ReplicarSubsistemasRequest request)
    {
        if (request.OrganismoOrigemId == request.OrganismoDestinoId)
            return ResponseFactory.Fail<ReplicarSubsistemasResponse>("Organismo de origem e destino iguais");

        var origemSpec = new SubsistemasByOrganismoSpec(request.OrganismoOrigemId);
        var destinoSpec = new SubsistemasByOrganismoSpec(request.OrganismoDestinoId);

        var origem = (await _repository.GetListAsync<SubsistemaServico, Guid>(origemSpec)).ToList();
        var destino = (await _repository.GetListAsync<SubsistemaServico, Guid>(destinoSpec)).ToList();

        if (origem.Count == 0)
            return ResponseFactory.Fail<ReplicarSubsistemasResponse>("O organismo de origem não tem subsistemas para replicar.");

        var removidosDestino = 0;
        if (destino.Count > 0)
        {
            await _repository.RemoveRangeAsync<SubsistemaServico, Guid>(destino.Select(x => x.Id));
            removidosDestino = destino.Count;
        }

        var novos = origem.Select(x => new SubsistemaServico
        {
            Id = Guid.NewGuid(),
            ServicoId = x.ServicoId,
            SubsistemaId = x.SubsistemaId,
            OrganismoId = request.OrganismoDestinoId,
            ValorServico = x.ValorServico,
            ValorOrganismo = x.ValorOrganismo,
            MargemOrganismoPercent = x.MargemOrganismoPercent,
            ValorUtente = x.ValorUtente,
            MargemUtentePercent = x.MargemUtentePercent,
            Inativo = x.Inativo,
        }).ToList();

        await _repository.CreateRangeAsync<SubsistemaServico, Guid>(novos);
        await _repository.SaveChangesAsync();

        return ResponseFactory.Success(new ReplicarSubsistemasResponse
        {
            TotalOrigem = origem.Count,
            RemovidosDestino = removidosDestino,
            CriadosDestino = novos.Count,
        });
    }
}
