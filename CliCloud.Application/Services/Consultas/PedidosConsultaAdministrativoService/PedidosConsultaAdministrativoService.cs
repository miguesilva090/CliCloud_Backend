using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using ResponseStatus = CliCloud.Application.Common.Wrapper.ResponseStatus;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Specifications;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Application.Services.Core.EmailService;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Application.Services.Core.SmsService;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService;

public class PedidosConsultaAdministrativoService(
  IRepositoryAsync repository,
  IMarcacoesAdministrativoService marcacoesAdministrativoService,
  IServicoSms servicoSms,
  IConfiguracaoEmailService configuracaoEmailService,
  ICurrentClinicaService? currentClinicaService = null
) : IPedidosConsultaAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly IMarcacoesAdministrativoService _marcacoesAdministrativoService =
    marcacoesAdministrativoService;
  private readonly IServicoSms _servicoSms = servicoSms;
  private readonly IConfiguracaoEmailService _configuracaoEmailService = configuracaoEmailService;
  private readonly ICurrentClinicaService? _currentClinicaService = currentClinicaService;

  public async Task<PaginatedResponse<PedidoConsultaTableDTO>> GetPaginatedAsync(
    PedidoConsultaTableFilter filter
  )
  {
    Guid? clinicaId = filter.ClinicaId ?? await ResolveClinicaIdAsync();
    filter.ClinicaId = clinicaId;

    if (!clinicaId.HasValue)
    {
      int pageEmpty = Math.Max(1, filter.PageNumber);
      int sizeEmpty = Math.Max(1, filter.PageSize);
      return new PaginatedResponse<PedidoConsultaTableDTO>([], 0, pageEmpty, sizeEmpty);
    }

    if (filter.Filters?.Count > 0)
    {
      filter.PageNumber = 1;
    }

    string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
    var spec = new PedidoConsultaSearchTable(filter, order);
    List<PedidoConsulta> all = (
      await _repository.GetListAsync<PedidoConsulta, int>(spec)
    ).ToList();

    int total = all.Count;
    int page = Math.Max(1, filter.PageNumber);
    int size = Math.Max(1, filter.PageSize);
    List<PedidoConsulta> pageItems = all.Skip((page - 1) * size).Take(size).ToList();

    List<PedidoConsultaTableDTO> data = pageItems
      .Select(MapToTableDto)
      .ToList();
    await HydrateLegacyNamesAsync(data);

    return new PaginatedResponse<PedidoConsultaTableDTO>(data, total, page, size);
  }

  public async Task<Response<PedidoConsultaDTO>> GetByIdAsync(int codigo)
  {
    PedidoConsulta? entity = await LoadPedidoAsync(codigo);
    if (entity is null)
    {
      return ResponseFactory.Fail<PedidoConsultaDTO>("Pedido não encontrado.");
    }

    PedidoConsultaDTO dto = await MapToDetailDtoAsync(entity);
    return ResponseFactory.Success(dto);
  }

  public async Task<Response<int>> SetRecusadoAsync(
    int codigo,
    SetPedidoConsultaRecusadoRequest request
  )
  {
    PedidoConsulta? entity = await LoadPedidoAsync(codigo);
    if (entity is null)
    {
      return ResponseFactory.Fail<int>("Pedido não encontrado.");
    }

    entity.Recusado = request.Recusado;
    _ = await _repository.UpdateAsync<PedidoConsulta, int>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<int>> DeleteAsync(int codigo)
  {
    PedidoConsulta? entity = await LoadPedidoAsync(codigo);
    if (entity is null)
    {
      return ResponseFactory.Fail<int>("Pedido não encontrado.");
    }

    await _repository.RemoveAsync<PedidoConsulta, int>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(codigo);
  }

  public async Task<Response<int[]>> DeleteMultipleAsync(IEnumerable<int> codigos)
  {
    List<int> ids = codigos.Distinct().ToList();
    foreach (int id in ids)
    {
      PedidoConsulta? entity = await LoadPedidoAsync(id);
      if (entity is not null)
      {
        await _repository.RemoveAsync<PedidoConsulta, int>(entity);
      }
    }

    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(ids.ToArray());
  }

  public async Task<Response<PedidoConsultaFicheiroDTO>> DownloadFicheiroAsync(int codigo)
  {
    PedidoConsulta? entity = await LoadPedidoAsync(codigo);
    if (entity is null || string.IsNullOrWhiteSpace(entity.Ficheiro))
    {
      return ResponseFactory.Fail<PedidoConsultaFicheiroDTO>("Ficheiro não encontrado.");
    }

    string nome = entity.Ficheiro.Trim();
    string? basePath = null;
    if (entity.ClinicaId != Guid.Empty)
    {
      Clinica? clinica = await _repository.GetByIdAsync<Clinica, Guid>(entity.ClinicaId);
      basePath = clinica?.DiretoriaDocumentos;
    }

    string fullPath;
    if (!string.IsNullOrWhiteSpace(basePath))
    {
      fullPath = Path.Combine(basePath.TrimEnd('\\', '/'), nome);
    }
    else
    {
      string appBase = AppDomain.CurrentDomain.BaseDirectory;
      fullPath = Path.Combine(appBase, "UserFiles", "GlobalBooking", nome);
    }

    if (!File.Exists(fullPath))
    {
      return ResponseFactory.Fail<PedidoConsultaFicheiroDTO>(
        "Ficheiro não existe no servidor."
      );
    }

    byte[] bytes = await File.ReadAllBytesAsync(fullPath);
    return ResponseFactory.Success(
      new PedidoConsultaFicheiroDTO
      {
        Nome = nome,
        ConteudoBase64 = Convert.ToBase64String(bytes),
      }
    );
  }

  public async Task<Response<PedidoConsultaUtentesCandidatosDTO>> PesquisarUtentesAsync(
    int codigo
  )
  {
    PedidoConsulta? pedido = await LoadPedidoAsync(codigo);
    if (pedido?.UtentePedido is null)
    {
      return ResponseFactory.Fail<PedidoConsultaUtentesCandidatosDTO>("Pedido não encontrado.");
    }

    PedidoConsultaUtente pu = pedido.UtentePedido;
    var result = new PedidoConsultaUtentesCandidatosDTO();
    HashSet<Guid> seen = [];

    async Task AddMatchesAsync(IEnumerable<Utente> list, Action flag)
    {
      foreach (Utente u in list)
      {
        if (!seen.Add(u.Id))
        {
          continue;
        }

        flag();
        result.Utentes.Add(
          new PedidoConsultaUtenteCandidatoDTO
          {
            Id = u.Id,
            Nome = u.Nome,
            NumeroUtente = u.NumeroUtente,
            NumeroContribuinte = u.NumeroContribuinte,
            Email = u.Email,
          }
        );
      }
    }

    if (!string.IsNullOrWhiteSpace(pu.NIF))
    {
      List<Utente> byNif = (
        await _repository.GetListAsync<Utente, Guid>(
          new UtenteByNumeroContribuinteSpecification(pu.NIF.Trim())
        )
      ).ToList();
      if (byNif.Count > 0)
      {
        result.ExisteNif = true;
      }

      await AddMatchesAsync(byNif, () => result.ExisteNif = true);
    }

    List<Utente> byNome = (
      await _repository.GetListAsync<Utente, Guid>(new UtenteMatchName(pu.Nome))
    ).ToList();
    await AddMatchesAsync(byNome, () => result.ExisteNome = true);

    if (!string.IsNullOrWhiteSpace(pu.Email))
    {
      List<Utente> byEmail = (
        await _repository.GetListAsync<Utente, Guid>(
          new UtenteByEmailSpecification(pu.Email.Trim())
        )
      ).ToList();
      await AddMatchesAsync(byEmail, () => result.ExisteEmail = true);
    }

    if (!string.IsNullOrWhiteSpace(pu.Telemovel))
    {
      var contactos = (
        await _repository.GetListAsync<EntidadeContacto, Guid>(
          new EntidadeContactoByValorSpecification(pu.Telemovel.Trim())
        )
      ).ToList();
      List<Guid> utenteIds = contactos.Select(c => c.EntidadeId).Distinct().ToList();
      foreach (Guid utenteId in utenteIds)
      {
        Utente? u = await _repository.GetByIdAsync<Utente, Guid>(utenteId);
        if (u is not null)
        {
          await AddMatchesAsync([u], () => result.ExisteTelemovel = true);
        }
      }
    }

    return ResponseFactory.Success(result);
  }

  public async Task<Response<Guid>> CriarUtenteFromPedidoAsync(int codigo, bool forcar)
  {
    if (!forcar)
    {
      Response<PedidoConsultaUtentesCandidatosDTO> candidatos = await PesquisarUtentesAsync(
        codigo
      );
      if (
        candidatos.Data is not null
        && candidatos.Status == ResponseStatus.Success
        && (
          candidatos.Data.ExisteNif
          || candidatos.Data.ExisteNome
          || candidatos.Data.ExisteEmail
          || candidatos.Data.ExisteTelemovel
        )
      )
      {
        return ResponseFactory.Fail<Guid>(
          "Existem utentes semelhantes. Confirme criação forçada (forcar=true)."
        );
      }
    }

    PedidoConsulta? pedido = await LoadPedidoAsync(codigo);
    if (pedido?.UtentePedido is null)
    {
      return ResponseFactory.Fail<Guid>("Pedido não encontrado.");
    }

    Guid? organismoId = await ResolveOrganismoIdFromCodinstAsync(pedido.UtentePedido.Codinst);
    var utente = new Utente
    {
      Id = Guid.NewGuid(),
      Nome = pedido.UtentePedido.Nome,
      Email = pedido.UtentePedido.Email,
      NumeroContribuinte = string.IsNullOrWhiteSpace(pedido.UtentePedido.NIF)
        ? "999999990"
        : pedido.UtentePedido.NIF.Trim(),
      TipoEntidade = EntidadeTipo.Utente,
      Status = Status.Ativo,
      OrganismoId = organismoId,
      Observacoes = "Pedido de Consulta - GlobalBooking",
    };

    Utente created = await _repository.CreateAsync<Utente, Guid>(utente);
    _ = await _repository.SaveChangesAsync();

    if (!string.IsNullOrWhiteSpace(pedido.UtentePedido.Telemovel))
    {
      var contacto = new EntidadeContacto
      {
        Id = Guid.NewGuid(),
        EntidadeId = created.Id,
        EntidadeContactoTipoId = 1,
        Valor = pedido.UtentePedido.Telemovel.Trim(),
        Principal = true,
      };
      _ = await _repository.CreateAsync<EntidadeContacto, Guid>(contacto);
      _ = await _repository.SaveChangesAsync();
    }

    return ResponseFactory.Success(created.Id);
  }

  public async Task<Response<GuardarPedidoConsultaMarcacaoResultDTO>> GuardarMarcacaoAsync(
    int codigo,
    GuardarPedidoConsultaMarcacaoRequest request
  )
  {
    PedidoConsulta? pedido = await LoadPedidoAsync(codigo);
    if (pedido is null)
    {
      return ResponseFactory.Fail<GuardarPedidoConsultaMarcacaoResultDTO>("Pedido não encontrado.");
    }

    if (pedido.Agendado)
    {
      return ResponseFactory.Fail<GuardarPedidoConsultaMarcacaoResultDTO>(
        pedido.CodigoAdmissao.HasValue
          ? $"Este pedido já está agendado (admissão legado {pedido.CodigoAdmissao})."
          : "Este pedido já está agendado."
      );
    }

    Guid utenteId;
    if (request.UtenteId.HasValue)
    {
      utenteId = request.UtenteId.Value;
    }
    else
    {
      Response<Guid> criar = await CriarUtenteFromPedidoAsync(
        codigo,
        request.ForcarNovoUtente
      );
      if (criar.Status != ResponseStatus.Success || criar.Data == Guid.Empty)
      {
        return ResponseFactory.Fail<GuardarPedidoConsultaMarcacaoResultDTO>(
          criar.Messages?.Values.FirstOrDefault()?.FirstOrDefault()
            ?? "Não foi possível criar utente."
        );
      }

      utenteId = criar.Data;
    }

    var createMarcacao = new CreateMarcacaoAdministrativoRequest
    {
      UtenteId = utenteId,
      MedicoId = request.MedicoId,
      EspecialidadeId = request.EspecialidadeId,
      OrganismoId = request.OrganismoId,
      TipoConsultaId = request.TipoConsultaId,
      TipoAdmissaoId = request.TipoAdmissaoId,
      Data = request.Data.Date,
      HoraInicio = request.Hora,
      Obs = string.IsNullOrWhiteSpace(request.Obs)
        ? "Admissão vinda do GlobalBooking"
        : request.Obs,
    };

    Response<Guid> marcacaoRes = await _marcacoesAdministrativoService.CreateAsync(
      createMarcacao
    );
    if (marcacaoRes.Status != ResponseStatus.Success)
    {
      return ResponseFactory.Fail<GuardarPedidoConsultaMarcacaoResultDTO>(
        marcacaoRes.Messages?.Values.FirstOrDefault()?.FirstOrDefault()
          ?? "Não foi possível criar marcação."
      );
    }

    Guid marcacaoId = marcacaoRes.Data;
    Response<Guid> syncRes = await _marcacoesAdministrativoService.SincronizarAdmissaoAsync(
      marcacaoId
    );

    var avisos = new List<string>();
    pedido.Agendado = true;
    _ = await _repository.UpdateAsync<PedidoConsulta, int>(pedido);
    _ = await _repository.SaveChangesAsync();

    if (request.EnviarEmail)
    {
      Response<int> emailRes = await EnviarEmailAsync(codigo, 2);
      if (emailRes.Status != ResponseStatus.Success)
      {
        avisos.Add(
          emailRes.Messages?.Values.FirstOrDefault()?.FirstOrDefault()
            ?? "Falha no envio de email."
        );
      }
    }

    if (request.EnviarSms)
    {
      Response<int> smsRes = await EnviarSmsAsync(codigo, 4);
      if (smsRes.Status != ResponseStatus.Success)
      {
        avisos.Add(
          smsRes.Messages?.Values.FirstOrDefault()?.FirstOrDefault()
            ?? "Falha no envio de SMS."
        );
      }
    }

    return ResponseFactory.Success(
      new GuardarPedidoConsultaMarcacaoResultDTO
      {
        MarcacaoId = marcacaoId,
        AdmissaoId =
          syncRes.Status == ResponseStatus.Success ? syncRes.Data : null,
        Avisos = avisos,
      }
    );
  }

  public async Task<Response<int>> EnviarEmailAsync(int codigo, int tipo)
  {
    if (tipo is not (1 or 2))
    {
      return ResponseFactory.Fail<int>("Tipo de email inválido (1=pedido, 2=agendado).");
    }

    Guid? clinicaId = await ResolveClinicaIdAsync();
    if (!clinicaId.HasValue)
    {
      return ResponseFactory.Fail<int>("Clínica não definida.");
    }

    PedidoConsulta? pedido = await LoadPedidoAsync(codigo);
    if (pedido?.UtentePedido is null)
    {
      return ResponseFactory.Fail<int>("Pedido não encontrado.");
    }

    if (tipo == 2 && !pedido.Agendado)
    {
      return ResponseFactory.Fail<int>("O pedido deve estar agendado para enviar email de agendamento.");
    }

    Response<MedicoLegadoResolveDTO> medicoRes = await _marcacoesAdministrativoService.ResolveMedicoLegadoAsync(
      pedido.CodigoMedico ?? string.Empty
    );
    string medicoNome = medicoRes.Data?.MedicoNome ?? pedido.CodigoMedico ?? string.Empty;

    var emailRequest = new EnviarEmailPorCodigoRequest
    {
      CodigoConfiguracao = $"6.{tipo}",
      EmailDestino = pedido.UtentePedido.Email,
      NomeUtente = pedido.UtentePedido.Nome,
      Data = pedido.Data,
      Hora = pedido.Hora,
      NomeMedicoOuProfissional = medicoNome,
      Modulo = "GlobalBooking",
    };

    Response<Guid> sendRes = await _configuracaoEmailService.EnviarEmailPorCodigoAsync(
      clinicaId.Value,
      emailRequest
    );
    if (sendRes.Status != ResponseStatus.Success)
    {
      return ResponseFactory.Fail<int>(
        sendRes.Messages?.Values.FirstOrDefault()?.FirstOrDefault()
          ?? "Falha no envio de email."
      );
    }

    if (tipo == 1)
    {
      pedido.EmailPedido = true;
    }
    else
    {
      pedido.EmailAgendado = true;
    }

    _ = await _repository.UpdateAsync<PedidoConsulta, int>(pedido);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(pedido.Id);
  }

  public async Task<Response<int>> EnviarSmsAsync(int codigo, int tipo)
  {
    int tipoNormalizado = tipo is 3 or 4 ? tipo - 2 : tipo;
    if (tipoNormalizado is not (1 or 2))
    {
      return ResponseFactory.Fail<int>("Tipo de SMS inválido (1=pedido, 2=agendado ou 3/4 legado).");
    }

    Guid? clinicaId = await ResolveClinicaIdAsync();
    if (!clinicaId.HasValue)
    {
      return ResponseFactory.Fail<int>("Clínica não definida.");
    }

    PedidoConsulta? pedido = await LoadPedidoAsync(codigo);
    if (pedido?.UtentePedido is null)
    {
      return ResponseFactory.Fail<int>("Pedido não encontrado.");
    }

    if (tipoNormalizado == 2 && !pedido.Agendado)
    {
      return ResponseFactory.Fail<int>("O pedido deve estar agendado para enviar SMS de agendamento.");
    }

    Response<MedicoLegadoResolveDTO> medicoRes = await _marcacoesAdministrativoService.ResolveMedicoLegadoAsync(
      pedido.CodigoMedico ?? string.Empty
    );
    string medicoNome = medicoRes.Data?.MedicoNome ?? pedido.CodigoMedico ?? string.Empty;

    var smsRequest = new EnviarSmsPorCodigoRequest
    {
      CodigoConfiguracao = $"6.{tipoNormalizado}",
      NumeroDestinatario = pedido.UtentePedido.Telemovel,
      NomeUtente = pedido.UtentePedido.Nome,
      NomeMedicoOuProfissional = medicoNome,
      Data = pedido.Data,
      Hora = pedido.Hora,
      Modulo = "GlobalBooking",
    };

    Response<Guid> sendRes = await _servicoSms.EnviarSmsPorCodigoAsync(
      clinicaId.Value,
      smsRequest
    );
    if (sendRes.Status != ResponseStatus.Success)
    {
      return ResponseFactory.Fail<int>(
        sendRes.Messages?.Values.FirstOrDefault()?.FirstOrDefault()
          ?? "Falha no envio de SMS."
      );
    }

    if (tipoNormalizado == 1)
    {
      pedido.SmsPedido = true;
    }
    else
    {
      pedido.SmsAgendado = true;
    }

    _ = await _repository.UpdateAsync<PedidoConsulta, int>(pedido);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(pedido.Id);
  }

  private async Task<PedidoConsulta?> LoadPedidoAsync(int codigo)
  {
    Guid? clinicaId = await ResolveClinicaIdAsync();
    return (
      await _repository.GetListAsync<PedidoConsulta, int>(
        new PedidoConsultaByIdSpec(codigo, clinicaId)
      )
    ).FirstOrDefault();
  }

  private static PedidoConsultaTableDTO MapToTableDto(PedidoConsulta entity)
  {
    return new PedidoConsultaTableDTO
    {
      Codigo = entity.Id,
      CodigoPedidosConsultaUtente = entity.CodigoPedidosConsultaUtente,
      CodigoEspecialidade = entity.CodigoEspecialidade,
      Nome = entity.UtentePedido?.Nome,
      Telemovel = entity.UtentePedido?.Telemovel,
      Email = entity.UtentePedido?.Email,
      Data = entity.Data,
      Hora = entity.Hora,
      CodigoMedico = entity.CodigoMedico,
      Agendado = entity.Agendado,
      EmailPedido = entity.EmailPedido,
      SmsPedido = entity.SmsPedido,
      EmailAgendado = entity.EmailAgendado,
      SmsAgendado = entity.SmsAgendado,
      Recusado = entity.Recusado,
      TemFicheiro = !string.IsNullOrWhiteSpace(entity.Ficheiro),
    };
  }

  private async Task<PedidoConsultaDTO> MapToDetailDtoAsync(PedidoConsulta entity)
  {
    var dto = new PedidoConsultaDTO
    {
      Codigo = entity.Id,
      CodigoPedidosConsultaUtente = entity.CodigoPedidosConsultaUtente,
      CodigoEspecialidade = entity.CodigoEspecialidade,
      Nome = entity.UtentePedido?.Nome,
      Email = entity.UtentePedido?.Email,
      Telemovel = entity.UtentePedido?.Telemovel,
      NIF = entity.UtentePedido?.NIF,
      Codinst = entity.UtentePedido?.Codinst ?? 0,
      CodigoMedico = entity.CodigoMedico,
      Data = entity.Data,
      Hora = entity.Hora,
      Agendado = entity.Agendado,
      EmailPedido = entity.EmailPedido,
      SmsPedido = entity.SmsPedido,
      EmailAgendado = entity.EmailAgendado,
      SmsAgendado = entity.SmsAgendado,
      Recusado = entity.Recusado,
      CodigoAdmissao = entity.CodigoAdmissao,
      TemFicheiro = !string.IsNullOrWhiteSpace(entity.Ficheiro),
    };

    if (entity.UtentePedido is not null)
    {
      dto.Organismo = await ResolveOrganismoNomeFromCodinstAsync(entity.UtentePedido.Codinst);
    }

    var tableRow = new PedidoConsultaTableDTO
    {
      Codigo = dto.Codigo,
      CodigoEspecialidade = dto.CodigoEspecialidade,
      CodigoMedico = dto.CodigoMedico,
    };
    await HydrateLegacyNamesAsync([tableRow]);
    dto.Especialidade = tableRow.Especialidade;
    dto.Medico = tableRow.Medico;

    return dto;
  }

  private async Task HydrateLegacyNamesAsync(List<PedidoConsultaTableDTO> rows)
  {
    if (rows.Count == 0)
    {
      return;
    }

    foreach (PedidoConsultaTableDTO row in rows)
    {
      if (!string.IsNullOrWhiteSpace(row.CodigoMedico))
      {
        Response<MedicoLegadoResolveDTO> medicoRes =
          await _marcacoesAdministrativoService.ResolveMedicoLegadoAsync(row.CodigoMedico);
        if (medicoRes.Status == ResponseStatus.Success && medicoRes.Data != null)
        {
          row.Medico = medicoRes.Data.MedicoNome;
        }
      }
    }
  }

  private async Task<string?> ResolveOrganismoNomeFromCodinstAsync(int codinst)
  {
    string key = codinst.ToString();
    Organismo? org = (
      await _repository.GetListAsync<Organismo, Guid>(
        new OrganismoByCodigoClinicaSpecification(key)
      )
    ).FirstOrDefault();
    return org?.Nome ?? org?.NomeComercial;
  }

  private async Task<Guid?> ResolveOrganismoIdFromCodinstAsync(int codinst)
  {
    string key = codinst.ToString();
    Organismo? org = (
      await _repository.GetListAsync<Organismo, Guid>(
        new OrganismoByCodigoClinicaSpecification(key)
      )
    ).FirstOrDefault();
    return org?.Id;
  }

  private async Task<Guid?> ResolveClinicaIdAsync()
  {
    if (
      _currentClinicaService is not null
      && !string.IsNullOrWhiteSpace(_currentClinicaService.ClinicaId)
      && Guid.TryParse(_currentClinicaService.ClinicaId, out Guid parsed)
    )
    {
      return parsed;
    }

    Clinica? clinica = (
      await _repository.GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected())
    ).FirstOrDefault();
    return clinica?.Id;
  }
}
