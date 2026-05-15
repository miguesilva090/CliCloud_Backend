#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Doencas;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Funcionarios;
using CliCloud.Domain.Entities.Seguradoras;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Consultas
{
    [Table("Admissao", Schema = "Consultas")]
    public class Admissao : AuditableEntityWithSoftDelete
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public Guid? ConsultaMarcacaoId { get; set; }
        public ConsultaMarcacao? ConsultaMarcacao { get; set; }

        /// <summary>Consulta criada no fecho (FK em Consulta.AdmissaoId).</summary>
        public Consulta? Consulta { get; set; }

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
        
        public Guid? MotivoConsultaId { get; set; }
        public MotivoConsulta? MotivoConsulta { get; set; }

        public Guid? TipoAdmissaoId { get; set; }
        public TipoAdmissao? TipoAdmissao { get; set; }

        public Guid? TipoConsultaId { get; set; }
        public TipoConsultaItem? TipoConsultaItem { get; set; }

        public Guid? OrganismoId { get; set; }
        public Organismo? Organismo { get; set; }

        public Guid? SeguradoraId { get; set; }
        public Seguradora? Seguradora { get; set; }

        public Guid? TratamentoId { get; set; }
        public Tratamento? Tratamento { get; set; }
        
        public DateTime? Data { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFim { get; set; }
        public TimeSpan? HoraChegada { get; set; }

        public StatusConsulta? StatusConsulta { get; set; }
        public OrigemAdmissao Origem { get; set; }

        public bool? Confirmado { get; set; }
        public bool? Efetuado { get; set; }
        public string? Credencial { get; set; }
        public int? CredencialExterna { get; set; }
        public string? NumDestacavel { get; set; }
        public int? Ordem { get; set; }

        public int? Sinistrado { get; set; }
        public int? Justificacao { get; set; }
        public string? MotivoJustificacao { get; set; }

        public string? Diagnostico { get; set; }
        public Guid? DoencaPrincipalId { get; set; }
        public Doenca? DoencaPrincipal { get; set; }

        public Guid? DoencaSecundariaId { get; set; }
        public Doenca? DoencaSecundaria { get; set; }

        public string? Obs { get; set; }

        public DateTime? DataHoraMarcacao { get; set; }

        public ICollection<AdmissaoServico> Servicos { get; set; } = [];
    }
}