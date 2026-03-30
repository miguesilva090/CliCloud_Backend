using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.RuaService.DTOs;
using CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;
using CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CliCloud.Application.Services.Utility.ConcelhoService.DTOs;
using CliCloud.Application.Services.Utility.DistritoService.DTOs;
using CliCloud.Application.Services.Utility.PaisService.DTOs;
// using CliCloud.Application.Services.Especialidades.EspecialidadeService.DTOs; // TODO: Descomentar quando EspecialidadeService for criado
using CliCloud.Application.Services.Sexos.SexoService.DTOs;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs;

namespace CliCloud.Application.Services.Medicos.MedicoService.DTOs
{
    public class MedicoDTO : IDto
    {
        // Campos da entidade base Entidade
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public string? NumeroContribuinte { get; set; }
        public Guid? RuaId { get; set; }
        public RuaDTO? Rua { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public CodigoPostalDTO? CodigoPostal { get; set; }
        public Guid? FreguesiaId { get; set; }
        public FreguesiaDTO? Freguesia { get; set; }
        public Guid? ConcelhoId { get; set; }
        public ConcelhoDTO? Concelho { get; set; }
        public Guid? DistritoId { get; set; }
        public DistritoDTO? Distrito { get; set; }
        public Guid? PaisId { get; set; }
        public PaisDTO? Pais { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public string? Observacoes { get; set; }
        public int? Status { get; set; }
        public string? UrlFoto { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<EntidadeContactoDTO>? EntidadeContactos { get; set; }

        // Campos específicos de EntidadePessoa
        public DateOnly? DataNascimento { get; set; }
        public Guid? SexoId { get; set; }
        public SexoLightDTO? Sexo { get; set; }
        public Guid? EstadoCivilId { get; set; }
        public EstadoCivilLightDTO? EstadoCivil { get; set; }
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

        // Campos específicos de Medico
        public bool Director { get; set; }
        public Guid? EspecialidadeId { get; set; }
        // public EspecialidadeDTO? Especialidade { get; set; } // TODO: Descomentar quando EspecialidadeService for criado
        public string? EspecialidadeNome { get; set; }
        public double? Margem { get; set; }
        public string? LoginPRVR { get; set; }
        public bool ComunicacaoNif { get; set; }
        public bool? ComunicacaoNifAdse { get; set; }
        public string? GrupoFuncional { get; set; }
        public string? Letra { get; set; }
        public int CartaoCidadaoMedico { get; set; }
        public Guid? IdUtilizador { get; set; }
        public bool Globalbooking { get; set; }
    }
}
