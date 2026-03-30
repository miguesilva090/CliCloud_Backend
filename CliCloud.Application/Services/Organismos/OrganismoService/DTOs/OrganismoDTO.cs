using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.RuaService.DTOs;
using CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;
using CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CliCloud.Application.Services.Utility.ConcelhoService.DTOs;
using CliCloud.Application.Services.Utility.DistritoService.DTOs;
using CliCloud.Application.Services.Utility.PaisService.DTOs;
using CliCloud.Application.Services.Bancos.BancoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Organismos.OrganismoService.DTOs
{
    public class OrganismoDTO : IDto
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

        // Campos específicos de Organismo
        public string? NomeComercial { get; set; }
        public string? Abreviatura { get; set; }
        public int? PrazoPagamento { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? DescontoUtente { get; set; }
        public CondicaoPagamento? CondicaoPagamento { get; set; }
        public TipoModoPagamento? TipoModoPagamento { get; set; }
        public Guid? BancoId { get; set; }
        public BancoDTO? Banco { get; set; }
        public string? NumeroIdentificacaoBancaria { get; set; }
        public string? Apolice { get; set; }
        public decimal? Avenca { get; set; }
        public DateOnly? DataInicioContrato { get; set; }
        public DateOnly? DataFimContrato { get; set; }
        public int? NumeroPagamentos { get; set; }
        public string? CodigoClinica { get; set; }
        public int? Faltas { get; set; }
        public string? Contacto { get; set; }
        public string? Categoria { get; set; }
        public string? Ars { get; set; }
        public string? Subregiao { get; set; }
        public string? Regiao { get; set; }
        public string? FraseADM { get; set; }
        public int? Bloqueio { get; set; }
        public bool LimitarConsultas { get; set; }
        public int? NumeroConsultas { get; set; }
        public bool ContabilizarFaltas { get; set; }
        public int? AssinarPagaDocumento { get; set; }
        public int? AdmissaoCC { get; set; }
        public int? FaturaCredencial { get; set; }
        public bool DiscriminaServicos { get; set; }
        public string? DesignaTratamentos { get; set; }
        public bool ApresentarCredenciaisPrimeiraSessaoTratamento { get; set; }
        public bool ApresentarCredenciaisPrimeiraConsulta { get; set; }
        public string? CodigoFaturacao { get; set; }
        public int? FiltroFaturacao { get; set; }
        public string? CServicoFaturaResumo { get; set; }
        public int FaturarPorDatas { get; set; }
        public bool TRUST { get; set; }
        public bool ADM { get; set; }
        public bool SADGNR { get; set; }
        public bool SADPSP { get; set; }
        public bool Globalbooking { get; set; }
        public bool AlterarPrecoTratamento { get; set; }
        public string? ContabContaFA { get; set; }
        public string? ContabContaFR { get; set; }
        public string? ContabTipoContaFA { get; set; }
        public string? ContabTipoContaFR { get; set; }
        public int? CodigoULSNova { get; set; }
        public int? TratamentoCred { get; set; }
        public int Nacional { get; set; }
        public int? CodigoRegiaoAtestadoCC { get; set; }
    }
}
