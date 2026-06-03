#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class FaturaGlobalObterResponse : IDto
{
    public Guid OrganismoId { get; set; }
    public Guid? UtenteId { get; set; }
    public FaturaGlobalClienteDTO Cliente { get; set; } = new();
    public DateTime DataDe { get; set; }
    public DateTime DataAte { get; set; }
    public List<FaturaGlobalLinhaDTO> Linhas { get; set; } = [];
    public List<FaturaGlobalAdmissaoDTO> Admissoes { get; set; } = [];
}

public class FaturaGlobalClienteDTO 
{
    public string Nome { get; set; } = string.Empty;
    public string Morada { get; set; } = string.Empty;
    public string? Localidade { get; set; }
    public string? NumeroContribuinte { get; set; }
    public Guid? CodigoPostalId { get; set; }

}

public class FaturaGlobalLinhaDTO 
{
    public Guid? ServicoId { get; set; }
    public string? CodigoArtigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoUtente { get; set; }
    public decimal PrecoOrganismo { get; set; }
    public Guid? TaxaIvaId { get; set; }
    public decimal TaxaIvaPercentagem { get; set; }
    public Guid? MotivoIsencaoId { get; set; }
    public List<Guid> AdmissaoServicosIds { get; set; } = [];
}

public class FaturaGlobalAdmissaoDTO 
{
    public Guid AdmissaoId { get; set; }
    public int ModuloOrigem { get; set; }
    public string CodigoExibicao { get; set; } = string.Empty;
    public int? Ordem { get; set; }
}
