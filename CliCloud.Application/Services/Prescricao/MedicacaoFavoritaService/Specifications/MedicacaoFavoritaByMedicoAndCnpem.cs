using Ardalis.Specification;
using CliCloud.Domain.Entities.Prescricao;

namespace CliCloud.Application.Services.Prescricao.MedicacaoFavoritaService.Specifications
{
    public class MedicacaoFavoritaByMedicoAndCnpem : Specification<MedicacaoFavorita>
    {
        public MedicacaoFavoritaByMedicoAndCnpem(Guid medicoId, string cnpem)
        {
            var key = cnpem.Trim();
            _ = Query.Where(x => x.MedicoId == medicoId && x.Cnpem == key);
        }
    }
}