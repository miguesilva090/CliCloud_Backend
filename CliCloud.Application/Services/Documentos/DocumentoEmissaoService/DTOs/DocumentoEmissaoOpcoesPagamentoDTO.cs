#nullable enable

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class DocumentoEmissaoOpcoesPagamentoDTO
{
    public List<PagamentoOpcaoDTO> CondicoesPagamento { get; set; } = [];
    public List<PagamentoOpcaoDTO> ModosPagamento { get; set; } = [];
    public List<OpcaoTextoDTO> TiposSerie { get; set; } = [];
    public List<OpcaoTextoDTO> ImpostosRetencao { get; set; } = [];
    public List<PagamentoOpcaoDTO> ReferenciasMb { get; set; } = [];
}
