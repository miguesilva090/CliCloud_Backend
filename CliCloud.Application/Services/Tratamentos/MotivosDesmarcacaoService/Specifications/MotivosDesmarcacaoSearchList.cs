using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;


namespace CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.Specifications
{
    public class MotivosDesmarcacaoSearchList : Specification<MotivosDesmarcacao>
    {
        public MotivosDesmarcacaoSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Descricao);

        }
    }
}
