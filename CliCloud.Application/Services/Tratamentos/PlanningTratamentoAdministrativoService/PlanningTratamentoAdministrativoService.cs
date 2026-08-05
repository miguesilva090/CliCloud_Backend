using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService.Specifications;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService;

public class PlanningTratamentoAdministrativoService(IRepositoryAsync repository)
  : IPlanningTratamentoAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private static readonly TimeSpan DefaultDuracao = TimeSpan.FromMinutes(30);

  public async Task<Response<PlanningSessoesResponse>> GetSessoesAsync(
    PlanningSessoesRequest request
  )
  {
    Tecnico? tecnico = await _repository.GetByIdAsync<Tecnico, Guid>(request.TecnicoId);
    if (tecnico is null)
    {
      return ResponseFactory.Fail<PlanningSessoesResponse>("Técnico não encontrado.");
    }

    TipoTecnico tipo = (TipoTecnico)request.TipoTecnico;
    SessoesPlanningTecnicoPeriodoSpec spec =
      new(request.TecnicoId, tipo, request.DataDe, request.DataAte);
    IEnumerable<SessaoTratamento> sessoes =
      await _repository.GetListAsync<SessaoTratamento, Guid>(spec);

    List<PlanningSessaoEventoDTO> eventos = [];
    foreach (SessaoTratamento s in sessoes)
    {
      PlanningSessaoEventoDTO? ev = MapEvento(s, tipo);
      if (ev is not null)
      {
        eventos.Add(ev);
      }
    }

    return ResponseFactory.Success(new PlanningSessoesResponse { Eventos = eventos });
  }

  private static PlanningSessaoEventoDTO? MapEvento(SessaoTratamento s, TipoTecnico tipo)
  {
    if (s.Data is null)
    {
      return null;
    }

    string? hora = ResolverHora(s, tipo);
    if (string.IsNullOrWhiteSpace(hora))
    {
      return null;
    }

    TimeSpan? ini = ParseHora(hora);
    if (ini is null)
    {
      return null;
    }

    string? duracaoTxt = ResolverDuracao(s, tipo);
    TimeSpan duracao = ParseHora(duracaoTxt) ?? DefaultDuracao;
    DateTime start = s.Data.Value.Date.Add(ini.Value);
    DateTime end = start.Add(duracao);

    Tratamento? t = s.Tratamento;
    string utenteNome = t?.Utente?.Nome?.Trim() ?? "Utente";

    return new PlanningSessaoEventoDTO
    {
      SessaoId = s.Id,
      TratamentoId = s.TratamentoId,
      Title = utenteNome,
      Start = start.ToString("o"),
      End = end.ToString("o"),
      TipoEvento = ResolverTipoEvento(s, t),
      NumSessao = s.NumSessao,
      HoraInicio = FormatHora(ini.Value),
      Duracao = FormatHora(duracao),
      UtenteNome = utenteNome,
      TratamentoDesignacao = t?.Designacao,
      NumSessoesTratamento = t?.NumSessao,
      NFaltas = t?.NFalta,
      DataInicTratamento = t?.DataInic,
      DataFimTratamento = t?.DataFim,
      Faltou = (s.Faltou ?? 0) == 1,
      Confirmado = (s.Confirmado ?? 0) == 1,
      Efetuado = (s.Efetuado ?? 0) == 1,
    };
  }

  private static int ResolverTipoEvento(SessaoTratamento s, Tratamento? t)
  {
    if ((s.Faltou ?? 0) == 1)
    {
      return 6;
    }

    int tecnicos =
      (s.FisioterapeutaId.HasValue ? 1 : 0)
      + (s.AuxiliarId.HasValue ? 1 : 0)
      + (s.OutroTecnicoId.HasValue ? 1 : 0);
    if (tecnicos > 1)
    {
      return 11;
    }

    if ((t?.Provisorio ?? 0) == 1)
    {
      return 5;
    }

    bool ultima =
      t?.DataFim is DateTime df
      && s.Data is DateTime sd
      && df.Date == sd.Date;
    if (ultima)
    {
      return (t?.ConfDfim ?? 0) == 1 ? 4 : 3;
    }

    if ((s.NumSessao ?? 0) == 1)
    {
      return 1;
    }

    return 2;
  }

  private static string? ResolverHora(SessaoTratamento s, TipoTecnico tipo) =>
    tipo switch
    {
      TipoTecnico.Auxiliar =>
        !string.IsNullOrWhiteSpace(s.HoraAux) ? s.HoraAux : s.HoraInic,
      TipoTecnico.Outro =>
        !string.IsNullOrWhiteSpace(s.HoraOutro) ? s.HoraOutro : s.HoraInic,
      _ => !string.IsNullOrWhiteSpace(s.HoraFisio) ? s.HoraFisio : s.HoraInic,
    };

  private static string? ResolverDuracao(SessaoTratamento s, TipoTecnico tipo) =>
    tipo switch
    {
      TipoTecnico.Auxiliar =>
        !string.IsNullOrWhiteSpace(s.DuracaoAux) ? s.DuracaoAux : s.Duracao,
      TipoTecnico.Outro =>
        !string.IsNullOrWhiteSpace(s.DuracaoOutro) ? s.DuracaoOutro : s.Duracao,
      _ => !string.IsNullOrWhiteSpace(s.DuracaoFisio) ? s.DuracaoFisio : s.Duracao,
    };

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
}
