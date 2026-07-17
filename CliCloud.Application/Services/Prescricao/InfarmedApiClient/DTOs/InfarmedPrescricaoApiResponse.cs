namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

internal sealed class InfarmedPrescricaoApiResponse
{
    public InfarmedPrescricaoResumoApiDto? Resumo { get; set; }
    
    public MedicamentoPrecoDto? PrecoPvp { get; set; }
    public MedicamentoPrecoDto? PrecoReferencia { get; set; }
    public MedicamentoPrecoDto? PrecoUnitario { get; set; }
    public MedicamentoPrecoDto? PvpNotificado { get; set; }
    public MedicamentoPrecoDto? PvpMax100Re { get; set; }

    public MedicamentoComparticipacaoDto? ComparticipacaoGeral { get; set; }
    public List<MedicamentoComparticipacaoDto> ComparticipacoesEspeciais { get; set; } = [];
    public MedicamentoComparticipacaoDto? ComparticipacaoEfectiva { get; set; }
    public decimal? TaxaComparticipacaoEfectiva { get; set; }
    public List<int> PatologiasConsideradas { get; set; } = [];
    public MedicamentoPrescricaoBaseCalculoDto? BaseCalculo { get; set; }

    public bool PrescritivelAmbulatorio { get; set; }
    public bool PrescritivelMesSeguinte { get; set; }
    public string? GrupoHomogeneo { get; set; }

    public List<MedicamentoPrescricaoOpcaoDto> OpcoesEquivalentes { get; set; } = [];
}

internal sealed class InfarmedPrescricaoResumoApiDto
{
    public string Cnpem { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Dosagem { get; set; }
    public string? Embalagem { get; set; }
    public string? NrRegisto { get; set; }
    public string? PrincipioActivo { get; set; }
    public string? FormaFarmaceutica { get; set; }
    public bool Generico { get; set; }
}