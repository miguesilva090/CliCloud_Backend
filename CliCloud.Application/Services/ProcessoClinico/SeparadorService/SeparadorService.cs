using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.SeparadorService.Filters;
using CliCloud.Application.Services.ProcessoClinico.SeparadorService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService;

public class SeparadorService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentTenantUserService currentTenantUserService
) : ISeparadorService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentTenantUserService _currentTenantUserService = currentTenantUserService;

    private Guid? TryGetCurrentUserId()
    {
        _currentTenantUserService.SetUser();
        return Guid.TryParse(_currentTenantUserService.UserId, out Guid userId) ? userId : null;
    }

    public async Task<Response<IEnumerable<SeparadorDTO>>> GetSeparadorAsync(string keyword = "")
    {
        SeparadorSearchList specification = new(keyword);
        IEnumerable<SeparadorDTO> list =
            await _repository.GetListAsync<Separador, SeparadorDTO, Guid>(specification);
        return ResponseFactory.Success<IEnumerable<SeparadorDTO>>(list);
    }

    public async Task<Response<IEnumerable<SeparadorFichaClinicaDTO>>> GetSeparadoresFichaClinicaVisiveisAsync(
        Guid clinicaId,
        Guid? medicoId,
        Guid? especialidadeId
    )
    {
        Guid? currentUserId = TryGetCurrentUserId();
        IEnumerable<Separador> baseSeparadores = (await _repository.GetListAsync<Separador, Guid>())
            .Where(x => x.Ativo)
            .OrderBy(x => x.Ordem)
            .ThenBy(x => x.Nome);

        IEnumerable<SeparadorVinculo> baseVinculos = await _repository.GetListAsync<SeparadorVinculo, Guid>();
        var baseVinculosBySeparador = baseVinculos
            .GroupBy(x => x.SeparadorId)
            .ToDictionary(g => g.Key, g => g.ToList());

        bool IsVisibleByVinculos(IEnumerable<(TipoVinculoSeparador Tipo, Guid EntidadeId)> vinculos)
        {
            var list = vinculos.ToList();
            if (list.Count == 0)
            {
                return true; // sem restrições: visível
            }

            // Sem contexto de médico/especialidade no runtime, não bloquear os separadores
            // personalizados por vínculos; evita esconder tudo para utilizadores sem médico atual.
            if (!medicoId.HasValue && !especialidadeId.HasValue)
            {
                return true;
            }

            bool medicoMatch = medicoId.HasValue
                && list.Any(v => v.Tipo == TipoVinculoSeparador.Medico && v.EntidadeId == medicoId.Value);
            bool especialidadeMatch = especialidadeId.HasValue
                && list.Any(v => v.Tipo == TipoVinculoSeparador.Especialidade && v.EntidadeId == especialidadeId.Value);

            return medicoMatch || especialidadeMatch;
        }

        List<SeparadorFichaClinicaDTO> result = new();

        foreach (Separador separador in baseSeparadores)
        {
            List<SeparadorVinculo> vinculos = baseVinculosBySeparador.GetValueOrDefault(separador.Id, []);
            bool visible = IsVisibleByVinculos(vinculos.Select(v => (v.Tipo, v.EntidadeId)));
            if (!visible)
            {
                continue;
            }

            result.Add(new SeparadorFichaClinicaDTO
            {
                SeparadorId = separador.Id,
                Nome = separador.Nome,
                Codigo = separador.Codigo,
                Ordem = separador.Ordem,
                Origem = "Base",
                FormularioId = null
            });
        }

        IEnumerable<SeparadorPersonalizado> personalizados =
            (await _repository.GetListAsync<SeparadorPersonalizado, Guid>())
            .Where(x =>
                x.ClinicaId == clinicaId
                && x.Ativo
                && (!currentUserId.HasValue || x.UtilizadorId == currentUserId.Value)
            )
            .OrderBy(x => x.Ordem)
            .ThenBy(x => x.NomeSeparador);

        IEnumerable<SeparadorPersonalizadoVinculo> personalizadosVinculos =
            await _repository.GetListAsync<SeparadorPersonalizadoVinculo, Guid>();
        var personalizadosVinculosBySeparador = personalizadosVinculos
            .GroupBy(x => x.SeparadorPersonalizadoId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (SeparadorPersonalizado separador in personalizados)
        {
            List<SeparadorPersonalizadoVinculo> vinculos =
                personalizadosVinculosBySeparador.GetValueOrDefault(separador.Id, []);
            bool visible = IsVisibleByVinculos(vinculos.Select(v => (v.Tipo, v.EntidadeId)));
            if (!visible)
            {
                continue;
            }

            result.Add(new SeparadorFichaClinicaDTO
            {
                SeparadorId = separador.Id,
                Nome = separador.NomeSeparador,
                Codigo = string.Empty,
                Ordem = separador.Ordem,
                Origem = "Personalizado",
                FormularioId = separador.FormularioId
            });
        }

        return ResponseFactory.Success<IEnumerable<SeparadorFichaClinicaDTO>>(result);
    }

    public async Task<PaginatedResponse<SeparadorDTO>> GetSeparadorPaginatedAsync(
        SeparadorTableFilter filter
    )
    {
        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            filter.PageNumber = 1;
        }

        string dynamicOrder =
            filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
        SeparadorSearchTable specification = new(filter.Keyword, dynamicOrder);

        return await _repository.GetPaginatedResultsAsync<Separador, SeparadorDTO, Guid>(
            filter.PageNumber,
            filter.PageSize,
            specification
        );
    }

    public async Task<Response<SeparadorDTO>> GetSeparadorAsync(Guid id)
    {
        try
        {
            SeparadorDTO dto = await _repository.GetByIdAsync<Separador, SeparadorDTO, Guid>(id);
            return ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<SeparadorDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CreateSeparadorAsync(CreateSeparadorRequest request)
    {
        SeparadorMatchNome specification = new(request.Nome);
        bool exists = await _repository.ExistsAsync<Separador, Guid>(specification);
        if (exists)
        {
            return ResponseFactory.Fail<Guid>("Já existe um separador com este nome.");
        }

        Separador entity = _mapper.Map<Separador>(request);

        try
        {
            Separador response = await _repository.CreateAsync<Separador, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(response.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateSeparadorAsync(UpdateSeparadorRequest request, Guid id)
    {
        Separador entityInDb = await _repository.GetByIdAsync<Separador, Guid>(id);
        if (entityInDb == null)
        {
            return ResponseFactory.Fail<Guid>("Separador não encontrado.");
        }

        SeparadorMatchNome specification = new(request.Nome, id);
        bool exists = await _repository.ExistsAsync<Separador, Guid>(specification);
        if (exists)
        {
            return ResponseFactory.Fail<Guid>("Já existe um separador com este nome.");
        }

        _ = _mapper.Map(request, entityInDb);

        try
        {
            Separador response = await _repository.UpdateAsync<Separador, Guid>(entityInDb);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(response.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> DeleteSeparadorAsync(Guid id)
    {
        try
        {
            Separador? entity = await _repository.RemoveByIdAsync<Separador, Guid>(id);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(entity.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleSeparadorAsync(IEnumerable<Guid> ids)
    {
        try
        {
            List<Guid> idsList = ids.ToList();
            List<Guid> successfullyDeletedIds = new();
            List<string> failedDeletions = new();

            foreach (Guid id in idsList)
            {
                try
                {
                    Separador? entity = await _repository.GetByIdAsync<Separador, Guid>(id);
                    if (entity == null)
                    {
                        failedDeletions.Add($"Separador com ID {id} não encontrado.");
                        continue;
                    }

                    Separador? deletedEntity = await _repository.RemoveByIdAsync<Separador, Guid>(id);
                    if (deletedEntity != null)
                    {
                        _ = await _repository.SaveChangesAsync();
                        successfullyDeletedIds.Add(id);
                    }
                    else
                    {
                        failedDeletions.Add($"Falha ao eliminar separador com ID {id}.");
                    }
                }
                catch (Exception)
                {
                    failedDeletions.Add($"Erro ao eliminar separador com ID {id}.");
                    _repository.ClearChangeTracker();
                }
            }

            if (successfullyDeletedIds.Count == idsList.Count)
            {
                return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }

            if (successfullyDeletedIds.Count > 0)
            {
                return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(
                    successfullyDeletedIds,
                    $"Eliminados {successfullyDeletedIds.Count} de {idsList.Count} separadores."
                );
            }

            return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
        }
    }
}
