#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Domain.Entities.Tratamentos
{
  [Table("ServicoSessao", Schema = "Tratamentos")]
  public class ServicoSessao : AuditableEntityWithSoftDelete
  {
    // Relacionamento com SessaoTratamento
    public Guid SessaoTratamentoId { get; set; }
    public SessaoTratamento SessaoTratamento { get; set; } = null!;
    
    // Relacionamentos com técnicos
    public Guid? FisioterapeutaId { get; set; }
    public Tecnico? Fisioterapeuta { get; set; }
    
    public Guid? AuxiliarId { get; set; }
    public Tecnico? Auxiliar { get; set; }
    
    // Relacionamento com Servico (normalizado)
    public Guid? ServicoId { get; set; }
    public Servico? Servico { get; set; }
    
    // Horários
    public string? HoraInic { get; set; }
    public int? IHoraIni { get; set; }
    public string? HoraFim { get; set; }
    public int? IHoraFim { get; set; }
    
    // Duração
    public string? Duracao { get; set; }
    public int? IDuraca { get; set; }
    
    // Ordem e aparelho
    public int? Ordem { get; set; }
    public Guid? AparelhoId { get; set; }
    public Aparelho? Aparelho { get; set; }
    
    // Financeiro
    public decimal? Preco { get; set; }
    public decimal? DescInst { get; set; }
    public decimal? ValorDesc { get; set; }
    public decimal? ValorUt { get; set; }
    
    // Outros campos
    public string? Obs { get; set; }
  }
}
