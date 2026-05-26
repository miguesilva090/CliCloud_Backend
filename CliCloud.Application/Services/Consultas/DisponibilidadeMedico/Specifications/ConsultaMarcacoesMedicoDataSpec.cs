using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.DisponibilidadeMedico.Specifications;

public sealed class ConsultaMarcacoesMedicoDataSpec : Specification<ConsultaMarcacao>
{
    public ConsultaMarcacoesMedicoDataSpec(Guid medicoId, DateTime data)
    {
        _ = Query.Where(x => x.DeletedOn == null && x.MedicoId == medicoId && x.Data.HasValue && x.Data.Value.Date == data.Date);
    }
}