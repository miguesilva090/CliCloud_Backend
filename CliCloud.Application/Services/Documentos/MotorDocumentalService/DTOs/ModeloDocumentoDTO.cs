using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums.Documentos;

namespace CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;


public class ModeloDocumentoDTO: IDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public TipoModeloDocumento Tipo { get; set; }
    public int Versao { get; set; }
    public bool Ativo { get; set; }
    public string ConteudoHtml { get; set; } = string.Empty;
    

}