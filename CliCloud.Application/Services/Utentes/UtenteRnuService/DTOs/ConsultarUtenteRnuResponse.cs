using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utentes.UtenteRnuService.DTOs;

public class ConsultarUtenteRnuResponse : IDto 
{
    public string? CodigoMensagem { get; set; }
    public string? DescricaoMensagem { get; set; }

    public string? NumeroSns { get; set; }
    public string? NomeCompleto { get; set; }
    public string? NomesProprios { get; set; }
    public string? Apelidos { get; set; }
    public DateTime? DataNascimento { get; set; } 
    public string? Sexo { get; set; }
    public string? PaisNacionalidade { get; set; }
    public bool? Obito { get; set; }
    public bool? Duplicado { get; set; }

    public List<EntidadeResponsavelRnuDTO> EntidadesResponsaveis { get; set; } = [];

    public string RawXml { get; set; } = string.Empty;
}

public class EntidadeResponsavelRnuDTO : IDto 
{
    public string? Codigo { get; set; }
    public string? Descricao { get; set; }
    
}