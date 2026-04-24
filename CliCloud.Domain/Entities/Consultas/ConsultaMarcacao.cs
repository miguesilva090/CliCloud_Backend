#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Enums;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Funcionarios;

namespace CliCloud.Domain.Entities.Consultas
{
  [Table("ConsultaMarcacao", Schema = "Consultas")]
    public class ConsultaMarcacao : AuditableEntityWithSoftDelete
  {
    public Guid? ConsultaId { get; set; }
    public Consulta? Consulta { get; set; }
    
    public Guid UtenteId { get; set; }
    public Utente Utente { get; set; } = null!;
    
    public Guid? MedicoId { get; set; }
    public Medico? Medico { get; set; }
    
    public Guid? EspecialidadeId { get; set; }
    public Especialidade? Especialidade { get; set; }

    public Guid? TecnicoId { get; set; }
    public Tecnico? Tecnico { get; set; }

    public Guid? FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }

    public Guid? MedicoExternoId { get; set; }
    public MedicoExterno? MedicoExterno { get; set; }

    public Guid? SalaId { get; set; }
    public Sala? Sala { get; set; }

    public DateTime? Data { get; set; }
    public TimeSpan? HoraMarcacao { get; set; }

    public Guid? MotivoConsultaId { get; set; }
    public MotivoConsulta? MotivoConsulta { get; set; }

    public Guid? TipoAdmissaoId { get; set; }
    public TipoAdmissao? TipoAdmissao { get; set; }

    public Guid? TipoConsultaId { get; set; }
    public TipoConsultaItem? TipoConsultaItem { get; set; }

    public string? NumDestacavel { get; set; }
    public bool EmTratamento { get; set; }

    public StatusConsulta? StatusConsulta { get; set; }
   
    public string? Obs { get; set; }
    
  }
}
