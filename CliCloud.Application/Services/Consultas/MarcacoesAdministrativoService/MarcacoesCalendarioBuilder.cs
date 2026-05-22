using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;

internal static class MarcacoesCalendarioBuilder
{
  private static readonly TimeSpan DefaultClinicOpen = new(8, 0, 0);
  private static readonly TimeSpan DefaultClinicClose = new(20, 0, 0);
  private static readonly TimeSpan DefaultSlot = new(0, 30, 0);

  public static MarcacaoCalendarioDTO Build(
    MarcacaoCalendarioRequest request,
    Clinica? clinica,
    HorarioMedico? horario,
    List<FolgasMedico> folgas,
    List<Feriado> feriados,
    List<HorarioMedicoVariavel> horariosVariaveis,
    List<ConsultaMarcacao> marcacoes
  )
  {
    TimeSpan slot = horario?.MinMarcacao ?? DefaultSlot;
    TimeSpan primeiraConsulta = horario?.PrimeiraConsulta ?? TimeSpan.Zero;
    TimeSpan intervaloMarcacao = slot < primeiraConsulta && primeiraConsulta > TimeSpan.Zero
      ? primeiraConsulta
      : slot;

    (TimeSpan clinicOpen, TimeSpan clinicClose) = ResolveClinicBounds(clinica);
    List<MarcacaoCalendarioEventoDTO> eventos = [];
    int seq = 1;

    for (DateTime day = request.DataDe.Date; day.Date <= request.DataAte.Date; day = day.AddDays(1))
    {
      if (IsFeriado(feriados, day))
      {
        AddBlock(eventos, "Feriado", day, clinicOpen, clinicClose, ref seq);
        continue;
      }

      if (IsFolgaTodoDia(folgas, day))
      {
        AddBlock(eventos, "Horario Folga", day, clinicOpen, clinicClose, ref seq);
        continue;
      }

      HorarioMedicoVariavel? variavel = horariosVariaveis
        .FirstOrDefault(x => x.Data.Date == day.Date);

      List<(TimeSpan Inicio, TimeSpan Fim, bool IsVariavel)> periodos =
        ResolvePeriodosTrabalho(day, horario, variavel);

      AddIndisponivelForDay(eventos, day, clinicOpen, clinicClose, periodos, ref seq);
      AddFolgasParciais(eventos, folgas, day, clinicOpen, clinicClose, ref seq);

      if (variavel != null && periodos.Any(p => p.IsVariavel))
      {
        foreach ((TimeSpan inicio, TimeSpan fim, _) in periodos.Where(p => p.IsVariavel))
        {
          AddBlock(eventos, "Horario Variavel", day, inicio, fim, ref seq);
        }
      }
    }

    foreach (ConsultaMarcacao marcacao in marcacoes)
    {
      MarcacaoCalendarioEventoDTO? evt = MapMarcacao(marcacao, intervaloMarcacao, primeiraConsulta);
      if (evt != null)
      {
        eventos.Add(evt);
      }
    }

    (TimeSpan limiteManha, TimeSpan limiteTarde) = ResolveCalendarLimits(
      clinica,
      clinicOpen,
      clinicClose,
      eventos
    );

    (List<int> diasUteis, List<int> diasOcultos) = ResolveDiasClinica(clinica);

    return new MarcacaoCalendarioDTO
    {
      Config = new MarcacaoCalendarioConfigDTO
      {
        IntervaloMarcacao = FormatTimeSpan(intervaloMarcacao),
        PrimeiraConsulta = primeiraConsulta > TimeSpan.Zero
          ? FormatTimeSpan(primeiraConsulta)
          : null,
        LimiteManha = FormatHm(limiteManha),
        LimiteTarde = FormatHm(limiteTarde),
        DiasUteis = diasUteis,
        DiasOcultos = diasOcultos,
      },
      Eventos = eventos,
    };
  }

  /// <summary>
  /// Folgas da clínica (legado window.Domingo…Sabado + GetDiasFolgaClinica / hiddenDays).
  /// FullCalendar: 0=Domingo … 6=Sábado.
  /// </summary>
  private static (List<int> DiasUteis, List<int> DiasOcultos) ResolveDiasClinica(Clinica? clinica)
  {
    List<int> folga = [];
    if (clinica?.FolgaDom == true)
    {
      folga.Add(0);
    }

    if (clinica?.FolgaSeg == true)
    {
      folga.Add(1);
    }

    if (clinica?.FolgaTer == true)
    {
      folga.Add(2);
    }

    if (clinica?.FolgaQua == true)
    {
      folga.Add(3);
    }

    if (clinica?.FolgaQui == true)
    {
      folga.Add(4);
    }

    if (clinica?.FolgaSex == true)
    {
      folga.Add(5);
    }

    if (clinica?.FolgaSab == true)
    {
      folga.Add(6);
    }

    List<int> diasUteis = Enumerable.Range(0, 7).Except(folga).ToList();
    return (diasUteis, folga);
  }

  private static MarcacaoCalendarioEventoDTO? MapMarcacao(
    ConsultaMarcacao marcacao,
    TimeSpan slot,
    TimeSpan primeiraConsulta
  )
  {
    if (!marcacao.Data.HasValue || !marcacao.HoraMarcacao.HasValue)
    {
      return null;
    }

    DateTime data = marcacao.Data.Value.Date;
    TimeSpan hora = marcacao.HoraMarcacao.Value;
    TimeSpan duracao = EhPrimeiraConsulta(marcacao) && primeiraConsulta > TimeSpan.Zero
      ? primeiraConsulta
      : slot;

    string utenteNumero = marcacao.Utente?.NumeroUtente ?? string.Empty;
    string utenteNome = marcacao.Utente?.Nome ?? string.Empty;
    string title = string.IsNullOrWhiteSpace(utenteNumero)
      ? utenteNome
      : $"{utenteNumero} - {utenteNome}";

    if (!string.IsNullOrWhiteSpace(marcacao.Medico?.Nome))
    {
      title += $" | {marcacao.Medico.Nome}";
    }

    if (!string.IsNullOrWhiteSpace(marcacao.Sala?.Nome))
    {
      title += $" | {marcacao.Sala.Nome}";
    }

    string dataIso = data.ToString("yyyy-MM-dd");
    string horaTexto = FormatHm(hora);
    string legacyId =
      $"{utenteNumero}|{marcacao.MedicoId}|{dataIso}|{horaTexto}|{marcacao.Id}|{marcacao.TipoConsultaItem?.CodigoLegado ?? 0}|{marcacao.TipoAdmissaoId}";

    TimeSpan fim = hora.Add(duracao);
    if (fim < hora)
    {
      fim = hora.Add(slot);
    }

    return new MarcacaoCalendarioEventoDTO
    {
      Id = legacyId,
      Title = title,
      Start = FormatIsoDateTime(data, hora),
      End = FormatIsoDateTime(data, fim),
      TipoEvento = "Marcacao",
      MarcacaoId = marcacao.Id,
      CodigoLegadoTipoConsulta = marcacao.TipoConsultaItem?.CodigoLegado,
      SalaCodigo = marcacao.Sala != null ? marcacao.Sala.NumeroSala.ToString() : null,
    };
  }

  private static bool EhPrimeiraConsulta(ConsultaMarcacao marcacao) =>
    marcacao.TipoConsultaItem?.CodigoLegado == 1;

  private static void AddIndisponivelForDay(
    List<MarcacaoCalendarioEventoDTO> eventos,
    DateTime day,
    TimeSpan clinicOpen,
    TimeSpan clinicClose,
    List<(TimeSpan Inicio, TimeSpan Fim, bool IsVariavel)> periodos,
    ref int seq
  )
  {
    if (periodos.Count == 0)
    {
      AddBlock(eventos, "Indisponível", day, clinicOpen, clinicClose, ref seq);
      return;
    }

    List<(TimeSpan Inicio, TimeSpan Fim)> merged = periodos
      .Select(p => (p.Inicio, p.Fim))
      .OrderBy(p => p.Inicio)
      .ToList();

    TimeSpan cursor = clinicOpen;
    foreach ((TimeSpan inicio, TimeSpan fim) in merged)
    {
      if (inicio > cursor)
      {
        AddBlock(eventos, "Indisponível", day, cursor, inicio, ref seq);
      }

      if (fim > cursor)
      {
        cursor = fim;
      }
    }

    if (cursor < clinicClose)
    {
      AddBlock(eventos, "Indisponível", day, cursor, clinicClose, ref seq);
    }
  }

  private static void AddFolgasParciais(
    List<MarcacaoCalendarioEventoDTO> eventos,
    List<FolgasMedico> folgas,
    DateTime day,
    TimeSpan clinicOpen,
    TimeSpan clinicClose,
    ref int seq
  )
  {
    foreach (FolgasMedico folga in folgas)
    {
      if (day.Date < folga.DataDe.Date || day.Date > folga.DataAte.Date || folga.TodoDia || folga.MesInteiro)
      {
        continue;
      }

      if (folga.ManhaInicio.HasValue && folga.ManhaFim.HasValue)
      {
        AddBlock(
          eventos,
          "Horario Folga",
          day,
          folga.ManhaInicio.Value,
          folga.ManhaFim.Value,
          ref seq
        );
      }

      if (folga.TardeInicio.HasValue && folga.TardeFim.HasValue)
      {
        AddBlock(
          eventos,
          "Horario Folga",
          day,
          folga.TardeInicio.Value,
          folga.TardeFim.Value,
          ref seq
        );
      }
    }
  }

  private static List<(TimeSpan Inicio, TimeSpan Fim, bool IsVariavel)> ResolvePeriodosTrabalho(
    DateTime day,
    HorarioMedico? horario,
    HorarioMedicoVariavel? variavel
  )
  {
    if (variavel != null)
    {
      List<(TimeSpan, TimeSpan, bool)> list = [];
      if (variavel.ManhaInicio.HasValue && variavel.ManhaFim.HasValue)
      {
        list.Add((variavel.ManhaInicio.Value, variavel.ManhaFim.Value, true));
      }

      if (variavel.TardeInicio.HasValue && variavel.TardeFim.HasValue)
      {
        list.Add((variavel.TardeInicio.Value, variavel.TardeFim.Value, true));
      }

      if (list.Count > 0)
      {
        return list;
      }
    }

    if (horario?.Horarios == null || horario.Horarios.Count == 0)
    {
      return [];
    }

    DiaSemana dia = (DiaSemana)(int)day.DayOfWeek;
    return horario.Horarios
      .Where(h => h.DiaSemana == dia && h.Inicio.HasValue && h.Fim.HasValue)
      .Select(h => (h.Inicio!.Value, h.Fim!.Value, false))
      .OrderBy(p => p.Item1)
      .ToList();
  }

  private static (TimeSpan Open, TimeSpan Close) ResolveClinicBounds(Clinica? clinica)
  {
    TimeSpan? manhaIni = ParseHm(clinica?.HoraInicManha);
    TimeSpan? tardeIni = ParseHm(clinica?.HoraInicTarde);
    TimeSpan? manhaFim = ParseHm(clinica?.HoraFimManha);
    TimeSpan? tardeFim = ParseHm(clinica?.HoraFimTarde);

    TimeSpan open = new[]
      {
        manhaIni,
        tardeIni,
      }
      .Where(x => x.HasValue)
      .Select(x => x!.Value)
      .DefaultIfEmpty(DefaultClinicOpen)
      .Min();

    TimeSpan close = new[]
      {
        manhaFim,
        tardeFim,
      }
      .Where(x => x.HasValue)
      .Select(x => x!.Value)
      .DefaultIfEmpty(DefaultClinicClose)
      .Max();

    if (close <= open)
    {
      return (DefaultClinicOpen, DefaultClinicClose);
    }

    return (open, close);
  }

  private static (TimeSpan Min, TimeSpan Max) ResolveCalendarLimits(
    Clinica? clinica,
    TimeSpan clinicOpen,
    TimeSpan clinicClose,
    List<MarcacaoCalendarioEventoDTO> eventos
  )
  {
    TimeSpan min = clinicOpen;
    TimeSpan max = clinicClose;

    foreach (MarcacaoCalendarioEventoDTO evt in eventos)
    {
      if (!TryParseEventBounds(evt, out TimeSpan start, out TimeSpan end))
      {
        continue;
      }

      if (start < min)
      {
        min = start;
      }

      if (end > max)
      {
        max = end;
      }
    }

    if (min >= max)
    {
      (TimeSpan open, TimeSpan close) = ResolveClinicBounds(clinica);
      return (open, close);
    }

    return (min, max);
  }

  private static bool TryParseEventBounds(
    MarcacaoCalendarioEventoDTO evt,
    out TimeSpan start,
    out TimeSpan end
  )
  {
    start = TimeSpan.Zero;
    end = TimeSpan.Zero;
    if (!DateTime.TryParse(evt.Start, out DateTime startDt)
      || !DateTime.TryParse(evt.End, out DateTime endDt))
    {
      return false;
    }

    start = startDt.TimeOfDay;
    end = endDt.TimeOfDay;
    return true;
  }

  private static bool IsFeriado(List<Feriado> feriados, DateTime day) =>
    feriados.Any(f => f.Ativo && f.Data.Date == day.Date);

  public static bool IsFolgaTodoDia(List<FolgasMedico> folgas, DateTime day) =>
    folgas.Any(f =>
      day.Date >= f.DataDe.Date
      && day.Date <= f.DataAte.Date
      && (f.TodoDia || f.MesInteiro)
    );

  private static void AddBlock(
    List<MarcacaoCalendarioEventoDTO> eventos,
    string title,
    DateTime day,
    TimeSpan start,
    TimeSpan end,
    ref int seq
  )
  {
    if (start >= end)
    {
      return;
    }

    eventos.Add(
      new MarcacaoCalendarioEventoDTO
      {
        Id = $"bg-{seq++}",
        Title = title,
        Start = FormatIsoDateTime(day, start),
        End = FormatIsoDateTime(day, end),
        TipoEvento = title,
      }
    );
  }

  private static string FormatIsoDateTime(DateTime day, TimeSpan time) =>
    $"{day:yyyy-MM-dd}T{FormatTimeSpan(time)}";

  private static TimeSpan? ParseHm(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      return null;
    }

    string[] parts = value.Split(':');
    if (parts.Length < 2)
    {
      return null;
    }

    if (!int.TryParse(parts[0], out int h) || !int.TryParse(parts[1], out int m))
    {
      return null;
    }

    return new TimeSpan(h, m, 0);
  }

  private static string FormatTimeSpan(TimeSpan ts) =>
    $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";

  private static string FormatHm(TimeSpan ts) =>
    $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}";

  /// <summary>
  /// Legado: existe pelo menos um intervalo livre no dia (ObterDisponibilidadeMedicosMesAtualPorEspecialidade).
  /// </summary>
  public static bool TemSlotLivreNoDia(MarcacaoCalendarioDTO calendario)
  {
    List<MarcacaoCalendarioEventoDTO> eventos = calendario.Eventos;
    if (eventos.Exists(e => e.TipoEvento == "Feriado"))
    {
      return false;
    }

    TimeSpan interval = ParseHm(calendario.Config.IntervaloMarcacao) ?? DefaultSlot;
    if (interval <= TimeSpan.Zero)
    {
      interval = DefaultSlot;
    }

    TimeSpan dayStart = ParseHm(calendario.Config.LimiteManha) ?? DefaultClinicOpen;
    TimeSpan dayEnd = ParseHm(calendario.Config.LimiteTarde) ?? DefaultClinicClose;
    if (dayEnd <= dayStart)
    {
      dayStart = DefaultClinicOpen;
      dayEnd = DefaultClinicClose;
    }

    List<(TimeSpan Inicio, TimeSpan Fim)> bloqueios = [];
    foreach (MarcacaoCalendarioEventoDTO evt in eventos)
    {
      if (evt.TipoEvento is not ("Marcacao" or "Indisponível" or "Horario Folga"))
      {
        continue;
      }

      if (!TryParseEventBounds(evt, out TimeSpan inicio, out TimeSpan fim))
      {
        continue;
      }

      bloqueios.Add((inicio, fim));
    }

    if (bloqueios.Count == 0
      && eventos.Exists(e => e.TipoEvento == "Indisponível"))
    {
      return false;
    }

    for (TimeSpan t = dayStart; t.Add(interval) <= dayEnd; t += interval)
    {
      TimeSpan fimSlot = t.Add(interval);
      bool ocupado = bloqueios.Exists(b => t < b.Fim && fimSlot > b.Inicio);
      if (!ocupado)
      {
        return true;
      }
    }

    return false;
  }
}
