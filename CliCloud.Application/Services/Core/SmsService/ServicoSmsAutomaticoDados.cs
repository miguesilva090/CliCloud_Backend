using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Core.Sms;
using CliCloud.Application.Services.Core.SmsService.Specifications;
using CliCloud.Domain.Entities.Tecnicos;
using System.Globalization;

namespace CliCloud.Application.Services.Core.SmsService;

public class ServicoSmsAutomaticoDados ( IRepositoryAsync repository) : IServicoSmsAutomaticoDados
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<List<SmsAutomaticoEventoDTO>> ObterEventosConsultasAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var consultas = (await _repository.GetListAsync<ConsultaMarcacao, Guid>(cancellationToken: ct))
            .Where(x => x.Data != null && x.Data.Value.Date == dataAlvo.Date)
            .ToList();

        return await ProjetarConsultasAsync(clinicaId, "1", "SMSAutomatico-Consultas", consultas, templateMensagem, ct);
    }

    public async Task<List<SmsAutomaticoEventoDTO>> ObterEventosPrimeiraSessaoAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var sessoes = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where( x => x.Data.HasValue && x.Data.Value.Date == dataAlvo.Date && (x.NumSessao ?? 0) == 1) 
            .ToList();

        return await ProjetarSessoesAsync(clinicaId, "2.1", "SMSAutomatico-PrimeiraSessao", sessoes, templateMensagem, ct);
    }

    public async Task<List<SmsAutomaticoEventoDTO>> ObterEventosSessoesAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var sessoesDia = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where(x => x.TratamentoId != Guid.Empty && (x.NumSessao ?? 0) > 1)
            .ToList();

        var maxPorTratamento = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where( x => x.TratamentoId != Guid.Empty && (x.NumSessao ?? 0) > 0)
            .GroupBy(x => x.TratamentoId)
            .ToDictionary( g => g.Key, g => g.Max(s => s.NumSessao ?? 0));

        var sessoesIntermedias = sessoesDia
            .Where( s => 
            {
                var max = maxPorTratamento.TryGetValue(s.TratamentoId, out var maxSessao) ? maxSessao : int .MaxValue;
                return (s.NumSessao ?? 0) < max;
            })
            .ToList();

        return await ProjetarSessoesAsync(clinicaId, "2.2", "SMSAutomatico-Sessoes", sessoesIntermedias, templateMensagem, ct);
    }

    public async Task<List<SmsAutomaticoEventoDTO>> ObterEventosSessoesFinaisAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var sessoesDia = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where(x => x.Data.HasValue && x.Data.Value.Date == dataAlvo.Date && x.TratamentoId != Guid.Empty && (x.NumSessao ?? 0) > 0)
            .ToList();

        var maxPorTratamento = (await _repository.GetListAsync<SessaoTratamento, Guid>(cancellationToken: ct))
            .Where( x => x.TratamentoId != Guid.Empty && ( x.NumSessao ?? 0) > 0)
            .GroupBy( x => x.TratamentoId)
            .ToDictionary( g => g.Key, g => g.Max(s => s.NumSessao ?? 0));

        var finais = sessoesDia
            .Where(s =>
            {
                var max = maxPorTratamento.TryGetValue(s.TratamentoId, out var maxSessao) ? maxSessao : int.MaxValue;
                return max > 0 && (s.NumSessao ?? 0) == max;
            })
            .ToList();

        return await ProjetarSessoesAsync(clinicaId, "2.3", "SMSAutomatico-SessoesFinais", finais, templateMensagem, ct);
    }

    public async Task<List<SmsAutomaticoEventoDTO>> ObterEventosAniversariosAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
    {
        var utentes = (await _repository.GetListAsync<Utente, Guid>(cancellationToken: ct))
            .Where( x => x.DataNascimento.HasValue 
                && x.DataNascimento.Value.Month == dataAlvo.Month
                && x.DataNascimento.Value.Day == dataAlvo.Day)
            .ToList();

        var contactos = await _repository.GetListAsync<EntidadeContacto, Guid>(cancellationToken: ct);
        var contactosPorEntidade = contactos.GroupBy(x => x.EntidadeId).ToDictionary(g => g.Key, g => g.ToList());

        var lista = new List<SmsAutomaticoEventoDTO>();
        foreach( var utente in utentes )
        {
            var numero = ObterMelhorContacto(contactosPorEntidade, utente.Id);
            if( string.IsNullOrWhiteSpace(numero)) continue;

            var placeholders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Utente"] = utente.Nome,
                ["Data"] = dataAlvo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            };

            lista.Add(new SmsAutomaticoEventoDTO
            {
                CodigoRegra = "3",
                ClinicaId = clinicaId,
                NumeroDestino = numero, 
                MensagemTemplate = templateMensagem,
                Placeholders = placeholders,
                Modulo = "SMSAutomatico-Aniversarios"
            });
        }

        return lista;
    }

    public Task<List<SmsAutomaticoEventoDTO>> ObterEventosAulasAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct)
        => Task.FromResult(new List<SmsAutomaticoEventoDTO>());

    private async Task<List<SmsAutomaticoEventoDTO>> ProjetarConsultasAsync(
        Guid clinicaId, string codigoRegra, string modulo , List<ConsultaMarcacao> consultas, string template, CancellationToken ct)
        {
            if( consultas.Count == 0) return [];

            var utenteIds = consultas.Select(x => x.UtenteId).Distinct().ToList();
            var medicoIds = consultas.Where(x => x.MedicoId.HasValue).Select(x => x.MedicoId!.Value).Distinct().ToList();
            var especialidadeIds = consultas.Where( x => x.EspecialidadeId.HasValue).Select(x => x.EspecialidadeId!.Value).Distinct().ToList();

        var cfgConsulta = (await _repository.GetListAsync<ConfiguracaoSmsAutomatica, Guid>(
            new ConfiguracaoSmsAutomaticaPorClinicaSpec(clinicaId, "1"), ct)).FirstOrDefault();
        var medicosSelecionados = (await _repository.GetListAsync<ConfiguracaoSmsAutomaticaMedico, Guid>(
            new ConfiguracaoSmsAutomaticaMedicoSpec(clinicaId, "1"), ct))
            .Select(x => x.CodigoMedico)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var utentes = (await _repository.GetListAsync<Utente, Guid>(cancellationToken: ct))
                .Where(x => utenteIds.Contains(x.Id)).ToDictionary( x => x.Id);
            var medicos = (await _repository.GetListAsync<Medico, Guid>(cancellationToken: ct))
                .Where(x => medicoIds.Contains(x.Id)).ToDictionary( x => x.Id);
            var especialidades = (await _repository.GetListAsync<Especialidade, Guid>(cancellationToken: ct))
                .Where(x => especialidadeIds.Contains(x.Id)).ToDictionary( x => x.Id);

            var contactos = await _repository.GetListAsync<EntidadeContacto, Guid>(cancellationToken: ct);
            var contactosPorEntidade = contactos.GroupBy( x => x.EntidadeId).ToDictionary(g => g.Key, g=> g.ToList());

            var lista = new List<SmsAutomaticoEventoDTO>();
            foreach( var consulta in consultas )
            {
                if(!utentes.TryGetValue(consulta.UtenteId, out var utente)) continue;

                var numero = ObterMelhorContacto(contactosPorEntidade, utente.Id);
                if( string.IsNullOrWhiteSpace(numero)) continue;

                var medicoNome = string.Empty;
                var medicoCodigo = string.Empty;
                if(consulta.MedicoId.HasValue && medicos.TryGetValue(consulta.MedicoId.Value, out var medico ))
                {
                    medicoNome = medico.Nome;
                    medicoCodigo = medico.Letra ?? string.Empty;
                }

                if (cfgConsulta is not null && !cfgConsulta.TodosMedicos)
                {
                    if (string.IsNullOrWhiteSpace(medicoCodigo) || !medicosSelecionados.Contains(medicoCodigo))
                    {
                        continue;
                    }
                }

                var especialidade = string.Empty;
                if(consulta.EspecialidadeId.HasValue && especialidades.TryGetValue(consulta.EspecialidadeId.Value, out var esp))
                    especialidade = esp.Nome;

                var placeholders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Utente"] = utente.Nome, 
                    ["Data"] = consulta.Data?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty,
                    ["Hora"] = consulta.HoraMarcacao?.ToString(@"hh\:mm", CultureInfo.InvariantCulture) ?? string.Empty,
                    ["Medico"] = medicoNome, 
                    ["Especialidade"] = especialidade,
                    ["Fisioterapeuta"] = string.Empty,
                    ["Nsessao"] = string.Empty
                };

                lista.Add(new SmsAutomaticoEventoDTO
                {
                    CodigoRegra = codigoRegra,
                    ClinicaId = clinicaId,
                    NumeroDestino = numero, 
                    MensagemTemplate = template, 
                    Placeholders = placeholders,
                    Modulo = modulo, 
                    CodigoMedico = string.IsNullOrWhiteSpace(medicoCodigo) ? null : medicoCodigo,
                });
            }

            return lista;
        }

        private async Task<List<SmsAutomaticoEventoDTO>> ProjetarSessoesAsync(
            Guid clinicaId, string codigoRegra, string modulo, List<SessaoTratamento> sessoes, string template, CancellationToken ct)
            {
                if ( sessoes.Count == 0) return [];

                var tratamentoIds = sessoes.Select( x => x.TratamentoId).Distinct().ToList();
                var tratamentos = (await _repository.GetListAsync<Tratamento, Guid>(cancellationToken: ct))
                    .Where( x => tratamentoIds.Contains(x.Id)).ToDictionary(x => x.Id);

                var utenteIds = tratamentos.Values.Where( x => x.UtenteId.HasValue).Select( x => x.UtenteId!.Value).Distinct().ToList();
                var medicoIds = tratamentos.Values.Where( x => x.MedicoId.HasValue).Select( x => x.MedicoId!.Value).Distinct().ToList();

                var utentes = (await _repository.GetListAsync<Utente, Guid>(cancellationToken: ct))
                    .Where( x => utenteIds.Contains(x.Id)).ToDictionary(x => x.Id);
                var medicos = (await _repository.GetListAsync<Medico, Guid>(cancellationToken: ct))
                    .Where( x => medicoIds.Contains(x.Id)).ToDictionary(x => x.Id);

                var fisioterapeutaIds = sessoes
                    .Select(x => x.FisioterapeutaId)
                    .Concat(tratamentos.Values.Select(x => x.FisioterapeutaId))
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .Distinct()
                    .ToList();

                var tecnicos = (await _repository.GetListAsync<Tecnico, Guid>(cancellationToken: ct))
                    .Where(x => fisioterapeutaIds.Contains(x.Id))
                    .ToDictionary(x => x.Id);

                var contactos = await _repository.GetListAsync<EntidadeContacto, Guid>(cancellationToken: ct);
                var contactosPorEntidade = contactos.GroupBy( x => x.EntidadeId).ToDictionary( g => g.Key, g => g.ToList());

                var lista = new List<SmsAutomaticoEventoDTO>();
                foreach( var sessao in sessoes ) 
                {
                    if (!tratamentos.TryGetValue(sessao.TratamentoId, out var tratamento) || !tratamento.UtenteId.HasValue) continue;
                    if (!utentes.TryGetValue(tratamento.UtenteId.Value, out var utente)) continue;

                    var numero = ObterMelhorContacto(contactosPorEntidade, utente.Id);
                    if( string.IsNullOrWhiteSpace(numero)) continue;

                    var medicoNome = string.Empty;
                    var medicoCodigo = string.Empty;

                    if( tratamento.MedicoId.HasValue && medicos.TryGetValue(tratamento.MedicoId.Value, out var medico ))
                    {
                        medicoNome = medico.Nome;
                        medicoCodigo = medico.Letra ?? string.Empty;
                    }

                string fisioNome = string.Empty;
                var fisioId = sessao.FisioterapeutaId ?? tratamento.FisioterapeutaId;
                if (fisioId.HasValue && tecnicos.TryGetValue(fisioId.Value, out var fisio))
                    fisioNome = fisio.Nome;

                    var placeholders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        ["Utente"] = utente.Nome,
                        ["Data"] = sessao.Data?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty,
                        ["Hora"] = sessao.HoraInic ?? string.Empty,
                        ["Medico"] = medicoNome,
                        ["Fisioterapeuta"] = fisioNome,
                        ["NumSessao"] = (sessao.NumSessao ?? 0).ToString(CultureInfo.InvariantCulture),
                        ["Nsessao"] = (sessao.NumSessao ?? 0).ToString(CultureInfo.InvariantCulture),
                    };

                    lista.Add( new SmsAutomaticoEventoDTO 
                    {
                        CodigoRegra = codigoRegra, 
                        ClinicaId = clinicaId,
                        NumeroDestino = numero, 
                        MensagemTemplate = template, 
                        Placeholders = placeholders,
                        Modulo = modulo, 
                        CodigoMedico = string.IsNullOrWhiteSpace(medicoCodigo) ? null : medicoCodigo,
                    });
                }

                return lista;
            }
    
        private static string ObterMelhorContacto(Dictionary<Guid, List<EntidadeContacto>> contactosPorEntidade, Guid entidadeId)
        {
            if(!contactosPorEntidade.TryGetValue(entidadeId, out var contactos)) return string.Empty;

            var principal = contactos.FirstOrDefault(x => x.Principal && !string.IsNullOrWhiteSpace(x.Valor));
            if (!string.IsNullOrWhiteSpace(principal?.Valor)) return principal.Valor!;

            var qualquer = contactos.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Valor));
            return qualquer?.Valor ?? string.Empty;
        }
}