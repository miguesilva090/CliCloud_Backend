using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs;
using CliCloud.Application.Services.Sexos.SexoService.DTOs;

namespace CliCloud.Application.Services.Utility.EntidadePessoaService.DTOs
{
    public class EntidadePessoaLightDTO : IDto
    {
        // Campos da entidade base Entidade
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NumeroContribuinte { get; set; }
        public Guid? RuaId { get; set; }
        public string? RuaNome { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public string? CodigoPostalCodigo { get; set; }
        public string? CodigoPostalLocalidade { get; set; }
        public Guid? FreguesiaId { get; set; }
        public string? FreguesiaNome { get; set; }
        public Guid? ConcelhoId { get; set; }
        public string? ConcelhoNome { get; set; }
        public Guid? DistritoId { get; set; }
        public string? DistritoNome { get; set; }
        public Guid? PaisId { get; set; }
        public string? PaisNome { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public int? Status { get; set; }

        // Campos específicos de EntidadePessoa
        public DateOnly? DataNascimento { get; set; }
        public Guid? SexoId { get; set; }
        public SexoLightDTO? Sexo { get; set; }
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
    }
}

