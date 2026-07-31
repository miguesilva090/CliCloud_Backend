namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

public sealed class LoteDirectEspRequisicaoData 
{
    public int Codigo { get; init; }
    public string? NumeroRequisicao { get; init; } = string.Empty;
    public string? CodigoMedico {get; init; } 
    public bool EnpAssinado {get; init; }
    public IReadOnlyList<string> CodigosMcdt {get; init; } = [];
    public IReadOnlyList<LoteDirectEspEfetuadoNaoPrescritoData> EfetuadosNaoPrescritos {get; init; } = [];
}

public sealed class LoteDirectEspEfetuadoNaoPrescritoData
{
    public string CodigoMcdt { get; init; } = string.Empty;
    public int NAmostras {get; init;}
}