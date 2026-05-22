using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Application.Services.Core.SmsService;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Consultas;

/// <summary>SMS automático nas marcações (legado sendConsultaSmsUtente / fluxos 6.1 e 6.2).</summary>
public static class MarcacaoConsultaSmsHelper
{
  public static async Task TentarDispararAsync(
    IRepositoryAsync repository,
    IServicoSms servicoSms,
    ConsultaMarcacao marcacao,
    string codigoConfiguracao
  )
  {
    try
    {
      Clinica? clinica = (
        await repository.GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected())
      ).FirstOrDefault();
      if (clinica is null)
      {
        return;
      }

      Utente? utente = await repository.GetByIdAsync<Utente, Guid>(marcacao.UtenteId);
      if (utente is null)
      {
        return;
      }

      var contactosSpec = new EntidadeContactosByEntidadeIdsSpecification([utente.Id]);
      List<EntidadeContacto> contactos = (
        await repository.GetListAsync<EntidadeContacto, Guid>(contactosSpec)
      ).ToList();
      string? contacto = contactos
        .Where(x => !string.IsNullOrWhiteSpace(x.Valor))
        .OrderByDescending(x => x.Principal)
        .Select(x => x.Valor!)
        .FirstOrDefault();
      if (string.IsNullOrWhiteSpace(contacto))
      {
        return;
      }

      string medicoNome = string.Empty;
      if (marcacao.MedicoId.HasValue)
      {
        Medico? medico = await repository.GetByIdAsync<Medico, Guid>(marcacao.MedicoId.Value);
        if (medico is not null)
        {
          medicoNome = medico.Nome ?? string.Empty;
        }
      }

      string especialidadeNome = string.Empty;
      if (marcacao.EspecialidadeId.HasValue)
      {
        Especialidade? especialidade = await repository.GetByIdAsync<Especialidade, Guid>(
          marcacao.EspecialidadeId.Value
        );
        if (especialidade is not null)
        {
          especialidadeNome = especialidade.Nome ?? string.Empty;
        }
      }

      var smsRequest = new EnviarSmsPorCodigoRequest
      {
        CodigoConfiguracao = codigoConfiguracao,
        NumeroDestinatario = contacto,
        NomeUtente = utente.Nome ?? string.Empty,
        NomeMedicoOuProfissional = medicoNome,
        NomeEspecialidade = especialidadeNome,
        Data = marcacao.Data,
        Hora = marcacao.HoraMarcacao?.ToString(@"hh\:mm"),
        Modulo = "MarcacoesAdm",
      };

      _ = await servicoSms.EnviarSmsPorCodigoAsync(clinica.Id, smsRequest);
    }
    catch
    {
      /* não bloquear o fluxo principal */
    }
  }
}
