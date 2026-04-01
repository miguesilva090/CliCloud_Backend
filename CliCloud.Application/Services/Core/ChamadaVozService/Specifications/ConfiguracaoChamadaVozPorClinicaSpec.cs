using System;
using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.ChamadaVozService.Specifications
{
  public class ConfiguracaoChamadaVozPorClinicaSpec : Specification<ConfiguracaoChamadaVoz>
  {
    public ConfiguracaoChamadaVozPorClinicaSpec(Guid clinicaId)
    {
      _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
  }
}
