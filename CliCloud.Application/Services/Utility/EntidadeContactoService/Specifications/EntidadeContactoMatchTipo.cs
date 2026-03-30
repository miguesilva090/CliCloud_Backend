using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications
{
    public class EntidadeContactoMatchTipo : Specification<EntidadeContacto>
    {
        public EntidadeContactoMatchTipo(Guid entidadeId, int entidadeContactoTipoId)
        {
            _ = Query.Where(x => x.EntidadeId == entidadeId && x.EntidadeContactoTipoId == entidadeContactoTipoId);
        }
    }
}
