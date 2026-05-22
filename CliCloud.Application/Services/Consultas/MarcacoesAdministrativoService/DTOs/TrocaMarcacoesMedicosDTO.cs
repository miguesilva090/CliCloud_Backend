using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class TrocaMarcacoesMedicosDTO : IDto
{
    public Guid MarcacaoId { get; set; }
    public string? UtenteNome { get; set; }
    public int? UtenteNumero { get; set; }
    public string? HoraInicio { get; set; }
    public string? EspecialidadeDesignacao { get; set; }
}

public class TrocaMarcacoesMedicosConflitoDTO : IDto
{
    public Guid? MarcacaoId { get; set; }
    public string? UtenteNome { get; set; }
    public string? HoraInicio { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
}

public class TrocaMarcacoesMedicosPreviewDTO : IDto
{
    public int TotalOrigem { get; set; }
    public bool PodeExecutar { get; set; }
    public IEnumerable<TrocaMarcacoesMedicosDTO> Itens { get; set; } = [];
    public IEnumerable<TrocaMarcacoesMedicosConflitoDTO> Conflitos { get; set; } = [];
}

public class TrocaMarcacoesMedicosResultDTO : IDto 
{
    public int QuantidadeTransferida { get; set; }
    public IEnumerable<Guid> MarcacaoIds { get; set; } = [];
    public int QuantidadeAdmissoesAtualizadas { get; set; }
    
}