using Ardalis.Specification;
using CliCloud.Domain.Entities.Prescricao;

namespace CliCloud.Application.Services.Prescricao.MedicacaoCronicaService.Specifications
{
    public class MedicacaoCronicaByUtenteId : Specification<MedicacaoCronica>
    {
        public MedicacaoCronicaByUtenteId(Guid utenteId, bool apenasAtivos = true)
        {
            _ = Query.Where(x => x.UtenteId == utenteId);
            if (apenasAtivos)
                _ = Query.Where(x => x.DataFim == null);
            _ = Query.OrderBy(x => x.Designacao);
        }
    }
}
