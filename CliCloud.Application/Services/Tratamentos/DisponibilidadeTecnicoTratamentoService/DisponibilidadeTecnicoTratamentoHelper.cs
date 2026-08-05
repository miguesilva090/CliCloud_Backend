using CliCloud.Application.Common;
using CliCloud.Application.Services.Tecnicos.FolgasTecnicoService.Specifications;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.Specifications;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService.Specifications;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.Specifications;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService;

public static class DisponibilidadeTecnicoTratamentoHelper
{
  private static readonly TimeSpan DefaultIntervalo = TimeSpan.FromMinutes(15);

  /// <summary>
  /// Legado ClinFolg: dia da semana marcado como folga na clínica.
  /// newCC: Clinica.FolgaSeg … FolgaDom (DayOfWeek alinhado a DiaSemana).
  /// </summary>
  public static bool EhFolgaSemanalClinica(Clinica clinica, DateTime data)
  {
    return (DiaSemana)(int)data.DayOfWeek switch
    {
      DiaSemana.Segunda => clinica.FolgaSeg == true,
      DiaSemana.Terca => clinica.FolgaTer == true,
      DiaSemana.Quarta => clinica.FolgaQua == true,
      DiaSemana.Quinta => clinica.FolgaQui == true,
      DiaSemana.Sexta => clinica.FolgaSex == true,
      DiaSemana.Sabado => clinica.FolgaSab == true,
      DiaSemana.Domingo => clinica.FolgaDom == true,
      _ => false,
    };
  }

  public static async Task<(string Duracao, List<string> Horas)> ObterHorasPossiveisAsync(
    IRepositoryAsync repository,
    Tecnico tecnico,
    DateTime data,
    int unidadeTempo,
    Guid? ignorarSessaoId
  )
  {
    DateTime dia = data.Date;
    DiaSemana diaSemana = (DiaSemana)(int)dia.DayOfWeek;

    HorarioTecnicoSearchByTecnicoId horarioSpec = new(tecnico.Id);
    IEnumerable<HorarioTecnico> horarios =
      await repository.GetListAsync<HorarioTecnico, Guid>(horarioSpec);
    HorarioTecnico? horario = horarios.OrderBy(h => h.CreatedOn).LastOrDefault();

    TimeSpan intervalo = ParseIntervalo(horario?.MinMarcacao) ?? DefaultIntervalo;
    string duracao = FormatHora(intervalo);

    SortedSet<string> horas = [];

    if (horario?.Horarios != null)
    {
      foreach (HorarioTecnicoDia slot in horario.Horarios.Where(d => d.DiaSemana == diaSemana))
      {
        TimeSpan? ini = ParseHora(slot.Inicio);
        TimeSpan? fim = ParseHora(slot.Fim);
        if (ini is null || fim is null || fim <= ini)
        {
          continue;
        }

        for (TimeSpan t = ini.Value; t < fim.Value; t += intervalo)
        {
          _ = horas.Add(FormatHora(t));
        }
      }
    }

    HorarioTecnicoVariavelSearchByTecnicoId varSpec = new(tecnico.Id);
    IEnumerable<HorarioTecnicoVariavel> variaveis =
      await repository.GetListAsync<HorarioTecnicoVariavel, Guid>(varSpec);
    HorarioTecnicoVariavel? variavel = variaveis.FirstOrDefault(v => v.Data.Date == dia);
    if (variavel != null)
    {
      AddRange(horas, variavel.ManhaInicio, variavel.ManhaFim, intervalo);
      AddRange(horas, variavel.TardeInicio, variavel.TardeFim, intervalo);
    }

    FolgasTecnicoSearchByTecnicoId folgaSpec = new(tecnico.Id);
    IEnumerable<FolgasTecnico> folgas =
      await repository.GetListAsync<FolgasTecnico, Guid>(folgaSpec);
    foreach (FolgasTecnico folga in folgas.Where(f => CobreDia(f, dia)))
    {
      if (folga.TodoDia || folga.MesInteiro)
      {
        horas.Clear();
        break;
      }

      RemoveRange(horas, folga.ManhaInicio, folga.ManhaFim, intervalo);
      RemoveRange(horas, folga.TardeInicio, folga.TardeFim, intervalo);
    }

    int max = Math.Max(1, tecnico.MaxTratamentos);
    SessoesOcupacaoTecnicoDiaSpec ocupSpec =
      new(tecnico.Id, tecnico.TipoTecnico, dia, ignorarSessaoId);
    IEnumerable<SessaoTratamento> sessoes =
      await repository.GetListAsync<SessaoTratamento, Guid>(ocupSpec);

    Dictionary<string, int> ocupacao = new(StringComparer.Ordinal);
    foreach (SessaoTratamento s in sessoes)
    {
      string? hora = ResolverHoraOcupacao(s, tecnico.TipoTecnico);
      if (string.IsNullOrWhiteSpace(hora))
      {
        continue;
      }

      string key = NormalizarHora(hora);
      int unidades = ResolverUnidadesSessao(s, tecnico.TipoTecnico);
      ocupacao[key] = ocupacao.TryGetValue(key, out int n) ? n + unidades : unidades;
    }

    List<string> livres = [];
    foreach (string h in horas)
    {
      int usados = ocupacao.TryGetValue(h, out int n) ? n : 0;
      if (usados + unidadeTempo <= max)
      {
        livres.Add(h);
      }
    }

    return (duracao, livres);
  }

  private static int ResolverUnidadesSessao(SessaoTratamento s, TipoTecnico tipo)
  {
    Tratamento? t = s.Tratamento;
    int? unidades = tipo switch
    {
      TipoTecnico.Auxiliar => t?.UnidadeTempoAux,
      TipoTecnico.Outro => t?.UnidadeTempoOutro,
      _ => t?.UnidadeTempoFisio,
    };

    return unidades.HasValue && unidades.Value > 0 ? unidades.Value : 1;
  }

  private static string? ResolverHoraOcupacao(SessaoTratamento s, TipoTecnico tipo) =>
    tipo switch
    {
      TipoTecnico.Auxiliar =>
        !string.IsNullOrWhiteSpace(s.HoraAux) ? s.HoraAux : s.HoraInic,
      TipoTecnico.Outro =>
        !string.IsNullOrWhiteSpace(s.HoraOutro) ? s.HoraOutro : s.HoraInic,
      _ => !string.IsNullOrWhiteSpace(s.HoraFisio) ? s.HoraFisio : s.HoraInic,
    };

  private static bool CobreDia(FolgasTecnico f, DateTime dia)
  {
    DateTime de = f.DataDe.Date;
    DateTime ate = f.DataAte.Date;
    return dia >= de && dia <= ate;
  }

  private static void AddRange(
    SortedSet<string> horas,
    TimeSpan? ini,
    TimeSpan? fim,
    TimeSpan intervalo
  )
  {
    if (ini is null || fim is null || fim <= ini)
    {
      return;
    }

    for (TimeSpan t = ini.Value; t < fim.Value; t += intervalo)
    {
      _ = horas.Add(FormatHora(t));
    }
  }

  private static void RemoveRange(
    SortedSet<string> horas,
    TimeSpan? ini,
    TimeSpan? fim,
    TimeSpan intervalo
  )
  {
    if (ini is null || fim is null || fim <= ini)
    {
      return;
    }

    for (TimeSpan t = ini.Value; t < fim.Value; t += intervalo)
    {
      _ = horas.Remove(FormatHora(t));
    }
  }

  private static TimeSpan? ParseIntervalo(string? value) => ParseHora(value);

  private static TimeSpan? ParseHora(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      return null;
    }

    string v = value.Trim();
    if (TimeSpan.TryParse(v, out TimeSpan ts))
    {
      return ts;
    }

    string[] parts = v.Split(':');
    if (
      parts.Length >= 2
      && int.TryParse(parts[0], out int h)
      && int.TryParse(parts[1], out int m)
    )
    {
      return new TimeSpan(h, m, 0);
    }

    return null;
  }

  private static string FormatHora(TimeSpan t) => $"{(int)t.TotalHours:00}:{t.Minutes:00}";

  private static string NormalizarHora(string hora)
  {
    TimeSpan? ts = ParseHora(hora);
    return ts is null ? hora.Trim() : FormatHora(ts.Value);
  }
}