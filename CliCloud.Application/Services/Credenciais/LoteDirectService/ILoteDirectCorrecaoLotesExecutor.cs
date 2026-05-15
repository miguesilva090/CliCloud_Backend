namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectCorrecaoLotesExecutor {
    Task ExecutarAsync(int ano, int mes, CancellationToken cancellationToken = default);
}