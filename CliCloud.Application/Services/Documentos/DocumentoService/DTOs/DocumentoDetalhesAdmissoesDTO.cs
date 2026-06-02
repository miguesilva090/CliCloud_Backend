#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs;

public sealed class DocumentoAdmissaoDetalheDTO : IDto
{
    public Guid? AdmissaoId { get; set; }
    public Guid? ConsultaId { get; set; }
    public string Origem { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}

public sealed class DocumentoDetalhesAdmissoesDTO : IDto
{
    public Guid DocumentoId { get; set; }
    public List<DocumentoAdmissaoDetalheDTO> Itens { get; set; } = [];
}
