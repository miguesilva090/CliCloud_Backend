using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeService.DTOs;
using CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs;
using CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs;
using CliCloud.Application.Services.Sexos.SexoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Utentes.UtenteService.DTOs
{
    public class UtenteTableDTO : IDto
    {
        // Campos da entidade base Entidade
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public string? NumeroContribuinte { get; set; }
        public Guid? RuaId { get; set; }
        public EntidadeTableRuaDTO? Rua { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public EntidadeTableCodigoPostalDTO? CodigoPostal { get; set; }
        public Guid? FreguesiaId { get; set; }
        public EntidadeTableFreguesiaDTO? Freguesia { get; set; }
        public Guid? ConcelhoId { get; set; }
        public EntidadeTableConcelhoDTO? Concelho { get; set; }
        public Guid? DistritoId { get; set; }
        public EntidadeTableDistritoDTO? Distrito { get; set; }
        public Guid? PaisId { get; set; }
        public EntidadeTablePaisDTO? Pais { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public int? Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ContactoCount { get; set; }
        public IEnumerable<EntidadeContactoDTO>? EntidadeContactos { get; set; }

        // Campos específicos de EntidadePessoa
        public DateOnly? DataNascimento { get; set; }
        public Guid? SexoId { get; set; }
        public SexoLightDTO? Sexo { get; set; }
        public Guid? EstadoCivilId { get; set; }
        public EstadoCivilLightDTO? EstadoCivil { get; set; }
        public Guid? HabilitacaoId { get; set; }
        public HabilitacaoLightDTO? Habilitacao { get; set; }
        public Guid? ProfissaoId { get; set; }
        public ProfissaoLightDTO? Profissao { get; set; }
        public string? Nacionalidade { get; set; }
        public string? Naturalidade { get; set; }
        public string? NumeroCartaoIdentificacao { get; set; }
        public DateOnly? DataEmissaoCartaoIdentificacao { get; set; }
        public DateOnly? DataValidadeCartaoIdentificacao { get; set; }
        public string? Arquivo { get; set; }
        public string? Carteira { get; set; }
        public string? NomeUtilizador { get; set; }
        public string? UrlFotoAssinatura { get; set; }
        public string? NumeroIdentificacaoBancaria { get; set; }

        // Campos específicos de Utente
        public Guid? GrupoSanguineoId { get; set; }
        public GrupoSanguineoLightDTO? GrupoSanguineo { get; set; }
        public Guid? ProvenienciaUtenteId { get; set; }
        public ProvenienciaUtenteLightDTO? ProvenienciaUtente { get; set; }
        public string? NumeroUtente { get; set; }
        public string? NumeroSegurancaSocial { get; set; }
        public bool Desistencia { get; set; }
        public bool Cronico { get; set; }
        public TipoConsulta TipoConsulta { get; set; }
    }
}
