using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Specifications;
using CliCloud.Application.Services.Medicos.FolgasMedicoService.Specifications;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.Specifications;
using CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.Specifications;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Application.Services.Core.SmsService;
using CliCloud.Application.Services.Medicos.MedicoService.Specifications;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;

public partial class MarcacoesAdministrativoService(
  IRepositoryAsync repository,
  IServicoSms servicoSms,
  ICurrentClinicaService? currentClinicaService = null
) : IMarcacoesAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly IServicoSms _servicoSms = servicoSms;
  private readonly ICurrentClinicaService? _currentClinicaService = currentClinicaService;

  public async Task<PaginatedResponse<MarcacaoAdministrativoTableDTO>> GetPaginatedAsync(
    MarcacaoAdministrativoTableFilter filter
  )
  {
    int pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
    int pageSize = filter.PageSize <= 0 ? 20 : filter.PageSize;

    var spec = new MarcacaoAdministrativoSearchTable(filter);
    IEnumerable<ConsultaMarcacao> list = await _repository.GetListAsync<ConsultaMarcacao, Guid>(spec);

    List<MarcacaoAdministrativoTableDTO> projected = list
      .Select(x => new MarcacaoAdministrativoTableDTO
      {
        Id = x.Id,
        Data = x.Data,
        HoraInicio = FormatHoraTimeSpan(x.HoraMarcacao),
        HoraFim = null,
        UtenteNumero = x.Utente?.NumeroUtente,
        UtenteNome = x.Utente?.Nome,
        MedicoNome = x.Medico?.Nome,
        EspecialidadeDesignacao = x.Especialidade?.Nome,
        OrganismoNome = null,
        StatusConsulta = x.StatusConsulta.HasValue ? (int)x.StatusConsulta.Value : null,
        StatusConsultaLabel = x.StatusConsulta?.ToString()
      })
      .ToList();

    int total = projected.Count;
    List<MarcacaoAdministrativoTableDTO> paged = projected
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToList();

    return new PaginatedResponse<MarcacaoAdministrativoTableDTO>(paged, total, pageNumber, pageSize);
  }

  public async Task<Response<MarcacaoAdministrativoDTO>> GetByIdAsync(Guid id)
  {
    var spec = new MarcacaoAdministrativoByIdSpec(id);
    ConsultaMarcacao? entity = (
      await _repository.GetListAsync<ConsultaMarcacao, Guid>(spec)
    ).FirstOrDefault();
    if (entity == null)
    {
      return ResponseFactory.Fail<MarcacaoAdministrativoDTO>("Marcação não encontrada.");
    }

    Admissao? admissao = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByConsultaMarcacaoSpec(id))
    ).FirstOrDefault();

    return ResponseFactory.Success(new MarcacaoAdministrativoDTO
    {
      Id = entity.Id,
      UtenteId = entity.UtenteId,
      MedicoId = entity.MedicoId,
      EspecialidadeId = entity.EspecialidadeId,
      OrganismoId = admissao?.OrganismoId,
      Data = entity.Data,
      HoraInicio = entity.HoraMarcacao,
      HoraFim = admissao?.HoraFim,
      TipoConsultaId = entity.TipoConsultaId,
      TipoAdmissaoId = entity.TipoAdmissaoId,
      Credencial = admissao?.Credencial,
      Obs = entity.Obs,
      StatusConsulta = entity.StatusConsulta.HasValue ? (int)entity.StatusConsulta.Value : null,
      Confirmado = admissao?.Confirmado,
      Efetuado = admissao?.Efetuado,
      UtenteNome = entity.Utente?.Nome,
      UtenteNumero = entity.Utente?.NumeroUtente,
      MedicoNome = entity.Medico?.Nome,
      EspecialidadeDesignacao = entity.Especialidade?.Nome,
      TipoConsultaDesignacao = entity.TipoConsultaItem?.Designacao,
      SalaId = entity.SalaId,
      SalaNome = entity.Sala?.Nome,
      AdmissaoId = admissao?.Id,
    });
  }

  public async Task<Response<Guid>> CreateAsync(CreateMarcacaoAdministrativoRequest request)
  {
    var entity = new ConsultaMarcacao
    {
      UtenteId = request.UtenteId,
      MedicoId = request.MedicoId,
      EspecialidadeId = request.EspecialidadeId,
      Data = request.Data.Date,
      HoraMarcacao = request.HoraInicio,
      TipoConsultaId = request.TipoConsultaId,
      TipoAdmissaoId = request.TipoAdmissaoId,
      Obs = request.Obs
    };

    ConsultaMarcacao created = await _repository.CreateAsync<ConsultaMarcacao, Guid>(entity);
    await MarcacaoAdmissaoSyncHelper.SyncAdmissaoFromMarcacaoAsync(
      _repository,
      created,
      new MarcacaoAdmissaoSyncHelper.MarcacaoAdmissaoFields
      {
        OrganismoId = request.OrganismoId,
        Credencial = request.Credencial,
        HoraFim = request.HoraFim,
      }
    );
    _ = await _repository.SaveChangesAsync();
    await MarcacaoConsultaSmsHelper.TentarDispararAsync(_repository, _servicoSms, created, "6.1");
    return ResponseFactory.Success(created.Id);
  }

  public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateMarcacaoAdministrativoRequest request)
  {
    ConsultaMarcacao? entity = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(id);
    if(entity == null)
    {
      return ResponseFactory.Fail<Guid>("Marcação não encontrada.");
    }

    entity.UtenteId = request.UtenteId;
    entity.MedicoId = request.MedicoId;
    entity.EspecialidadeId = request.EspecialidadeId;
    entity.Data = request.Data.Date;
    entity.HoraMarcacao = request.HoraInicio;
    entity.TipoConsultaId = request.TipoConsultaId;
    entity.TipoAdmissaoId = request.TipoAdmissaoId;
    entity.Obs = request.Obs;

    _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(entity);
    await MarcacaoAdmissaoSyncHelper.SyncAdmissaoFromMarcacaoAsync(
      _repository,
      entity,
      new MarcacaoAdmissaoSyncHelper.MarcacaoAdmissaoFields
      {
        OrganismoId = request.OrganismoId,
        Credencial = request.Credencial,
        HoraFim = request.HoraFim,
        SalaId = entity.SalaId,
      }
    );
    _ = await _repository.SaveChangesAsync();
    await MarcacaoConsultaSmsHelper.TentarDispararAsync(_repository, _servicoSms, entity, "6.2");
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> DesmarcarAsync(
    Guid id,
    DesmarcarMarcacaoAdministrativoRequest request
  )
  {
    ConsultaMarcacao? entity = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(id);
    if(entity == null)
    {
      return ResponseFactory.Fail<Guid>("Marcação não encontrada.");
    }

    entity.StatusConsulta = StatusConsulta.Desmarcada;
    entity.Obs = string.IsNullOrWhiteSpace(entity.Obs)
      ? request.Motivo
      : $"{entity.Obs}\n[{DateTime.Now:dd/MM/yyyy HH:mm}] {request.Motivo}";

    _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(entity);
    await MarcacaoAdmissaoSyncHelper.SyncDesmarcarFromMarcacaoAsync(_repository, entity);
    _ = await _repository.SaveChangesAsync();
    await MarcacaoConsultaSmsHelper.TentarDispararAsync(_repository, _servicoSms, entity, "6.2");
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> MudarHorarioAsync(
    Guid id,
    MudarHorarioMarcacaoAdministrativoRequest request
  )
  {
    ConsultaMarcacao? entity = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(id);
    if(entity == null)
    {
      return ResponseFactory.Fail<Guid>("Marcação não encontrada.");
    }

    entity.Data = request.Data.Date;
    entity.HoraMarcacao = request.HoraInicio;

    _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(entity);
    Admissao? admissao = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByConsultaMarcacaoSpec(id))
    ).FirstOrDefault();
    if (admissao != null)
    {
      admissao.Data = entity.Data;
      admissao.HoraInicio = entity.HoraMarcacao;
      await AdmissaoHoraCalculoHelper.AplicarHoraFimAsync(admissao, _repository);
      _ = await _repository.UpdateAsync<Admissao, Guid>(admissao);
    }

    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }


  public async Task<Response<IEnumerable<SalaDisponivelDTO>>> GetSalasDisponiveisAsync(
    SalasDisponiveisRequest request
  )
  {
    IEnumerable<Sala> salas = await _repository.GetListAsync<Sala, Guid>();
    IEnumerable<ConsultaMarcacao> marcacoes = await _repository.GetListAsync<ConsultaMarcacao, Guid>();

    HashSet<Guid> ocupadasNoSlot = marcacoes
      .Where(x =>
        x.SalaId.HasValue
        && x.Data.HasValue
        && x.Data.Value.Date == request.Data.Date
        && x.HoraMarcacao.HasValue
        && x.HoraMarcacao.Value == request.HoraInicio
        && (
          x.StatusConsulta == null
          || (
            x.StatusConsulta != StatusConsulta.Desmarcada
            && x.StatusConsulta != StatusConsulta.Suspensa
          )
        )
      )
      .Select(x => x.SalaId!.Value)
      .ToHashSet();

    IEnumerable<Sala> baseSalas = salas.Where(s => s.Ativa);
    if (request.ClinicaId.HasValue)
    {
      baseSalas = baseSalas.Where(s => s.ClinicaId == request.ClinicaId.Value);
    }

    List<SalaDisponivelDTO> result = baseSalas
      .OrderBy(s => s.NumeroSala)
      .Select(s => new SalaDisponivelDTO
      {
        Id = s.Id,
        Nome = s.Nome,
        NumeroSala = s.NumeroSala,
        ClinicaId = s.ClinicaId,
        Ativa = s.Ativa,
        Disponivel = !ocupadasNoSlot.Contains(s.Id),
      })
      .ToList();

    return ResponseFactory.Success<IEnumerable<SalaDisponivelDTO>>(result);
  }

  public async Task<Response<Guid>> AssociarSalaAsync(Guid marcacaoId, AssociarSalaMarcacaoRequest request)
  {
    ConsultaMarcacao? marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(marcacaoId);
    if(marcacao == null)
    {
      return ResponseFactory.Fail<Guid>("Marcação não encontrada.");
    }

    Sala? sala = await _repository.GetByIdAsync<Sala, Guid>(request.SalaId);
    if(sala == null || !sala.Ativa)
    {
      return ResponseFactory.Fail<Guid>("Sala inválida ou inativa.");
    }

    if(!marcacao.Data.HasValue || !marcacao.HoraMarcacao.HasValue)
    {
      return ResponseFactory.Fail<Guid>("Marcação não tem data e hora definida.");
    }

    IEnumerable<ConsultaMarcacao> marcacoes = await _repository.GetListAsync<ConsultaMarcacao, Guid>();
    bool conflito = marcacoes.Any(x =>
      x.Id != marcacao.Id
      && x.SalaId == request.SalaId
      && x.Data.HasValue
      && x.Data.Value.Date == marcacao.Data.Value.Date
      && x.HoraMarcacao.HasValue
      && x.HoraMarcacao.Value == marcacao.HoraMarcacao.Value
      && (
        x.StatusConsulta == null
        || (
          x.StatusConsulta != StatusConsulta.Desmarcada
          && x.StatusConsulta != StatusConsulta.Suspensa
        )
      )
    );

    if (conflito)
    {
      return ResponseFactory.Fail<Guid>("Sala já está ocupada para a data e hora da marcação");
    }

    marcacao.SalaId = request.SalaId;
    _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
    _ = await _repository.SaveChangesAsync();

    return ResponseFactory.Success(marcacao.Id);
  }

  public async Task<Response<Guid>> RemoverSalaAsync(Guid marcacaoId)
  {
    ConsultaMarcacao? marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(marcacaoId);
    if(marcacao == null)
    {
      return ResponseFactory.Fail<Guid>("Marcação não encontrada");
    }

    marcacao.SalaId = null;
    _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
    _ = await _repository.SaveChangesAsync();

    return ResponseFactory.Success(marcacao.Id);
  }

  public async Task<Response<TrocaMarcacoesMedicosPreviewDTO>> PreviewTrocaMedicosAsync(TrocaMarcacoesMedicosRequest request)
  {
    TrocaMarcacoesMedicosPreviewDTO preview = await BuildTrocaMedicosPreviewAsync(request);
    return ResponseFactory.Success(preview);
  }

  public async Task<Response<TrocaMarcacoesMedicosResultDTO>> ExecutarTrocaMedicosAsync(TrocaMarcacoesMedicosRequest request)
  {
    TrocaMarcacoesMedicosPreviewDTO preview = await BuildTrocaMedicosPreviewAsync(request);
    if(!preview.PodeExecutar)
    {
      string mensagem = preview.Conflitos.FirstOrDefault()?.Mensagem
        ?? "Não foi possível trocar as marcações. Verifique os conflitos";
      return ResponseFactory.Fail<TrocaMarcacoesMedicosResultDTO>(mensagem);
    }

    if(preview.TotalOrigem == 0)
    {
      return ResponseFactory.Fail<TrocaMarcacoesMedicosResultDTO>(
        "Não existem marcações ativas para o médico e data de origem indicados"
      );
    }

    DateTime dataDestino = request.DataDestino.Date;
    List<Guid> marcacaoIds = preview.Itens.Select(x => x.MarcacaoId).ToList();

    IEnumerable<ConsultaMarcacao> todasMarcacoes = 
      await _repository.GetListAsync<ConsultaMarcacao, Guid>();
    List<ConsultaMarcacao> origem = todasMarcacoes
      .Where(x => marcacaoIds.Contains(x.Id))
      .ToList();

    foreach(ConsultaMarcacao marcacao in origem)
    {
      marcacao.MedicoId = request.MedicoDestinoId;
      marcacao.Data = dataDestino;
      _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
    }
    
    IEnumerable<Admissao> admissoes = await _repository.GetListAsync<Admissao, Guid>();
    List<Admissao> admissoesLigadas = admissoes 
      .Where(a => 
        a.ConsultaMarcacaoId.HasValue
        && marcacaoIds.Contains(a.ConsultaMarcacaoId.Value)
        && a.DeletedOn == null
      ).ToList();

    foreach(Admissao admissao in admissoesLigadas)
    {
      admissao.MedicoId = request.MedicoDestinoId;
      admissao.Data = dataDestino;
      _ = await _repository.UpdateAsync<Admissao, Guid>(admissao);
    }

    _ = await _repository.SaveChangesAsync();

      return ResponseFactory.Success( new TrocaMarcacoesMedicosResultDTO
      {
        QuantidadeTransferida = origem.Count,
        MarcacaoIds = marcacaoIds,
        QuantidadeAdmissoesAtualizadas = admissoesLigadas.Count,
      }
    );
  }

  private async Task<TrocaMarcacoesMedicosPreviewDTO> BuildTrocaMedicosPreviewAsync(TrocaMarcacoesMedicosRequest request)
  {
    var conflitos = new List<TrocaMarcacoesMedicosConflitoDTO>();
    DateTime dataOrigem = request.DataOrigem.Date;
    DateTime dataDestino = request.DataDestino.Date;

    Medico? medicoOrigem = await _repository.GetByIdAsync<Medico, Guid>(request.MedicoOrigemId);
    Medico? medicoDestino = await _repository.GetByIdAsync<Medico, Guid>(request.MedicoDestinoId);

    if(medicoOrigem == null || medicoOrigem.DeletedOn != null)
    {
      conflitos.Add(
        new TrocaMarcacoesMedicosConflitoDTO
        {
          Codigo = "MedicoInvalido",
          Mensagem = "Médico de origem inválido",
        }
      );
    }
    if(medicoDestino == null || medicoDestino.DeletedOn != null)
    {
      conflitos.Add(
        new TrocaMarcacoesMedicosConflitoDTO
        {
          Codigo = "MedicoInvalido",
          Mensagem = "Médico de destino inválido",
        }
      );
    }

    if(conflitos.Count > 0)
    {
      return new TrocaMarcacoesMedicosPreviewDTO
      {
        TotalOrigem = 0,
        PodeExecutar = false,
        Itens = [],
        Conflitos = conflitos,
      };
    }

    IEnumerable<ConsultaMarcacao> todasMarcacoes = 
      await _repository.GetListAsync<ConsultaMarcacao, Guid>();

    List<ConsultaMarcacao> origem = todasMarcacoes
      .Where(x =>
        x.MedicoId == request.MedicoOrigemId
        && x.Data.HasValue
        && x.Data.Value.Date == dataOrigem
        && IsMarcacaoAtiva(x)
      )
      .OrderBy(x => x.HoraMarcacao)
      .ToList();

    if (origem.Count == 0)
    {
      conflitos.Add(
        new TrocaMarcacoesMedicosConflitoDTO
        {
          Codigo = "SemMarcacoesOrigem",
          Mensagem =
            "Não existem marcações ativas para o médico e data de origem indicados.",
        }
      );

      return new TrocaMarcacoesMedicosPreviewDTO
      {
        TotalOrigem = 0,
        PodeExecutar = false,
        Itens = [],
        Conflitos = conflitos,
      };
    }

    await HydrateMarcacoesUtenteEspecialidadeAsync(origem);

    HashSet<TimeSpan> horasOcupadasDestino = todasMarcacoes
      .Where(x =>
        x.MedicoId == request.MedicoDestinoId
        && x.Data.HasValue
        && x.Data.Value.Date == dataDestino
        && IsMarcacaoAtiva(x)
        && x.HoraMarcacao.HasValue
      )
      .Select(x => x.HoraMarcacao!.Value)
      .ToHashSet();

    HorarioMedico? horarioDestino = await ObterHorarioMedicoAsync(request.MedicoDestinoId);
    HorarioMedicoVariavel? horarioVariavelDestino =
      await ObterHorarioVariavelNaDataAsync(request.MedicoDestinoId, dataDestino);
    List<FolgasMedico> folgasDestino = await ObterFolgasMedicoAsync(request.MedicoDestinoId);

    List<TrocaMarcacoesMedicosDTO> itens = origem
      .Select(x => new TrocaMarcacoesMedicosDTO
      {
        MarcacaoId = x.Id,
        UtenteNome = x.Utente?.Nome,
        UtenteNumero = ParseUtenteNumero(x.Utente?.NumeroUtente),
        HoraInicio = FormatHoraTimeSpan(x.HoraMarcacao),
        EspecialidadeDesignacao = x.Especialidade?.Nome,
      })
      .ToList();

    foreach (ConsultaMarcacao marcacao in origem)
    {
      string utenteNome = marcacao.Utente?.Nome ?? string.Empty;
      string horaTexto = FormatHoraTimeSpan(marcacao.HoraMarcacao) ?? string.Empty;

      if (!marcacao.HoraMarcacao.HasValue)
      {
        conflitos.Add(
          new TrocaMarcacoesMedicosConflitoDTO
          {
            MarcacaoId = marcacao.Id,
            UtenteNome = utenteNome,
            HoraInicio = horaTexto,
            Codigo = "SlotOcupado",
            Mensagem = "Marcação sem hora definida; não pode ser transferida.",
          }
        );
        continue;
      }

      TimeSpan hora = marcacao.HoraMarcacao.Value;

      if (horasOcupadasDestino.Contains(hora))
      {
        conflitos.Add(
          new TrocaMarcacoesMedicosConflitoDTO
          {
            MarcacaoId = marcacao.Id,
            UtenteNome = utenteNome,
            HoraInicio = horaTexto,
            Codigo = "SlotOcupado",
            Mensagem =
              "Já existe uma marcação ativa para o médico de destino na mesma data e hora.",
          }
        );
      }

      if (EstaBloqueadoPorFolga(folgasDestino, dataDestino, hora))
      {
        conflitos.Add(
          new TrocaMarcacoesMedicosConflitoDTO
          {
            MarcacaoId = marcacao.Id,
            UtenteNome = utenteNome,
            HoraInicio = horaTexto,
            Codigo = "Folga",
            Mensagem = "O médico de destino tem folga/férias na data e hora indicadas.",
          }
        );
      }

      if (!EstaDentroHorarioMedico(dataDestino, hora, horarioDestino, horarioVariavelDestino))
      {
        conflitos.Add(
          new TrocaMarcacoesMedicosConflitoDTO
          {
            MarcacaoId = marcacao.Id,
            UtenteNome = utenteNome,
            HoraInicio = horaTexto,
            Codigo = "ForaHorario",
            Mensagem =
              "A hora da marcação não está dentro do horário do médico de destino na data indicada.",
          }
        );
      }
    }

    return new TrocaMarcacoesMedicosPreviewDTO
    {
      TotalOrigem = origem.Count,
      PodeExecutar = conflitos.Count == 0,
      Itens = itens,
      Conflitos = conflitos,
    };
  }

  private async Task HydrateMarcacoesUtenteEspecialidadeAsync(List<ConsultaMarcacao> marcacoes)
  {
    if (marcacoes.Count == 0)
    {
      return;
    }

    IEnumerable<Utente> utentes = await _repository.GetListAsync<Utente, Guid>();
    IEnumerable<Especialidade> especialidades =
      await _repository.GetListAsync<Especialidade, Guid>();

    foreach (ConsultaMarcacao marcacao in marcacoes)
    {
      if (marcacao.Utente == null)
      {
        Utente? utente = utentes.FirstOrDefault(u => u.Id == marcacao.UtenteId);
        if (utente != null)
        {
          marcacao.Utente = utente;
        }
      }

      if (marcacao.Especialidade == null && marcacao.EspecialidadeId.HasValue)
      {
        Especialidade? especialidade = especialidades.FirstOrDefault(e =>
          e.Id == marcacao.EspecialidadeId.Value
        );
        if (especialidade != null)
        {
          marcacao.Especialidade = especialidade;
        }
      }
    }
  }

  private static int? ParseUtenteNumero(string? numeroUtente) =>
    int.TryParse(numeroUtente, out int numero) ? numero : null;

  private static bool IsMarcacaoAtiva(ConsultaMarcacao marcacao) =>
    marcacao.DeletedOn == null
    && (
      marcacao.StatusConsulta == null
      || (
        marcacao.StatusConsulta != StatusConsulta.Desmarcada
        && marcacao.StatusConsulta != StatusConsulta.Suspensa
      )
    );

  private async Task<HorarioMedico?> ObterHorarioMedicoAsync(Guid medicoId)
  {
    var spec = new HorarioMedicoSearchByMedicoId(medicoId);
    IEnumerable<HorarioMedico> list =
      await _repository.GetListAsync<HorarioMedico, Guid>(spec);
    return list.FirstOrDefault(x => x.DeletedOn == null);
  }

  private async Task<HorarioMedicoVariavel?> ObterHorarioVariavelNaDataAsync(
    Guid medicoId,
    DateTime data
  )
  {
    var spec = new HorarioMedicoVariavelSearchByMedicoId(medicoId);
    IEnumerable<HorarioMedicoVariavel> list =
      await _repository.GetListAsync<HorarioMedicoVariavel, Guid>(spec);
    return list.FirstOrDefault(x => x.DeletedOn == null && x.Data.Date == data.Date);
  }

  private async Task<List<FolgasMedico>> ObterFolgasMedicoAsync(Guid medicoId)
  {
    var spec = new FolgasMedicoSearchByMedicoId(medicoId);
    IEnumerable<FolgasMedico> list = await _repository.GetListAsync<FolgasMedico, Guid>(spec);
    return list.Where(x => x.DeletedOn == null).ToList();
  }

  private static bool HoraDentroIntervalo(TimeSpan hora, TimeSpan? inicio, TimeSpan? fim) =>
    inicio.HasValue && fim.HasValue && hora >= inicio.Value && hora < fim.Value;

  private static bool EstaBloqueadoPorFolga(
    IEnumerable<FolgasMedico> folgas,
    DateTime data,
    TimeSpan hora
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

      if (HoraDentroIntervalo(hora, folga.ManhaInicio, folga.ManhaFim))
      {
        return true;
      }

      if (HoraDentroIntervalo(hora, folga.TardeInicio, folga.TardeFim))
      {
        return true;
      }
    }

    return false;
  }

  private static bool EstaDentroHorarioMedico(
    DateTime data,
    TimeSpan hora,
    HorarioMedico? horario,
    HorarioMedicoVariavel? horarioVariavel
  )
  {
    if (horarioVariavel != null)
    {
      if (HoraDentroIntervalo(hora, horarioVariavel.ManhaInicio, horarioVariavel.ManhaFim))
      {
        return true;
      }

      if (HoraDentroIntervalo(hora, horarioVariavel.TardeInicio, horarioVariavel.TardeFim))
      {
        return true;
      }

      return false;
    }

    if (horario?.Horarios == null || horario.Horarios.Count == 0)
    {
      return true;
    }

    DiaSemana dia = (DiaSemana)(int)data.DayOfWeek;
    List<HorarioMedicoDia> slotsDia = horario.Horarios
      .Where(h => h.DiaSemana == dia && h.Inicio.HasValue && h.Fim.HasValue)
      .ToList();

    if (slotsDia.Count == 0)
    {
      return false;
    }

    return slotsDia.Any(s => HoraDentroIntervalo(hora, s.Inicio, s.Fim));
  }

  /// <summary>TimeSpan não suporta formato DateTime «HH» — causa FormatException.</summary>
  private static string? FormatHoraTimeSpan(TimeSpan? value)
  {
    if (!value.HasValue)
    {
      return null;
    }

    TimeSpan t = value.Value;
    return $"{t.Hours:D2}:{t.Minutes:D2}";
  }
}
