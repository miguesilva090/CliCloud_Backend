using Ardalis.Specification;
using CliCloud.Domain.Entities.Prescricao;

namespace CliCloud.Application.Services.Prescricao.MedicacaoFavoritaService.Specifications
{
    public class MedicacaoFavoritaByMedicoId : Specification<MedicacaoFavorita>
    {
        public MedicacaoFavoritaByMedicoId(Guid medicoId, int? tipoLinha = null)
        {
            _ = Query.Where(x => x.MedicoId == medicoId);
            if(tipoLinha.HasValue)
                _ = Query.Where(x => x.TipoLinha == tipoLinha.Value);
            _ = Query.OrderBy(x => x.Designacao);
        }
    }
}