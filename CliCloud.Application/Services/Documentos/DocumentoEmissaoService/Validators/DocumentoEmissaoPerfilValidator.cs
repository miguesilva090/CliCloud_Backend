#nullable enable

using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;
using System.Linq;
using CliCloud.Domain.Entities.Documentos;
using DocumentoEmissaoCalculoHelper = CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DocumentoEmissaoCalculoHelper;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Validators;

public static class DocumentoEmissaoPerfilValidator
{
    public static string? Validar(
        TipoDocumento tipo,
        EmitirDocumentoRequest request,
        int regraFaturacao
    )
    {
        string abrev = tipo.Abreviatura.Trim().ToUpperInvariant();

        if (abrev is "GT" or "GR")
        {
            if (!request.DataTransporte.HasValue)
                return "Data de transporte é obrigatória para guias de transporte.";
            if (string.IsNullOrWhiteSpace(request.HoraTransporte))
                return "Hora de transporte é obrigatória.";
        }

        if (abrev.StartsWith("NC", StringComparison.Ordinal) ||
            abrev.StartsWith("DV", StringComparison.Ordinal))
        {
            if (!request.DocumentoOrigemId.HasValue &&
                string.IsNullOrWhiteSpace(request.IdentificadorUnicoDocumentoOrigem))
                return "Nota de crédito exige documento de origem.";
        }

        if (request.RetencaoAtiva)
        {
            if (string.IsNullOrWhiteSpace(request.RetencaoImposto))
                return "Indique o imposto da retenção na fonte.";
            decimal retTaxa = request.RetencaoTaxa ?? 0m;
            decimal retValor = request.RetencaoValor ?? 0m;
            if (retTaxa <= 0m && retValor <= 0m)
                return "Indique valor ou taxa da retenção.";
            if (string.IsNullOrWhiteSpace(request.RetencaoMotivo))
                return "Indique o motivo da retenção na fonte.";
        }

        if (request.IsentoIva && !request.MotivoIsencaoId.HasValue)
            return "Indique o motivo de isenção de IVA.";

        foreach (var linha in request.Linhas)
        {
            if (linha.TaxaIvaPercentagem == 0m
                && !linha.MotivoIsencaoId.HasValue
                && !request.MotivoIsencaoId.HasValue)
            {
                return $"Linha {linha.NumeroLinha}: indique o motivo de isenção (taxa 0%).";
            }
        }

        if (request.RetencaoAtiva
            && !request.RetencaoCodigoMotivo.HasValue
            && string.IsNullOrWhiteSpace(request.RetencaoMotivo))
        {
            return "Indique o motivo da retenção na fonte.";
        }

        if (request.GerarReferenciaMb is 1 or 2)
        {
            decimal totalEstimado = DocumentoEmissaoCalculoHelper.CalcularTotalEstimadoAPagar(
                request,
                regraFaturacao
            );
            if (totalEstimado <= 0)
                return "O total do documento deve ser superior a zero para gerar referência MB.";
        }

        if (string.IsNullOrWhiteSpace(request.NomeCliente))
            return "Nome do cliente é obrigatório.";
        if (string.IsNullOrWhiteSpace(request.MoradaCliente))
            return "Morada do cliente é obrigatória.";

        string? nif = request.NumeroContribuinteCliente?.Trim();
        if (
            !string.IsNullOrEmpty(nif)
            && (nif == "123456789" || nif == "999999990")
        )
        {
            decimal mercadorias = DocumentoEmissaoCalculoHelper
                .CalcularTotaisDocumento(
                    request
                        .Linhas.Select(l =>
                            DocumentoEmissaoCalculoHelper.CalcularLinha(
                                l,
                                regraFaturacao,
                                request.DescontoCliente ?? 0m,
                                request.DescontoPagamento ?? 0m,
                                request.PercentagemDescontoGlobal,
                                request.IsentoIva
                            )
                        )
                        .ToList(),
                    regraFaturacao,
                    request.Outros ?? 0m,
                    request.RetencaoAtiva ? request.RetencaoValor ?? 0m : 0m
                )
                .Mercadorias;

            if (mercadorias > 1000m)
            {
                return "Consumidor final: total de mercadorias/serviços superior a 1000€ com NIF 999999990 ou 123456789 não permitido.";
            }
        }

        return null;
    }
}
