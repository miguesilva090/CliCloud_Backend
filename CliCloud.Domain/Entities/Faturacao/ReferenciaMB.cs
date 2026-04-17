using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Faturacao;

[Table("ReferenciaMB", Schema = "Faturacao")]
public class ReferenciaMB : AuditableEntity
{
    public Guid ClinicaId { get; set; }

    public Guid? UtenteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;
    public string? Mensagem { get; set; }

    public string? EntidadeMb { get; set; }
    public string? ReferenciaCodigo { get; set; }
    
    public decimal Valor { get; set; }
    public DateTime DataReferenciaGerada { get; set; }
    public DateTime? DataLimitePagamento { get; set; }
    public DateTime? DataPagamento { get; set; }

    public bool Liquidada { get; set; }
    public bool Anulada { get; set; }

    public string? RequestId { get; set; }
    public int CodigoEmpresaServico { get; set; } = 1;

}