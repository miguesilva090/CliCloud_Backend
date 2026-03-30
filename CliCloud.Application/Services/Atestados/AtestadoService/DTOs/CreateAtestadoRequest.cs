using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Atestados.AtestadoService.DTOs
{
  public class CreateAtestadoRequest : IDto
  {
    public Guid UtenteId { get; set; }
    public Guid MedicoId { get; set; }
    public Guid ClinicaId { get; set; }
    public Guid? CodigoPostalId { get; set; }
    public DateTime DataAtestado { get; set; }
    public string? NumeroSPMS { get; set; }
    public string? Observacoes { get; set; }
    public string? NumeroSNS { get; set; }

    public List<CreateAtestadoCategoriaItem> Categorias { get; set; } = [];
    public List<CreateAtestadoRestricaoItem> Restricoes { get; set; } = [];
    public List<CreateAtestadoRestricaoAnteriorItem> RestricoesAnteriores { get; set; } = [];
  }

  public class CreateAtestadoCategoriaItem : IDto
  {
    public Guid CartaConducaoId { get; set; }
    public int Apto { get; set; }
    public int AptoGrupo2 { get; set; }
  }

  public class CreateAtestadoRestricaoItem : IDto
  {
    public Guid CartaConducaoRestricaoId { get; set; }
    public Guid CartaConducaoId { get; set; }
    public string? Anotacoes { get; set; }
  }

  public class CreateAtestadoRestricaoAnteriorItem : IDto
  {
    public Guid CartaConducaoRestricaoId { get; set; }
    public string? Anotacoes { get; set; }
  }
}
