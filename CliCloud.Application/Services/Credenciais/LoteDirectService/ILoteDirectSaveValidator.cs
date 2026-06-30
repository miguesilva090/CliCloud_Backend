using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectSaveValidator : ITransientService
{
    Task<string?> ValidateAsync(CreateLoteDirectRequest request, Guid? excludeId = null);
}
