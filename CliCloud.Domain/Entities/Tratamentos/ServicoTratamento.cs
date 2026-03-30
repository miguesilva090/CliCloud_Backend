#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Domain.Entities.Tratamentos
{
  [Table("ServicoTratamento", Schema = "Tratamentos")]
  public class ServicoTratamento : AuditableEntity
  {
    // Relacionamento com Tratamento
    public Guid TratamentoId { get; set; }
    public Tratamento Tratamento { get; set; } = null!;
    
    // Relacionamento com Servico (normalizado)
    public Guid? ServicoId { get; set; }
    public Servico? Servico { get; set; }
    
    // Duração (pode sobrescrever duração base do serviço)
    public string? Duracao { get; set; }
    public int? IDuraca { get; set; }
    
    // Ordem e uso de técnicos
    public int? Ordem { get; set; }
    public int? UsaFisioter { get; set; }
    public int? UsaAuxiliar { get; set; }
    public int? UsaOutro { get; set; }
    
    // Financeiro
    public decimal? Preco { get; set; }
    public decimal? DescInst { get; set; }
    public decimal? ValorDesc { get; set; }
    public decimal? ValorUt { get; set; }
    
    // Outros campos
    public string? Obs { get; set; }
    public Guid? SessaoTratamentoId { get; set; }
  }
}
