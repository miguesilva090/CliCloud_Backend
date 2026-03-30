using System.Globalization;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using FolgasTecnicoEntity = CliCloud.Domain.Entities.Tecnicos.FolgasTecnico;
using CliCloud.Application.Services.Tecnicos.FolgasTecnicoService.DTOs;
using CliCloud.Application.Services.Tecnicos.FolgasTecnicoService.Specifications;

namespace CliCloud.Application.Services.Tecnicos.FolgasTecnicoService
{
    public class FolgasTecnicoService : IFolgasTecnicoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public FolgasTecnicoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private static TimeSpan? ParseTimeSpan(string? timeString)
        {
            if (string.IsNullOrWhiteSpace(timeString)) return null;
            return TimeSpan.TryParse(timeString, out var result) ? result : null;
        }

        private static string? FormatTimeSpan(TimeSpan? ts)
        {
            return ts.HasValue ? ts.Value.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture) : null;
        }

        public async Task<Response<IEnumerable<FolgasTecnicoDTO>>> GetByTecnicoIdAsync(Guid tecnicoId)
        {
            try
            {
                var spec = new FolgasTecnicoSearchByTecnicoId(tecnicoId);
                var entities = await _repository.GetListAsync<FolgasTecnicoEntity, Guid>(spec);
                var list = entities.Select(e => 
                {
                    var dto = _mapper.Map<FolgasTecnicoDTO>(e);
                    dto.ManhaInicio = FormatTimeSpan(e.ManhaInicio);
                    dto.ManhaFim = FormatTimeSpan(e.ManhaFim);
                    dto.TardeInicio = FormatTimeSpan(e.TardeInicio);
                    dto.TardeFim = FormatTimeSpan(e.TardeFim);
                    return dto;
                });
                return ResponseFactory.Success<IEnumerable<FolgasTecnicoDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<FolgasTecnicoDTO>>(ex.Message);
            }
        }

        public async Task<Response<FolgasTecnicoDTO>> GetAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync<FolgasTecnicoEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<FolgasTecnicoDTO>("Férias/folga não encontrada");


                var dto = _mapper.Map<FolgasTecnicoDTO>(entity);
                dto.ManhaInicio = FormatTimeSpan(entity.ManhaInicio);
                dto.ManhaFim = FormatTimeSpan(entity.ManhaFim);
                dto.TardeInicio = FormatTimeSpan(entity.TardeInicio);
                dto.TardeFim = FormatTimeSpan(entity.TardeFim);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FolgasTecnicoDTO>(ex.Message);
            }
        }
        
        public async Task<Response<Guid>> CreateAsync(CreateFolgasTecnicoRequest request)
        {
            if (!Guid.TryParse(request.TecnicoId, out var tecnicoGuid))
                return ResponseFactory.Fail<Guid>("TecnicoId inválido");

            var entity = _mapper.Map<FolgasTecnicoEntity>(request);
            entity.TecnicoId = tecnicoGuid;
            entity.ManhaInicio = ParseTimeSpan(request.ManhaInicio);
            entity.ManhaFim = ParseTimeSpan(request.ManhaFim);
            entity.TardeInicio = ParseTimeSpan(request.TardeInicio);
            entity.TardeFim = ParseTimeSpan(request.TardeFim);

            try
            {
                var created = await _repository.CreateAsync<FolgasTecnicoEntity, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateAsync(UpdateFolgasTecnicoRequest request, Guid id)
        {
            var entity = await _repository.GetByIdAsync<FolgasTecnicoEntity, Guid>(id);
            if (entity == null)
                return ResponseFactory.Fail<Guid>("Férias/folga não encontrada");

            if (!Guid.TryParse(request.TecnicoId, out var tecnicoGuid))
                return ResponseFactory.Fail<Guid>("TecnicoId inválido");

            _mapper.Map(request, entity);
            entity.TecnicoId = tecnicoGuid;
            entity.ManhaInicio = ParseTimeSpan(request.ManhaInicio);
            entity.ManhaFim = ParseTimeSpan(request.ManhaFim);
            entity.TardeInicio = ParseTimeSpan(request.TardeInicio);
            entity.TardeFim = ParseTimeSpan(request.TardeFim);

            try
            {
                var updated = await _repository.UpdateAsync<FolgasTecnicoEntity, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(updated.Id);
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
                var entity = await _repository.RemoveByIdAsync<FolgasTecnicoEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Férias/folga não encontrada");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            try
            {
                var idsList = ids.ToList();
                var deleted = new List<Guid>();

                foreach (var id in idsList)
                {
                    try
                    {
                        var entity = await _repository.RemoveByIdAsync<FolgasTecnicoEntity, Guid>(id);
                        if (entity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            deleted.Add(id);
                        }
                    }
                    catch
                    {
                        _repository.ClearChangeTracker();
                    }
                }

                if (deleted.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(deleted);
                if (deleted.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(deleted, $"Eliminadas {deleted.Count} de {idsList.Count}");
                return ResponseFactory.Fail<IEnumerable<Guid>>("Nenhuma férias/folga eliminada");
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
