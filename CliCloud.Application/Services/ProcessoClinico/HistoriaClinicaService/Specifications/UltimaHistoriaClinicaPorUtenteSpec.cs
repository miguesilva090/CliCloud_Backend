using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.HistoriaClinica;

namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.Specifications
{
    /// <summary>
    /// Devolve os registos de história clínica de um utente,
    /// ordenados do mais recente para o mais antigo.
    /// Usado para determinar qual é a última entrada.
    /// </summary>
    public class UltimaHistoriaClinicaPorUtenteSpec : Specification<HistoriaClinica>
    {
        public UltimaHistoriaClinicaPorUtenteSpec(Guid utenteId)
        {
            Query.Where(x => x.UtenteId == utenteId);
            Query.OrderByDescending(x => x.CreatedOn);
        }
    }
}

