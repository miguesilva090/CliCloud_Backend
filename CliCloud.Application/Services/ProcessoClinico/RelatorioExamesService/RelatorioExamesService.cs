using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Application.Services.Core.EmailService;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.Specifications;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioExames;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService
{
    public class RelatorioExamesService : IRelatorioExamesService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguracaoEmailService _configuracaoEmailService;

        public RelatorioExamesService(IRepositoryAsync repository, IMapper mapper, IConfiguracaoEmailService configuracaoEmailService)
        {
            _repository = repository;
            _mapper = mapper;
            _configuracaoEmailService = configuracaoEmailService;
        }

        public async Task<Response<RelatorioExamesDTO?>> GetByUtenteAndMedicoAsync(Guid utenteId, Guid medicoId)
        {
            // O médico efetivo é o associado ao utente; o parametro medicoId (utilizador logado) pode não corresponder ao Medicos.Medico.
            var utente = await _repository.GetByIdAsync<Utente, Guid>(utenteId);
            if (utente?.MedicoId is null)
            {
                return ResponseFactory.Success<RelatorioExamesDTO?>(null);
            }

            var spec = new RelatorioExamesByUtenteMedicoSpec(utenteId, utente.MedicoId.Value);
            var list = await _repository.GetListAsync<RelatorioExames, RelatorioExamesDTO, Guid>(spec);
            var dto = list.FirstOrDefault();
            return ResponseFactory.Success<RelatorioExamesDTO?>(dto);
        }

        public async Task<Response<Guid>> CreateRelatorioExamesAsync(CreateRelatorioExamesRequest request, Guid medicoId)
        {
            // Determinar o médico a partir do utente para garantir integridade da FK
            var utente = await _repository.GetByIdAsync<Utente, Guid>(request.UtenteId);
            if (utente?.MedicoId is null)
            {
                return ResponseFactory.Fail<Guid>("Médico não associado ao utente.");
            }

            var entity = new RelatorioExames
            {
                Id = Guid.NewGuid(),
                UtenteId = request.UtenteId,
                MedicoId = utente.MedicoId.Value,
                Texto = request.Texto,
            };

            await _repository.CreateAsync<RelatorioExames, Guid>(entity);
            await _repository.SaveChangesAsync();
            await TentarDispararEmailFluxoAsync(entity);

            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> UpdateRelatorioExamesAsync(UpdateRelatorioExamesRequest request, Guid medicoId)
        {
            var utente = await _repository.GetByIdAsync<Utente, Guid>(request.UtenteId);
            if (utente?.MedicoId is null)
            {
                return ResponseFactory.Fail<Guid>("Médico não associado ao utente.");
            }

            var spec = new RelatorioExamesByUtenteMedicoSpec(request.UtenteId, utente.MedicoId.Value);
            var list = await _repository.GetListAsync<RelatorioExames, Guid>(spec);
            var existing = list.FirstOrDefault();

            if (existing is null)
            {
                var create = new CreateRelatorioExamesRequest
                {
                    UtenteId = request.UtenteId,
                    Texto = request.Texto,
                };
                return await CreateRelatorioExamesAsync(create, medicoId);
            }

            existing.Texto = request.Texto;

            await _repository.UpdateAsync<RelatorioExames, Guid>(existing);
            await _repository.SaveChangesAsync();
            await TentarDispararEmailFluxoAsync(existing);

            return ResponseFactory.Success(existing.Id);
        }

        private async Task TentarDispararEmailFluxoAsync(RelatorioExames relatorio)
        {
            try
            {
                var clinica = (await _repository.GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected())).FirstOrDefault();
                if (clinica is null) return;

                var utente = await _repository.GetByIdAsync<Utente, Guid>(relatorio.UtenteId);
                if (utente is null || string.IsNullOrWhiteSpace(utente.Email)) return;

                string medicoNome = string.Empty;
                var medico = await _repository.GetByIdAsync<Medico, Guid>(relatorio.MedicoId);
                if (medico is not null) medicoNome = medico.Nome ?? string.Empty;

                var emailRequest = new EnviarEmailPorCodigoRequest
                {
                    CodigoConfiguracao = "8.4",
                    EmailDestino = utente.Email.Trim(),
                    NomeUtente = utente.Nome ?? string.Empty,
                    NomeMedicoOuProfissional = medicoNome,
                    NomeEspecialidade = "Relatórios",
                    Data = DateTime.Today,
                    Modulo = "RelatorioExames",
                };

                _ = await _configuracaoEmailService.EnviarEmailPorCodigoAsync(clinica.Id, emailRequest);
            }
            catch
            {
                // Não bloquear o fluxo principal por falha de Email.
            }
        }
    }
}

