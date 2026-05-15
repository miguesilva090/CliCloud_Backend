using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;

public class FechoDiarioAdministrativoService(IRepositoryAsync repository) : IFechoDiarioAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;

  public async Task<Response<FechoDiarioResultDTO>> ExecutarFechoAsync(FechoDiarioRequest request)
  {
    DateTime data = request.Data!.Value.Date;
    List<Admissao> admissoes = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissoesParaFechoSpec(data))
    ).ToList();

    var result = new FechoDiarioResultDTO();
    List<Consulta> consultasExistentes = (await _repository.GetListAsync<Consulta, Guid>()).ToList();

    foreach (Admissao admissao in admissoes)
    {
      result.TotalProcessadas++;
      try
      {
        if (consultasExistentes.Any(c => c.AdmissaoId == admissao.Id && c.DeletedOn == null))
        {
          result.Erros.Add($"Admissão {admissao.Id}: já promovida.");
          continue;
        }

        Consulta consulta = AdmissaoPromocaoHelper.CriarConsultaDesdeAdmissao(admissao);
        Consulta created = await _repository.CreateAsync<Consulta, Guid>(consulta);

        foreach (ServicoConsulta servico in AdmissaoPromocaoHelper.MapearServicos(admissao, created.Id))
        {
          servico.Id = Guid.NewGuid();
          _ = await _repository.CreateAsync<ServicoConsulta, Guid>(servico);
        }

        if (admissao.ConsultaMarcacaoId.HasValue)
        {
          ConsultaMarcacao marcacao =
            await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(admissao.ConsultaMarcacaoId.Value);
          marcacao.ConsultaId = created.Id;
          _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
        }

        await _repository.RemoveByIdAsync<Admissao, Guid>(admissao.Id);
        result.TotalConsultasCriadas++;
      }
      catch (Exception ex)
      {
        result.Erros.Add($"Admissão {admissao.Id}: {ex.Message}");
      }
    }

    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(result);
  }
}
