using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;

public class ListaEsperaTratamentoServicoDTO : IDto
{
    public Guid Id { get; set; }
    public Guid? ServicoId { get; set; }
    public Guid? SubsistemaServicoId { get; set; }
    public string? CodigoServico { get; set; }
    public string? Designacao { get; set; }
    public string? SubsistemaDesignacao { get; set; }
    public string? Duracao { get; set; }
    public int Ordem { get; set; }
}

public class ListaEsperaTratamentoServicoRequest : IDto
{
    public Guid? ServicoId { get; set; }
    public Guid? SubsistemaServicoId { get; set; }
    public string? CodigoServico { get; set; }
    public string? Designacao { get; set; }
    public string? SubsistemaDesignacao { get; set; }
    public string? Duracao { get; set; }
    public int Ordem { get; set; }
}
