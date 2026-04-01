using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ChamadaUtentesService.DTOs;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Core.ChamadaUtentesService
{
  public class ChamadaUtentesService(IRepositoryAsync repository) : IChamadaUtentesService
  {
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ChamadaUtenteDadosDTO>> ObterDadosChamadaConsultaAsync(
      Guid clinicaId,
      Guid marcacaoConsultaId,
      bool chamarOutraVez
    )
    {
      try
      {
        var marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(marcacaoConsultaId);
        if (marcacao == null)
          return ResponseFactory.Fail<ChamadaUtenteDadosDTO>("Marcação de consulta não encontrada.");

        var nomeUtente = await ObterNomeUtenteAsync(marcacao.UtenteId);
        var nomeProfissional = await ObterNomeProfissionalConsultaAsync(marcacao);

        var estado = await ObterEstadoExistenteAsync(clinicaId, "Consulta", marcacaoConsultaId);
        if (estado.ExisteChamadaAtiva || (estado.ExisteChamadaFeita && !chamarOutraVez))
          return ResponseFactory.Success(new ChamadaUtenteDadosDTO
          {
            Tipo = "Consulta",
            ReferenciaId = marcacaoConsultaId,
            NomeUtente = nomeUtente,
            NomeProfissional = nomeProfissional,
            Sala = null,
            Senha = marcacao.NumDestacavel,
            ExisteChamadaAtiva = estado.ExisteChamadaAtiva,
            ExisteChamadaFeita = estado.ExisteChamadaFeita,
          });

        return ResponseFactory.Success(new ChamadaUtenteDadosDTO
        {
          Tipo = "Consulta",
          ReferenciaId = marcacaoConsultaId,
          NomeUtente = nomeUtente,
          NomeProfissional = nomeProfissional,
          Sala = null,
          Senha = marcacao.NumDestacavel,
          ExisteChamadaAtiva = false,
          ExisteChamadaFeita = false,
        });
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ChamadaUtenteDadosDTO>(ex.Message);
      }
    }

    public async Task<Response<ChamadaUtenteDadosDTO>> ObterDadosChamadaTratamentoAsync(
      Guid clinicaId,
      Guid sessaoTratamentoId,
      bool chamarOutraVez
    )
    {
      try
      {
        var sessao = await _repository.GetByIdAsync<SessaoTratamento, Guid>(sessaoTratamentoId);
        if (sessao == null)
          return ResponseFactory.Fail<ChamadaUtenteDadosDTO>("Sessão de tratamento não encontrada.");

        var tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(sessao.TratamentoId);
        if (tratamento == null)
          return ResponseFactory.Fail<ChamadaUtenteDadosDTO>("Tratamento não encontrado.");

        var nomeUtente = tratamento.UtenteId.HasValue
          ? await ObterNomeUtenteAsync(tratamento.UtenteId.Value)
          : string.Empty;

        var nomeTecnico = await ObterNomeTecnicoTratamentoAsync(sessao, tratamento);
        var estado = await ObterEstadoExistenteAsync(clinicaId, "Tratamento", sessaoTratamentoId);

        if (estado.ExisteChamadaAtiva || (estado.ExisteChamadaFeita && !chamarOutraVez))
          return ResponseFactory.Success(new ChamadaUtenteDadosDTO
          {
            Tipo = "Tratamento",
            ReferenciaId = sessaoTratamentoId,
            NomeUtente = nomeUtente,
            NomeProfissional = nomeTecnico,
            Sala = tratamento.LocalTratamento?.Designacao,
            Senha = sessao.NumDestacavel,
            ExisteChamadaAtiva = estado.ExisteChamadaAtiva,
            ExisteChamadaFeita = estado.ExisteChamadaFeita,
          });

        return ResponseFactory.Success(new ChamadaUtenteDadosDTO
        {
          Tipo = "Tratamento",
          ReferenciaId = sessaoTratamentoId,
          NomeUtente = nomeUtente,
          NomeProfissional = nomeTecnico,
          Sala = tratamento.LocalTratamento?.Designacao,
          Senha = sessao.NumDestacavel,
          ExisteChamadaAtiva = false,
          ExisteChamadaFeita = false,
        });
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ChamadaUtenteDadosDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> ChamarUtenteConsultaAsync(
      Guid clinicaId,
      Guid marcacaoConsultaId,
      ChamarConsultaRequest request
    )
    {
      try
      {
        var marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(marcacaoConsultaId);
        if (marcacao == null)
          return ResponseFactory.Fail<Guid>("Marcação de consulta não encontrada.");

        var estado = await ObterEstadoExistenteAsync(clinicaId, "Consulta", marcacaoConsultaId);
        if (estado.ExisteChamadaAtiva)
          return ResponseFactory.Fail<Guid>("Já existe uma chamada ativa para esta consulta.");

        var chamada = new ChamadaUtente
        {
          ClinicaId = clinicaId,
          Tipo = "Consulta",
          ReferenciaId = marcacaoConsultaId,
          UtenteId = marcacao.UtenteId,
          NomeUtente = await ObterNomeUtenteAsync(marcacao.UtenteId),
          NomeProfissional = await ObterNomeProfissionalConsultaAsync(marcacao),
          Sala = string.IsNullOrWhiteSpace(request.Sala) ? null : request.Sala.Trim(),
          Senha = marcacao.NumDestacavel,
          DataHoraChamada = DateTime.Now,
          Estado = 0,
        };

        var created = await _repository.CreateAsync<ChamadaUtente, Guid>(chamada);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> ChamarUtenteTratamentoAsync(
      Guid clinicaId,
      Guid sessaoTratamentoId,
      ChamarSessaoTratamentoRequest request
    )
    {
      try
      {
        var sessao = await _repository.GetByIdAsync<SessaoTratamento, Guid>(sessaoTratamentoId);
        if (sessao == null)
          return ResponseFactory.Fail<Guid>("Sessão de tratamento não encontrada.");

        var tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(sessao.TratamentoId);
        if (tratamento == null)
          return ResponseFactory.Fail<Guid>("Tratamento não encontrado.");

        var estado = await ObterEstadoExistenteAsync(clinicaId, "Tratamento", sessaoTratamentoId);
        if (estado.ExisteChamadaAtiva)
          return ResponseFactory.Fail<Guid>("Já existe uma chamada ativa para esta sessão.");

        var nomeTecnico = string.IsNullOrWhiteSpace(request.NomeTecnico)
          ? await ObterNomeTecnicoTratamentoAsync(sessao, tratamento)
          : request.NomeTecnico.Trim();

        var chamada = new ChamadaUtente
        {
          ClinicaId = clinicaId,
          Tipo = "Tratamento",
          ReferenciaId = sessaoTratamentoId,
          UtenteId = tratamento.UtenteId,
          NomeUtente = tratamento.UtenteId.HasValue
            ? await ObterNomeUtenteAsync(tratamento.UtenteId.Value)
            : string.Empty,
          NomeProfissional = nomeTecnico,
          Sala = string.IsNullOrWhiteSpace(request.Sala) ? tratamento.LocalTratamento?.Designacao : request.Sala.Trim(),
          Senha = sessao.NumDestacavel,
          DataHoraChamada = DateTime.Now,
          Estado = 0,
        };

        var created = await _repository.CreateAsync<ChamadaUtente, Guid>(chamada);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<bool>> AtualizarEstadoAsync(Guid clinicaId, Guid chamadaId, int estado)
    {
      try
      {
        var chamada = await _repository.GetByIdAsync<ChamadaUtente, Guid>(chamadaId);
        if (chamada == null || chamada.ClinicaId != clinicaId)
          return ResponseFactory.Fail<bool>("Chamada não encontrada.");

        chamada.Estado = estado;
        _ = await _repository.UpdateAsync<ChamadaUtente, Guid>(chamada);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(true);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<bool>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<ChamadaUtenteFilaItemDTO>>> ObterChamadasAsync(Guid clinicaId, string? tipo = null)
    {
      try
      {
        var list = await _repository.GetListAsync<ChamadaUtente, Guid>();
        var query = list
          .Where(x => x.ClinicaId == clinicaId)
          .Where(x => string.IsNullOrWhiteSpace(tipo) || string.Equals(x.Tipo, tipo, StringComparison.OrdinalIgnoreCase))
          .OrderByDescending(x => x.DataHoraChamada)
          .Take(100)
          .Select(x => new ChamadaUtenteFilaItemDTO
          {
            Id = x.Id,
            Tipo = x.Tipo,
            ReferenciaId = x.ReferenciaId,
            NomeUtente = x.NomeUtente,
            NomeProfissional = x.NomeProfissional,
            Sala = x.Sala,
            Senha = x.Senha,
            DataHoraChamada = x.DataHoraChamada,
            Estado = x.Estado,
          })
          .ToList();

        return ResponseFactory.Success<IEnumerable<ChamadaUtenteFilaItemDTO>>(query);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<ChamadaUtenteFilaItemDTO>>(ex.Message);
      }
    }

    private async Task<(bool ExisteChamadaAtiva, bool ExisteChamadaFeita)> ObterEstadoExistenteAsync(
      Guid clinicaId,
      string tipo,
      Guid referenciaId
    )
    {
      var chamadas = await _repository.GetListAsync<ChamadaUtente, Guid>();
      var scoped = chamadas.Where(x =>
        x.ClinicaId == clinicaId
        && string.Equals(x.Tipo, tipo, StringComparison.OrdinalIgnoreCase)
        && x.ReferenciaId == referenciaId
      );

      return (scoped.Any(x => x.Estado == 0), scoped.Any(x => x.Estado == 1));
    }

    private async Task<string> ObterNomeUtenteAsync(Guid utenteId)
    {
      var ut = await _repository.GetByIdAsync<Utente, Guid>(utenteId);
      return ut?.Nome ?? string.Empty;
    }

    private async Task<string?> ObterNomeProfissionalConsultaAsync(ConsultaMarcacao marcacao)
    {
      if (marcacao.MedicoId.HasValue)
      {
        var medico = await _repository.GetByIdAsync<Medico, Guid>(marcacao.MedicoId.Value);
        if (!string.IsNullOrWhiteSpace(medico?.Nome))
          return medico.Nome;
      }

      if (marcacao.TecnicoId.HasValue)
      {
        var tecnico = await _repository.GetByIdAsync<Tecnico, Guid>(marcacao.TecnicoId.Value);
        if (!string.IsNullOrWhiteSpace(tecnico?.Nome))
          return tecnico.Nome;
      }

      return null;
    }

    private async Task<string?> ObterNomeTecnicoTratamentoAsync(SessaoTratamento sessao, Tratamento tratamento)
    {
      if (sessao.FisioterapeutaId.HasValue)
      {
        var tecnico = await _repository.GetByIdAsync<Tecnico, Guid>(sessao.FisioterapeutaId.Value);
        if (!string.IsNullOrWhiteSpace(tecnico?.Nome))
          return tecnico.Nome;
      }

      if (sessao.AuxiliarId.HasValue)
      {
        var tecnico = await _repository.GetByIdAsync<Tecnico, Guid>(sessao.AuxiliarId.Value);
        if (!string.IsNullOrWhiteSpace(tecnico?.Nome))
          return tecnico.Nome;
      }

      if (sessao.OutroTecnicoId.HasValue)
      {
        var tecnico = await _repository.GetByIdAsync<Tecnico, Guid>(sessao.OutroTecnicoId.Value);
        if (!string.IsNullOrWhiteSpace(tecnico?.Nome))
          return tecnico.Nome;
      }

      if (tratamento.FisioterapeutaId.HasValue)
      {
        var tecnico = await _repository.GetByIdAsync<Tecnico, Guid>(tratamento.FisioterapeutaId.Value);
        if (!string.IsNullOrWhiteSpace(tecnico?.Nome))
          return tecnico.Nome;
      }

      return null;
    }
  }
}
