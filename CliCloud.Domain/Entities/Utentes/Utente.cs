#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Empresas;
using CliCloud.Domain.Entities.GruposSanguineos;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.ProvenienciasUtente;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;
using CentroSaudeEntity = CliCloud.Domain.Entities.CentroSaude.CentroSaude;

namespace CliCloud.Domain.Entities.Utentes
{
  [Table("Utente", Schema = "Utentes")]
  public class Utente : EntidadePessoa
  {
    public string? NomePai {get;set;}
    public string? NomeMae {get;set;}
    public string? NumeroSegurancaSocial {get;set;}
    public Guid? GrupoSanguineoId { get; set; }
    public Entities.GruposSanguineos.GrupoSanguineo? GrupoSanguineo { get; set; }
    public Guid? ProvenienciaUtenteId { get; set; }
    public ProvenienciaUtente? ProvenienciaUtente { get; set; }
    public Guid? OrganismoId { get; set; }
    public Organismo? Organismo { get; set; }
    public Guid? SeguradoraId { get; set; }
    public Organismo? SeguradoraOrganismo { get; set; }
    public Guid? EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }
    public Guid? CentroSaudeId { get; set; }
    public CentroSaudeEntity? CentroSaude { get; set; }
    public Guid? MedicoExternoId { get; set; }
    public MedicoExterno? MedicoExterno { get; set; }
    public Guid? MedicoId { get; set; }
    public Medico? Medico { get; set; }
    public string? NumeroUtente {get;set;}
    public string? Aviso {get;set;}
    public bool Desistencia {get;set;}
    public bool Cronico {get;set;}
    public TipoConsulta TipoConsulta {get;set;}
    public bool Migrante {get;set;}
    public int MarkConsentimento {get;set;}
    public int RgpdConsentimento {get;set;}
    public DateTime? DataConsentimentoRgpd {get;set;}
    public DateTime? DataRevogacaoRgpd {get;set;}
    public DateTime? DataConsentimentoMark {get;set;}
    public DateTime? DataRevogacaoMark {get;set;}
    public bool MarkTratamentoDados {get;set;}
    public StatusValidacao? CCValidado {get;set;}
    public DateTime? CCDataValidacao {get;set;}
    public DateOnly? DataValidadeCU {get;set;}
    public string? NDocMigrante {get;set;}
    public int? CondicaoSns { get; set; }
    public Guid? EntidadeFinanceiraResponsavelId { get; set; }
    public string? NumeroBeneficiarioEfr { get; set; }
    public DateOnly? DataValidadeEfr { get; set; }
    public string? MigranteTipoCartao { get; set; }
    public DateTime? DataRegisto {get;set;}
    public TipoTaxaModeradora? TipoTaxaModeradora {get;set;}

    public Guid? IdUtilizador { get; set; }
    public ICollection<UtenteSubsistemaLinha> SubsistemaLinhas { get; set; } = [];
  }
}