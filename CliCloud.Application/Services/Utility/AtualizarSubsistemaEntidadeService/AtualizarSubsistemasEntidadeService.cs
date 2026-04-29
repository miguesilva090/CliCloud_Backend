using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.AtualizarSubsistemasEntidadeService.DTOs;
using CliCloud.Application.Services.Utility.AtualizarSubsistemasEntidadeService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Utility.AtualizarSubsistemasEntidadeService;

public class AtualizarSubsistemasEntidadeService(IRepositoryAsync repository) : IAtualizarSubsistemasEntidadeService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<AtualizarSubsistemasEntidadeResponse>> AtualizarAsync(AtualizarSubsistemasEntidadeRequest request)
    {
        if (request.OrganismoOrigemId == request.OrganismoDestinoId)
        {
            return ResponseFactory.Fail<AtualizarSubsistemasEntidadeResponse>("Organismo de origem e destino iguais");
        }

        var origem = (await _repository.GetListAsync<SubsistemaServico, Guid>(
            new SubsistemasByOrganismoSpec(request.OrganismoOrigemId)
        )).ToList();

        var destino = (await _repository.GetListAsync<SubsistemaServico, Guid>(
            new SubsistemasByOrganismoSpec(request.OrganismoDestinoId)
        )).ToList();

        if (origem.Count == 0)
            return ResponseFactory.Fail<AtualizarSubsistemasEntidadeResponse>("Organismo de origem não tem subsistemas para atualizar.");

        var destinoKeys = destino
            .Select(x => $"{x.ServicoId:N}|{x.SubsistemaId:N}")
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var aCriar = new List<SubsistemaServico>();
        var jaExistiam = 0;

        foreach (var item in origem)
        {
            var key = $"{item.ServicoId:N}|{item.SubsistemaId:N}";
            if (destinoKeys.Contains(key))
            {
                jaExistiam++;
                continue;
            }

            aCriar.Add(new SubsistemaServico
            {
                Id = Guid.NewGuid(),
                ServicoId = item.ServicoId,
                SubsistemaId = item.SubsistemaId,
                OrganismoId = request.OrganismoDestinoId,
                ValorServico = item.ValorServico,
                ValorOrganismo = item.ValorOrganismo,
                MargemOrganismoPercent = item.MargemOrganismoPercent,
                ValorUtente = item.ValorUtente,
                MargemUtentePercent = item.MargemUtentePercent,
                Inativo = item.Inativo,
            });
        }

        if (aCriar.Count > 0)
            await _repository.CreateRangeAsync<SubsistemaServico, Guid>(aCriar);

        await _repository.SaveChangesAsync();

        return ResponseFactory.Success(new AtualizarSubsistemasEntidadeResponse
        {
            TotalOrigem = origem.Count,
            JaExistiamDestino = jaExistiam,
            CriadosDestino = aCriar.Count,
        });
    }
}