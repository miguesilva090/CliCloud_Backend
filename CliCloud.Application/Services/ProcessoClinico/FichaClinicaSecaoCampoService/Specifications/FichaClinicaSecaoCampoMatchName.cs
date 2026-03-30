using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.Specifications
{
    public class FichaClinicaSecaoCampoMatchName : Specification<FichaClinicaSecaoCampo>
    {
        public FichaClinicaSecaoCampoMatchName(Guid separadorId, string nome)
        {
            _ = Query.Where(x => x.SeparadorId == separadorId && x.Nome == nome);
        }
    }
}

