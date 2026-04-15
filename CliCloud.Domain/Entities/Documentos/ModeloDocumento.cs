using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Enums.Documentos;

namespace CliCloud.Domain.Entities.Documentos;

public class ModeloDocumento : AuditableEntity
{
    public Guid ClinicaId { get; set; }

    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public TipoModeloDocumento Tipo { get; set; } = TipoModeloDocumento.Generico;
    public EstadoModeloDocumento Estado { get; set; } = EstadoModeloDocumento.Rascunho;

    public int Versao { get; set; } = 1;
    public bool Ativo { get; set; } = true;

    public string ConteudoHtml { get; set; } = string.Empty;
}