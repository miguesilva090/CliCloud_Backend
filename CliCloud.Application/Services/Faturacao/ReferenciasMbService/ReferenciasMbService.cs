using System.Globalization;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;
using CliCloud.Domain.Entities.Core.ConfigReferenciaMB;
using CliCloud.Domain.Entities.Faturacao;
using Microsoft.Extensions.Configuration;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService;

public partial class ReferenciasMbService(IRepositoryAsync repository, IConfiguration configuration) : IReferenciasMbService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IConfiguration _configuration = configuration;
    private const string SecretMask = "********";

    public async Task<Response<ConfigReferenciaMbDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
    {
        ConfigReferenciaMB? entity;
        try
        {
            IEnumerable<ConfigReferenciaMB> all = await _repository.GetListAsync<ConfigReferenciaMB, Guid>();
            entity = all.FirstOrDefault(x => x.ClinicaId == clinicaId);
        }
        catch (Exception ex) when (IsMissingConfigTable(ex))
        {
            return ResponseFactory.Success(new ConfigReferenciaMbDTO
            {
                Id = Guid.Empty,
                ClinicaId = clinicaId,
                ValorMinimo = 0,
                PrazoPagamento = 0
            });
        }

        if (entity == null)
        {
            return ResponseFactory.Success(new ConfigReferenciaMbDTO
            {
                Id = Guid.Empty,
                ClinicaId = clinicaId,
                ValorMinimo = 0,
                PrazoPagamento = 0
            });
        }

        return ResponseFactory.Success(new ConfigReferenciaMbDTO
        {
            Id = entity.Id,
            ClinicaId = entity.ClinicaId,
            ValorMinimo = entity.ValorMinimo,
            PrazoPagamento = entity.PrazoPagamento,
            ServicoUrl = entity.ServicoUrl,
            CodigoEntidade = entity.CodigoEntidade,
            SubEntidade = entity.SubEntidade,
            ChaveBackOffice = entity.ChaveBackOffice,
            IfThenKey = MaskSecret(entity.IfThenKey)
        });
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfigReferenciaMbRequest request)
    {
        ConfigReferenciaMB entity;
        try
        {
            IEnumerable<ConfigReferenciaMB> all = await _repository.GetListAsync<ConfigReferenciaMB, Guid>();
            entity = all.FirstOrDefault(x => x.ClinicaId == clinicaId) ?? new ConfigReferenciaMB
            {
                ClinicaId = clinicaId
            };
        }
        catch (Exception ex) when (IsMissingConfigTable(ex))
        {
            return ResponseFactory.Fail<Guid>("Tabela Configuração MB não existe.");
        }

        entity.ValorMinimo = request.ValorMinimo;
        entity.PrazoPagamento = request.PrazoPagamento;
        entity.ServicoUrl = request.ServicoUrl?.Trim();
        entity.CodigoEntidade = request.CodigoEntidade?.Trim();
        entity.SubEntidade = request.SubEntidade?.Trim();
        entity.ChaveBackOffice = request.ChaveBackOffice?.Trim();
        entity.IfThenKey = ResolveSecret(request.IfThenKey, entity.IfThenKey);

        if (entity.Id == Guid.Empty)
            _ = await _repository.CreateAsync<ConfigReferenciaMB, Guid>(entity);
        else
            _ = await _repository.UpdateAsync<ConfigReferenciaMB, Guid>(entity);

        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
    }

    public async Task<Response<string>> ConstruirCallbackIfThenAsync(Guid clinicaId)
    {
        ConfigReferenciaMB? conf;
        try
        {
            IEnumerable<ConfigReferenciaMB> all = await _repository.GetListAsync<ConfigReferenciaMB, Guid>();
            conf = all.FirstOrDefault(x => x.ClinicaId == clinicaId);
        }
        catch (Exception ex) when (IsMissingConfigTable(ex))
        {
            return ResponseFactory.Fail<string>("Tabela Configuração MB não existe.");
        }

        if (conf == null || string.IsNullOrWhiteSpace(conf.IfThenKey))
            return ResponseFactory.Fail<string>("Sem chave anti-phishing configurada.");

        string baseUrl = (_configuration["PublicBaseUrl"] ?? "https://localhost:8094").TrimEnd('/');
        string callback =
            $"{baseUrl}/client/faturacao/referencias-mb/callback/ifthen?key={conf.IfThenKey}&requestId=[REQUEST_ID]&entity=[ENTITY]&reference=[REFERENCE]&payment_datetime=[PAYMENT_DATETIME]&orderId=[ORDER_ID]&amount=[AMOUNT]";
        return ResponseFactory.Success(callback);
    }

    public async Task<Response<IEnumerable<ReferenciaMbTableDTO>>> ListarHistoricoAsync(Guid clinicaId)
    {
        IEnumerable<ReferenciaMB> all = await _repository.GetListAsync<ReferenciaMB, Guid>();

        List<ReferenciaMbTableDTO> data = all
            .Where(x => x.ClinicaId == clinicaId)
            .OrderByDescending(x => x.DataReferenciaGerada)
            .Select(x => new ReferenciaMbTableDTO
            {
                Id = x.Id,
                ClienteNome = x.ClienteNome,
                Descricao = x.Descricao,
                Mensagem = x.Mensagem,
                EntidadeMb = x.EntidadeMb,
                ReferenciaCodigo = x.ReferenciaCodigo,
                Valor = x.Valor,
                DataReferenciaGerada = x.DataReferenciaGerada,
                DataLimitePagamento = x.DataLimitePagamento,
                DataPagamento = x.DataPagamento,
                Liquidada = x.Liquidada,
                Anulada = x.Anulada,
                Servico = string.IsNullOrWhiteSpace(x.EntidadeMb) ? "MB Way" : "Referência MB"
            })
            .ToList();

        return ResponseFactory.Success<IEnumerable<ReferenciaMbTableDTO>>(data);
    }

    public async Task<Response<Guid>> MarcarLiquidadaAsync(Guid clinicaId, Guid referenciaId)
    {
        ReferenciaMB reg = await _repository.GetByIdAsync<ReferenciaMB, Guid>(referenciaId);
        if (reg == null || reg.ClinicaId != clinicaId)
            return ResponseFactory.Fail<Guid>("Referência não encontrada.");

        reg.Liquidada = true;
        reg.DataPagamento = DateTime.Now;

        _ = await _repository.UpdateAsync<ReferenciaMB, Guid>(reg);
        _ = await _repository.SaveChangesAsync();

        return ResponseFactory.Success(referenciaId);
    }

    public async Task<Response<Guid>> AnularAsync(Guid clinicaId, Guid referenciaId, AnularReferenciaMbRequest request)
    {
        ReferenciaMB reg = await _repository.GetByIdAsync<ReferenciaMB, Guid>(referenciaId);
        if (reg == null || reg.ClinicaId != clinicaId)
            return ResponseFactory.Fail<Guid>("Referência não encontrada.");

        reg.Anulada = true;
        reg.Mensagem = request.Observacao.Trim();

        _ = await _repository.UpdateAsync<ReferenciaMB, Guid>(reg);
        _ = await _repository.SaveChangesAsync();

        return ResponseFactory.Success(referenciaId);
    }

    public async Task<Response<string>> ReceberCallbackIfThenAsync(IfThenCallbackRequest request)
    {
        ConfigReferenciaMB? conf;
        try
        {
            IEnumerable<ConfigReferenciaMB> configs = await _repository.GetListAsync<ConfigReferenciaMB, Guid>();
            conf = configs.FirstOrDefault(
                x => !string.IsNullOrWhiteSpace(x.IfThenKey) && x.IfThenKey == request.Key
            );
        }
        catch (Exception ex) when (IsMissingConfigTable(ex))
        {
            return ResponseFactory.Fail<string>("Tabela Configuração MB não existe.");
        }

        if (conf == null)
            return ResponseFactory.Fail<string>("Chave inválida.");

        IEnumerable<ReferenciaMB> refs = await _repository.GetListAsync<ReferenciaMB, Guid>();
        ReferenciaMB? reg = refs.FirstOrDefault(x =>
            x.ClinicaId == conf.ClinicaId
            && x.RequestId == request.RequestId
            && x.EntidadeMb == request.Entity
            && x.ReferenciaCodigo == request.Reference
        );

        if (reg == null)
            return ResponseFactory.Success("OK");

        reg.Liquidada = true;
        reg.DataPagamento = ParseDataPagamento(request.PaymentDateTime) ?? DateTime.Now;

        _ = await _repository.UpdateAsync<ReferenciaMB, Guid>(reg);
        _ = await _repository.SaveChangesAsync();

        return ResponseFactory.Success("OK");
    }

    private static DateTime? ParseDataPagamento(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (DateTime.TryParseExact(value, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            return dt;

        if (DateTime.TryParse(value, out dt))
            return dt;

        return null;
    }

    private static string? MaskSecret(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? value : SecretMask;
    }

    private static string? ResolveSecret(string? incoming, string? existing)
    {
        if (string.IsNullOrWhiteSpace(incoming))
            return existing;

        string trimmed = incoming.Trim();
        if (trimmed == SecretMask)
            return existing;

        return trimmed;
    }

    private static bool IsMissingConfigTable(Exception ex)
    {
        return ex.Message.Contains("Invalid object name", StringComparison.OrdinalIgnoreCase)
            && ex.Message.Contains("ConfigReferenciaMB", StringComparison.OrdinalIgnoreCase);
    }
}