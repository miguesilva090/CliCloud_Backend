using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Notificacoes.NotificacaoService.DTOs;
using CliCloud.Application.Services.Notificacoes.NotificacaoService.Filters;
using CliCloud.Application.Services.Notificacoes.NotificacaoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Notificacoes;
using NotificacaoEntity = CliCloud.Domain.Entities.Notificacoes.Notificacao;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService;

public class NotificacaoService : INotificacaoService
{
  private readonly IRepositoryAsync _repository;
  private readonly IMapper _mapper;
  private readonly ICurrentTenantUserService _currentTenantUserService;
  private readonly ICurrentClinicaService _currentClinicaService;

  public NotificacaoService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentTenantUserService currentTenantUserService,
    ICurrentClinicaService currentClinicaService)
  {
    _repository = repository;
    _mapper = mapper;
    _currentTenantUserService = currentTenantUserService;
    _currentClinicaService = currentClinicaService;
  }

  public async Task<PaginatedResponse<NotificacaoTableDTO>> GetNotificacaoPaginatedAsync(NotificacaoTableFilter filter)
  {
    (Guid utilizadorId, Guid? clinicaId) = await ResolveContextoAsync();
    if (filter.Filters is { Count: > 0 })
      filter.PageNumber = 1;

    string dynamicOrder = filter.Sorting is { Count: > 0 } ? GSHelpers.GenerateOrderByString(filter) : "";
    NotificacaoSearchTable specification = new(
      filter.Filters ?? [],
      dynamicOrder,
      filter.ListMode,
      utilizadorId,
      clinicaId);

    return await _repository.GetPaginatedResultsAsync<NotificacaoEntity, NotificacaoTableDTO, Guid>(
      filter.PageNumber,
      filter.PageSize,
      specification);
  }

  public async Task<Response<IEnumerable<NotificacaoTableDTO>>> GetAllNotificacaoAsync(NotificacaoAllFilter filter)
  {
    try
    {
      filter ??= new NotificacaoAllFilter();
      (Guid utilizadorId, Guid? clinicaId) = await ResolveContextoAsync();
      string dynamicOrder = filter.GetOrderByString();
      List<TableFilter> tableFilters = filter.Filters ?? [];
      NotificacaoSearchTable specification = new(
        tableFilters,
        dynamicOrder,
        filter.ListMode,
        utilizadorId,
        clinicaId);

      IEnumerable<NotificacaoTableDTO> list =
        await _repository.GetListAsync<NotificacaoEntity, NotificacaoTableDTO, Guid>(specification);
      return ResponseFactory.Success<IEnumerable<NotificacaoTableDTO>>(list);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<IEnumerable<NotificacaoTableDTO>>(ex.Message);
    }
  }

  public async Task<Response<NotificacaoDTO>> GetNotificacaoAsync(Guid id)
  {
    try
    {
      (Guid utilizadorId, Guid? clinicaId) = await ResolveContextoAsync();
      NotificacaoByIdWithTipo specification = new();
      NotificacaoEntity entity = await _repository.GetByIdAsync<NotificacaoEntity, Guid>(id, specification);
      if (!PodeVisualizar(entity, utilizadorId, clinicaId))
        return ResponseFactory.Fail<NotificacaoDTO>("Notificação não encontrada ou sem permissão.");

      NotificacaoDTO dto = _mapper.Map<NotificacaoDTO>(entity);
      return ResponseFactory.Success(dto);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<NotificacaoDTO>(ex.Message);
    }
  }

  public async Task<Response<Guid>> CreateNotificacaoAsync(CreateNotificacaoRequest request)
  {
    try
    {
      (Guid utilizadorId, Guid? clinicaId) = await ResolveContextoAsync();
      bool tipoExiste = await _repository.ExistsAsync<NotificacaoTipo, Guid>(
        new NotificacaoTipoExistsById(request.NotificacaoTipoId));
      if (!tipoExiste)
        return ResponseFactory.Fail<Guid>("Tipo de notificação inválido.");

      NotificacaoEntity entity = _mapper.Map(request, new NotificacaoEntity { Id = Guid.NewGuid() });
      entity.RemetenteId = utilizadorId;
      if (entity.ClinicaDestinoId == null)
        entity.ClinicaDestinoId = clinicaId;

      NotificacaoEntity response = await _repository.CreateAsync<NotificacaoEntity, Guid>(entity);
      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(response.Id);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<Guid>(ex.Message);
    }
  }

  public async Task<Response<Guid>> MarcarComoLidaAsync(Guid id)
  {
    try
    {
      (Guid utilizadorId, Guid? clinicaId) = await ResolveContextoAsync();
      NotificacaoEntity entity = await _repository.GetByIdAsync<NotificacaoEntity, Guid>(id);
      if (!PodeVisualizar(entity, utilizadorId, clinicaId))
        return ResponseFactory.Fail<Guid>("Notificação não encontrada ou sem permissão.");

      if (entity.DestinatarioUtilizadorId != utilizadorId)
        return ResponseFactory.Fail<Guid>("Só é possível marcar como lida uma notificação pessoal recebida na sua caixa.");

      entity.DataLeitura = DateTime.UtcNow;
      entity.LeituraPor = utilizadorId;
      entity.Estado = 1;
      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(entity.Id);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<Guid>(ex.Message);
    }
  }

  public async Task<Response<Guid>> DeleteNotificacaoAsync(Guid id)
  {
    try
    {
      (Guid utilizadorId, Guid? clinicaId) = await ResolveContextoAsync();
      NotificacaoEntity entity = await _repository.GetByIdAsync<NotificacaoEntity, Guid>(id);
      if (!PodeEliminar(entity, utilizadorId, clinicaId))
        return ResponseFactory.Fail<Guid>("Não tem permissão para eliminar esta notificação.");

      NotificacaoEntity removed = await _repository.RemoveByIdAsync<NotificacaoEntity, Guid>(id);
      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(removed.Id);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<Guid>(ex.Message);
    }
  }

  private async Task<(Guid UtilizadorId, Guid? ClinicaId)> ResolveContextoAsync()
  {
    _currentTenantUserService.SetUser();
    await _currentClinicaService.SetClinicaAsync();

    if (string.IsNullOrWhiteSpace(_currentTenantUserService.UserId)
        || !Guid.TryParse(_currentTenantUserService.UserId.Trim(), out Guid utilizadorId))
      throw new InvalidOperationException("Utilizador autenticado inválido.");

    Guid? clinicaId = null;
    if (!string.IsNullOrWhiteSpace(_currentClinicaService.ClinicaId)
        && Guid.TryParse(_currentClinicaService.ClinicaId.Trim(), out Guid cid))
      clinicaId = cid;

    return (utilizadorId, clinicaId);
  }

  private static bool ClinicaPermiteAcesso(NotificacaoEntity n, Guid? clinicaId)
  {
    if (clinicaId == null)
      return n.ClinicaDestinoId == null;
    return n.ClinicaDestinoId == null || n.ClinicaDestinoId == clinicaId;
  }

  private static bool PodeVisualizar(NotificacaoEntity n, Guid utilizadorId, Guid? clinicaId)
  {
    if (!ClinicaPermiteAcesso(n, clinicaId))
      return false;

    if (n.DestinatarioUtilizadorId == utilizadorId)
      return true;
    if (n.RemetenteId == utilizadorId)
      return true;
    return n.ClinicaDestinoId == clinicaId && n.DestinatarioUtilizadorId == null;
  }

  private static bool PodeEliminar(NotificacaoEntity n, Guid utilizadorId, Guid? clinicaId)
  {
    if (!ClinicaPermiteAcesso(n, clinicaId))
      return false;

    if (n.RemetenteId == utilizadorId)
      return true;
    if (n.DestinatarioUtilizadorId == utilizadorId)
      return true;
    return false;
  }
}
