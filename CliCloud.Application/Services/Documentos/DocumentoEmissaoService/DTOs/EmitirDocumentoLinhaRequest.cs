#nullable enable 

using System.ComponentModel.DataAnnotations;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class EmitirDocumentoLinhaRequest : IDto
{
    public int NumeroLinha { get; set; }

    [Required]
    public required string Descricao { get; set; }

    public string? CodigoArtigo { get; set; }

    public Guid? ServicoId { get; set; }
    public Guid? AdmissaoServicoId { get; set; }

    [Range(0.0000001, double.MaxValue)]
    public decimal Quantidade { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PrecoUnitario { get; set; }

    public decimal? PercentagemDesconto { get; set; }
    public decimal? ValorDesconto { get; set; }

    public decimal? DescontoTipo1 { get; set; }
    public decimal? DescontoTipo2 { get; set; }
    public decimal? DescontoTipo3 { get; set; }
    public Guid? TaxaIvaId { get; set; }
    public Guid? MotivoIsencaoId { get; set; }
    
    [Range(0, 100)]
    public decimal TaxaIvaPercentagem { get; set; }
}