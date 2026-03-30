using System.Text.Json.Serialization;

namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class AvisosClinicaLegacyDTO
  {
    // Legacy: msg_falta_pagamento
    [JsonPropertyName("msg_falta_pagamento")]
    public string? MsgFaltaPagamento { get; set; }

    // Legacy: msg_credenciais (legacy: MsgApresentarCredenciais)
    [JsonPropertyName("msg_credenciais")]
    public string? MsgCredenciais { get; set; }
  }
}

