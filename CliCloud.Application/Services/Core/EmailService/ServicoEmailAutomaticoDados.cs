using System.Globalization;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Core.EmailService;

public class ServicoEmailAutomaticoDados(IRepositoryAsync repository) : IServicoEmailAutomaticoDados
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<List<EmailAutomaticoEventoDTO>> ObterEventosConsultasAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var consultas = (await _repository.GetListAsync<ConsultaMarcacao, Guid>(cancellationToken: ct))
            .Where(x => x.Data.HasValue && x.Data.Value.Date == dataAlvo.Date)
            .ToList();
        return await ProjetarConsultasAsync(
            clinicaId,
            "1",
            "Consulta",
            "EmailAutomatico-Consultas",
            consultas,
            templateMensagem,
            ct);
    }

    public async Task<List<EmailAutomaticoEventoDTO>> ObterEventosPrimeiraSessaoAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var sessoes = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where(x => x.Data.HasValue && x.Data.Value.Date == dataAlvo.Date && (x.NumSessao ?? 0) == 1)
            .ToList();
        return await ProjetarSessoesAsync(clinicaId, "2.1", "Primeira Sessão", sessoes, templateMensagem, ct);
    }

    public async Task<List<EmailAutomaticoEventoDTO>> ObterEventosSessoesAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var sessoes = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where(x => x.Data.HasValue && x.Data.Value.Date == dataAlvo.Date && (x.NumSessao ?? 0) > 1)
            .ToList();
        return await ProjetarSessoesAsync(clinicaId, "2.2", "Sessões Tratamento", sessoes, templateMensagem, ct);
    }

    public async Task<List<EmailAutomaticoEventoDTO>> ObterEventosSessoesFinaisAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var sessoesDia = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where(x => x.Data.HasValue && x.Data.Value.Date == dataAlvo.Date && x.TratamentoId != Guid.Empty && (x.NumSessao ?? 0) > 0)
            .ToList();
        var maxPorTratamento = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where(x => x.TratamentoId != Guid.Empty && (x.NumSessao ?? 0) > 0)
            .GroupBy(x => x.TratamentoId)
            .ToDictionary(g => g.Key, g => g.Max(s => s.NumSessao ?? 0));

        var finais = sessoesDia.Where(s =>
        {
            var max = maxPorTratamento.TryGetValue(s.TratamentoId, out var m) ? m : int.MaxValue;
            return (s.NumSessao ?? 0) == max;
        }).ToList();

        return await ProjetarSessoesAsync(clinicaId, "2.3", "Última Sessão", finais, templateMensagem, ct);
    }

    public async Task<List<EmailAutomaticoEventoDTO>> ObterEventosAniversariosAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var utentes = (await _repository.GetListAsync<Utente, Guid>(cancellationToken: ct))
            .Where(x => x.DataNascimento.HasValue && x.DataNascimento.Value.Month == dataAlvo.Month && x.DataNascimento.Value.Day == dataAlvo.Day && !string.IsNullOrWhiteSpace(x.Email))
            .ToList();

        return utentes.Select(u =>
        {
            var p = CriarPlaceholdersBase();
            p["Utente"] = u.Nome ?? string.Empty;
            p["Data"] = dataAlvo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return new EmailAutomaticoEventoDTO
            {
                CodigoRegra = "3",
                ClinicaId = clinicaId,
                EmailDestino = u.Email!.Trim(),
                Assunto = "Aniversário",
                MensagemTemplate = templateMensagem,
                Placeholders = p,
                Modulo = "EmailAutomatico-Aniversarios"
            };
        }).ToList();
    }

    public Task<List<EmailAutomaticoEventoDTO>> ObterEventosPedidoAgendamentoAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
        => ObterEventosConsultasPorCodigoAsync(
            clinicaId,
            dataAlvo,
            "6.1",
            "Pedido Agendamento",
            "EmailAutomatico-PedidoAgendamento",
            templateMensagem,
            ct);

    public Task<List<EmailAutomaticoEventoDTO>> ObterEventosPedidoAgendamentoConfirmadoAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
        => ObterEventosConsultasPorCodigoAsync(
            clinicaId,
            dataAlvo,
            "6.2",
            "Pedido Agendamento Confirmado",
            "EmailAutomatico-PedidoAgendamentoConfirmado",
            templateMensagem,
            ct);

    public async Task<List<EmailAutomaticoEventoDTO>> ObterEventosAulasAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        // No domínio novo não existe entidade de "aulas"; usamos sessões como fallback funcional.
        var sessoes = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where(x => x.Data.HasValue && x.Data.Value.Date == dataAlvo.Date)
            .ToList();

        if (sessoes.Count == 0) return [];

        var tratamentoIds = sessoes.Select(x => x.TratamentoId).Distinct().ToList();
        var tratamentos = (await _repository.GetListAsync<Tratamento, Guid>(cancellationToken: ct))
            .Where(x => tratamentoIds.Contains(x.Id) && x.UtenteId.HasValue)
            .ToDictionary(x => x.Id);

        var utenteIds = tratamentos.Values
            .Where(x => x.UtenteId.HasValue)
            .Select(x => x.UtenteId!.Value)
            .Distinct()
            .ToList();
        var utentes = (await _repository.GetListAsync<Utente, Guid>(cancellationToken: ct))
            .Where(x => utenteIds.Contains(x.Id) && !string.IsNullOrWhiteSpace(x.Email))
            .ToDictionary(x => x.Id);

        var tecnicoIds = sessoes
            .Select(x => x.FisioterapeutaId)
            .Concat(tratamentos.Values.Select(x => x.FisioterapeutaId))
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .Distinct()
            .ToList();
        var tecnicos = (await _repository.GetListAsync<Tecnico, Guid>(cancellationToken: ct))
            .Where(x => tecnicoIds.Contains(x.Id))
            .ToDictionary(x => x.Id);

        var result = new List<EmailAutomaticoEventoDTO>();
        foreach (var sessao in sessoes)
        {
            if (!tratamentos.TryGetValue(sessao.TratamentoId, out var tratamento) || !tratamento.UtenteId.HasValue) continue;
            if (!utentes.TryGetValue(tratamento.UtenteId.Value, out var utente)) continue;

            var profissional = string.Empty;
            var profissionalId = sessao.FisioterapeutaId ?? tratamento.FisioterapeutaId;
            if (profissionalId.HasValue && tecnicos.TryGetValue(profissionalId.Value, out var tecnico))
                profissional = tecnico.Nome ?? string.Empty;

            var placeholders = CriarPlaceholdersBase();
            placeholders["Utente"] = utente.Nome ?? string.Empty;
            placeholders["Data"] = sessao.Data?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty;
            placeholders["Hora"] = sessao.HoraInic ?? string.Empty;
            placeholders["Profissional"] = profissional;
            placeholders["Modalidade"] = tratamento.Designacao ?? string.Empty;

            result.Add(new EmailAutomaticoEventoDTO
            {
                CodigoRegra = "7.1",
                ClinicaId = clinicaId,
                EmailDestino = utente.Email!.Trim(),
                Assunto = "Aula Modalidade",
                MensagemTemplate = templateMensagem,
                Placeholders = placeholders,
                Modulo = "EmailAutomatico-Aulas"
            });
        }

        return result;
    }

    private async Task<List<EmailAutomaticoEventoDTO>> ProjetarSessoesAsync(Guid clinicaId, string codigo, string assunto, List<SessaoTratamento> sessoes, string template, CancellationToken ct)
    {
        if (sessoes.Count == 0) return [];

        var tratamentoIds = sessoes.Select(x => x.TratamentoId).Distinct().ToList();
        var tratamentos = (await _repository.GetListAsync<Tratamento, Guid>(cancellationToken: ct))
            .Where(x => tratamentoIds.Contains(x.Id) && x.UtenteId.HasValue).ToDictionary(x => x.Id);
        var utenteIds = tratamentos.Values.Select(x => x.UtenteId!.Value).Distinct().ToList();
        var medicoIds = tratamentos.Values
            .Where(x => x.MedicoId.HasValue)
            .Select(x => x.MedicoId!.Value)
            .Distinct()
            .ToList();
        var utentes = (await _repository.GetListAsync<Utente, Guid>(cancellationToken: ct))
            .Where(x => utenteIds.Contains(x.Id) && !string.IsNullOrWhiteSpace(x.Email)).ToDictionary(x => x.Id);
        var medicos = (await _repository.GetListAsync<Medico, Guid>(cancellationToken: ct))
            .Where(x => medicoIds.Contains(x.Id))
            .ToDictionary(x => x.Id);

        var tecnicoIds = sessoes
            .Select(x => x.FisioterapeutaId)
            .Concat(tratamentos.Values.Select(x => x.FisioterapeutaId))
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .Distinct()
            .ToList();
        var tecnicos = (await _repository.GetListAsync<Tecnico, Guid>(cancellationToken: ct))
            .Where(x => tecnicoIds.Contains(x.Id))
            .ToDictionary(x => x.Id);

        var especialidadeIds = medicos.Values
            .Where(x => x.EspecialidadeId.HasValue)
            .Select(x => x.EspecialidadeId!.Value)
            .Distinct()
            .ToList();
        var especialidades = (await _repository.GetListAsync<Especialidade, Guid>(cancellationToken: ct))
            .Where(x => especialidadeIds.Contains(x.Id))
            .ToDictionary(x => x.Id);

        var result = new List<EmailAutomaticoEventoDTO>();
        foreach (var sessao in sessoes)
        {
            if (!tratamentos.TryGetValue(sessao.TratamentoId, out var tratamento)) continue;
            if (!utentes.TryGetValue(tratamento.UtenteId!.Value, out var utente)) continue;

            var medicoNome = string.Empty;
            var especialidadeNome = string.Empty;
            if (tratamento.MedicoId.HasValue && medicos.TryGetValue(tratamento.MedicoId.Value, out var medico))
            {
                medicoNome = medico.Nome ?? string.Empty;
                if (medico.EspecialidadeId.HasValue && especialidades.TryGetValue(medico.EspecialidadeId.Value, out var esp))
                    especialidadeNome = esp.Nome ?? string.Empty;
            }

            var fisioNome = string.Empty;
            var fisioId = sessao.FisioterapeutaId ?? tratamento.FisioterapeutaId;
            if (fisioId.HasValue && tecnicos.TryGetValue(fisioId.Value, out var fisio))
                fisioNome = fisio.Nome ?? string.Empty;

            var placeholders = CriarPlaceholdersBase();
            placeholders["Utente"] = utente.Nome ?? string.Empty;
            placeholders["Data"] = sessao.Data?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty;
            placeholders["Hora"] = sessao.HoraInic ?? string.Empty;
            placeholders["Medico"] = medicoNome;
            placeholders["Especialidade"] = especialidadeNome;
            placeholders["Fisioterapeuta"] = fisioNome;
            placeholders["Profissional"] = fisioNome;
            placeholders["Nsessao"] = (sessao.NumSessao ?? 0).ToString(CultureInfo.InvariantCulture);
            placeholders["NumSessao"] = (sessao.NumSessao ?? 0).ToString(CultureInfo.InvariantCulture);

            result.Add(new EmailAutomaticoEventoDTO
            {
                CodigoRegra = codigo,
                ClinicaId = clinicaId,
                EmailDestino = utente.Email!.Trim(),
                Assunto = assunto,
                MensagemTemplate = template,
                Placeholders = placeholders,
                Modulo = $"EmailAutomatico-{codigo}"
            });
        }
        return result;
    }

    private async Task<List<EmailAutomaticoEventoDTO>> ObterEventosConsultasPorCodigoAsync(
        Guid clinicaId,
        DateTime dataAlvo,
        string codigo,
        string assunto,
        string modulo,
        string templateMensagem,
        CancellationToken ct)
    {
        var consultas = (await _repository.GetListAsync<ConsultaMarcacao, Guid>(cancellationToken: ct))
            .Where(x => x.Data.HasValue && x.Data.Value.Date == dataAlvo.Date)
            .ToList();

        return await ProjetarConsultasAsync(clinicaId, codigo, assunto, modulo, consultas, templateMensagem, ct);
    }

    private async Task<List<EmailAutomaticoEventoDTO>> ProjetarConsultasAsync(
        Guid clinicaId,
        string codigo,
        string assunto,
        string modulo,
        List<ConsultaMarcacao> consultas,
        string template,
        CancellationToken ct)
    {
        if (consultas.Count == 0) return [];

        var utenteIds = consultas.Select(x => x.UtenteId).Distinct().ToList();
        var medicoIds = consultas
            .Where(x => x.MedicoId.HasValue)
            .Select(x => x.MedicoId!.Value)
            .Distinct()
            .ToList();
        var especialidadeIds = consultas
            .Where(x => x.EspecialidadeId.HasValue)
            .Select(x => x.EspecialidadeId!.Value)
            .Distinct()
            .ToList();

        var utentes = (await _repository.GetListAsync<Utente, Guid>(cancellationToken: ct))
            .Where(x => utenteIds.Contains(x.Id) && !string.IsNullOrWhiteSpace(x.Email))
            .ToDictionary(x => x.Id);
        var medicos = (await _repository.GetListAsync<Medico, Guid>(cancellationToken: ct))
            .Where(x => medicoIds.Contains(x.Id))
            .ToDictionary(x => x.Id);
        var especialidades = (await _repository.GetListAsync<Especialidade, Guid>(cancellationToken: ct))
            .Where(x => especialidadeIds.Contains(x.Id))
            .ToDictionary(x => x.Id);

        var result = new List<EmailAutomaticoEventoDTO>();
        foreach (var consulta in consultas)
        {
            if (!utentes.TryGetValue(consulta.UtenteId, out var utente)) continue;

            var medicoNome = string.Empty;
            if (consulta.MedicoId.HasValue && medicos.TryGetValue(consulta.MedicoId.Value, out var medico))
                medicoNome = medico.Nome ?? string.Empty;

            var especialidadeNome = string.Empty;
            if (consulta.EspecialidadeId.HasValue && especialidades.TryGetValue(consulta.EspecialidadeId.Value, out var esp))
                especialidadeNome = esp.Nome ?? string.Empty;

            var data = consulta.Data?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty;
            var hora = consulta.HoraMarcacao?.ToString(@"hh\:mm", CultureInfo.InvariantCulture) ?? string.Empty;

            var placeholders = CriarPlaceholdersBase();
            placeholders["Utente"] = utente.Nome ?? string.Empty;
            placeholders["Data"] = data;
            placeholders["Hora"] = hora;
            placeholders["Medico"] = medicoNome;
            placeholders["Especialidade"] = especialidadeNome;
            if (codigo == "6.2")
            {
                placeholders["HoraNova"] = hora;
                placeholders["DataNova"] = data;
            }

            result.Add(new EmailAutomaticoEventoDTO
            {
                CodigoRegra = codigo,
                ClinicaId = clinicaId,
                EmailDestino = utente.Email!.Trim(),
                Assunto = assunto,
                MensagemTemplate = template,
                Placeholders = placeholders,
                Modulo = modulo
            });
        }

        return result;
    }

    private static Dictionary<string, string> CriarPlaceholdersBase() => new(StringComparer.OrdinalIgnoreCase)
    {
        ["Utente"] = string.Empty,
        ["Hora"] = string.Empty,
        ["Data"] = string.Empty,
        ["Medico"] = string.Empty,
        ["Especialidade"] = string.Empty,
        ["Fisioterapeuta"] = string.Empty,
        ["Nsessao"] = string.Empty,
        ["NumSessao"] = string.Empty,
        ["Profissional"] = string.Empty,
        ["Modalidade"] = string.Empty,
        ["HoraAntiga"] = string.Empty,
        ["DataAntiga"] = string.Empty,
        ["HoraNova"] = string.Empty,
        ["DataNova"] = string.Empty
    };
}
