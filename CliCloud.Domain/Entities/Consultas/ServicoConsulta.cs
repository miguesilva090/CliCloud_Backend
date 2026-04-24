#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Domain.Entities.Consultas
{
  [Table("ServicoConsulta", Schema = "Consultas")]
  public class ServicoConsulta : AuditableEntityWithSoftDelete
  {
    // Relacionamento com Consulta
    public Guid ConsultaId { get; set; }
    public Consulta Consulta { get; set; } = null!;
    
    // Relacionamento com Servico (normalizado)
    public Guid? ServicoId { get; set; }
    public Servico? Servico { get; set; }
    
    // Valor do serviço (pode sobrescrever preço base do serviço)
    public decimal? ValorServico { get; set; }
    
    // Artigo
    public string? CodigoArtigo { get; set; }
    public string? NomeArtigo { get; set; }
    public decimal? ValorArtigo { get; set; }
    public decimal? Quantidade { get; set; }
    
    // Margens e receitas
    public decimal? MargemMed { get; set; }
    public decimal? MargemIns { get; set; }
    public decimal? RecMed { get; set; }
    public decimal? RecInst { get; set; }
    
    // Descontos
    public decimal? DescInst { get; set; }
    public decimal? DescCli { get; set; }
    public decimal? ValorDesc { get; set; }
    
    // Outros campos
    public int? Ordem { get; set; }
    public string? Dente { get; set; }
    public Guid? ExameId { get; set; }
    public Exame? Exame { get; set; }
    public int Linha { get; set; }
    public string? NCheque { get; set; }
    public int? Electrocardiograma { get; set; }
    public decimal? ValorUt { get; set; }
  }
}
