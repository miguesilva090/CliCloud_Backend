namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;

public class FicheiroEletronicoSadGnrLinhaDTO
{
    public Guid DocumentoUtenteId { get; set; }
    public int NumeroDocumentoUtente { get; set; }
    public DateTime DataUtente { get; set; }
    public string Beneficiario { get; set; } = string.Empty;
    public decimal ValorTotalReciboUtente { get; set; }
    public decimal ValorBeneficiarioReciboUtente { get; set; }
    public string CodigoServico { get; set; } = string.Empty;
    public DateTime DataAtoMedico { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorAtoMedico { get; set; }
    public decimal ValorBeneficiarioAtoMedico { get; set; }
    public decimal ValorOrganismoAtoMedico { get; set; }
    public string Dente { get; set; } = string.Empty;
}

public class FicheiroEletronicoAdmLinhaDTO
{
    public decimal TotalFatura { get; set; }
    public string Beneficiario { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string CodigoServico { get; set; } = string.Empty;
    public int? CodigoTratAdmiss { get; set; }
    public decimal Comparticipacao { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorPVP { get; set; }
    public decimal ValorADM { get; set; }
    public decimal ValorBeneficiario { get; set; }
}

public class FicheiroEletronicoSadPspLinhaDTO 
{
    public string Beneficiario { get; set; } = string.Empty;
    public string CodigoServico { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal ValorAtoMedico { get; set; }

}

public sealed class FicheiroEletronicoGeradoDTO 
{
    public byte[] Bytes { get; set; } = [];
    public string Nome { get; set; } = string.Empty;
    public List<string> Erros { get; set; } = [];
}