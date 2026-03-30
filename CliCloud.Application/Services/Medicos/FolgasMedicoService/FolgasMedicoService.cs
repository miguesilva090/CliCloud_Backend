using System.Globalization;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using FolgasMedicoEntity = CliCloud.Domain.Entities.Medicos.FolgasMedico;
using CliCloud.Application.Services.Medicos.FolgasMedicoService.DTOs;
using CliCloud.Application.Services.Medicos.FolgasMedicoService.Specifications;

namespace CliCloud.Application.Services.Medicos.FolgasMedicoService
{
    public class FolgasMedicoService : IFolgasMedicoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public FolgasMedicoService(IRepositoryAsync repository, IMapper mapper)
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

        public async Task<Response<IEnumerable<FolgasMedicoDTO>>> GetByMedicoIdAsync(Guid medicoId)
        {
            try
            {
                var spec = new FolgasMedicoSearchByMedicoId(medicoId);
                var entities = await _repository.GetListAsync<FolgasMedicoEntity, Guid>(spec);
                var list = entities.Select(e =>
                {
                    var dto = _mapper.Map<FolgasMedicoDTO>(e);
                    dto.ManhaInicio = FormatTimeSpan(e.ManhaInicio);
                    dto.ManhaFim = FormatTimeSpan(e.ManhaFim);
                    dto.TardeInicio = FormatTimeSpan(e.TardeInicio);
                    dto.TardeFim = FormatTimeSpan(e.TardeFim);
                    return dto;
                });
                return ResponseFactory.Success<IEnumerable<FolgasMedicoDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<FolgasMedicoDTO>>(ex.Message);
            }
        }

        public async Task<Response<FolgasMedicoDTO>> GetAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync<FolgasMedicoEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<FolgasMedicoDTO>("Férias/folga não encontrada");

                var dto = _mapper.Map<FolgasMedicoDTO>(entity);
                dto.ManhaInicio = FormatTimeSpan(entity.ManhaInicio);
                dto.ManhaFim = FormatTimeSpan(entity.ManhaFim);
                dto.TardeInicio = FormatTimeSpan(entity.TardeInicio);
                dto.TardeFim = FormatTimeSpan(entity.TardeFim);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FolgasMedicoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateAsync(CreateFolgasMedicoRequest request)
        {
            if (!Guid.TryParse(request.MedicoId, out var medicoGuid))
                return ResponseFactory.Fail<Guid>("MedicoId inválido");

            var entity = _mapper.Map<FolgasMedicoEntity>(request);
            entity.MedicoId = medicoGuid;
            entity.ManhaInicio = ParseTimeSpan(request.ManhaInicio);
            entity.ManhaFim = ParseTimeSpan(request.ManhaFim);
            entity.TardeInicio = ParseTimeSpan(request.TardeInicio);
            entity.TardeFim = ParseTimeSpan(request.TardeFim);

            try
            {
                var created = await _repository.CreateAsync<FolgasMedicoEntity, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateAsync(UpdateFolgasMedicoRequest request, Guid id)
        {
            var entity = await _repository.GetByIdAsync<FolgasMedicoEntity, Guid>(id);
            if (entity == null)
                return ResponseFactory.Fail<Guid>("Férias/folga não encontrada");

            if (!Guid.TryParse(request.MedicoId, out var medicoGuid))
                return ResponseFactory.Fail<Guid>("MedicoId inválido");

            _mapper.Map(request, entity);
            entity.MedicoId = medicoGuid;
            entity.ManhaInicio = ParseTimeSpan(request.ManhaInicio);
            entity.ManhaFim = ParseTimeSpan(request.ManhaFim);
            entity.TardeInicio = ParseTimeSpan(request.TardeInicio);
            entity.TardeFim = ParseTimeSpan(request.TardeFim);

            try
            {
                var updated = await _repository.UpdateAsync<FolgasMedicoEntity, Guid>(entity);
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
                var entity = await _repository.RemoveByIdAsync<FolgasMedicoEntity, Guid>(id);
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
                        var entity = await _repository.RemoveByIdAsync<FolgasMedicoEntity, Guid>(id);
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
