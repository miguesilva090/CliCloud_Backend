namespace CliCloud.Application.Services.Core.EmailService.DTOs;

public class AtualizarTemplatesFluxoEmailItemRequest 
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Assunto { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public int Ativo { get; set; } = 1;
}

public class AtualizarTemplatesFluxoEmailRequest 
{
    public List<AtualizarTemplatesFluxoEmailItemRequest> Templates { get; set; } = [];
}