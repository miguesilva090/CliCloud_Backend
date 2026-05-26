using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.DisponibilidadeSala.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.DisponibilidadeSala;

public class DisponibilidadeSalaRequest
{
  public Guid? SalaId { get; set; }
  public DateTime? Data { get; set; }
  public TimeSpan? HoraInicio { get; set; }
  public TimeSpan? HoraFim { get; set; }
  public Guid? IgnorarMarcacaoId { get; set; }
  public Guid? IgnorarAdmissaoId { get; set; }
  public bool RequerSala { get; set; }
}

public class DisponibilidadeSalaResult
{
  public bool Valido { get; set; }
  public string? Mensagem { get; set; }
  public Guid? SalaId { get; set; }

  public static DisponibilidadeSalaResult Ok(Guid? salaId) =>
    new() { Valido = true, SalaId = salaId };

  public static DisponibilidadeSalaResult Fail(string mensagem) =>
    new() { Valido = false, Mensagem = mensagem };
}

public static class DisponibilidadeSalaHelper
{
  private static readonly TimeSpan DefaultDuracao = TimeSpan.FromMinutes(30);

  public static async Task<DisponibilidadeSalaResult> ValidarAsync(
    IRepositoryAsync repository,
    DisponibilidadeSalaRequest request
  )
  {
    if (!request.SalaId.HasValue)
    {
      return request.RequerSala
        ? DisponibilidadeSalaResult.Fail("Sala é obrigatória.")
        : DisponibilidadeSalaResult.Ok(null);
    }

    if (!request.Data.HasValue)
    {
      return DisponibilidadeSalaResult.Fail("Data é obrigatória.");
    }

    if (!request.HoraInicio.HasValue)
    {
      return DisponibilidadeSalaResult.Fail("Hora de início é obrigatória.");
    }

    if (request.HoraFim.HasValue && request.HoraFim.Value <= request.HoraInicio.Value)
    {
      return DisponibilidadeSalaResult.Fail("Hora de fim inválida.");
    }

    TimeSpan horaInicio = request.HoraInicio.Value;
    TimeSpan horaFim = request.HoraFim ?? horaInicio.Add(DefaultDuracao);

    Sala? sala = await repository.GetByIdAsync<Sala, Guid>(request.SalaId.Value);
    if (sala == null || sala.DeletedOn != null || !sala.Ativa)
    {
      return DisponibilidadeSalaResult.Fail("Sala inválida ou inativa.");
    }

    DateTime data = request.Data.Value.Date;
    TimeSpan duracao = horaFim - horaInicio;

    List<ConsultaMarcacao> marcacoes = (
      await repository.GetListAsync<ConsultaMarcacao, Guid>(
        new ConsultaMarcacoesSalaDataSpec(request.SalaId.Value, data)
      )
    ).ToList();

    if (ExisteConflitoMarcacao(marcacoes, request, horaInicio, horaFim, duracao))
    {
      return DisponibilidadeSalaResult.Fail("Já existe uma marcação ativa nesta sala.");
    }

    List<Admissao> admissoes = (
      await repository.GetListAsync<Admissao, Guid>(
        new AdmissoesSalaDataSpec(request.SalaId.Value, data)
      )
    ).ToList();

    if (ExisteConflitoAdmissao(admissoes, request, horaInicio, horaFim, duracao))
    {
      return DisponibilidadeSalaResult.Fail("Já existe uma consulta a decorrer nesta sala.");
    }

    return DisponibilidadeSalaResult.Ok(request.SalaId.Value);
  }

  private static bool ExisteConflitoMarcacao(
    IEnumerable<ConsultaMarcacao> marcacoes,
    DisponibilidadeSalaRequest request,
    TimeSpan inicio,
    TimeSpan fim,
    TimeSpan duracao
  ) =>
    marcacoes.Any(x =>
      (!request.IgnorarMarcacaoId.HasValue || x.Id != request.IgnorarMarcacaoId.Value)
      && IsStatusAtivo(x.StatusConsulta)
      && x.HoraMarcacao.HasValue
      && IntervalosSobrepoem(inicio, fim, x.HoraMarcacao.Value, ResolverFimMarcacao(x, duracao))
    );

  private static bool ExisteConflitoAdmissao(
    IEnumerable<Admissao> admissoes,
    DisponibilidadeSalaRequest request,
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

  private static TimeSpan ResolverFimMarcacao(ConsultaMarcacao marcacao, TimeSpan duracao)
  {
    if (!marcacao.HoraMarcacao.HasValue)
    {
      return TimeSpan.Zero;
    }

    return marcacao.HoraMarcacao.Value.Add(duracao);
  }

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
    TimeSpan outroInicio,
    TimeSpan outroFim
  ) =>
    outroInicio < fim && outroFim > inicio;
}
