using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.TiposConsulta.TipoConsultaService.Specifications
{
    public sealed class TipoConsultaMatchDesignacao : Specification<TipoConsultaItem>
    {
        public TipoConsultaMatchDesignacao(string designacao)
        {
            Query.Where(x => x.Designacao == designacao);
        }
    }
}
