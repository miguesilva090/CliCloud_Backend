using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.ExameService.DTOs
{
  public class ExameDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public Guid MedicoId { get; set; }
    public DateTime DataPrescricao { get; set; }
    public Guid? PrioridadeId { get; set; }
    public string? NumeroPrescricao { get; set; }
    public Guid? OrganismoId { get; set; }
    public string? Observacoes { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public List<ExameLinhaDTO>? Linhas { get; set; }
  }
}
