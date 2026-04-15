using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.ConsentimentoService.DTOs;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums.Documentos;
using System.Globalization;

namespace CliCloud.Application.Services.Documentos.ConsentimentoService;

public class ConsentimentoService(
    IRepositoryAsync repository,
    ICurrentClinicaService currentClinicaService
): IConsentimentoService
{
    private readonly IRepositoryAsync _repository = repository;
    
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    private async Task<Guid> ObterClinicaIdAsync()
    {
        await _currentClinicaService.SetClinicaAsync();
        if(Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId) && clinicaId != Guid.Empty)
        {
            return clinicaId;
        }

        throw new InvalidOperationException("Clínica atual inválida.");
    }

    public async Task<Response<PedidoConsentimentoDTO>> CriarPedidoAsync(CriarPedidoConsentimentoRequest request)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            string tipoConsentimento = string.IsNullOrWhiteSpace(request.TipoConsentimento)
                ? "RGPD"
                : request.TipoConsentimento.Trim();

            InstanciaDocumento instancia = await _repository.GetByIdAsync<InstanciaDocumento, Guid>(request.InstanciaDocumentoId);

            if(instancia == null || instancia.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<PedidoConsentimentoDTO>("Instância não encontrada.");
            }

            IEnumerable<PedidoConsentimento> existentes = await _repository.GetListAsync<PedidoConsentimento, Guid>();
            bool jaExistePendente = existentes.Any(x =>
                x.ClinicaId == clinicaId
                && x.InstanciaDocumentoId == request.InstanciaDocumentoId
                && x.TipoConsentimento == tipoConsentimento
                && x.Estado == EstadoPedidoConsentimento.Pendente
            );

            if (jaExistePendente)
            {
                return ResponseFactory.Fail<PedidoConsentimentoDTO>("Já existe um pedido pendente para esta instância.");
            }

            PedidoConsentimento entity = new()
            {
                Id = Guid.NewGuid(),
                ClinicaId = clinicaId,
                InstanciaDocumentoId = request.InstanciaDocumentoId,
                UtenteId = request.UtenteId,
                TipoConsentimento = tipoConsentimento,
                Canal = request.Canal?.Trim(),
                ExpiraEm = request.ExpiraEm,
                Estado = EstadoPedidoConsentimento.Pendente,
            };

            _ = await _repository.CreateAsync<PedidoConsentimento, Guid>(entity);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(MapToDto(entity));
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<PedidoConsentimentoDTO>(ex.Message);
        }
    }

    public async Task<Response<PedidoConsentimentoDTO>> ObterPedidoAsync(Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            PedidoConsentimento entity = await _repository.GetByIdAsync<PedidoConsentimento, Guid>(id);

            if(entity == null || entity.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<PedidoConsentimentoDTO>("Pedido não encontrado.");
            }

            return ResponseFactory.Success(MapToDto(entity));
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<PedidoConsentimentoDTO>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<PedidoConsentimentoDTO>>> ObterPedidosAsync(
        Guid? utenteId = null,
        int? estado = null
    )
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            IEnumerable<PedidoConsentimento> todos = await _repository.GetListAsync<PedidoConsentimento, Guid>();

            IEnumerable<PedidoConsentimento> query = todos.Where(x => x.ClinicaId == clinicaId);

            if(utenteId.HasValue)
            {
                query = query.Where(x => x.UtenteId == utenteId.Value);
            }

            if(estado.HasValue && Enum.IsDefined(typeof(EstadoPedidoConsentimento), estado.Value))
            {
                EstadoPedidoConsentimento estadoEnum = (EstadoPedidoConsentimento)estado.Value;
                query = query.Where(x => x.Estado == estadoEnum);
            }

            List<PedidoConsentimentoDTO> data = query
                .OrderByDescending(x => x.CreatedOn)
                .Select(MapToDto)
                .ToList();

            return ResponseFactory.Success<IEnumerable<PedidoConsentimentoDTO>>(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<PedidoConsentimentoDTO>>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CancelarPedidoAsync(Guid id, string? observacoes = null)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            PedidoConsentimento entity = await _repository.GetByIdAsync<PedidoConsentimento, Guid>(id);

            if(entity == null || entity.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<Guid>("Pedido não encontrado.");
            }

            if(entity.Estado == EstadoPedidoConsentimento.Assinado)
            {
                return ResponseFactory.Fail<Guid>("Não é possível cancelar um pedido já assinado.");
            }

            entity.Estado = EstadoPedidoConsentimento.Cancelado;
            if(!string.IsNullOrWhiteSpace(observacoes))
            {
                entity.Observacoes = observacoes.Trim();
            }

            _ = await _repository.UpdateAsync<PedidoConsentimento, Guid>(entity);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(entity.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> MarcarAssinadoAsync(Guid id , MarcarPedidoConsentimentoAssinadoRequest request)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            PedidoConsentimento entity = await _repository.GetByIdAsync<PedidoConsentimento, Guid>(id);

            if(entity == null || entity.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<Guid>("Pedido não encontrado");
            }

            if(entity.Estado == EstadoPedidoConsentimento.Cancelado)
            {
                return ResponseFactory.Fail<Guid>("Não é possível assinar um pedido cancelado ");
            }

            if (entity.Estado == EstadoPedidoConsentimento.Assinado)
            {
                return ResponseFactory.Fail<Guid>("Pedido já se encontra assinado.");
            }

            entity.Estado = EstadoPedidoConsentimento.Assinado;
            entity.AssinadoEm = DateTime.UtcNow;
            entity.AssinadoPor = request.AssinadoPor?.Trim();
            entity.Observacoes = request.Observacoes?.Trim();

            InstanciaDocumento instancia = await _repository.GetByIdAsync<InstanciaDocumento, Guid>(
                entity.InstanciaDocumentoId
            );

            if(instancia != null && instancia.ClinicaId == clinicaId)
            {
                instancia.Assinado = true;
                instancia.AssinadoEm = entity.AssinadoEm;
                instancia.AssinadoPor = entity.AssinadoPor;

                string nomeAssinante = string.IsNullOrWhiteSpace(entity.AssinadoPor)
                    ? "Utente"
                    : entity.AssinadoPor.Trim();
                string nomeConsentimento = nomeAssinante;
                if (entity.UtenteId.HasValue)
                {
                    Utente utente = await _repository.GetByIdAsync<Utente, Guid>(entity.UtenteId.Value);
                    if (utente != null && !string.IsNullOrWhiteSpace(utente.Nome))
                    {
                        nomeConsentimento = utente.Nome.Trim();
                    }
                }

                string dataAssinatura = (entity.AssinadoEm ?? DateTime.UtcNow)
                    .ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-PT"));
                string consentimentoMarcador = nomeConsentimento;
                string assinaturaMarcador = $"{nomeAssinante} ({dataAssinatura})";

                if (!string.IsNullOrWhiteSpace(request.AssinaturaBase64))
                {
                    string assinaturaDataUrl = request.AssinaturaBase64.StartsWith("data:image", StringComparison.OrdinalIgnoreCase)
                        ? request.AssinaturaBase64
                        : $"data:image/png;base64,{request.AssinaturaBase64}";

                    assinaturaMarcador =
                        $"<img alt=\"Assinatura do utente\" src=\"{assinaturaDataUrl}\" style=\"max-height:120px;max-width:320px;\" />";
                }

                instancia.ConteudoHtml = SubstituirMarcador(instancia.ConteudoHtml, "UtenteConsentimento", consentimentoMarcador);
                instancia.ConteudoHtml = SubstituirMarcador(instancia.ConteudoHtml, "UtenteAssinatura", assinaturaMarcador);

                _ = await _repository.UpdateAsync<InstanciaDocumento, Guid>(instancia);
            }

            _ = await _repository.UpdateAsync<PedidoConsentimento, Guid>(entity);
            _ = await _repository.SaveChangesAsync();


            return ResponseFactory.Success(entity.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    private static PedidoConsentimentoDTO MapToDto(PedidoConsentimento x)
    {
        return new PedidoConsentimentoDTO
        {
            Id = x.Id,
            InstanciaDocumentoId = x.InstanciaDocumentoId,
            UtenteId = x.UtenteId,
            TipoConsentimento = x.TipoConsentimento, 
            Estado = x.Estado,
            Canal = x.Canal,
            ExpiraEm = x.ExpiraEm,
            AssinadoEm = x.AssinadoEm,
            AssinadoPor = x.AssinadoPor,
            Observacoes = x.Observacoes,
            CreatedOn = x.CreatedOn
        };
    }

    private static string SubstituirMarcador(string html, string campo, string valor)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return html;
        }

        return html
            .Replace($"<<{campo}>>", valor)
            .Replace($"&lt;&lt;{campo}&gt;&gt;", valor);
    }
}