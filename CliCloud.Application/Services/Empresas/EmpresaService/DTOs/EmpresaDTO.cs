using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.RuaService.DTOs;
using CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;
using CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CliCloud.Application.Services.Utility.ConcelhoService.DTOs;
using CliCloud.Application.Services.Utility.DistritoService.DTOs;
using CliCloud.Application.Services.Utility.PaisService.DTOs;
using CliCloud.Application.Services.Bancos.BancoService.DTOs;

namespace CliCloud.Application.Services.Empresas.EmpresaService.DTOs
{
    public class EmpresaDTO : IDto
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

        // Campos específicos de Empresa (inspirados em Dados.Comum.Empresa)
        public int? PrazoPagamento { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? DescontoUtente { get; set; }
        public Guid? CondicaoPagamentoId { get; set; }
        public Guid? ModoPagamentoId { get; set; }
        public Guid? BancoId { get; set; }
        public BancoDTO? Banco { get; set; }
        public Guid? OrganismoId { get; set; }
        public string? NumeroIdentificacaoBancaria { get; set; }
        public string? Apolice { get; set; }
        public decimal? Avenca { get; set; }
        public DateOnly? DataInicioContrato { get; set; }
        public DateOnly? DataFimContrato { get; set; }
        public int? NumeroPagamentos { get; set; }
        public string? Categoria { get; set; }
        public string? Actividade { get; set; }
        public int? Cae { get; set; }
        public string? CodigoClinica { get; set; }
        public int? NumeroTrabalhadores { get; set; }
        public decimal? ValorTrabalhador { get; set; }
        public int? Rescindindo { get; set; }
        public string? Contacto { get; set; }
    }
}

