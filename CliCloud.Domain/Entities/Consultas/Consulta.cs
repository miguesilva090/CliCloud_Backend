#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Funcionarios;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Seguradoras;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Consultas
{
  [Table("Consulta", Schema = "Consultas")]
  public class Consulta : AuditableEntityWithSoftDelete
  {
    public Guid? UtenteId { get; set; }
    public Utente? Utente { get; set; }
    
    public Guid? MedicoId { get; set; }
    public Medico? Medico { get; set; }
    
    public Guid? EspecialidadeId { get; set; }
    public Especialidade? Especialidade { get; set; }
    
    public Guid? TecnicoId { get; set; }
    public Tecnico? Tecnico { get; set; }

    public Guid? SalaId { get; set; }
    public Sala? Sala { get; set; }

    public Guid? MedicoExternoId { get; set; }
    public MedicoExterno? MedicoExterno { get; set; }
    
    // Dados da consulta
    public DateTime? Data { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFim { get; set; }
    public StatusConsulta? StatusConsulta { get; set; }
    
    // Documentos
    public Guid? DocumentoId { get; set; }
    public Documento? Documento { get; set; }
    public Guid? TipoDocumentoId { get; set; }
    public TipoDocumento? TipoDocumento { get; set; }
    
    // Organismo e credenciais
    public Guid? OrganismoId { get; set; }
    public Organismo? Organismo { get; set; }
    public string? Credencial { get; set; }
    public int? CredencialExterna { get; set; }
    
    // Seguradora e sinistro
    public Guid? SeguradoraId { get; set; }
    public Seguradora? Seguradora { get; set; }
    public int? Sinistrado { get; set; }
    public int? Justificacao { get; set; }
    public string? MotivoJustificacao { get; set; }
    
    // Outros campos
    public Guid? TratamentoId { get; set; }
    public Tratamento? Tratamento { get; set; }
    public Guid? FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }
    public string? Obs { get; set; }
    public string? Diagnostico { get; set; }
    
    public Guid? TipoConsultaId { get; set; }
    public TipoConsultaItem? TipoConsultaItem { get; set; }
    
    public Guid? ConsultaMarcacaoId { get; set; }
    public ConsultaMarcacao? ConsultaMarcacao { get; set; }
    
    public ICollection<ServicoConsulta> Servicos { get; set; } = [];
  }
}
