using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.DTOs;

namespace CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService
{
    public interface IDocumentosFichaClinicaService : ITransientService
    {
        Task<Response<IEnumerable<DocumentosFichaClinicaDTO>>> GetByUtenteAsync(Guid utenteId, string? categoria = "Clinico");

        // Create: o service recebe os metadados + info calculada no controller (caminho, extensão, tipo)
        Task<Response<Guid>> CreateAsync(
            CreateDocumentosFichaClinicaRequest request,
            string nomeFicheiro,
            string caminhoRelativo,
            string terminacao,
            string tipo,      // "Documento"/"Foto"/"Video"
            bool isVideo);

        Task<Response<Guid>> DeleteAsync(Guid id);

        Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IList<Guid> ids);
    }
}