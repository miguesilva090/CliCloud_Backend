using Ardalis.Specification;
using CliCloud.Domain.Entities.Alergias;

namespace CliCloud.Application.Services.AlergiaUtenteService.Specifications
{
    public class AlergiaUtenteByUtenteAlergia : Specification<AlergiaUtente>
    {
        public AlergiaUtenteByUtenteAlergia(Guid utenteId, Guid? alergiaId)
        {
            _ = Query.Where(h => h.UtenteId == utenteId && h.AlergiaId == alergiaId);
        }
    }
}
