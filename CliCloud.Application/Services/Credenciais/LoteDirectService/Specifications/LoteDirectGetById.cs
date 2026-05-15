using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications
{
    public class LoteDirectGetById : Specification<LoteDirect>
    {
        public LoteDirectGetById()
        {
            Query.Include(x => x.Utente);
            Query.Include(x => x.Medico);
            Query.Include(x => x.MedicoExterno);
            Query.Include(x => x.TipoServicoRegisto);
            Query.Include(x => x.ServicoConsultaRegisto);
        }
    }
}
