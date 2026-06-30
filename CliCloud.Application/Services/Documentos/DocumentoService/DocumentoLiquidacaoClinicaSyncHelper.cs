using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.ConsultaService.Specifications;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoService;

/// <summary>
/// Paridade legado: liquidar fatura → marcar admissão(ões) como pagas.
/// </summary>
internal static class DocumentoLiquidacaoClinicaSyncHelper
{
    public static async Task SincronizarAposLiquidacaoAsync(
        IRepositoryAsync repository,
        Guid documentoFaturaId,
        Guid clinicaId,
        CancellationToken cancellationToken = default)
    {
        HashSet<Guid> admissaoIds = await ResolverAdmissaoIdsAsync(
            repository,
            documentoFaturaId,
            clinicaId,
            cancellationToken);

        if (admissaoIds.Count == 0)
        {
            return;
        }

        foreach (Guid admissaoId in admissaoIds)
        {
            List<Admissao> admList = (
                await repository.GetListAsync<Admissao, Guid>(
                    new AdmissaoByIdWithServicosSpec(admissaoId),
                    cancellationToken)
            ).ToList();

            Admissao? admissao = admList.FirstOrDefault();
            if (admissao == null)
            {
                continue;
            }

            admissao.Pago = true;
            _ = await repository.UpdateAsync<Admissao, Guid>(admissao);

            Consulta? consulta = (
                await repository.GetListAsync<Consulta, Guid>(
                    new ConsultaPorAdmissaoSpec(admissaoId),
                    cancellationToken)
            ).FirstOrDefault();

            if (consulta == null)
            {
                continue;
            }

            List<ConsultaFaturacao> faturacoes = (
                await repository.GetListAsync<ConsultaFaturacao, Guid>(
                    new ConsultaFaturacaoByConsultaId(consulta.Id),
                    cancellationToken)
            ).ToList();

            foreach (ConsultaFaturacao faturacao in faturacoes)
            {
                faturacao.Pago = true;
                _ = await repository.UpdateAsync<ConsultaFaturacao, Guid>(faturacao);
            }
        }
    }

    private static async Task<HashSet<Guid>> ResolverAdmissaoIdsAsync(
        IRepositoryAsync repository,
        Guid documentoFaturaId,
        Guid clinicaId,
        CancellationToken ct)
    {
        HashSet<Guid> ids = [];

        List<DocumentoOrigemClinica> origens = (
            await repository.GetListAsync<DocumentoOrigemClinica, Guid>(
                new DocumentoOrigemClinicaByDocumentoIdSpec(documentoFaturaId),
                ct)
        ).ToList();

        foreach (DocumentoOrigemClinica origem in origens)
        {
            if (origem.AdmissaoId is Guid admissaoId && admissaoId != Guid.Empty)
            {
                _ = ids.Add(admissaoId);
            }
        }

        List<ConsultaFaturacao> faturacoesDocumento = (
            await repository.GetListAsync<ConsultaFaturacao, Guid>(
                new ConsultaFaturacaoByDocumentoIdSpec(documentoFaturaId),
                ct)
        ).ToList();

        foreach (ConsultaFaturacao fat in faturacoesDocumento)
        {
            if (!fat.ConsultaId.HasValue)
            {
                continue;
            }

            Admissao? admissao = (
                await repository.GetListAsync<Admissao, Guid>(
                    new AdmissaoByConsultaIdSpec(fat.ConsultaId.Value),
                    ct)
            ).FirstOrDefault();

            if (admissao != null)
            {
                _ = ids.Add(admissao.Id);
            }
        }

        Documento? documento = (
            await repository.GetListAsync<Documento, Guid>(
                new DocumentoByIdWithLinhasSpec(documentoFaturaId, clinicaId),
                ct)
        ).FirstOrDefault();

        List<Guid> servicoIds = (documento?.Linhas ?? [])
            .Where(l => l.AdmissaoServicoId.HasValue && l.AdmissaoServicoId.Value != Guid.Empty)
            .Select(l => l.AdmissaoServicoId!.Value)
            .Distinct()
            .ToList();

        if (servicoIds.Count == 0)
        {
            return ids;
        }

        List<AdmissaoServico> servicos = (
            await repository.GetListAsync<AdmissaoServico, Guid>(
                new AdmissaoServicoByIdsSpec(servicoIds),
                ct)
        ).ToList();

        foreach (AdmissaoServico servico in servicos)
        {
            if (servico.AdmissaoId != Guid.Empty)
            {
                _ = ids.Add(servico.AdmissaoId);
            }
        }

        return ids;
    }
}
