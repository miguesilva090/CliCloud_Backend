using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Services.Core.SmsService.Specifications;
using CliCloud.Domain.Entities.Core.Sms;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Utility;
using System.Globalization;

namespace CliCloud.Application.Services.Core.SmsService
{
    public class ServicoWebhookSms(IRepositoryAsync repository) : IServicoWebhookSms
    {
        private readonly IRepositoryAsync _repository = repository;

        public async Task ProcessarEntregueAsync(IEnumerable<WebhookEstadoSmsDTO> payload)
            => await ProcessarEstadoAsync(payload, "Entregue", true);

        public async Task ProcessarNaoEntregueAsync(IEnumerable<WebhookEstadoSmsDTO> payload)
            => await ProcessarEstadoAsync(payload, "Não Entregue", true);

        public async Task ProcessarPendenteAsync(IEnumerable<WebhookEstadoSmsDTO> payload)
            => await ProcessarEstadoAsync(payload, "Pendente", false);

        public async Task ProcessarInboundAsync(IEnumerable<WebhookInboundSmsDTO> payload)
        {
            var utentes = await _repository.GetListAsync<Utente, Guid>();
            var contactos = await _repository.GetListAsync<EntidadeContacto, Guid>();

            foreach (var item in payload)
            {
                Guid? organizationId = null;
                if (!string.IsNullOrWhiteSpace(item.OrganizationId)
                    && Guid.TryParse(item.OrganizationId, out var orgId))
                {
                    organizationId = orgId;
                }

                var clinicaId = await ResolverClinicaIdInboundAsync(organizationId);
                var numeroOrigemNormalizado = NormalizarNumeroTelemovel(item.From);
                var codigoUtente = TentarObterCodigoUtentePorNumero(numeroOrigemNormalizado, utentes, contactos);

                var entidade = new SmsRecebido
                {
                    ClinicaId = clinicaId,
                    OrganizacaoId = organizationId,
                    DataHoraRecebimento = item.ReceiveDateTime,
                    NumeroOrigem = item.From ?? string.Empty,
                    NumeroDestino = item.To ?? string.Empty,
                    Keyword = item.Keyword ?? string.Empty,
                    TextoMensagem = item.Text ?? string.Empty,
                    Encoding = item.Encoding,
                    Mcc = item.Mcc ?? string.Empty,
                    Mnc = item.Mnc ?? string.Empty,
                    TotalSegmentos = item.TotalSegments,
                    DataHoraProcessamento = DateTime.Now,
                    CodigoUtente = codigoUtente
                };

                await _repository.CreateAsync<SmsRecebido, Guid>(entidade);
            }

            await _repository.SaveChangesAsync();
        }

        private async Task ProcessarEstadoAsync(IEnumerable<WebhookEstadoSmsDTO> payload, string estadoFinal, bool permitirSobrescrever)
        {
            foreach (var item in payload)
            {
                if (string.IsNullOrWhiteSpace(item.CustomPayload))
                    continue;

                if (!Guid.TryParse(item.CustomPayload, out var idMensagem))
                    continue;

                var spec = new HistoricoSmsPorIdMensagemSpec(idMensagem);
                var lista = await _repository.GetListAsync<HistoricoSms, Guid>(spec);
                var historico = lista.FirstOrDefault();
                if (historico == null)
                    continue;

                if (!permitirSobrescrever
                    && !string.Equals(historico.Status, "Pendente", StringComparison.OrdinalIgnoreCase))
                    continue;

                historico.Status = estadoFinal;
                historico.DataHoraEnvio = item.ReportDateTime ?? DateTime.Now;
                historico.MensagemErro = string.IsNullOrWhiteSpace(item.ReportDescription)
                    ? historico.MensagemErro
                    : item.ReportDescription;

                _ = await _repository.UpdateAsync<HistoricoSms, Guid>(historico);
            }

            await _repository.SaveChangesAsync();
        }

        private async Task<Guid> ResolverClinicaIdInboundAsync(Guid? organizationId)
        {
            if (!organizationId.HasValue)
                return Guid.Empty;

            var cfgs = await _repository.GetListAsync<ConfiguracaoSms, Guid>();
            var cfg = cfgs.FirstOrDefault(x => x.ArpooneOrganizationID == organizationId.Value);
            if (cfg != null)
                return cfg.ClinicaId;

            return organizationId.Value;
        }

        private static int? TentarObterCodigoUtentePorNumero(
            string numeroOrigemNormalizado,
            IEnumerable<Utente> utentes,
            IEnumerable<EntidadeContacto> contactos
        )
        {
            if (string.IsNullOrWhiteSpace(numeroOrigemNormalizado))
                return null;

            var candidatos = GerarCandidatosNumero(numeroOrigemNormalizado);
            if (candidatos.Count == 0)
                return null;

            var contactosMatch = contactos
                .Where(c => c.EntidadeContactoTipoId == 1 || c.EntidadeContactoTipoId == 2)
                .Select(c => new
                {
                    c.EntidadeId,
                    Numero = NormalizarNumeroTelemovel($"{c.Indicativo}{c.Valor}")
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Numero) && candidatos.Contains(x.Numero))
                .ToList();

            if (contactosMatch.Count == 0)
                return null;

            var utentesPorId = utentes.ToDictionary(u => u.Id, u => u);

            foreach (var match in contactosMatch)
            {
                if (!utentesPorId.TryGetValue(match.EntidadeId, out var utente))
                    continue;

                if (TentarExtrairCodigoUtente(utente, out var codigo))
                    return codigo;
            }

            return null;
        }

        private static bool TentarExtrairCodigoUtente(Utente utente, out int codigo)
        {
            codigo = 0;
            var numeroUtente = (utente.NumeroUtente ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(numeroUtente))
                return false;

            var apenasDigitos = new string(numeroUtente.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(apenasDigitos))
                return false;

            return int.TryParse(apenasDigitos, NumberStyles.None, CultureInfo.InvariantCulture, out codigo);
        }

        private static string NormalizarNumeroTelemovel(string? numero)
        {
            var apenasDigitos = new string((numero ?? string.Empty).Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(apenasDigitos))
                return string.Empty;

            if (apenasDigitos.StartsWith("351", StringComparison.Ordinal) && apenasDigitos.Length == 12)
                return apenasDigitos;

            if (apenasDigitos.StartsWith('0') && apenasDigitos.Length == 10)
                return $"351{apenasDigitos[1..]}";

            if (apenasDigitos.Length == 9)
                return $"351{apenasDigitos}";

            return apenasDigitos;
        }

        private static HashSet<string> GerarCandidatosNumero(string numeroNormalizado)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(numeroNormalizado))
                return set;

            set.Add(numeroNormalizado);

            if (numeroNormalizado.StartsWith("351", StringComparison.Ordinal) && numeroNormalizado.Length == 12)
            {
                var semPrefixo = numeroNormalizado[3..];
                set.Add(semPrefixo);
                if (!semPrefixo.StartsWith("0", StringComparison.Ordinal))
                    set.Add($"0{semPrefixo}");
            }
            else if (numeroNormalizado.Length == 9)
            {
                set.Add($"351{numeroNormalizado}");
                set.Add($"0{numeroNormalizado}");
            }
            else if (numeroNormalizado.StartsWith("0", StringComparison.Ordinal) && numeroNormalizado.Length == 10)
            {
                set.Add($"351{numeroNormalizado[1..]}");
                set.Add(numeroNormalizado[1..]);
            }

            return set;
        }
    }
}