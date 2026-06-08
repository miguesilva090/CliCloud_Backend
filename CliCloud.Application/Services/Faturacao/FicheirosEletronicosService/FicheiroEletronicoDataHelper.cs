#nullable enable

using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas;
using CliCloud.Application.Services.Consultas.ConsultaService.Specifications;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;
using CliCloud.Application.Services.Servicos;
using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;
using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Faturacao;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService;

internal static class FicheiroEletronicoDataHelper
{
    public static void ValidarClinicaParaGeracao(Clinica clinica)
    {
        string nif = (clinica.NumeroContribuinte ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(nif))
            throw new InvalidOperationException("A clínica não tem um contribuinte associado");
        if(nif.Length != 9)
            throw new InvalidOperationException("O contribuinte da clínica deve ter 9 dígitos");

        _ = FicheiroEletronicoFormatHelper.ObterFilial(clinica.Sucursal);
    }

    public static async Task<Documento> ObterDocumentoOrganismoAsync(
        IRepositoryAsync repository,
        Guid clinicaId, 
        Guid documentoId,
        CancellationToken ct)
    {
        Documento? doc = ( 
            await repository.GetListAsync<Documento, Guid>(
                new DocumentoOrganismoParaFicheiroEletronicoSpec(documentoId, clinicaId),
                ct)
        ).FirstOrDefault();

        if( doc is null)
            throw new InvalidOperationException("Documento não encontrado");

        return doc;
    }

    public static async Task<Organismo> ObterOrganismoAsync(
        IRepositoryAsync repository,
        Guid organismoId,
        CancellationToken ct)
    {
        Organismo? organismo = ( 
            await repository.GetListAsync<Organismo, Guid>(
                new OrganismoComMoradaByIdSpec(organismoId),
                ct
            )
        ).FirstOrDefault();

        if (organismo is null)
            throw new InvalidOperationException("Organismo não encontrado");

        return organismo;
    }

    public static async Task<DateTime?> ObterUltimaGeracaoAsync(
        IRepositoryAsync repository,
        Guid clinicaId,
        FicheiroEletronicoSigla sigla, 
        CancellationToken ct)
    {
        FicheiroEletronicoRegisto? ultimo = (
            await repository.GetListAsync<FicheiroEletronicoRegisto, Guid>(
                new FicheiroEletronicoRegistosByClinicaSiglaSpec(clinicaId, sigla),
                ct)
            ).FirstOrDefault();
        return ultimo?.DataGeracao;
    }

    public static async Task<List<FicheiroEletronicoSadGnrLinhaDTO>> ObterLinhasSadGnrAsync(
        IRepositoryAsync repository,
        Guid documentoOrganismoId,
        Guid organismoId,
        CancellationToken ct)
    {
        List<DocumentoLinha> linhas = (
            await repository.GetListAsync<DocumentoLinha, Guid>(
                new DocumentoLinhasFicheiroEletronicoByDocumentoIdSpec(documentoOrganismoId),
                ct)
        ).ToList();
        
        if(linhas.Count == 0)
            throw new InvalidOperationException("A fatura não tem admissões associadas");

        var resultado = new List<FicheiroEletronicoSadGnrLinhaDTO>();

        foreach(DocumentoLinha linha in linhas)
        {
            AdmissaoServico srv = linha.AdmissaoServico!;
            Documento? docUtente = await ObterDocumentoUtentePorAdmissaoAsync(repository, srv.AdmissaoId, ct);
            if(docUtente is null) continue;

            decimal qtd = srv.Quantidade ?? linha.Quantidade;
            if(qtd <= 0) qtd = 1;

            (decimal valorServicoTotal, decimal valorUtenteTotal, decimal valorOrganismoTotal) =
                AdmissaoServicoValoresHelper.ResolverTotaisLinha(srv, qtd);

            resultado.Add(new FicheiroEletronicoSadGnrLinhaDTO{
                DocumentoUtenteId = docUtente.Id,
                NumeroDocumentoUtente = docUtente.NumeroDocumento,
                DataUtente = docUtente.Data ?? DateTime.Today,
                Beneficiario = (docUtente.Beneficiario ?? docUtente.NumeroContribuinteCliente ?? string.Empty).Trim(),
                ValorTotalReciboUtente = valorServicoTotal,
                ValorBeneficiarioReciboUtente = valorUtenteTotal,
                CodigoServico = CodigoServicoOrganismoHelper.Resolver(srv, linha),
                DataAtoMedico = srv.Admissao.Data ?? docUtente.Data ?? DateTime.Today,
                Quantidade = (int)Math.Round(qtd, MidpointRounding.AwayFromZero),
                ValorAtoMedico = valorServicoTotal,
                ValorBeneficiarioAtoMedico = valorUtenteTotal,
                ValorOrganismoAtoMedico = valorOrganismoTotal,
                Dente = srv.Dente ?? string.Empty,
            });
        }

        foreach(var grp in resultado.GroupBy(x => x.DocumentoUtenteId))
        {
            // Legado H2: SUM(PrecoUnitarioTotalArtigo*Qtd) e tfu.TotalFatura (= SUM TotalLinha utente).
            // O novo FR pode gravar TotalDocumento = valor_serv; usar totais da admissão (como D2).
            decimal totalLinhas = grp.Sum(x => x.ValorAtoMedico);
            decimal totalFaturaUtente = grp.Sum(x => x.ValorBeneficiarioAtoMedico);

            foreach (FicheiroEletronicoSadGnrLinhaDTO item in grp)
            {
                item.ValorTotalReciboUtente = totalLinhas;
                item.ValorBeneficiarioReciboUtente = totalFaturaUtente;
            }
        }

        if (resultado.Count == 0)
            throw new InvalidOperationException(
                "A fatura não tem recibos de utente (FR) emitidos para as admissões associadas. "
                + "Emita a fatura recibo na admissão antes de gerar o ficheiro eletrónico.");

        return resultado;
    }

    public static async Task<List<FicheiroEletronicoAdmLinhaDTO>> ObterLinhasAdmAsync(
        IRepositoryAsync repository,
        Documento documentoOrganismo,
        Guid organismoId,
        CancellationToken ct)
    
    {
        List<DocumentoLinha> linhas = (
            await repository.GetListAsync<DocumentoLinha, Guid>(
                new DocumentoLinhasFicheiroEletronicoByDocumentoIdSpec(documentoOrganismo.Id),
                ct)
            ).ToList();
        
        if(linhas.Count == 0)
            throw new InvalidOperationException("A fatura não tem admissões associadas");

        decimal totalFatura = documentoOrganismo.TotalDocumento ?? 0;
        var resultado = new List<FicheiroEletronicoAdmLinhaDTO>();

        foreach(DocumentoLinha linha in linhas)
        {
            AdmissaoServico srv = linha.AdmissaoServico!;
            decimal qtd = srv.Quantidade ?? linha.Quantidade;
            if (qtd <= 0) qtd = 1;

            Documento? docUtente = await ObterDocumentoUtentePorAdmissaoAsync(repository, srv.AdmissaoId, ct);

            resultado.Add(new FicheiroEletronicoAdmLinhaDTO
            {
                TotalFatura = totalFatura,
                Beneficiario = (docUtente?.Beneficiario ?? docUtente?.NumeroContribuinteCliente ?? string.Empty).Trim(),
                Data = srv.Admissao.Data ?? documentoOrganismo.Data ?? DateTime.Today,
                CodigoServico = CodigoServicoOrganismoHelper.Resolver(srv, linha),
                CodigoTratAdmiss = null,
                Comparticipacao = srv.DescInst ?? 0,
                Quantidade = (int)Math.Round(qtd, MidpointRounding.AwayFromZero),
                ValorPVP = (srv.ValorServico ?? srv.ValorArtigo ?? linha.PrecoUnitario) * qtd,
                ValorADM = (srv.ValorDesc ?? 0) * qtd,
                ValorBeneficiario = (srv.ValorUt ?? 0) * qtd,
            });
        }

        return resultado;
    }

    public static async Task<List<FicheiroEletronicoSadPspLinhaDTO>> ObterLinhasSadPspAsync(
        IRepositoryAsync repository,
        Guid documentoOrganismoId,
        Guid organismoId,
        CancellationToken ct)
        {
            List<DocumentoLinha> linhas = (
                await repository.GetListAsync<DocumentoLinha, Guid>(
                    new DocumentoLinhasFicheiroEletronicoByDocumentoIdSpec(documentoOrganismoId),
                    ct)
                ).ToList();
            
            if(linhas.Count == 0)
                throw new InvalidOperationException("A fatura não tem admissões associadas");

            var resultado = new List<FicheiroEletronicoSadPspLinhaDTO>();

            foreach(DocumentoLinha linha in linhas)
            {
                AdmissaoServico srv = linha.AdmissaoServico!;
                decimal qtd = srv.Quantidade ?? linha.Quantidade;
                if (qtd <= 0) qtd = 1;

                Documento? docUtente = await ObterDocumentoUtentePorAdmissaoAsync(repository, srv.AdmissaoId, ct);

                resultado.Add(new FicheiroEletronicoSadPspLinhaDTO
                {
                    Beneficiario = (docUtente?.Beneficiario ?? docUtente?.NumeroContribuinteCliente ?? string.Empty).Trim(),
                    CodigoServico = CodigoServicoOrganismoHelper.Resolver(srv, linha),
                    Data = srv.Admissao.Data ?? DateTime.Today,
                    ValorAtoMedico = (srv.ValorDesc ?? 0) * qtd,
                });
            }

            return resultado;
        }

    private static async Task<Documento?> ObterDocumentoUtentePorAdmissaoAsync(
        IRepositoryAsync repository,
        Guid admissaoId,
        CancellationToken ct)
    {
        // Legado ADMISS.CodigoFatura — recibo utente (FR), distinto da FA global ao organismo.
        List<DocumentoOrigemClinica> origens = (
            await repository.GetListAsync<DocumentoOrigemClinica, Guid>(
                new DocumentoOrigemClinicaByAdmissaoIdSpec(admissaoId),
                ct)
        ).ToList();

        Documento? recibo = origens
            .Select(o => o.Documento)
            .Where(EhDocumentoReciboUtenteEmitido)
            .OrderByDescending(d => d!.Data ?? d!.DataSistemaRegisto)
            .FirstOrDefault();

        if (recibo != null)
            return recibo;

        Consulta? consulta = (
            await repository.GetListAsync<Consulta, Guid>(
                new ConsultaByAdmissaoIdSpec(admissaoId),
                ct)
        ).FirstOrDefault();

        if (consulta != null)
        {
            ConsultaFaturacao? fat = (
                await repository.GetListAsync<ConsultaFaturacao, Guid>(
                    new ConsultaFaturacaoByConsultaId(consulta.Id),
                    ct)
            ).FirstOrDefault(x => x.DocumentoId != null);

            if (fat?.DocumentoId != null)
            {
                Documento? docFat = await repository.GetByIdAsync<Documento, Guid>(
                    fat.DocumentoId.Value,
                    cancellationToken: ct);

                if (EhDocumentoReciboUtenteEmitido(docFat))
                    return docFat;
            }
        }

        List<DocumentoLinha> linhas = (
            await repository.GetListAsync<DocumentoLinha, Guid>(
                new DocumentoLinhasReciboUtentePorAdmissaoSpec(admissaoId),
                ct)
        ).ToList();

        return linhas
            .Select(l => l.Documento)
            .Where(EhDocumentoReciboUtenteEmitido)
            .OrderByDescending(d => d!.Data ?? d!.DataSistemaRegisto)
            .FirstOrDefault();
    }

    private static bool EhDocumentoReciboUtenteEmitido(Documento? doc)
    {
        if (doc == null || doc.Anulado || doc.EstadoDocumento != EstadoDocumento.Emitido)
            return false;

        TipoDocumento? tipo = doc.TipoDocumento;
        if (tipo == null)
            return false;

        return DocumentoEmissaoAdmissaoFlagsHelper.IsFaturaRecibo(tipo);
    }
}