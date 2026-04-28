using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService.Specifications;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService
{
    public class HistoriaDentariaService(IRepositoryAsync repository) : IHistoriaDentariaService
    {
        private readonly IRepositoryAsync _repository = repository;

        public async Task<Response<IReadOnlyList<HistoriaDentariaDTO>>> GetByUtenteAsync(Guid utenteId)
        {
            try
            {
                var spec = new HistoriaDentariaPorUtenteOrderedSpec(utenteId);
                var list = await _repository.GetListAsync<HistoriaDentaria, HistoriaDentariaDTO, Guid>(spec);
                return ResponseFactory.Success<IReadOnlyList<HistoriaDentariaDTO>>(list.ToList());
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IReadOnlyList<HistoriaDentariaDTO>>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateAsync(CreateHistoriaDentariaRequest request, Guid utilizadorLogadoId)
        {
            try
            {
                var medicoSpec = new MedicoPorIdUtilizadorSpec(utilizadorLogadoId);
                var medicos = await _repository.GetListAsync<Medico, Guid>(medicoSpec);
                var medico = medicos.FirstOrDefault();
                if (medico is null)
                    return ResponseFactory.Fail<Guid>("Utilizador não está associado a um médico.");

                if (string.IsNullOrWhiteSpace(request.HistoriaHtml?.Trim()))
                    return ResponseFactory.Fail<Guid>("Conteúdo do relatório em falta.");

                var agora = DateTime.UtcNow;
                var entity = new HistoriaDentaria
                {
                    Id = Guid.NewGuid(),
                    UtenteId = request.UtenteId,
                    MedicoId = medico.Id,
                    DataRegisto = agora,
                    HistoriaHtml = request.HistoriaHtml.Trim(),
                };

                _ = await _repository.CreateAsync<HistoriaDentaria, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}
