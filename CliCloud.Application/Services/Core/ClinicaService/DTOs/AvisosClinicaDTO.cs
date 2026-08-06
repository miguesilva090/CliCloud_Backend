using System.Text.Json.Serialization;

namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class AvisosClinicaDTO
  {
    [JsonPropertyName("msg_falta_pagamento")]
    public string? MsgFaltaPagamento { get; set; }

    [JsonPropertyName("msg_credenciais")]
    public string? MsgCredenciais { get; set; }
  }
}
