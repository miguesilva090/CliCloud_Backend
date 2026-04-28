using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.ReplicarPatologiasService.DTOs;
using CliCloud.Application.Services.Utility.ReplicarPatologiasService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Utility.ReplicarPatologiasService;

public class ReplicarPatologiasService(IRepositoryAsync repository) : IReplicarPatologiasService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ReplicarPatologiasResponse>> ReplicarPatologiasAsync(ReplicarPatologiasRequest request)
    {
        if (request.OrganismoOrigemId == request.OrganismoDestinoId)
            return ResponseFactory.Fail<ReplicarPatologiasResponse>("Organismo de origem e destino iguais");

        var srcSpec = new PatologiasByOrganismoWithIncludesSpec(request.OrganismoOrigemId);
        var dstSpec = new PatologiasByOrganismoWithIncludesSpec(request.OrganismoDestinoId);

        var origem = (await _repository.GetListAsync<Patologia, Guid>(srcSpec)).ToList();
        var destino = (await _repository.GetListAsync<Patologia, Guid>(dstSpec)).ToList();

        var existingByDesignacao = destino 
            .GroupBy(x => x.Designacao.Trim().ToLower())
            .ToDictionary(g => g.Key, g => g.First());

        var result = new ReplicarPatologiasResponse{ Total = origem.Count };

        foreach ( var p in origem)
        {
            var key = p.Designacao.Trim().ToLower();
            if(existingByDesignacao.ContainsKey(key) && !request.SubstituirExistentes)
            {
                result.Ignoradas++;
                result.Itens.Add(new() { PatologiaOrigemId = p.Id, Designacao = p.Designacao, Estado = "Ignorada", Mensagem = "Patologia já existe no destino "});
                continue;
            }

            var novaId = Guid.NewGuid();
            var nova = new Patologia 
            {
                Id = novaId,
                Designacao = p.Designacao,
                LocalTratamentoId = p.LocalTratamentoId,
                OrganismoId = request.OrganismoDestinoId,
                EspecificacaoTecnica = p.EspecificacaoTecnica,
                Doencas = p.Doencas,
                Inativo = p.Inativo,
            };
            await _repository.CreateAsync<Patologia, Guid>(nova);

            foreach ( var ps in p.PatologiaServicos)
            {
                await _repository.CreateAsync<PatologiaServico, Guid>(new PatologiaServico 
                { 
                    Id = Guid.NewGuid(),
                    PatologiaId = novaId,
                    SubsistemaServicoId = ps.SubsistemaServicoId,
                    Duracao = ps.Duracao,
                    Ordem = ps.Ordem,
                    Fisioterapia = ps.Fisioterapia,
                    Auxiliar = ps.Auxiliar,
                    ValorUtente = ps.ValorUtente,
                    ValorOrganismo = ps.ValorOrganismo,
                    PercentagemInstituicao = ps.PercentagemInstituicao,
                    PrecoEur = ps.PrecoEur,
                    Observacoes = ps.Observacoes

                });
            }

            foreach ( var pd in p.PatologiaDoencas)
                nova.PatologiaDoencas.Add(new PatologiaDoenca
                {
                    PatologiaId = novaId,
                    DoencaId = pd.DoencaId
                });

            result.Criadas++;
            result.Itens.Add(new() { PatologiaOrigemId = p.Id, PatologiaDestinoId = novaId, Designacao = p.Designacao, Estado = "Criada"});
        }

        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(result);
            
    }
}