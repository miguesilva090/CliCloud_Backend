using AutoMapper;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.DTOs;
using CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.Specifications;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService
{
    public class UtentePatologiaComparticipacaoService : IUtentePatologiaComparticipacaoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public UtentePatologiaComparticipacaoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<UtentePatologiaComparticipacaoDTO>>> GetByUtenteIdAsync(
            Guid utenteId)
        {
            try
            {
                var specification = new UtentePatologiaComparticipacaoByUtenteId(utenteId);
                var list = await _repository
                    .GetListAsync<UtentePatologiaComparticipacao, UtentePatologiaComparticipacaoDTO, Guid>(
                        specification
                    );
                return ResponseFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<UtentePatologiaComparticipacaoDTO>>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateAsync(CreateUtentePatologiaComparticipacaoRequest request)
        {
            var existsSpec = new UtentePatologiaComparticipacaoByUtenteAndCodigo(
                request.UtenteId,
                request.CodigoComparticipacao
            );

            if (await _repository.ExistsAsync<UtentePatologiaComparticipacao, Guid>(existsSpec))
            {
                return ResponseFactory.Fail<Guid>(
                    "O utente já tem associada a patologia indicada");
            }

            var entity = _mapper.Map(request, new UtentePatologiaComparticipacao());

            try
            {
                var created = await _repository
                    .CreateAsync<UtentePatologiaComparticipacao, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repository
                    .GetByIdAsync<UtentePatologiaComparticipacao, Guid>(id);
                if (entity is null)
                    return ResponseFactory.Fail<Guid>("Patologia de comparticipação não encontrada");

                await _repository.RemoveByIdAsync<UtentePatologiaComparticipacao, Guid>(id);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<UtentePatologiaComparticipacaoDTO>>> ReplaceByUtenteAsync(
            ReplaceUtentePatologiasComparticipacaoRequest request)
        {
            try
            {
                var desired = request.Items
                    .GroupBy(x => x.CodigoComparticipacao)
                    .Select(g => g.First())
                    .ToList();

                var existing = (await _repository
                    .GetListAsync<UtentePatologiaComparticipacao, Guid>(
                        new UtentePatologiaComparticipacaoByUtenteId(request.UtenteId)
                    )).ToList();

                var desiredCodes = desired.Select(x => x.CodigoComparticipacao).ToHashSet();
                var existingCodes = existing.Select(x => x.CodigoComparticipacao).ToHashSet();

                foreach (var row in existing.Where(x => !desiredCodes.Contains(x.CodigoComparticipacao)))
                {
                    await _repository.RemoveAsync<UtentePatologiaComparticipacao, Guid>(row);
                }

                foreach (var item in desired.Where(x => !existingCodes.Contains(x.CodigoComparticipacao)))
                {
                    var entity = new UtentePatologiaComparticipacao
                    {
                        UtenteId = request.UtenteId,
                        CodigoComparticipacao = item.CodigoComparticipacao,
                        Designacao = item.Designacao,
                    };
                    _ = await _repository.CreateAsync<UtentePatologiaComparticipacao, Guid>(entity);
                }

                foreach (var row in existing.Where(x => desiredCodes.Contains(x.CodigoComparticipacao)))
                {
                    var match = desired.First(d => d.CodigoComparticipacao == row.CodigoComparticipacao);
                    if (!string.Equals(row.Designacao, match.Designacao, StringComparison.Ordinal))
                    {
                        row.Designacao = match.Designacao;
                        await _repository.UpdateAsync<UtentePatologiaComparticipacao, Guid>(row);
                    }
                }

                _ = await _repository.SaveChangesAsync();

                var list = await _repository
                    .GetListAsync<UtentePatologiaComparticipacao, UtentePatologiaComparticipacaoDTO, Guid>(
                        new UtentePatologiaComparticipacaoByUtenteId(request.UtenteId)
                    );
                return ResponseFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<UtentePatologiaComparticipacaoDTO>>(ex.Message);
            }
        }
    }
}