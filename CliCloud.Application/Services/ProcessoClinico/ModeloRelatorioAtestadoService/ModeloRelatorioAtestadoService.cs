using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.ModeloRelatorioAtestadoService.DTOs;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;

namespace CliCloud.Application.Services.ProcessoClinico.ModeloRelatorioAtestadoService
{
    public class ModeloRelatorioAtestadoService : IModeloRelatorioAtestadoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public ModeloRelatorioAtestadoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ModeloRelatorioAtestadoDTO>>> GetModelosAsync(Guid empresaId, Guid? medicoId)
        {
            var all = await _repository.GetListAsync<ModeloRelatorioAtestado, ModeloRelatorioAtestadoDTO, Guid>();

            var result = all
                .Where(m => m.EmpresaId == empresaId && (m.MedicoId == null || m.MedicoId == medicoId))
                .OrderByDescending(m => m.CreatedOn)
                .ToList();

            return ResponseFactory.Success<IEnumerable<ModeloRelatorioAtestadoDTO>>(result);
        }

        public async Task<Response<ModeloRelatorioAtestadoDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync<ModeloRelatorioAtestado, ModeloRelatorioAtestadoDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<ModeloRelatorioAtestadoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateAsync(CreateModeloRelatorioAtestadoRequest request, Guid empresaId)
        {
            var entity = _mapper.Map(request, new ModeloRelatorioAtestado());
            entity.EmpresaId = empresaId;

            try
            {
                var created = await _repository.CreateAsync<ModeloRelatorioAtestado, Guid>(entity);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateModeloRelatorioAtestadoRequest request)
        {
            var entity = await _repository.GetByIdAsync<ModeloRelatorioAtestado, Guid>(id);
            if (entity == null)
            {
                return ResponseFactory.Fail<Guid>("Modelo não encontrado.");
            }

            entity = _mapper.Map(request, entity);

            try
            {
                var updated = await _repository.UpdateAsync<ModeloRelatorioAtestado, Guid>(entity);
                await _repository.SaveChangesAsync();
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
                var deleted = await _repository.RemoveByIdAsync<ModeloRelatorioAtestado, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(deleted.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

