namespace CliCloud.Application.Services.Utility.FundirUtentesService;

public interface IFundirUtentesFusaoRunner
{
    Task ExecutarFusaoReferenciasAsync(
        Guid utenteOrigemId,
        Guid utenteApagarId,
        CancellationToken cancellationToken = default);
}
