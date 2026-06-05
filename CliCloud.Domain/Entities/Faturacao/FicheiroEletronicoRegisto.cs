using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Faturacao;

[Table("FicheiroEletronicoRegisto", Schema = "Faturacao")]
public class FicheiroEletronicoRegisto : AuditableEntityWithSoftDelete
{
    public Guid ClinicaId { get; set; }

    public Guid DocumentoId { get; set; }

    [StringLength(50)]
    public string NumeroDocumentoExibicao { get; set; } = string.Empty;

    public FicheiroEletronicoSigla Sigla { get; set; }

    public DateTime DataGeracao { get; set; }

    public DateTime? DataDocumento { get; set; }

    [StringLength(250)]
    public string NomeFicheiro { get; set; } = string.Empty;
}
