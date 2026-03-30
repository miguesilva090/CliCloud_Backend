using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.ModeloRelatorioAtestadoService.DTOs;

namespace CliCloud.Application.Services.ProcessoClinico.ModeloRelatorioAtestadoService
{
    public interface IModeloRelatorioAtestadoService : ITransientService
    {
        Task<Response<IEnumerable<ModeloRelatorioAtestadoDTO>>> GetModelosAsync(Guid empresaId, Guid? medicoId);
        Task<Response<ModeloRelatorioAtestadoDTO>> GetByIdAsync(Guid id);
        Task<Response<Guid>> CreateAsync(CreateModeloRelatorioAtestadoRequest request, Guid empresaId);
        Task<Response<Guid>> UpdateAsync(Guid id, UpdateModeloRelatorioAtestadoRequest request);
        Task<Response<Guid>> DeleteAsync(Guid id);
    }
}

