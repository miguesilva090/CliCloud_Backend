namespace CliCloud.Domain.Entities.Faturacao;

public static class AdseEstados 
{
    public const int CoPagamentoNenhum = 0;
    public const int CoPagamentoPorComunicarSemPdf = 1;
    public const int CoPagamentoPorComunicarComPdf = 2;
    public const int CoPagamentoComunicado = 3;
    public const int CoPagamentoFechado = 4;

    public const int PreFaturaCriada = 1;
    public const int PreFaturaAberta = 2;
    public const int PreFaturaFechada = 3;

    public const int OperacaoValidar = 1;
    public const int OperacaoComunicar = 2;
    public const int OperacaoEliminar = 3;
    public const int OperacaoSubstituirPdf = 4;
    public const int OperacaoSubstituirRelatorio = 5;

    public const string TipoTratamentos = "TA";
    public const string TipoConsultas = "CA";
    public const string TipoExames = "EX";

    public static int CoPagamentoDeDescricao(string? descricao) => descricao switch
    {
        "Fechado" => CoPagamentoFechado,
        "Comunicado" => CoPagamentoComunicado, 
        "Por Comunicar c/ PDF" => CoPagamentoPorComunicarComPdf,
        "Por Comunicar s/ PDF" => CoPagamentoPorComunicarSemPdf,
        _ => CoPagamentoNenhum,
    };

    public static string DescricaoCoPagamento(int estado) => estado switch 
    {
        CoPagamentoPorComunicarSemPdf => "Por Comunicar s/ PDF",
        CoPagamentoPorComunicarComPdf => "Por Comunicar c/ PDF",
        CoPagamentoComunicado => "Comunicado",
        CoPagamentoFechado => "Fechado",
        _ => string.Empty,
    };

    public static string DescricaoPreFatura(int estado) => estado switch
    {
        PreFaturaCriada => "Criada",
        PreFaturaAberta => "Aberta",
        PreFaturaFechada => "Fechada",
        _ => string.Empty,
    };

    public static string CodigoPreFatura(string tipo, int numOrdem) => $"{tipo}{numOrdem}";
}