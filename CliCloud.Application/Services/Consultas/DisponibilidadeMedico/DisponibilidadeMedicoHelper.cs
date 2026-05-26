using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.DisponibilidadeMedico.Specifications;
using CliCloud.Application.Services.Medicos.FolgasMedicoService.Specifications;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.Specifications;
using CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.DisponibilidadeMedico;

public class DisponibilidadeMedicoRequest
{
  public Guid? MedicoId { get; set; }
  public Guid? TipoConsultaId { get; set; }
  public DateTime? Data { get; set; }
  public TimeSpan? HoraInicio { get; set; }
  public TimeSpan? HoraFim { get; set; }
  public TimeSpan? Duracao { get; set; }
  public Guid? IgnorarMarcacaoId { get; set; }
  public Guid? IgnorarAdmissaoId { get; set; }
  public bool RequerMedico { get; set; }
}

public class DisponibilidadeMedicoResult
{
  public bool Valido { get; set; }
  public string? Mensagem { get; set; }
  public TimeSpan? HoraFim { get; set; }
  public TimeSpan? Duracao { get; set; }
  public bool HorarioFlexivel { get; set; }

  public static DisponibilidadeMedicoResult Ok(
    TimeSpan? horaFim,
    TimeSpan? duracao,
    bool horarioFlexivel
  ) =>
    new()
    {
      Valido = true,
      HoraFim = horaFim,
      Duracao = duracao,
      HorarioFlexivel = horarioFlexivel,
    };

  public static DisponibilidadeMedicoResult Fail(string mensagem) =>
    new() { Valido = false, Mensagem = mensagem };
}

public static class DisponibilidadeMedicoHelper
{
  private static readonly TimeSpan DefaultSlot = TimeSpan.FromMinutes(15);

  public static async Task<DisponibilidadeMedicoResult> ValidarAsync(
    IRepositoryAsync repository,
    DisponibilidadeMedicoRequest request
  )
  {
    if (!request.MedicoId.HasValue)
    {
      return request.RequerMedico
        ? DisponibilidadeMedicoResult.Fail("Médico é obrigatório.")
        : DisponibilidadeMedicoResult.Ok(request.HoraFim, null, false);
    }

    if (!request.Data.HasValue)
    {
      return DisponibilidadeMedicoResult.Fail("Data é obrigatória.");
    }

    if (!request.HoraInicio.HasValue)
    {
      return DisponibilidadeMedicoResult.Fail("Hora de início é obrigatória.");
    }

    if (request.HoraFim.HasValue && request.HoraFim.Value <= request.HoraInicio.Value)
    {
      return DisponibilidadeMedicoResult.Fail("Hora de fim inválida.");
    }

    HorarioMedico? horario = await ObterHorarioMedicoAsync(repository, request.MedicoId.Value);
    if (horario == null)
    {
      return DisponibilidadeMedicoResult.Fail("O médico não tem horário configurado.");
    }

    TimeSpan duracao = await ResolverDuracaoAsync(repository, horario, request);
    if (duracao <= TimeSpan.Zero)
    {
      return DisponibilidadeMedicoResult.Fail("A duração da consulta é inválida.");
    }

    DateTime data = request.Data.Value.Date;
    TimeSpan horaInicio = request.HoraInicio.Value;
    TimeSpan horaFim = horaInicio.Add(duracao);

    HorarioMedicoVariavel? horarioVariavel =
      await ObterHorarioVariavelAsync(repository, request.MedicoId.Value, data);

    List<(TimeSpan Inicio, TimeSpan Fim)> periodos = ResolverPeriodos(
      data,
      horario,
      horarioVariavel
    );

    if (!periodos.Any(p => horaInicio >= p.Inicio && horaFim <= p.Fim))
    {
      return DisponibilidadeMedicoResult.Fail("O médico não tem horário disponível.");
    }

    List<FolgasMedico> folgas = await ObterFolgasAsync(repository, request.MedicoId.Value);
    if (EstaBloqueadoPorFolga(folgas, data, horaInicio, horaFim))
    {
      return DisponibilidadeMedicoResult.Fail("O médico não tem horário disponível.");
    }

    List<ConsultaMarcacao> marcacoes = (
      await repository.GetListAsync<ConsultaMarcacao, Guid>(
        new ConsultaMarcacoesMedicoDataSpec(request.MedicoId.Value, data)
      )
    ).ToList();

    if (ExisteConflitoMarcacao(marcacoes, request.IgnorarMarcacaoId, horaInicio, horaFim, duracao))
    {
      return DisponibilidadeMedicoResult.Fail("Já existe uma marcação ativa nesse horário.");
    }

    List<Admissao> admissoes = (
      await repository.GetListAsync<Admissao, Guid>(
        new AdmissoesMedicoDataSpec(request.MedicoId.Value, data)
      )
    ).ToList();

    if (ExisteConflitoAdmissao(admissoes, request, horaInicio, horaFim, duracao))
    {
      return DisponibilidadeMedicoResult.Fail("Já existe uma admissão ativa nesse horário.");
    }

    return DisponibilidadeMedicoResult.Ok(horaFim, duracao, horario.HorarioFlexivel);
  }

  private static async Task<HorarioMedico?> ObterHorarioMedicoAsync(
    IRepositoryAsync repository,
    Guid medicoId
  )
  {
    IEnumerable<HorarioMedico> list =
      await repository.GetListAsync<HorarioMedico, Guid>(
        new HorarioMedicoSearchByMedicoId(medicoId)
      );

    return list.FirstOrDefault(x => x.DeletedOn == null);
  }

  private static async Task<HorarioMedicoVariavel?> ObterHorarioVariavelAsync(
    IRepositoryAsync repository,
    Guid medicoId,
    DateTime data
  )
  {
    IEnumerable<HorarioMedicoVariavel> list =
      await repository.GetListAsync<HorarioMedicoVariavel, Guid>(
        new HorarioMedicoVariavelSearchByMedicoId(medicoId)
      );

    return list.FirstOrDefault(x => x.DeletedOn == null && x.Data.Date == data.Date);
  }

  private static async Task<List<FolgasMedico>> ObterFolgasAsync(
    IRepositoryAsync repository,
    Guid medicoId
  )
  {
    IEnumerable<FolgasMedico> list =
      await repository.GetListAsync<FolgasMedico, Guid>(
        new FolgasMedicoSearchByMedicoId(medicoId)
      );

    return list.Where(x => x.DeletedOn == null).ToList();
  }

  private static async Task<TimeSpan> ResolverDuracaoAsync(
    IRepositoryAsync repository,
    HorarioMedico horario,
    DisponibilidadeMedicoRequest request
  )
  {
    if (
      request.HoraInicio.HasValue
      && request.HoraFim.HasValue
      && request.HoraFim.Value > request.HoraInicio.Value
    )
    {
      return request.HoraFim.Value - request.HoraInicio.Value;
    }

    if (request.Duracao.HasValue && request.Duracao.Value > TimeSpan.Zero)
    {
      return request.Duracao.Value;
    }

    if (request.Duracao.HasValue)
    {
      return TimeSpan.Zero;
    }

    TipoConsultaItem? tipo = request.TipoConsultaId.HasValue
      ? await repository.GetByIdAsync<TipoConsultaItem, Guid>(request.TipoConsultaId.Value)
      : null;

    bool primeiraConsulta = AdmissaoTipoConsultaHelper.EhPrimeiraConsulta(tipo);

    TimeSpan? slot = primeiraConsulta
      ? horario.PrimeiraConsulta ?? horario.MinMarcacao
      : horario.MinMarcacao ?? horario.PrimeiraConsulta;

    return slot ?? DefaultSlot;
  }

  private static List<(TimeSpan Inicio, TimeSpan Fim)> ResolverPeriodos(
    DateTime data,
    HorarioMedico horario,
    HorarioMedicoVariavel? horarioVariavel
  )
  {
    if (horarioVariavel != null)
    {
      List<(TimeSpan Inicio, TimeSpan Fim)> periodos = [];
      AdicionarPeriodo(periodos, horarioVariavel.ManhaInicio, horarioVariavel.ManhaFim);
      AdicionarPeriodo(periodos, horarioVariavel.TardeInicio, horarioVariavel.TardeFim);

      if (periodos.Count > 0)
      {
        return periodos.OrderBy(x => x.Inicio).ToList();
      }
    }

    DiaSemana dia = (DiaSemana)(int)data.DayOfWeek;

    return horario.Horarios
      .Where(h => h.DiaSemana == dia && h.Inicio.HasValue && h.Fim.HasValue)
      .Select(h => (Inicio: h.Inicio.GetValueOrDefault(), Fim: h.Fim.GetValueOrDefault()))
      .OrderBy(h => h.Inicio)
      .ToList();
  }

  private static void AdicionarPeriodo(
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

  private static bool EstaBloqueadoPorFolga(
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

  private static bool ExisteConflitoMarcacao(
    IEnumerable<ConsultaMarcacao> marcacoes,
    Guid? ignorarMarcacaoId,
    TimeSpan inicio,
    TimeSpan fim,
    TimeSpan duracao
  ) =>
    marcacoes.Any(x =>
      (!ignorarMarcacaoId.HasValue || x.Id != ignorarMarcacaoId.Value)
      && IsStatusAtivo(x.StatusConsulta)
      && x.HoraMarcacao.HasValue
      && IntervalosSobrepoem(inicio, fim, x.HoraMarcacao.Value, x.HoraMarcacao.Value.Add(duracao))
    );

  private static bool ExisteConflitoAdmissao(
    IEnumerable<Admissao> admissoes,
    DisponibilidadeMedicoRequest request,
    TimeSpan inicio,
    TimeSpan fim,
    TimeSpan duracao
  ) =>
    admissoes.Any(x =>
      (!request.IgnorarAdmissaoId.HasValue || x.Id != request.IgnorarAdmissaoId.Value)
      && (!request.IgnorarMarcacaoId.HasValue || x.ConsultaMarcacaoId != request.IgnorarMarcacaoId.Value)
      && IsStatusAtivo(x.StatusConsulta)
      && x.HoraInicio.HasValue
      && IntervalosSobrepoem(inicio, fim, x.HoraInicio.Value, ResolverFimAdmissao(x, duracao))
    );

  private static TimeSpan ResolverFimAdmissao(Admissao admissao, TimeSpan duracao)
  {
    if (!admissao.HoraInicio.HasValue)
    {
      return TimeSpan.Zero;
    }

    TimeSpan inicio = admissao.HoraInicio.Value;
    if (admissao.HoraFim.HasValue && admissao.HoraFim.Value > inicio)
    {
      return admissao.HoraFim.Value;
    }

    return inicio.Add(duracao);
  }

  private static bool IsStatusAtivo(StatusConsulta? status) =>
    status == null
    || (
      status != StatusConsulta.Desmarcada
      && status != StatusConsulta.Suspensa
      && status != StatusConsulta.Concluida
    );

  private static bool IntervalosSobrepoem(
    TimeSpan inicio,
    TimeSpan fim,
    TimeSpan? outroInicio,
    TimeSpan? outroFim
  ) =>
    outroInicio.HasValue
    && outroFim.HasValue
    && outroInicio.Value < fim
    && outroFim.Value > inicio;
}
