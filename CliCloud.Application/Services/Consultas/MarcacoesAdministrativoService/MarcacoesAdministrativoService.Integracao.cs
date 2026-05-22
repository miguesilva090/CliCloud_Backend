using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Specifications;
using CliCloud.Application.Services.Medicos.MedicoService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;

public partial class MarcacoesAdministrativoService
{
  public async Task<Response<MedicoLegadoResolveDTO>> ResolveMedicoLegadoAsync(string key)
  {
    string trimmed = key?.Trim() ?? string.Empty;
    if (string.IsNullOrWhiteSpace(trimmed))
    {
      return ResponseFactory.Fail<MedicoLegadoResolveDTO>("Parâmetro vazio.");
    }

    if (Guid.TryParse(trimmed, out Guid medicoGuid))
    {
      Medico? byId = await _repository.GetByIdAsync<Medico, Guid>(medicoGuid);
      if (byId == null)
      {
        return ResponseFactory.Fail<MedicoLegadoResolveDTO>("Médico não encontrado.");
      }

      return ResponseFactory.Success(
        new MedicoLegadoResolveDTO
        {
          MedicoId = byId.Id,
          MedicoNome = byId.Nome,
          Letra = byId.Letra,
        }
      );
    }

    List<Medico> medicos = (
      await _repository.GetListAsync<Medico, Guid>(new MedicoByLetraSpecification(trimmed))
    ).ToList();

    if (medicos.Count == 0)
    {
      return ResponseFactory.Fail<MedicoLegadoResolveDTO>(
        $"Não foi encontrado médico com código/letra «{trimmed}»."
      );
    }

    Medico medico = medicos.Count == 1 ? medicos[0] : medicos[0];
    return ResponseFactory.Success(
      new MedicoLegadoResolveDTO
      {
        MedicoId = medico.Id,
        MedicoNome = medico.Nome,
        Letra = medico.Letra,
      }
    );
  }

  public async Task<Response<Guid>> SincronizarAdmissaoAsync(Guid marcacaoId)
  {
    var spec = new MarcacaoAdministrativoByIdSpec(marcacaoId);
    ConsultaMarcacao? entity = (
      await _repository.GetListAsync<ConsultaMarcacao, Guid>(spec)
    ).FirstOrDefault();
    if (entity == null)
    {
      return ResponseFactory.Fail<Guid>("Marcação não encontrada.");
    }

    Guid? organismoId = null;
    string? credencial = null;
    Admissao? admissaoExistente = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByConsultaMarcacaoSpec(marcacaoId))
    ).FirstOrDefault();
    if (admissaoExistente != null)
    {
      organismoId = admissaoExistente.OrganismoId;
      credencial = admissaoExistente.Credencial;
    }
    else
    {
      Utente? utente = await _repository.GetByIdAsync<Utente, Guid>(entity.UtenteId);
      organismoId = utente?.OrganismoId;
    }

    await MarcacaoAdmissaoSyncHelper.SyncAdmissaoFromMarcacaoAsync(
      _repository,
      entity,
      new MarcacaoAdmissaoSyncHelper.MarcacaoAdmissaoFields
      {
        OrganismoId = organismoId,
        Credencial = credencial,
        SalaId = entity.SalaId,
      }
    );
    _ = await _repository.SaveChangesAsync();

    Admissao? admissao = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByConsultaMarcacaoSpec(marcacaoId))
    ).FirstOrDefault();
    if (admissao == null)
    {
      return ResponseFactory.Fail<Guid>("Não foi possível criar a admissão ligada à marcação.");
    }

    return ResponseFactory.Success(admissao.Id);
  }
}
