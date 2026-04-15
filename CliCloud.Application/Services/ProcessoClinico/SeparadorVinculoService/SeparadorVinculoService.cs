using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService.Specifications;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService;

public class SeparadorVinculoService(IRepositoryAsync repository) : ISeparadorVinculoService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<IEnumerable<SeparadorVinculoDTO>>> GetBySeparadorAsync(Guid separadorId)
    {
        var spec = new SeparadorVinculoBySeparadorSpec(separadorId);
        IEnumerable<SeparadorVinculoDTO> result =
            (await _repository.GetListAsync<SeparadorVinculo, SeparadorVinculoDTO, Guid>(spec))
            .OrderBy(x => x.Tipo)
            .ThenBy(x => x.EntidadeId);
        return ResponseFactory.Success(result);
    }

    public async Task<Response<Guid>> CreateAsync(CreateSeparadorVinculoRequest request)
    {
        var dupSpec = new SeparadorVinculoMatchSpec(request.SeparadorId, request.Tipo, request.EntidadeId);
        bool exists = await _repository.ExistsAsync<SeparadorVinculo, Guid>(dupSpec);
        if (exists)
        {
            return ResponseFactory.Fail<Guid>("Este vínculo já existe.");
        }

        var entity = new SeparadorVinculo
        {
            SeparadorId = request.SeparadorId,
            Tipo = request.Tipo,
            EntidadeId = request.EntidadeId
        };

        try
        {
            var created = await _repository.CreateAsync<SeparadorVinculo, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> DeleteAsync(Guid id)
    {
        try
        {
            var removed = await _repository.RemoveByIdAsync<SeparadorVinculo, Guid>(id);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(removed.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }
}
