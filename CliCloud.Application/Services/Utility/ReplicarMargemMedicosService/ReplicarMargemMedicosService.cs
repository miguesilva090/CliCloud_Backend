using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.ReplicarMargemMedicosService.DTOs;
using CliCloud.Application.Services.Utility.ReplicarMargemMedicosService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Utility.ReplicarMargemMedicosService;

public class ReplicarMargemMedicosService(IRepositoryAsync repository) : IReplicarMargemMedicosService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ReplicarMargemMedicosResponse>> ReplicarAsync(ReplicarMargemMedicosRequest request)
    {
        if (request.MedicoOrigemId == request.MedicoDestinoId)
            return ResponseFactory.Fail<ReplicarMargemMedicosResponse>("Médico de origem e destino iguais");

        var origem = (await _repository.GetListAsync<MargemMedico, Guid>(
            new MargensByMedicoSpec(request.MedicoOrigemId))).ToList();

        if (origem.Count == 0)
            return ResponseFactory.Fail<ReplicarMargemMedicosResponse>("O médico de origem não tem serviços associados.");

        var destino = (await _repository.GetListAsync<MargemMedico, Guid>(
            new MargensByMedicoSpec(request.MedicoDestinoId))).ToList();

        var destinoByServico = destino
            .GroupBy(x => x.ServicoId)
            .ToDictionary(g => g.Key, g => g.First());

        var substituidas = 0;
        var criadas = 0;

        foreach (var margemOrigem in origem)
        {
            if (destinoByServico.TryGetValue(margemOrigem.ServicoId, out var existenteDestino))
            {
                await _repository.RemoveAsync<MargemMedico, Guid>(existenteDestino);
                substituidas++;
            }

            await _repository.CreateAsync<MargemMedico, Guid>(new MargemMedico
            {
                Id = Guid.NewGuid(),
                ServicoId = margemOrigem.ServicoId,
                MedicoId = request.MedicoDestinoId,
                ValorMargem = margemOrigem.ValorMargem,
                PercentagemMargem = margemOrigem.PercentagemMargem,
            });
            criadas++;
        }

        await _repository.SaveChangesAsync();

        return ResponseFactory.Success(new ReplicarMargemMedicosResponse
        {
            TotalOrigem = origem.Count,
            SubstituidasDestino = substituidas,
            CriadasDestino = criadas,
        });
    }
}
