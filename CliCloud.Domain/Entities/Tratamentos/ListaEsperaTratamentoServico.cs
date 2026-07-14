#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Domain.Entities.Tratamentos;

[Table("ListaEsperaTratamentoServico", Schema = "Tratamentos")]
public class ListaEsperaTratamentoServico : AuditableEntityWithSoftDelete
{
    public Guid ListaEsperaTratamentoId { get; set; }
    public ListaEsperaTratamento ListaEsperaTratamento { get; set; } = null!;

    public Guid? ServicoId { get; set; }
    public Servico? Servico { get; set; }

    public Guid? SubsistemaServicoId { get; set; }

    public string? CodigoServico { get; set; }
    public string? Designacao { get; set; }
    public string? SubsistemaDesignacao { get; set; }
    public string? Duracao { get; set; }
    public int? IDuraca { get; set; }
    public int Ordem { get; set; }
}
