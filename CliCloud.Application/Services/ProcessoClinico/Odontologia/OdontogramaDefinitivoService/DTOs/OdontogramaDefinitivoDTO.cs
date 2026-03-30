using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.DTOs;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.DTOs
{
    public class OdontogramaDefinitivoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public Guid ConsultaId { get; set; }
        public int NumeroDente { get; set; }
        public int? NumeroDenteAte { get; set; }
        public string? CodigoSuperficie { get; set; }
        public string? CodigoEstadoPadrao { get; set; }
        public string? CodigoTratamentoPadrao { get; set; }
        public string? CodigoEstadoPersonalizado { get; set; }
        public string? CodigoTratamentoPersonalizado { get; set; }
        public string Descricao { get; set; }
        public string? Observacoes { get; set; }
        public bool Faturar { get; set; }
        public int Quantidade { get; set; }
        public decimal? ValorServico { get; set; }
        public decimal? ValorUtente { get; set; }
        public decimal? ValorEntidade { get; set; }
        public Guid? LinhaFaturacaoId { get; set; }
        public EstadosDentariosDTO? EstadoPadrao { get; set; }
        public TiposTratamentoDentarioDTO? TiposTratamentoPadrao { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

