using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.Specifications;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioExames;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService
{
    public class RelatorioExamesService : IRelatorioExamesService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public RelatorioExamesService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

            return ResponseFactory.Success(existing.Id);
        }
    }
}

