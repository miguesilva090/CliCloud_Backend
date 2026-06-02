#nullable enable

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class PagamentoOpcaoDTO
{
    public int Valor { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
