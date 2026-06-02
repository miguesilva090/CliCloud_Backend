using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;
using CliCloud.Domain.Entities.Core.ConfigReferenciaMB;
using CliCloud.Domain.Entities.Faturacao;
using Microsoft.Extensions.Configuration;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService;

public partial class ReferenciasMbService
{
    private static readonly HttpClient IfThenHttp = new() { Timeout = TimeSpan.FromSeconds(60) };

    public async Task<Response<ReferenciaMbGeradaDTO>> GerarParaDocumentoAsync(GerarReferenciaDocumentoRequest request)
    {
        if (request.Modo is not (1 or 2))
            return ResponseFactory.Fail<ReferenciaMbGeradaDTO>("Modo de referência MB inválido.");

        if (request.Valor <= 0)
            return ResponseFactory.Fail<ReferenciaMbGeradaDTO>("O valor do documento deve ser superior a zero.");

        ConfigReferenciaMB? config;
        try
        {
            IEnumerable<ConfigReferenciaMB> all = await _repository.GetListAsync<ConfigReferenciaMB, Guid>();
            config = all.FirstOrDefault(x => x.ClinicaId == request.ClinicaId);
        }
        catch (Exception ex) when (IsMissingConfigTable(ex))
        {
            return ResponseFactory.Fail<ReferenciaMbGeradaDTO>("Configure o serviço de Referências MB na clínica.");
        }

        if (config == null || string.IsNullOrWhiteSpace(config.SubEntidade))
            return ResponseFactory.Fail<ReferenciaMbGeradaDTO>("Configure o serviço de Referências MB (entidade/sub-entidade IfThenPay).");

        if (request.Valor < config.ValorMinimo)
        {
            return ResponseFactory.Fail<ReferenciaMbGeradaDTO>(
                $"O valor mínimo para gerar referência MB é {config.ValorMinimo.ToString("N2", CultureInfo.GetCultureInfo("pt-PT"))} €.");
        }

        string orderId = !string.IsNullOrWhiteSpace(request.NumeroExibicao)
            ? request.NumeroExibicao.Trim()
            : request.DocumentoId.ToString("N")[..8];

        string descricao = $"Relativo ao documento {orderId}";
        int prazo = config.PrazoPagamento > 0 ? config.PrazoPagamento : 30;

        bool sandbox = string.Equals(
            _configuration["ReferenciaMb:IfThenSandbox"],
            "true",
            StringComparison.OrdinalIgnoreCase);

        string url = sandbox
            ? "https://api.ifthenpay.com/multibanco/reference/sandbox/init"
            : "https://api.ifthenpay.com/multibanco/reference/init";

        try
        {
            using HttpResponseMessage response = await IfThenHttp.PostAsJsonAsync(
                url,
                new
                {
                    mbKey = config.SubEntidade.Trim(),
                    orderId,
                    amount = request.Valor,
                    description = descricao,
                    expiryDays = prazo,
                });

            string body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return ResponseFactory.Fail<ReferenciaMbGeradaDTO>($"Serviço IfThenPay: {body}");

            IfThenReferenciaResponse? parsed = System.Text.Json.JsonSerializer.Deserialize<IfThenReferenciaResponse>(
                body,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (parsed == null || !string.Equals(parsed.Message, "Success", StringComparison.OrdinalIgnoreCase))
                return ResponseFactory.Fail<ReferenciaMbGeradaDTO>("Resposta inválida do serviço IfThenPay.");

            DateTime? dataLimite = ParseExpiry(parsed.ExpiryDate);
            bool mbWay = request.Modo == 2;

            ReferenciaMB reg = new()
            {
                ClinicaId = request.ClinicaId,
                DocumentoId = request.DocumentoId,
                UtenteId = request.UtenteId,
                ClienteNome = request.ClienteNome.Trim(),
                Descricao = descricao,
                Mensagem = mbWay ? "Pedido MB Way" : descricao,
                EntidadeMb = mbWay ? null : parsed.Entity,
                ReferenciaCodigo = mbWay ? null : parsed.Reference,
                Valor = request.Valor,
                DataReferenciaGerada = DateTime.Now,
                DataLimitePagamento = dataLimite,
                RequestId = parsed.RequestId,
                CodigoEmpresaServico = 1,
                Liquidada = false,
                Anulada = false,
            };

            _ = await _repository.CreateAsync<ReferenciaMB, Guid>(reg);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(new ReferenciaMbGeradaDTO
            {
                ReferenciaId = reg.Id,
                EntidadeMb = reg.EntidadeMb,
                ReferenciaCodigo = reg.ReferenciaCodigo,
                RequestId = reg.RequestId,
                Valor = reg.Valor,
                DataLimitePagamento = reg.DataLimitePagamento,
                MbWay = mbWay,
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<ReferenciaMbGeradaDTO>(ex.Message);
        }
    }

    private static DateTime? ParseExpiry(string? expiry)
    {
        if (string.IsNullOrWhiteSpace(expiry))
            return null;

        string[] parts = expiry.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 3
            && int.TryParse(parts[0], out int d)
            && int.TryParse(parts[1], out int m)
            && int.TryParse(parts[2], out int y))
        {
            return new DateTime(y, m, d);
        }

        return DateTime.TryParse(expiry, out DateTime dt) ? dt : null;
    }

    private sealed class IfThenReferenciaResponse
    {
        [JsonPropertyName("Message")]
        public string? Message { get; set; }

        [JsonPropertyName("Entity")]
        public string? Entity { get; set; }

        [JsonPropertyName("Reference")]
        public string? Reference { get; set; }

        [JsonPropertyName("RequestId")]
        public string? RequestId { get; set; }

        [JsonPropertyName("ExpiryDate")]
        public string? ExpiryDate { get; set; }
    }
}
