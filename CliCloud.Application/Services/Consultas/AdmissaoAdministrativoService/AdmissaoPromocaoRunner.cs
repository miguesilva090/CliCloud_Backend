using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

internal static class AdmissaoPromocaoRunner
{
  public static async Task<Guid> PromoverAsync(
    Admissao admissao,
    IRepositoryAsync repository,
    IRequisicaoEspFechoUpdater? requisicaoEspUpdater,
    CancellationToken cancellationToken = default
  )
  {
    bool jaPromovida = await repository.ExistsAsync<Consulta, Guid>(
      new ConsultaPorAdmissaoSpec(admissao.Id),
      cancellationToken
    );
    if (jaPromovida)
    {
      throw new InvalidOperationException($"Admissão {admissao.Id}: já promovida.");
    }

    Guid? consultaClinicaId = await ObterConsultaClinicaDaMarcacaoAsync(
      repository,
      admissao,
      cancellationToken
    );

    Guid consultaId = consultaClinicaId.HasValue
      ? await FundirAdmissaoEmConsultaExistenteAsync(
          repository,
          admissao,
          consultaClinicaId.Value,
          cancellationToken
        )
      : await CriarConsultaDesdeAdmissaoAsync(repository, admissao, cancellationToken);

    await AdmissaoFaturacaoPromocaoHelper.SincronizarDesdeAdmissaoAsync(
      repository,
      admissao,
      consultaId,
      cancellationToken
    );
    await AplicarCredencialEspAsync(requisicaoEspUpdater, admissao, cancellationToken);
    await repository.RemoveByIdAsync<Admissao, Guid>(admissao.Id);
    return consultaId;
  }

  private static async Task<Guid?> ObterConsultaClinicaDaMarcacaoAsync(
    IRepositoryAsync repository,
    Admissao admissao,
    CancellationToken cancellationToken
  )
  {
    if (!admissao.ConsultaMarcacaoId.HasValue)
    {
      return null;
    }

    ConsultaMarcacao marcacao = await repository.GetByIdAsync<ConsultaMarcacao, Guid>(
      admissao.ConsultaMarcacaoId.Value,
      cancellationToken: cancellationToken
    );
    return marcacao.ConsultaId;
  }

  private static async Task<Guid> FundirAdmissaoEmConsultaExistenteAsync(
    IRepositoryAsync repository,
    Admissao admissao,
    Guid consultaId,
    CancellationToken cancellationToken
  )
  {
    Consulta consulta = await repository.GetByIdAsync<Consulta, Guid>(
      consultaId,
      cancellationToken: cancellationToken
    );

    AdmissaoPromocaoHelper.MesclarAdmissaoEmConsultaExistente(consulta, admissao);
    _ = await repository.UpdateAsync<Consulta, Guid>(consulta);

    List<ServicoConsulta> servicosExistentes = (
      await repository.GetListAsync<ServicoConsulta, Guid>(
        new ServicosConsultaPorConsultaIdSpec(consultaId),
        cancellationToken
      )
    ).ToList();

    List<int> linhas = servicosExistentes.Select(s => s.Linha).ToList();
    foreach (ServicoConsulta servico in AdmissaoPromocaoHelper.MapearServicosNovos(
               admissao,
               consultaId,
               linhas
             ))
    {
      if (servico.Id == Guid.Empty)
      {
        servico.Id = Guid.NewGuid();
      }

      _ = await repository.CreateAsync<ServicoConsulta, Guid>(servico);
    }

    if (admissao.ConsultaMarcacaoId.HasValue)
    {
      ConsultaMarcacao marcacao = await repository.GetByIdAsync<ConsultaMarcacao, Guid>(
        admissao.ConsultaMarcacaoId.Value,
        cancellationToken: cancellationToken
      );
      if (marcacao.ConsultaId != consultaId)
      {
        marcacao.ConsultaId = consultaId;
        _ = await repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
      }
    }

    return consultaId;
  }

  private static async Task<Guid> CriarConsultaDesdeAdmissaoAsync(
    IRepositoryAsync repository,
    Admissao admissao,
    CancellationToken cancellationToken
  )
  {
    Consulta consulta = AdmissaoPromocaoHelper.CriarConsultaDesdeAdmissao(admissao);
    Consulta created = await repository.CreateAsync<Consulta, Guid>(consulta);

    foreach (ServicoConsulta servico in AdmissaoPromocaoHelper.MapearServicos(admissao, created.Id))
    {
      if (servico.Id == Guid.Empty)
      {
        servico.Id = Guid.NewGuid();
      }

      _ = await repository.CreateAsync<ServicoConsulta, Guid>(servico);
    }

    if (admissao.ConsultaMarcacaoId.HasValue)
    {
      ConsultaMarcacao marcacao = await repository.GetByIdAsync<ConsultaMarcacao, Guid>(
        admissao.ConsultaMarcacaoId.Value,
        cancellationToken: cancellationToken
      );
      marcacao.ConsultaId = created.Id;
      _ = await repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
    }

    return created.Id;
  }

  private static async Task AplicarCredencialEspAsync(
    IRequisicaoEspFechoUpdater? requisicaoEspUpdater,
    Admissao admissao,
    CancellationToken cancellationToken
  )
  {
    if (requisicaoEspUpdater == null || string.IsNullOrWhiteSpace(admissao.Credencial))
    {
      return;
    }

    await requisicaoEspUpdater.MarcarRealizadoSeAplicavelAsync(
      admissao.Credencial.Trim(),
      DateTime.UtcNow,
      cancellationToken
    );
  }
}
