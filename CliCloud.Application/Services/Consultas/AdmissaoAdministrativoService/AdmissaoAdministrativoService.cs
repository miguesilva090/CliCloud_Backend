using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.DisponibilidadeMedico;
using CliCloud.Application.Services.Consultas.DisponibilidadeSala;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

public class AdmissaoAdministrativoService(
  IRepositoryAsync repository,
  IMapper mapper,
  IRequisicaoEspFechoUpdater requisicaoEspFechoUpdater,
  IUtilizadorDisplayNameResolver utilizadorDisplayNameResolver,
  ICurrentClinicaService? currentClinicaService = null
) : IAdmissaoAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly IMapper _mapper = mapper;
  private readonly IRequisicaoEspFechoUpdater _requisicaoEspFechoUpdater = requisicaoEspFechoUpdater;
  private readonly IUtilizadorDisplayNameResolver _utilizadorDisplayNameResolver =
    utilizadorDisplayNameResolver;
  private readonly ICurrentClinicaService? _currentClinicaService = currentClinicaService;

  public async Task<PaginatedResponse<AdmissaoTableDTO>> GetPaginatedAsync(AdmissaoTableFilter filter)
  {
    if (filter.Filters?.Count > 0)
    {
      filter.PageNumber = 1;
    }

    string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
    DateTime dataRef = (filter.DataReferencia ?? DateTime.UtcNow).Date;
    var spec = new AdmissaoSearchTable(filter.Modo, dataRef, filter.Filters ?? [], order);
    PaginatedResponse<AdmissaoTableDTO> result =
      await _repository.GetPaginatedResultsAsync<Admissao, AdmissaoTableDTO, Guid>(
        filter.PageNumber,
        filter.PageSize,
        spec
      );

    await HydrateUtenteNumerosAsync(result.Data);
    return result;
  }

  public async Task<Response<AdmissaoDTO>> GetByIdAsync(Guid id)
  {
    List<Admissao> list = (await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(id))).ToList();
    Admissao? entity = list.FirstOrDefault();
    if (entity == null)
    {
      return ResponseFactory.Fail<AdmissaoDTO>("Admissão não encontrada.");
    }

    AdmissaoDTO dto = _mapper.Map<AdmissaoDTO>(entity);
    await HydrateUtenteNumeroAsync(dto, entity.UtenteId);
    return ResponseFactory.Success(dto);
  }

  public async Task<Response<AdmissaoDTO?>> GetByConsultaMarcacaoIdAsync(Guid consultaMarcacaoId)
  {
    List<Admissao> list = (
      await _repository.GetListAsync<Admissao, Guid>(
        new AdmissaoByConsultaMarcacaoSpec(consultaMarcacaoId)
      )
    ).ToList();
    Admissao? entity = list.FirstOrDefault();
    if (entity == null)
    {
      return ResponseFactory.Success<AdmissaoDTO?>(null);
    }

    AdmissaoDTO dto = _mapper.Map<AdmissaoDTO>(entity);
    await HydrateUtenteNumeroAsync(dto, entity.UtenteId);
    return ResponseFactory.Success<AdmissaoDTO?>(dto);
  }

  public async Task<Response<Guid>> CreateAsync(CreateAdmissaoRequest request)
  {
    if (request.ConsultaMarcacaoId.HasValue)
    {
      List<Admissao> existentes = (
        await _repository.GetListAsync<Admissao, Guid>(
          new AdmissaoByConsultaMarcacaoSpec(request.ConsultaMarcacaoId.Value)
        )
      ).ToList();
      if (existentes.Count > 0)
      {
        return ResponseFactory.Fail<Guid>("Já existe uma admissão para esta marcação.");
      }
    }

    Admissao entity = _mapper.Map<Admissao>(request);
    entity.Id = Guid.NewGuid();
    entity.Pago = false;
    entity.Faturado = false;

    if (request.ConsultaMarcacaoId.HasValue)
    {
      ConsultaMarcacao? marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
        request.ConsultaMarcacaoId.Value
      );
      if (marcacao != null)
      {
        entity.EmTratamento = marcacao.EmTratamento;
        entity.SalaId ??= marcacao.SalaId;
      }
    }

    NormalizeServicos(entity);
    await AdmissaoHoraCalculoHelper.AplicarHoraFimAsync(entity, _repository);
    DisponibilidadeMedicoResult disponibilidade = await DisponibilidadeMedicoHelper.ValidarAsync(
      _repository,
      new DisponibilidadeMedicoRequest
      {
        MedicoId = entity.MedicoId,
        TipoConsultaId = entity.TipoConsultaId,
        Data = entity.Data,
        HoraInicio = entity.HoraInicio,
        HoraFim = entity.HoraFim,
        IgnorarMarcacaoId = entity.ConsultaMarcacaoId,
      }
    );
    if (!disponibilidade.Valido)
    {
      return ResponseFactory.Fail<Guid>(disponibilidade.Mensagem!);
    }

    entity.HoraFim = disponibilidade.HoraFim ?? entity.HoraFim;
    Clinica? clinica = await ObterClinicaAtualAsync();
    DisponibilidadeSalaResult sala = await DisponibilidadeSalaHelper.ValidarAsync(
      _repository,
      new DisponibilidadeSalaRequest
      {
        SalaId = entity.SalaId,
        Data = entity.Data,
        HoraInicio = entity.HoraInicio,
        HoraFim = entity.HoraFim,
        IgnorarMarcacaoId = entity.ConsultaMarcacaoId,
        RequerSala = clinica?.GestaoSalas == true,
      }
    );
    if (!sala.Valido)
    {
      return ResponseFactory.Fail<Guid>(sala.Mensagem!);
    }

    entity.SalaId = sala.SalaId;
    _ = await _repository.CreateAsync<Admissao, Guid>(entity);
    await SyncMarcacaoSalaAsync(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateAdmissaoRequest request)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    bool? pago = entity.Pago;
    bool? faturado = entity.Faturado;
    string? obs = entity.Obs;
    _mapper.Map(request, entity);
    entity.Pago = pago;
    entity.Faturado = faturado;
    entity.Obs = obs;
    await ReplaceServicosAdmissaoAsync(entity, request.Servicos ?? []);
    await AdmissaoHoraCalculoHelper.AplicarHoraFimAsync(entity, _repository);
    DisponibilidadeMedicoResult disponibilidade = await DisponibilidadeMedicoHelper.ValidarAsync(
      _repository,
      new DisponibilidadeMedicoRequest
      {
        MedicoId = entity.MedicoId,
        TipoConsultaId = entity.TipoConsultaId,
        Data = entity.Data,
        HoraInicio = entity.HoraInicio,
        HoraFim = entity.HoraFim,
        IgnorarAdmissaoId = entity.Id,
        IgnorarMarcacaoId = entity.ConsultaMarcacaoId,
      }
    );
    if (!disponibilidade.Valido)
    {
      return ResponseFactory.Fail<Guid>(disponibilidade.Mensagem!);
    }

    entity.HoraFim = disponibilidade.HoraFim ?? entity.HoraFim;
    Clinica? clinica = await ObterClinicaAtualAsync();
    DisponibilidadeSalaResult sala = await DisponibilidadeSalaHelper.ValidarAsync(
      _repository,
      new DisponibilidadeSalaRequest
      {
        SalaId = entity.SalaId,
        Data = entity.Data,
        HoraInicio = entity.HoraInicio,
        HoraFim = entity.HoraFim,
        IgnorarAdmissaoId = entity.Id,
        IgnorarMarcacaoId = entity.ConsultaMarcacaoId,
        RequerSala = clinica?.GestaoSalas == true,
      }
    );
    if (!sala.Valido)
    {
      return ResponseFactory.Fail<Guid>(sala.Mensagem!);
    }

    entity.SalaId = sala.SalaId;
    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    await SyncMarcacaoSalaAsync(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> DeleteAsync(Guid id)
  {
    await _repository.RemoveByIdAsync<Admissao, Guid>(id);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> ConfirmarAsync(Guid id, bool confirmado)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    if (entity.StatusConsulta
        is StatusConsulta.Faltou
        or StatusConsulta.FaltouJustificada)
    {
      return ResponseFactory.Fail<Guid>("Não é possível alterar a presença de uma admissão com falta.");
    }

    entity.Confirmado = confirmado;
    if (confirmado)
    {
      entity.HoraChegada = DateTime.Now.TimeOfDay;
      entity.Ordem ??= await AdmissaoOrdemHelper.ObterProximaOrdemDiaAsync(entity, _repository);
    }

    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> SetConfirmaConsultaAsync(Guid id, bool confirmaConsulta)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    if (entity.StatusConsulta
        is StatusConsulta.Faltou
        or StatusConsulta.FaltouJustificada)
    {
      return ResponseFactory.Fail<Guid>("Não é possível alterar a confirmação de uma admissão com falta.");
    }

    entity.ConfirmaConsulta = confirmaConsulta;
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> SetEmTratamentoAsync(Guid id, bool emTratamento)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    entity.EmTratamento = emTratamento;
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> SetEfetuadoAsync(Guid id, bool efetuado)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    entity.Efetuado = efetuado;
    entity.EmTratamento = efetuado ? false : entity.EmTratamento;
    entity.Ordem = efetuado
      ? await AdmissaoOrdemHelper.ObterProximaOrdemDiaAsync(entity, _repository)
      : null;

    if (efetuado)
    {
      entity.StatusConsulta = StatusConsulta.Concluida;
    }
    else if (entity.StatusConsulta == StatusConsulta.Concluida)
    {
      entity.StatusConsulta = null;
    }

    if (entity.ConsultaMarcacaoId.HasValue)
    {
      ConsultaMarcacao? marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
        entity.ConsultaMarcacaoId.Value
      );

      if (marcacao != null)
      {
        Consulta? consulta = null;
        if (marcacao.ConsultaId.HasValue)
        {
          consulta = await _repository.GetByIdAsync<Consulta, Guid>(marcacao.ConsultaId.Value);
        }

        if (efetuado)
        {
          marcacao.StatusConsulta = StatusConsulta.Concluida;
          marcacao.EmTratamento = false;

          if (consulta != null && consulta.DeletedOn == null)
          {
            consulta.Efetuado = true;
            consulta.StatusConsulta = StatusConsulta.Concluida;
            consulta.HoraFim ??= DateTime.Now.TimeOfDay;
            _ = await _repository.UpdateAsync<Consulta, Guid>(consulta);
          }
        }
        else if (consulta == null || consulta.DeletedOn != null || consulta.Efetuado != true)
        {
          if (marcacao.StatusConsulta == StatusConsulta.Concluida)
          {
            marcacao.StatusConsulta = null;
          }
        }

        _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
      }
    }

    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> DesmarcarAsync(Guid id, DesmarcarAdmissaoRequest request)
  {
    if (string.IsNullOrWhiteSpace(request.Motivo))
    {
      return ResponseFactory.Fail<Guid>("Indique o motivo da desmarcação.");
    }

    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);

    if (entity.Pago == true || entity.Faturado == true)
    {
      return ResponseFactory.Fail<Guid>(
        "A admissão tem recibo ou fatura associados e não pode ser desmarcada."
      );
    }

    if (!string.IsNullOrWhiteSpace(entity.Credencial))
    {
      bool podeReverter = await _requisicaoEspFechoUpdater.ReverterAgendamentoSePossivelAsync(
        entity.Credencial.Trim()
      );
      if (!podeReverter)
      {
        return ResponseFactory.Fail<Guid>("A requisição já está efetivada e não pode ser desmarcada.");
      }
    }

    string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
    string linhaMotivo =
      $"{nomeAutor} - {DateTime.Now:dd-MM-yyyy HH:mm}{Environment.NewLine}{request.Motivo!.Trim()}";
    entity.Obs = string.IsNullOrWhiteSpace(entity.Obs)
      ? linhaMotivo
      : $"{linhaMotivo}{Environment.NewLine}{Environment.NewLine}{entity.Obs}";

    entity.StatusConsulta = StatusConsulta.Desmarcada;
    entity.DataHoraMarcacao = DateTime.UtcNow;
    entity.Confirmado = false;

    if (entity.ConsultaMarcacaoId.HasValue)
    {
      ConsultaMarcacao? marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
        entity.ConsultaMarcacaoId.Value
      );
      if (marcacao != null)
      {
        marcacao.StatusConsulta = StatusConsulta.Desmarcada;
        marcacao.Obs = entity.Obs;
        _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
      }
    }

    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<PromoverAdmissaoResultDTO>> PromoverParaConsultaAsync(Guid id)
  {
    List<Admissao> list = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(id))
    ).ToList();
    Admissao? admissao = list.FirstOrDefault();
    if (admissao == null)
    {
      return ResponseFactory.Fail<PromoverAdmissaoResultDTO>("Admissão não encontrada.");
    }

    if (admissao.StatusConsulta is StatusConsulta.Desmarcada or StatusConsulta.Suspensa)
    {
      return ResponseFactory.Fail<PromoverAdmissaoResultDTO>(
        "Não é possível passar para histórico uma admissão desmarcada ou suspensa."
      );
    }

    bool sugerirMarcacoesFisio = admissao.TipoAdmissao?.CodigoLegado == 1;

    try
    {
      Guid consultaId = await AdmissaoPromocaoRunner.PromoverAsync(
        admissao,
        _repository,
        _requisicaoEspFechoUpdater
      );
      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(
        new PromoverAdmissaoResultDTO
        {
          ConsultaId = consultaId,
          SugerirMarcacoesFisio = sugerirMarcacoesFisio,
        }
      );
    }
    catch (InvalidOperationException ex)
    {
      return ResponseFactory.Fail<PromoverAdmissaoResultDTO>(ex.Message);
    }
  }

  public async Task<Response<FechoDiarioResultDTO>> PromoverLoteAsync(PromoverAdmissaoLoteRequest request)
  {
    if (request.Ids == null || request.Ids.Count == 0)
    {
      return ResponseFactory.Fail<FechoDiarioResultDTO>("Selecione pelo menos uma admissão.");
    }

    var result = new FechoDiarioResultDTO { TotalElegiveis = request.Ids.Count };
    HashSet<Guid> ids = request.Ids.Distinct().ToHashSet();

    try
    {
      foreach (Guid id in ids)
      {
        result.TotalProcessadas++;

        List<Admissao> list = (
          await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(id))
        ).ToList();
        Admissao? admissao = list.FirstOrDefault();
        if (admissao == null)
        {
          result.TotalIgnoradas++;
          result.Avisos.Add($"Admissão {id}: não encontrada.");
          continue;
        }

        if (admissao.StatusConsulta == StatusConsulta.Desmarcada)
        {
          result.TotalIgnoradas++;
          result.Avisos.Add($"Admissão {id}: desmarcada (ignorada).");
          continue;
        }

        _ = await AdmissaoPromocaoRunner.PromoverAsync(
          admissao,
          _repository,
          _requisicaoEspFechoUpdater
        );
        result.TotalConsultasCriadas++;
      }

      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(result);
    }
    catch (Exception ex)
    {
      _repository.ClearChangeTracker();
      return ResponseFactory.Fail<FechoDiarioResultDTO>($"Promoção em lote cancelada: {ex.Message}");
    }
  }

  public async Task<Response<AdmissaoObservacoesDTO>> GetObservacoesAsync(Guid id)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    return ResponseFactory.Success(
      new AdmissaoObservacoesDTO { Observacoes = entity.Obs ?? string.Empty }
    );
  }

  public async Task<Response<Guid>> AppendObservacaoAsync(
    Guid id,
    AppendAdmissaoObservacaoRequest request
  )
  {
    if (string.IsNullOrWhiteSpace(request.Texto))
    {
      return ResponseFactory.Fail<Guid>("Indique o texto da observação.");
    }

    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
    entity.Obs = AdmissaoObservacoesHelper.FormatarObservacaoAppend(
      request.Texto,
      nomeAutor,
      entity.Obs
    );

    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  private static void NormalizeServicos(Admissao entity)
  {
    int linha = 1;
    foreach (AdmissaoServico servico in entity.Servicos.OrderBy(s => s.Linha))
    {
      if (servico.Id == Guid.Empty)
      {
        servico.Id = Guid.NewGuid();
      }

      servico.AdmissaoId = entity.Id;
      if (servico.Linha <= 0)
      {
        servico.Linha = linha;
      }

      linha++;
    }
  }

  private async Task ReplaceServicosAdmissaoAsync(
    Admissao entity,
    IReadOnlyList<AdmissaoServicoDTO> linhas
  )
  {
    List<AdmissaoServico> existentes = (
      await _repository.GetListAsync<AdmissaoServico, Guid>(
        new AdmissaoServicosPorAdmissaoIdSpec(entity.Id)
      )
    ).ToList();

    foreach (AdmissaoServico s in existentes)
    {
      await _repository.RemoveAsync<AdmissaoServico, Guid>(s);
    }

    entity.Servicos.Clear();
    foreach (AdmissaoServicoDTO dto in linhas)
    {
      AdmissaoServico servico = _mapper.Map<AdmissaoServico>(dto);
      servico.Id = servico.Id == Guid.Empty ? Guid.NewGuid() : servico.Id;
      servico.AdmissaoId = entity.Id;
      servico.Admissao = entity;
      entity.Servicos.Add(servico);
      _ = await _repository.CreateAsync<AdmissaoServico, Guid>(servico);
    }

    NormalizeServicos(entity);
  }

  private async Task HydrateUtenteNumerosAsync(IReadOnlyCollection<AdmissaoTableDTO> rows)
  {
    List<Guid> ids = rows.Select(r => r.UtenteId).Distinct().ToList();
    if (ids.Count == 0)
    {
      return;
    }

    var spec = new UtenteNumerosByIdsSpecification(ids);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();

    foreach (AdmissaoTableDTO row in rows)
    {
      UtenteNumeroLookupDTO? u = lookups.FirstOrDefault(x => x.Id == row.UtenteId);
      if (u != null)
      {
        row.UtenteNumero = u.NumeroUtente;
      }
    }
  }

  private async Task HydrateUtenteNumeroAsync(AdmissaoDTO dto, Guid utenteId)
  {
    var spec = new UtenteNumerosByIdsSpecification([utenteId]);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();
    UtenteNumeroLookupDTO? u = lookups.FirstOrDefault();
    if (u != null)
    {
      dto.UtenteNumero = u.NumeroUtente;
    }
  }

  private async Task<Clinica?> ObterClinicaAtualAsync()
  {
    if (_currentClinicaService == null
      || string.IsNullOrWhiteSpace(_currentClinicaService.ClinicaId)
      || !Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId))
    {
      return null;
    }

    return await _repository.GetByIdAsync<Clinica, Guid>(clinicaId);
  }

  public async Task<Response<AdmissaoDebitoFaturacaoDTO>> GetDebitoFaturacaoAsync(Guid admissaoId)
  {
    List<Admissao> list = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(admissaoId))
    ).ToList();

    Admissao? admissao = list.FirstOrDefault();
    if (admissao == null)
    {
      return ResponseFactory.Fail<AdmissaoDebitoFaturacaoDTO>("Admissão não encontrada.");
    }

    List<AdmissaoServico> servicos = (admissao.Servicos ?? []).ToList();
    int servicosTotal = servicos.Count;

    if (servicosTotal == 0)
    {
      return ResponseFactory.Success(
        new AdmissaoDebitoFaturacaoDTO
        {
          Debito = 0m,
          PodeFaturar = false,
          ServicosComDebito = 0,
          ServicosTotal = 0,
        }
      );
    }

    List<Guid> servicoIds = servicos.Select(s => s.Id).ToList();
    HashSet<Guid> jaFaturados = await AdmissaoServicoFaturacaoQueryHelper
      .ObterAdmissaoServicoIdsJaFaturadosAsync(_repository, servicoIds);

    decimal debito = 0m;
    int comDebito = 0;
    List<Guid> idsComDebito = [];

    foreach (AdmissaoServico s in servicos)
    {
      if (jaFaturados.Contains(s.Id))
      {
        continue;
      }

      decimal q = s.Quantidade.GetValueOrDefault(1m);
      if (q <= 0m)
      {
        q = 1m;
      }

      decimal valor = (s.ValorServico ?? s.ValorArtigo ?? 0m) * q;
      if (valor <= 0m)
      {
        continue;
      }

      debito += valor;
      comDebito++;
      idsComDebito.Add(s.Id);
    }

    bool podeFaturar = comDebito > 0 && debito > 0m;

    return ResponseFactory.Success(
      new AdmissaoDebitoFaturacaoDTO
      {
        Debito = debito,
        PodeFaturar = podeFaturar,
        ServicosComDebito = comDebito,
        ServicosTotal = servicosTotal,
        AdmissaoServicoIdsComDebito = idsComDebito,
      }
    );
  }

  private async Task SyncMarcacaoSalaAsync(Admissao entity)
  {
    if (!entity.ConsultaMarcacaoId.HasValue)
    {
      return;
    }

    ConsultaMarcacao? marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
      entity.ConsultaMarcacaoId.Value
    );
    if (marcacao == null)
    {
      return;
    }

    marcacao.SalaId = entity.SalaId;
    _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
  }

}