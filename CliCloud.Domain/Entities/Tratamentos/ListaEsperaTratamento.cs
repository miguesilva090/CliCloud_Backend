#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Seguradoras;
using CliCloud.Domain.Entities.Sinistros;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.Tratamentos;

[Table("ListaEsperaTratamento", Schema = "Tratamentos")]
public class ListaEsperaTratamento : AuditableEntityWithSoftDelete
{
    public int Ordem { get; set; }
    public DateTime DataEntrada { get; set; }
    public int? OrdemOrigem { get; set; }
    public Guid UtenteId { get; set; }
    public Utente Utente { get; set; } = null!;
    public Guid? MedicoId { get; set; }
    public Medico? Medico { get; set; }
    public Guid? OrganismoId { get; set; }
    public Organismo? Organismo { get; set; }
    public Guid? PrioridadeId { get; set; }
    public Prioridade? Prioridade { get; set; }
    public Guid? EstadoListaEsperaId { get; set; }
    public EstadoListaEspera? EstadoListaEspera { get; set; }
    public Guid? LocalTratamentoId { get; set; }
    public LocalTratamento? LocalTratamento { get; set; }
    public Guid? PatologiaId { get; set; }
    public Patologia? Patologia { get; set; }
    public Guid? SinistradoId { get; set; }
    public Sinistrado? Sinistrado { get; set; }
    public Guid? SeguradoraId { get; set; }
    public Seguradora? Seguradora { get; set; }
    public string? Designacao { get; set; }
    public int? NumSessoes { get; set; }
    public string? HoraDesejada { get; set; }
    public int? NFaltMax { get; set; }
    public int? NFaltComax { get; set; }
    public string? Credencial { get; set; }
    public DateTime? ValidadeCredencial { get; set; }
    public int? TaxaModeradora { get; set; }
    public string? Obs { get; set; }
    public string? TecObs { get; set; }
    public string? DuracaoTotal { get; set; }
    public bool CredencialExterna { get; set; }
    public bool Historico { get; set; }

    public int? CodigoLegado { get; set; }

    public ICollection<ListaEsperaTratamentoServico> Servicos { get; set; } = [];
}