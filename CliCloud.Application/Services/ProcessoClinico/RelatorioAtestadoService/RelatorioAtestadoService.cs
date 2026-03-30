using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.Filters;
using CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;
using CliCloud.Application.Services.Medicos.MedicoService;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService
{
    public class RelatorioAtestadoService : IRelatorioAtestadoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentTenantUserService _currentTenantUserService;
        private readonly IMedicoService _medicoService;

        public RelatorioAtestadoService(
            IRepositoryAsync repository,
            IMapper mapper,
            ICurrentTenantUserService currentTenantUserService,
            IMedicoService medicoService)
        {
            _repository = repository;
            _mapper = mapper;
            _currentTenantUserService = currentTenantUserService;
            _medicoService = medicoService;
        }

        // get full List
        public async Task<Response<IEnumerable<RelatorioAtestadoDTO>>> GetRelatorioAtestadoAsync(string keyword = "")
        {
            RelatorioAtestadoSearchList specification = new(keyword);
            IEnumerable<RelatorioAtestadoDTO> list =
                await _repository.GetListAsync<RelatorioAtestado, RelatorioAtestadoDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<RelatorioAtestadoDTO>>(list);
        }

        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<RelatorioAtestadoDTO>> GetRelatorioAtestadoPaginatedAsync(RelatorioAtestadoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                // set to first page if any search filters are applied
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
            RelatorioAtestadoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);

            PaginatedResponse<RelatorioAtestadoDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<RelatorioAtestado, RelatorioAtestadoDTO, Guid>(
                    filter.PageNumber,
                    filter.PageSize,
                    specification);

            return pagedResponse;
        }

        // get single RelatorioAtestado by Id 
        public async Task<Response<RelatorioAtestadoDTO>> GetRelatorioAtestadoAsync(Guid id)
        {
            try
            {
                RelatorioAtestadoDTO dto =
                    await _repository.GetByIdAsync<RelatorioAtestado, RelatorioAtestadoDTO, Guid>(id);

                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<RelatorioAtestadoDTO>(ex.Message);
            }
        }

        // create new RelatorioAtestado
        public async Task<Response<Guid>> CreateRelatorioAtestadoAsync(CreateRelatorioAtestadoRequest request)
        {
            RelatorioAtestadoMatchName specification = new(request.Titulo);
            bool exists = await _repository.ExistsAsync<RelatorioAtestado, Guid>(specification);
            if (exists)
            {
                return ResponseFactory.Fail<Guid>("Relatório/Atestado já existe");
            }

            // Resolver médico a partir do utilizador atual (IdUtilizador -> Medico.Id)
            string? userIdStr = _currentTenantUserService.UserId;
            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
            {
                return ResponseFactory.Fail<Guid>("Utilizador atual inválido para associar ao médico.");
            }

            var medicoRes = await _medicoService.GetMedicoByIdUtilizadorAsync(userId);
            if (medicoRes.Status != ResponseStatus.Success || medicoRes.Data is null)
            {
                return ResponseFactory.Fail<Guid>("Médico associado ao utilizador atual não encontrado.");
            }

            RelatorioAtestado newRelatorioAtestado = _mapper.Map(request, new RelatorioAtestado());
            newRelatorioAtestado.MedicoId = medicoRes.Data.Id;

            try
            {
                RelatorioAtestado response =
                    await _repository.CreateAsync<RelatorioAtestado, Guid>(newRelatorioAtestado);

                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update RelatorioAtestado
        public async Task<Response<Guid>> UpdateRelatorioAtestadoAsync(UpdateRelatorioAtestadoRequest request, Guid id)
        {
            RelatorioAtestado? RelatorioAtestadoInDb =
                await _repository.GetByIdAsync<RelatorioAtestado, Guid>(id);

            if (RelatorioAtestadoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Relatório/Atestado não encontrado");
            }

            RelatorioAtestado updatedRelatorioAtestado = _mapper.Map(request, RelatorioAtestadoInDb);
            // Nunca permitir que o cliente altere o médico do relatório
            updatedRelatorioAtestado.MedicoId = RelatorioAtestadoInDb.MedicoId;

            try
            {
                RelatorioAtestado response =
                    await _repository.UpdateAsync<RelatorioAtestado, Guid>(updatedRelatorioAtestado);

                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete RelatorioAtestado
        public async Task<Response<Guid>> DeleteRelatorioAtestadoAsync(Guid id)
        {
            try
            {
                RelatorioAtestado? entity =
                    await _repository.RemoveByIdAsync<RelatorioAtestado, Guid>(id);

                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        /// <summary>
        /// Obter todos os relatórios/atestados de um utente (para a aba Relatório/Atestado da ficha clínica).
        /// </summary>
        public async Task<Response<IEnumerable<RelatorioAtestadoDTO>>> GetByUtenteAsync(Guid utenteId)
        {
            var specification = new RelatorioAtestadoByUtenteSpec(utenteId);
            IEnumerable<RelatorioAtestadoDTO> list =
                await _repository.GetListAsync<RelatorioAtestado, RelatorioAtestadoDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<RelatorioAtestadoDTO>>(list);
        }

        /// <summary>
        /// Assinar um relatório/atestado: se ainda não estiver assinado, preenche AssinadoEm com a data/hora atual.
        /// </summary>
        public async Task<Response<DateTime>> AssinarRelatorioAtestadoAsync(Guid id)
        {
            RelatorioAtestado? entity =
                await _repository.GetByIdAsync<RelatorioAtestado, Guid>(id);

            if (entity == null)
            {
                return ResponseFactory.Fail<DateTime>("Relatório/Atestado não encontrado");
            }

            if (entity.AssinadoEm is null)
            {
                entity.AssinadoEm = DateTime.UtcNow;

                _ = await _repository.UpdateAsync<RelatorioAtestado, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
            }

            return ResponseFactory.Success(entity.AssinadoEm.Value);
        }
    }
}

