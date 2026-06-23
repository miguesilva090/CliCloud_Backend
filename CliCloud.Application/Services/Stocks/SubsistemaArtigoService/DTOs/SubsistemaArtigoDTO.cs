using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService.DTOs;

public class SubsistemaArtigoDTO : IDto 
{
    public Guid Id { get; set; }
    public Guid ArtigoId { get; set; }
    public Guid OrganismoId { get; set; }
    public string CodigoCartaoInstituicao { get; set; } = string.Empty;
    public decimal ValorServico { get; set; }
    public decimal MargemOrganismoPercent { get; set; }
    public decimal ValorOrganismo { get; set; }
    public decimal ValorUtente { get; set; }
    public bool Inativo { get; set; }
    public string? CodigoComplementarAdse { get; set; }

    public int? ArtigoCodigo { get; set; }
    public string? ArtigoNumero { get; set; }
    public string? ArtigoDescricao { get; set; }
    public string? OrganismoNome { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}