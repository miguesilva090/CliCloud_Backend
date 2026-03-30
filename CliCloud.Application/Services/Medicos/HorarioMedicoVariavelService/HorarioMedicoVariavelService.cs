using System.Globalization;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using HorarioMedicoVariavelEntity = CliCloud.Domain.Entities.Medicos.HorarioMedicoVariavel;
using CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.DTOs;
using CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.Specifications;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService
{
    public class HorarioMedicoVariavelService : IHorarioMedicoVariavelService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public HorarioMedicoVariavelService(IRepositoryAsync repository, IMapper mapper)
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

        public async Task<Response<IEnumerable<HorarioMedicoVariavelDTO>>> GetByMedicoIdAsync(Guid medicoId)
        {
            try
            {
                var spec = new HorarioMedicoVariavelSearchByMedicoId(medicoId);
                var entities = await _repository.GetListAsync<HorarioMedicoVariavelEntity, Guid>(spec);
                var list = entities.Select(e =>
                {
                    var dto = _mapper.Map<HorarioMedicoVariavelDTO>(e);
                    dto.ManhaInicio = FormatTimeSpan(e.ManhaInicio);
                    dto.ManhaFim = FormatTimeSpan(e.ManhaFim);
                    dto.TardeInicio = FormatTimeSpan(e.TardeInicio);
                    dto.TardeFim = FormatTimeSpan(e.TardeFim);
                    return dto;
                });
                return ResponseFactory.Success<IEnumerable<HorarioMedicoVariavelDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<HorarioMedicoVariavelDTO>>(ex.Message);
            }
        }

        public async Task<Response<HorarioMedicoVariavelDTO>> GetAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync<HorarioMedicoVariavelEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<HorarioMedicoVariavelDTO>("Horário variável não encontrado");

                var dto = _mapper.Map<HorarioMedicoVariavelDTO>(entity);
                dto.ManhaInicio = FormatTimeSpan(entity.ManhaInicio);
                dto.ManhaFim = FormatTimeSpan(entity.ManhaFim);
                dto.TardeInicio = FormatTimeSpan(entity.TardeInicio);
                dto.TardeFim = FormatTimeSpan(entity.TardeFim);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<HorarioMedicoVariavelDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateAsync(CreateHorarioMedicoVariavelRequest request)
        {
            if (!Guid.TryParse(request.MedicoId, out var medicoGuid))
                return ResponseFactory.Fail<Guid>("MedicoId inválido");

            var entity = _mapper.Map<HorarioMedicoVariavelEntity>(request);
            entity.MedicoId = medicoGuid;
            entity.ManhaInicio = ParseTimeSpan(request.ManhaInicio);
            entity.ManhaFim = ParseTimeSpan(request.ManhaFim);
            entity.TardeInicio = ParseTimeSpan(request.TardeInicio);
            entity.TardeFim = ParseTimeSpan(request.TardeFim);

            try
            {
                var created = await _repository.CreateAsync<HorarioMedicoVariavelEntity, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateAsync(UpdateHorarioMedicoVariavelRequest request, Guid id)
        {
            var entity = await _repository.GetByIdAsync<HorarioMedicoVariavelEntity, Guid>(id);
            if (entity == null)
                return ResponseFactory.Fail<Guid>("Horário variável não encontrado");

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
                var updated = await _repository.UpdateAsync<HorarioMedicoVariavelEntity, Guid>(entity);
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
                var entity = await _repository.RemoveByIdAsync<HorarioMedicoVariavelEntity, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Horário variável não encontrado");
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
                        var entity = await _repository.RemoveByIdAsync<HorarioMedicoVariavelEntity, Guid>(id);
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
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(deleted, $"Eliminados {deleted.Count} de {idsList.Count}");
                return ResponseFactory.Fail<IEnumerable<Guid>>("Nenhum horário variável eliminado");
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
