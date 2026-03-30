using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs
{
  public class PatologiaDTO : IDto
  {
    public Guid Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public Guid? LocalTratamentoId { get; set; }
    public string? LocalTratamentoDesignacao { get; set; }
    public Guid? OrganismoId { get; set; }
    public string? OrganismoNome { get; set; }
    public string? EspecificacaoTecnica { get; set; }
    public string? Doencas { get; set; }
    public bool Inativo { get; set; }
    public IEnumerable<PatologiaServicoDTO>? PatologiaServicos { get; set; }
    public IEnumerable<Guid>? DoencaIds { get; set; }
  }
}
