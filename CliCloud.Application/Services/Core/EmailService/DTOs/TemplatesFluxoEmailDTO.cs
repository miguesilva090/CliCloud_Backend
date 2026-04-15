using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.EmailService.DTOs;

public class TemplatesFluxoEmailItemDTO 
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Assunto { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public int Ativo { get; set; } = 1;
}

public class TemplatesFluxoEmailDTO
{
    public List<TemplatesFluxoEmailItemDTO> Templates { get; set; } = [];
}