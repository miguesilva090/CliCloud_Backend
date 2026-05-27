#nullable enable

using System.ComponentModel.DataAnnotations;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class CriarNotaCreditoLinhaRequest : IDto 
{
    [Required]
    public required Guid DocumentoLinhaOrigemId { get; set; }
    [Range(0.0000001, double.MaxValue)]
    public decimal Quantidade { get; set; }
    public decimal? PrecoUnitario { get; set; }
    public decimal? TaxaIvaPercentagem { get; set; }
    
}