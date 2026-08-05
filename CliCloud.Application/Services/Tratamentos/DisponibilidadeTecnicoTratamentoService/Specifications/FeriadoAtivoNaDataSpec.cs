using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.Specifications;

public class FeriadoAtivoNaDataSpec : Specification<Feriado>
{
    public FeriadoAtivoNaDataSpec(Guid clinicaId, DateTime data)
    {
        DateTime dia = data.Date;
        _ = Query.Where(x => 
            x.ClinicaId == clinicaId
            && x.Ativo
            && x.Data.Date == dia);
    }
}