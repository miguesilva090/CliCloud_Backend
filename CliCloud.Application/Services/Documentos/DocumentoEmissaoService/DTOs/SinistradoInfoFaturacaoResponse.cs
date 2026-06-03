#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class SinistradosInfoFaturacaoResponse : IDto 
{
    public Guid SinistradoId { get; set; }
    public string CodigoSinistro { get; set; } = string.Empty;
    public Guid OrganismoId { get; set; }
    public Guid? UtenteId { get; set; }
    public SinistradosInfoFaturacaoClienteDTO? Cliente { get; set; } = new();
    public string LinhaObservacao { get; set; } = string.Empty;
    public string? NumeroProcesso { get; set; }
    public List<SinistradosInfoFaturacaoLinhaDTO> Linhas { get; set; } = [];

}

public class SinistradosInfoFaturacaoClienteDTO 
{
    public string Nome { get; set; } = string.Empty;
    public string Morada { get; set; } = string.Empty;
    public string? Localidade { get; set; }
    public string? NumeroContribuinte { get; set; }
    public Guid? CodigoPostalId { get; set; }
}

public class SinistradosInfoFaturacaoLinhaDTO
{
    public Guid SinistradoLinhaServicoId { get; set; }
    public Guid? ServicoId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? CodigoServico { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoUtente { get; set; }
    public decimal PrecoOrganismo { get; set; }
    public Guid? TaxaIvaId { get; set; }
    public decimal TaxaIvaPercentagem { get; set; }
    public Guid? MotivoIsencaoId { get; set; }
}
