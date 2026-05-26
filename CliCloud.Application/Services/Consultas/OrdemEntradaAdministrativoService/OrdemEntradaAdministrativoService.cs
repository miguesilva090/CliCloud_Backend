using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.DisponibilidadeMedico;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;
using CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.Specifications;
using CliCloud.Application.Services.Medicos.FolgasMedicoService.Specifications;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.Specifications;
using CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.Specifications;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Application.Services.Consultas.DisponibilidadeSala;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService;

public class OrdemEntradaAdministrativoService(
  IRepositoryAsync repository,
  IUtilizadorDisplayNameResolver utilizadorDisplayNameResolver,
  IRequisicaoEspFechoUpdater requisicaoEspFechoUpdater,
  ICurrentClinicaService? currentClinicaService = null
) : IOrdemEntradaAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly IUtilizadorDisplayNameResolver _utilizadorDisplayNameResolver =
    utilizadorDisplayNameResolver;
  private readonly IRequisicaoEspFechoUpdater _requisicaoEspFechoUpdater =
    requisicaoEspFechoUpdater;

  private readonly ICurrentClinicaService? _currentClinicaService = currentClinicaService;

  public async Task<PaginatedResponse<OrdemEntradaTableDTO>> GetPaginatedAsync(
    OrdemEntradaTableFilter filter
  )
  {
    if(filter.Filters?.Count > 0)
    {
      filter.PageNumber = 1;
    }

    string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
    var spec = new OrdemEntradaSearchTable(filter, order);
    PaginatedResponse<OrdemEntradaTableDTO> result = 
      await _repository.GetPaginatedResultsAsync<Admissao, OrdemEntradaTableDTO, Guid>(
        filter.PageNumber, 
        filter.PageSize,
        spec
      );

    await HydrateOrdemEntradaUtenteNumerosAsync(result.Data);
    await HydrateOrdemEntradaCreatedByNomesAsync(result.Data);
    HydrateOrdemEntradaConsultaPromovida(result.Data);
    ApplyOrdemEntradaStatusLabels(result.Data);
    return result;
  }

  public async Task<Response<Guid>> AnularAsync(
    Guid id, 
    AnularOrdemEntradaRequest request
  )
  {
    if(string.IsNullOrWhiteSpace(request.Motivo))
    {
      return ResponseFactory.Fail<Guid>("Indique o motivo da anulação");
    }

    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    if(entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<Guid>("Admissão não encontrada");
    }
    if(entity.Pago == true || entity.Faturado == true)
    {
      return ResponseFactory.Fail<Guid>("A consulta tem recibo ou fatura associados e não pode ser anulada");
    }

    if (!string.IsNullOrWhiteSpace(entity.Credencial))
    {
      bool podeReverter = await _requisicaoEspFechoUpdater.ReverterAgendamentoSePossivelAsync(
        entity.Credencial.Trim()
      );
      if (!podeReverter)
      {
        return ResponseFactory.Fail<Guid>("A requisição já está efetivada e não pode ser anulada.");
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
    return ResponseFactory.Success(entity.Id);
  }

  private async Task HydrateOrdemEntradaUtenteNumerosAsync(
    IReadOnlyCollection<OrdemEntradaTableDTO> rows
  )
  {
    List<Guid> ids = rows.Select(r => r.UtenteId).Distinct().ToList();
    if(ids.Count == 0)
    {
      return;
    }

    var spec = new UtenteNumerosByIdsSpecification(ids);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();

    foreach(OrdemEntradaTableDTO row in rows)
    {
      UtenteNumeroLookupDTO? u = lookups.FirstOrDefault(x => x.Id == row.UtenteId);
      if(u != null)
      {
        row.UtenteNumero = u.NumeroUtente;
      }
    }
  }

  private async Task HydrateOrdemEntradaCreatedByNomesAsync(
    IReadOnlyCollection<OrdemEntradaTableDTO> rows
  )
  {
    List<Guid> ids = rows.Select(r => r.CreatedBy).Distinct().ToList();
    if (ids.Count == 0)
    {
      return;
    }

    IReadOnlyDictionary<Guid, string> nomes =
      await _utilizadorDisplayNameResolver.ResolveManyByIdsAsync(ids);

    foreach (OrdemEntradaTableDTO row in rows)
    {
      if (nomes.TryGetValue(row.CreatedBy, out string? nome))
      {
        row.CreatedByNome = nome;
      }
    }
  }

  private static void HydrateOrdemEntradaConsultaPromovida(
    IReadOnlyCollection<OrdemEntradaTableDTO> rows
  )
  {
    foreach(OrdemEntradaTableDTO row in rows)
    {
      if(row.ConsultaId.HasValue)
      {
        row.ConsultaPromovida = true;
      }
    }
  }

  private static void ApplyOrdemEntradaStatusLabels(
    IReadOnlyCollection<OrdemEntradaTableDTO> rows
  )
  {
    foreach(OrdemEntradaTableDTO row in rows)
    {
      row.StatusConsultaLabel = row.StatusConsulta?.ToString();
    }
  }

  public async Task<Response<OrdemEntradaRegistoDTO>> GetRegistoAsync(Guid id)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    if (entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<OrdemEntradaRegistoDTO>("Admissão não encontrada.");
    }

    OrdemEntradaRegistoDTO dto = new()
    {
      Id = entity.Id,
      UtenteId = entity.UtenteId,
      OrganismoId = entity.OrganismoId,
      MedicoId = entity.MedicoId,
      EspecialidadeId = entity.EspecialidadeId,
      TipoAdmissaoId = entity.TipoAdmissaoId,
      TipoConsultaId = entity.TipoConsultaId,
      Data = entity.Data,
      HoraInicio = entity.HoraInicio,
      HoraFim = entity.HoraFim,
      Obs = entity.Obs,
      DataHoraMarcacao = entity.DataHoraMarcacao,
      SalaId = entity.SalaId,
    };

    await HydrateOrdemEntradaRegistoAsync(dto, entity);
    return ResponseFactory.Success(dto);
  }

  public async Task<Response<Guid>> CreateRegistoAsync(
    SaveOrdemEntradaRegistoRequest request
  )
  {
    Admissao draft = new()
    {
      Id = Guid.NewGuid(),
      UtenteId = request.UtenteId,
      OrganismoId = request.OrganismoId,
      MedicoId = request.MedicoId,
      TipoAdmissaoId = request.TipoAdmissaoId,
      TipoConsultaId = request.TipoConsultaId,
      Data = request.Data.Date,
      HoraInicio = request.HoraInicio,
      Origem = OrigemAdmissao.Marcacao,
      Pago = false,
      Faturado = false,
      DataHoraMarcacao = DateTime.Now,
      SalaId = request.SalaId,
    };

    string? erroDados = await AplicarDadosDerivadosOrdemEntradaAsync(draft, request);
    if (erroDados != null)
    {
      return ResponseFactory.Fail<Guid>(erroDados);
    }

    string? erroDisponibilidade =
      await ValidarDisponibilidadeOrdemEntradaAsync(draft, request.Duracao, null);
    if (erroDisponibilidade != null)
    {
      return ResponseFactory.Fail<Guid>(erroDisponibilidade);
    }

    string? erroSala = await ValidarDisponibilidadeSalaOrdemEntradaAsync(draft, null);
    if (erroSala != null)
    {
      return ResponseFactory.Fail<Guid>(erroSala);
    }

    string? observacoesFormatadas = null;
    if (!string.IsNullOrWhiteSpace(request.Observacoes))
    {
      string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
      observacoesFormatadas =
        $"{nomeAutor} - {DateTime.Now:dd-MM-yyyy HH:mm}{Environment.NewLine}{request.Observacoes!.Trim()}";
    }

    ConsultaMarcacao marcacao = await _repository.CreateAsync<ConsultaMarcacao, Guid>(
      new ConsultaMarcacao
      {
        Id = Guid.NewGuid(),
        UtenteId = request.UtenteId,
        MedicoId = request.MedicoId,
        EspecialidadeId = draft.EspecialidadeId,
        Data = request.Data.Date,
        HoraMarcacao = request.HoraInicio,
        TipoAdmissaoId = request.TipoAdmissaoId,
        TipoConsultaId = request.TipoConsultaId,
        SalaId = draft.SalaId,
        Obs = observacoesFormatadas,
      }
    );
    Admissao admissao = await MarcacaoAdmissaoSyncHelper.SyncAdmissaoFromMarcacaoAsync(
      _repository,
      marcacao,
      new MarcacaoAdmissaoSyncHelper.MarcacaoAdmissaoFields
      {
        OrganismoId = request.OrganismoId,
        HoraFim = draft.HoraFim,
        SalaId = draft.SalaId,
      }
    );
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(admissao.Id);
  }

  public async Task<Response<Guid>> UpdateRegistoAsync(
    Guid id,
    SaveOrdemEntradaRegistoRequest request
  )
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    if (entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<Guid>("Admissão não encontrada.");
    }

    if (entity.Pago == true || entity.Faturado == true)
    {
      return ResponseFactory.Fail<Guid>(
        "A consulta tem recibo ou fatura associados e não pode ser alterada."
      );
    }

    entity.UtenteId = request.UtenteId;
    entity.MedicoId = request.MedicoId;
    entity.TipoAdmissaoId = request.TipoAdmissaoId;
    entity.TipoConsultaId = request.TipoConsultaId;
    entity.SalaId = request.SalaId;
    entity.Data = request.Data.Date;
    entity.HoraInicio = request.HoraInicio;
    entity.DataHoraMarcacao = DateTime.Now;
    entity.OrganismoId = request.OrganismoId;

    string? erroDados = await AplicarDadosDerivadosOrdemEntradaAsync(entity, request);
    if (erroDados != null)
    {
      return ResponseFactory.Fail<Guid>(erroDados);
    }

    string? erroDisponibilidade =
      await ValidarDisponibilidadeOrdemEntradaAsync(entity, request.Duracao, entity.Id);
    if (erroDisponibilidade != null)
    {
      return ResponseFactory.Fail<Guid>(erroDisponibilidade);
    }

    string? erroSala = await ValidarDisponibilidadeSalaOrdemEntradaAsync(
      entity,
      entity.ConsultaMarcacaoId
    );
    if (erroSala != null)
    {
      return ResponseFactory.Fail<Guid>(erroSala);
    }

    ConsultaMarcacao? marcacao = null;
    if (entity.ConsultaMarcacaoId.HasValue)
    {
      marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
        entity.ConsultaMarcacaoId.Value
      );
    }

    marcacao ??= await _repository.CreateAsync<ConsultaMarcacao, Guid>(
      new ConsultaMarcacao { Id = Guid.NewGuid(), Obs = entity.Obs }
    );
    marcacao.UtenteId = request.UtenteId;
    marcacao.MedicoId = request.MedicoId;
    marcacao.EspecialidadeId = entity.EspecialidadeId;
    marcacao.Data = request.Data.Date;
    marcacao.HoraMarcacao = request.HoraInicio;
    marcacao.TipoAdmissaoId = request.TipoAdmissaoId;
    marcacao.TipoConsultaId = request.TipoConsultaId;
    marcacao.SalaId = entity.SalaId;
    marcacao.StatusConsulta = entity.StatusConsulta;

    if (entity.ConsultaMarcacaoId.HasValue)
    {
      _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
    }

    Admissao admissao = await MarcacaoAdmissaoSyncHelper.SyncAdmissaoFromMarcacaoAsync(
      _repository,
      marcacao,
      new MarcacaoAdmissaoSyncHelper.MarcacaoAdmissaoFields
      {
        OrganismoId = entity.OrganismoId,
        HoraFim = entity.HoraFim,
        SalaId = entity.SalaId,
        Credencial = entity.Credencial,
      },
      entity
    );
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(admissao.Id);
  }

  public async Task<Response<OrdemEntradaHorasDisponiveisDTO>> GetHorasDisponiveisAsync(
    OrdemEntradaHorasDisponiveisRequest request
  )
  {
    if (request.MedicoId == Guid.Empty)
    {
      return ResponseFactory.Fail<OrdemEntradaHorasDisponiveisDTO>("Médico é obrigatório.");
    }

    if (request.TipoConsultaId == Guid.Empty)
    {
      return ResponseFactory.Fail<OrdemEntradaHorasDisponiveisDTO>(
        "Tipo de consulta é obrigatório."
      );
    }

    if (request.Data == default)
    {
      return ResponseFactory.Fail<OrdemEntradaHorasDisponiveisDTO>("Data é obrigatória.");
    }

    HorarioMedico? horario = await ObterHorarioMedicoOrdemEntradaAsync(request.MedicoId);
    if (horario == null)
    {
      return ResponseFactory.Fail<OrdemEntradaHorasDisponiveisDTO>(
        "O médico não tem horário configurado."
      );
    }

    TimeSpan intervalo = await ResolverDuracaoOrdemEntradaAsync(request.TipoConsultaId, horario);
    DateTime data = request.Data.Date;
    HorarioMedicoVariavel? horarioVariavel =
      await ObterHorarioVariavelOrdemEntradaAsync(request.MedicoId, data);
    List<FolgasMedico> folgas = await ObterFolgasMedicoOrdemEntradaAsync(request.MedicoId);
    List<Admissao> admissoes = await ObterAdmissoesOrdemEntradaMedicoDataAsync(
      request.MedicoId,
      data
    );

    List<(TimeSpan Inicio, TimeSpan Fim)> periodos =
      ResolverPeriodosOrdemEntrada(data, horario, horarioVariavel);
    List<string> horas = [];

    foreach ((TimeSpan inicio, TimeSpan fim) in periodos)
    {
      for (TimeSpan hora = inicio; hora.Add(intervalo) <= fim; hora = hora.Add(intervalo))
      {
        if (EstaBloqueadoPorFolgaOrdemEntrada(folgas, data, hora, hora.Add(intervalo)))
        {
          continue;
        }

        if (ExisteConflitoOrdemEntrada(admissoes, request.AdmissaoId, hora, hora.Add(intervalo)))
        {
          continue;
        }

        horas.Add(FormatHoraOrdemEntrada(hora));
      }
    }

    return ResponseFactory.Success(
      new OrdemEntradaHorasDisponiveisDTO
      {
        HorarioFlexivel = horario.HorarioFlexivel,
        Intervalo = intervalo,
        HorasPossiveis = horas.Distinct().OrderBy(x => x).ToList(),
      }
    );
  }

  private async Task<string?> AplicarDadosDerivadosOrdemEntradaAsync(
    Admissao entity,
    SaveOrdemEntradaRegistoRequest request
  )
  {
    Utente utente = await _repository.GetByIdAsync<Utente, Guid>(request.UtenteId);
    if (utente.DeletedOn != null)
    {
      return "Utente não encontrado.";
    }

    Organismo organismo = await _repository.GetByIdAsync<Organismo, Guid>(request.OrganismoId);
    if (organismo.DeletedOn != null)
    {
      return "Organismo não encontrado.";
    }

    Medico medico = await _repository.GetByIdAsync<Medico, Guid>(request.MedicoId);
    if (medico.DeletedOn != null)
    {
      return "Médico não encontrado.";
    }

    entity.EspecialidadeId = medico.EspecialidadeId;
    if (!entity.EspecialidadeId.HasValue)
    {
      return "O médico não tem especialidade configurada.";
    }

    return null;
  }

  private async Task<string?> ValidarDisponibilidadeOrdemEntradaAsync(
    Admissao entity,
    TimeSpan? duracao,
    Guid? ignorarAdmissaoId
  )
  {
    if (!entity.MedicoId.HasValue || !entity.TipoConsultaId.HasValue || !entity.Data.HasValue)
    {
      return "Médico, tipo de consulta e data são obrigatórios.";
    }

    HorarioMedico? horario = await ObterHorarioMedicoOrdemEntradaAsync(entity.MedicoId.Value);
    if (horario == null)
    {
      return "O médico não tem horário configurado.";
    }

    DisponibilidadeMedicoResult disponibilidade = await DisponibilidadeMedicoHelper.ValidarAsync(
      _repository,
      new DisponibilidadeMedicoRequest
      {
        MedicoId = entity.MedicoId,
        TipoConsultaId = entity.TipoConsultaId,
        Data = entity.Data,
        HoraInicio = entity.HoraInicio,
        Duracao = horario.HorarioFlexivel ? duracao : null,
        IgnorarAdmissaoId = ignorarAdmissaoId,
        IgnorarMarcacaoId = entity.ConsultaMarcacaoId,
        RequerMedico = true,
      }
    );
    if (!disponibilidade.Valido)
    {
      return disponibilidade.Mensagem;
    }

    entity.HoraFim = disponibilidade.HoraFim;
    return null;
  }

  private async Task<string?> ValidarDisponibilidadeSalaOrdemEntradaAsync(
    Admissao entity,
    Guid? ignorarMarcacaoId
  )
  {
    Clinica? clinica = await ObterClinicaAtualAsync();
    DisponibilidadeSalaResult disponibilidadeSala = await DisponibilidadeSalaHelper.ValidarAsync(
      _repository,
      new DisponibilidadeSalaRequest
      {
        SalaId = entity.SalaId,
        Data = entity.Data,
        HoraInicio = entity.HoraInicio,
        HoraFim = entity.HoraFim,
        IgnorarAdmissaoId = entity.Id,
        IgnorarMarcacaoId = ignorarMarcacaoId,
        RequerSala = clinica?.GestaoSalas == true,
      }
    );

    if (!disponibilidadeSala.Valido)
    {
      return disponibilidadeSala.Mensagem;
    }

    entity.SalaId = disponibilidadeSala.SalaId;
    return null;
  }

  private async Task HydrateOrdemEntradaRegistoAsync(
    OrdemEntradaRegistoDTO dto,
    Admissao entity
  )
  {
    Utente utente = await _repository.GetByIdAsync<Utente, Guid>(entity.UtenteId);
    dto.UtenteNome = utente.Nome;
    dto.UtenteNumero = utente.NumeroUtente;

    if (entity.OrganismoId.HasValue)
    {
      Organismo organismo = await _repository.GetByIdAsync<Organismo, Guid>(
        entity.OrganismoId.Value
      );
      dto.OrganismoNome = organismo.Nome;
    }

    if (entity.MedicoId.HasValue)
    {
      Medico medico = await _repository.GetByIdAsync<Medico, Guid>(entity.MedicoId.Value);
      dto.MedicoNome = medico.Nome;
    }

    if (entity.EspecialidadeId.HasValue)
    {
      Especialidade especialidade = await _repository.GetByIdAsync<Especialidade, Guid>(
        entity.EspecialidadeId.Value
      );
      dto.EspecialidadeDesignacao = especialidade.Nome;
    }

    if (entity.SalaId.HasValue)
    {
      Sala sala = await _repository.GetByIdAsync<Sala, Guid>(entity.SalaId.Value);
      dto.SalaNome = sala.Nome;
    }

    IReadOnlyDictionary<Guid, string> nomes =
      await _utilizadorDisplayNameResolver.ResolveManyByIdsAsync([entity.CreatedBy]);
    if (nomes.TryGetValue(entity.CreatedBy, out string? nome))
    {
      dto.CreatedByNome = nome;
    }
  }

  private async Task<HorarioMedico?> ObterHorarioMedicoOrdemEntradaAsync(Guid medicoId)
  {
    IEnumerable<HorarioMedico> list =
      await _repository.GetListAsync<HorarioMedico, Guid>(
        new HorarioMedicoSearchByMedicoId(medicoId)
      );
    return list.FirstOrDefault(x => x.DeletedOn == null);
  }

  private async Task<HorarioMedicoVariavel?> ObterHorarioVariavelOrdemEntradaAsync(
    Guid medicoId,
    DateTime data
  )
  {
    IEnumerable<HorarioMedicoVariavel> list =
      await _repository.GetListAsync<HorarioMedicoVariavel, Guid>(
        new HorarioMedicoVariavelSearchByMedicoId(medicoId)
      );
    return list.FirstOrDefault(x => x.DeletedOn == null && x.Data.Date == data.Date);
  }

  private async Task<List<FolgasMedico>> ObterFolgasMedicoOrdemEntradaAsync(Guid medicoId)
  {
    IEnumerable<FolgasMedico> list =
      await _repository.GetListAsync<FolgasMedico, Guid>(
        new FolgasMedicoSearchByMedicoId(medicoId)
      );
    return list.Where(x => x.DeletedOn == null).ToList();
  }

  private async Task<List<Admissao>> ObterAdmissoesOrdemEntradaMedicoDataAsync(
    Guid medicoId,
    DateTime data
  )
  {
    IEnumerable<Admissao> list = await _repository.GetListAsync<Admissao, Guid>(
      new AdmissoesOrdemEntradaPorMedicoDataSpec(medicoId, data)
    );
    return list
      .Where(x =>
        x.DeletedOn == null
        && x.StatusConsulta != StatusConsulta.Desmarcada
        && x.HoraInicio.HasValue
      )
      .ToList();
  }

  private async Task<TimeSpan> ResolverDuracaoOrdemEntradaAsync(
    Guid tipoConsultaId,
    HorarioMedico horario
  )
  {
    TipoConsultaItem? tipo = await _repository.GetByIdAsync<TipoConsultaItem, Guid>(
      tipoConsultaId
    );
    bool primeiraConsulta = AdmissaoTipoConsultaHelper.EhPrimeiraConsulta(tipo);
    TimeSpan? slot = primeiraConsulta
      ? horario.PrimeiraConsulta ?? horario.MinMarcacao
      : horario.MinMarcacao ?? horario.PrimeiraConsulta;
    return slot.GetValueOrDefault(TimeSpan.FromMinutes(15));
  }

  private static List<(TimeSpan Inicio, TimeSpan Fim)> ResolverPeriodosOrdemEntrada(
    DateTime data,
    HorarioMedico horario,
    HorarioMedicoVariavel? horarioVariavel
  )
  {
    if (horarioVariavel != null)
    {
      List<(TimeSpan Inicio, TimeSpan Fim)> periodos = [];
      AdicionarPeriodoOrdemEntrada(
        periodos,
        horarioVariavel.ManhaInicio,
        horarioVariavel.ManhaFim
      );
      AdicionarPeriodoOrdemEntrada(
        periodos,
        horarioVariavel.TardeInicio,
        horarioVariavel.TardeFim
      );
      if (periodos.Count > 0)
      {
        return periodos.OrderBy(x => x.Inicio).ToList();
      }
    }

    DiaSemana dia = (DiaSemana)(int)data.DayOfWeek;
    return horario.Horarios
      .Where(h => h.DiaSemana == dia && h.Inicio.HasValue && h.Fim.HasValue)
      .Select(h => (h.Inicio!.Value, h.Fim!.Value))
      .OrderBy(h => h.Item1)
      .ToList();
  }

  private static void AdicionarPeriodoOrdemEntrada(
    List<(TimeSpan Inicio, TimeSpan Fim)> periodos,
    TimeSpan? inicio,
    TimeSpan? fim
  )
  {
    if (inicio.HasValue && fim.HasValue && fim.Value > inicio.Value)
    {
      periodos.Add((inicio.Value, fim.Value));
    }
  }

  private static bool EstaBloqueadoPorFolgaOrdemEntrada(
    IEnumerable<FolgasMedico> folgas,
    DateTime data,
    TimeSpan inicio,
    TimeSpan fim
  )
  {
    foreach (FolgasMedico folga in folgas)
    {
      if (data.Date < folga.DataDe.Date || data.Date > folga.DataAte.Date)
      {
        continue;
      }

      if (folga.TodoDia || folga.MesInteiro)
      {
        return true;
      }

      if (IntervalosSobrepoem(inicio, fim, folga.ManhaInicio, folga.ManhaFim))
      {
        return true;
      }

      if (IntervalosSobrepoem(inicio, fim, folga.TardeInicio, folga.TardeFim))
      {
        return true;
      }
    }

    return false;
  }

  private static bool ExisteConflitoOrdemEntrada(
    IEnumerable<Admissao> admissoes,
    Guid? ignorarAdmissaoId,
    TimeSpan inicio,
    TimeSpan fim
  )
  {
    foreach (Admissao admissao in admissoes)
    {
      if (ignorarAdmissaoId.HasValue && admissao.Id == ignorarAdmissaoId.Value)
      {
        continue;
      }

      if (!admissao.HoraInicio.HasValue)
      {
        continue;
      }

      TimeSpan inicioExistente = admissao.HoraInicio.Value;
      TimeSpan fimExistente = admissao.HoraFim ?? inicioExistente;
      if (fimExistente <= inicioExistente)
      {
        fimExistente = inicioExistente.Add(TimeSpan.FromMinutes(15));
      }

      if (inicio < fimExistente && fim > inicioExistente)
      {
        return true;
      }
    }

    return false;
  }

  private static bool IntervalosSobrepoem(
    TimeSpan inicio,
    TimeSpan fim,
    TimeSpan? bloqueioInicio,
    TimeSpan? bloqueioFim
  ) =>
    bloqueioInicio.HasValue
    && bloqueioFim.HasValue
    && bloqueioInicio.Value < fim
    && bloqueioFim.Value > inicio;

  private static string FormatHoraOrdemEntrada(TimeSpan value) =>
    $"{value.Hours:D2}:{value.Minutes:D2}";

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
}
