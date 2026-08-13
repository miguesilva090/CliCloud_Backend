using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.MedicacaoFavoritaService.DTOs;
using CliCloud.Application.Services.Prescricao.MedicacaoFavoritaService.Specifications;
using CliCloud.Domain.Entities.Prescricao;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Prescricao.MedicacaoFavoritaService
{
    public class MedicacaoFavoritaService( IRepositoryAsync repository, IMapper mapper) : IMedicacaoFavoritaService
    {
        public async Task<Response<IEnumerable<MedicacaoFavoritaDTO>>> GetByMedicoIdAsync(Guid medicoId, int? tipoLinha = null)
        {
            try
            {
                var list = await repository
                    .GetListAsync<MedicacaoFavorita, MedicacaoFavoritaDTO, Guid>(
                        new MedicacaoFavoritaByMedicoId(medicoId, tipoLinha));
                return ResponseFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MedicacaoFavoritaDTO>>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateAsync(CreateMedicacaoFavoritaRequest request)
        {
            try
            {
                var medico = await repository.GetByIdAsync<Medico, Guid>(request.MedicoId);
                if (medico == null)
                    return ResponseFactory.Fail<Guid>("Médico não encontrado");

                if (request.TipoLinha is not (1 or 3))
                    return ResponseFactory.Fail<Guid>(
                        "Não é permitido adicionar aos favoritos para este tipo Prescrição");

                var cnpem = request.Cnpem.Trim();
                if (await repository.ExistsAsync<MedicacaoFavorita, Guid>(
                    new MedicacaoFavoritaByMedicoAndCnpem(request.MedicoId, cnpem)))
                {
                    return ResponseFactory.Fail<Guid>(
                        "O medicamento que selecionou, já se encontra nos favoritos.");
                }

                var entity = mapper.Map(request, new MedicacaoFavorita());
                entity.Cnpem = cnpem;
                entity.EmbId = string.IsNullOrWhiteSpace(request.EmbId)
                    ? null : request.EmbId.Trim();
                
                var created = await repository.CreateAsync<MedicacaoFavorita, Guid>(entity);
                _ = await repository.SaveChangesAsync();
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
                var entity = await repository.GetByIdAsync<MedicacaoFavorita, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Favorito não encontrado");

                await repository.RemoveByIdAsync<MedicacaoFavorita, Guid>(id);
                _ = await repository.SaveChangesAsync();
                return ResponseFactory.Success(id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}