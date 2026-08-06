#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas;

[Table("RequisicaoEsp", Schema = "Consultas")]
public class RequisicaoEsp : AuditableEntityWithSoftDelete
{
    public int Codigo { get; set; }

    [StringLength(20)]
    public string NumeroRequisicao { get; set; } = string.Empty;

    [StringLength(10)]
    public string? CodigoMedico { get; set; }

    public bool EnpAssinado { get; set; }

    public bool Historico { get; set; }

    public int Estado { get; set; }

    public DateTime DataCativacao { get; set; }

    public DateTime? DataAgendamento { get; set; }

    public DateTime? DataServico { get; set; }

    public DateTime? DataRealizacao { get; set; }

    public DateTime UltimaData { get; set; }

    public ICollection<RequisicaoEspLinha> Linhas { get; set; } = [];

    public ICollection<RequisicaoEspEfetuadoNaoPrescrito> EfetuadosNaoPrescritos { get; set; } = [];
}
