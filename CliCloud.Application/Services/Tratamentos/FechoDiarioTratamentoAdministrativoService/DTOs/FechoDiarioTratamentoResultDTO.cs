using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.DTOs;

public class FechoDiarioTratamentoResultDTO : IDto 
{
    public int TotalElegiveis { get; set; }
    public int TotalProcessadas { get; set; }
    public int TotalSessoesHistorico { get; set; }
    public int TotalTratamentosFechados { get; set; }
    public int TotalIgnoradas { get; set; }
    public List<string> Avisos { get; set; } = [];
    public List<string> Erros { get; set; } = [];
}