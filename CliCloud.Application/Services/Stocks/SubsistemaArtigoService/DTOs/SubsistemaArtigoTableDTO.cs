using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService.DTOs;

public class SubsistemaArtigoTableDTO : IDto
{
    public Guid Id { get; set; }
    public Guid ArtigoId { get; set; }
    public Guid OrganismoId { get; set; }
    public string CodigoCartaoInstituicao { get; set; } = string.Empty;
    public string? ArtigoNumero { get; set; }
    public string? ArtigoDescricao { get; set; }
    public string? OrganismoNome { get; set; }
    public decimal ValorServico { get; set; }
    public decimal MargemOrganismoPercent { get; set; }
    public decimal ValorOrganismo { get; set; }
    public decimal ValorUtente { get; set; }
    public bool Inativo { get; set; }
    public DateTime CreatedOn { get; set; }
}
