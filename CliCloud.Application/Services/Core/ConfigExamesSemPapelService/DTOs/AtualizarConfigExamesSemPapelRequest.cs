using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ConfigExamesSemPapelService.DTOs;

public class AtualizarConfigExamesSemPapelRequest : IDto 
{
    public int? CodigoEntidade { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public string? PesquisaPrestacao { get; set; }
    public string? Agendamento { get; set; }
    public string? Efetivacao { get; set; }
    public string? Anulacao { get; set; }
    public string? ConsultaCancelados { get; set; }
    public string? EfetuadosNaoPrescritos { get; set; }
    public string? TaxasModeradoras { get; set; }

    public string? RelatorioResultados { get; set; }
    public string? UsernamePartilhaResultados { get; set; }
    public string? PasswordPartilhaResultados { get; set; }

    public string? RelatorioResultadosSemRequisicao { get; set; }
    public string? UsernamePartilhaResultadosSemRequisicao { get; set; }
    public string? PasswordPartilhaResultadosSemRequisicao { get; set; }

    public string? AreaPrestacao { get; set; }
    
}