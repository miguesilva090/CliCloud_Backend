using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using MargemMedicoEntity = CliCloud.Domain.Entities.Medicos.MargemMedico;
using CliCloud.Application.Services.Medicos.MargemMedicoService.DTOs;
using CliCloud.Application.Services.Medicos.MargemMedicoService.Filters;
using CliCloud.Application.Services.Medicos.MargemMedicoService.Specifications;

namespace CliCloud.Application.Services.Medicos.MargemMedicoService
{
    public class MargemMedicoService : IMargemMedicoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public MargemMedicoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<MargemMedicoDTO>>> GetMargemMedicoAsync(string keyword = "")
        {
            MargemMedicoSearchList specification = new(keyword);
            IEnumerable<MargemMedicoDTO> list = await _repository.GetListAsync<MargemMedicoEntity, MargemMedicoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MargemMedicoDTO>>(list);
        }

        public async Task<Response<IEnumerable<MargemMedicoLightDTO>>> GetMargemMedicoLightAsync(string keyword = "")
        {
            MargemMedicoSearchList specification = new(keyword);
            IEnumerable<MargemMedicoLightDTO> list = await _repository.GetListAsync<MargemMedicoEntity, MargemMedicoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MargemMedicoLightDTO>>(list);
        }

        public async Task<PaginatedResponse<MargemMedicoTableDTO>> GetMargemMedicoPaginatedAsync(MargemMedicoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            MargemMedicoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<MargemMedicoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<MargemMedicoEntity, MargemMedicoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<MargemMedicoTableDTO>>> GetAllMargemMedicoAsync(MargemMedicoAllFilter filter)
        {
            try
            {
                filter ??= new MargemMedicoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                MargemMedicoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<MargemMedicoTableDTO> list = await _repository.GetListAsync<MargemMedicoEntity, MargemMedicoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<MargemMedicoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MargemMedicoTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<MargemMedicoDTO>> GetMargemMedicoAsync(Guid id)
        {
            try
            {
                MargemMedicoDTO dto = await _repository.GetByIdAsync<MargemMedicoEntity, MargemMedicoDTO, Guid>(id);
                return ResponseFactory.Success<MargemMedicoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MargemMedicoDTO>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<MargemMedicoDTO>>> GetMargemMedicoByMedicoIdAsync(Guid medicoId)
        {
            try
            {
                MargemMedicoSearchByMedicoId specification = new(medicoId);
                IEnumerable<MargemMedicoDTO> results = await _repository.GetListAsync<MargemMedicoEntity, MargemMedicoDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<MargemMedicoDTO>>(results);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MargemMedicoDTO>>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateMargemMedicoAsync(CreateMargemMedicoRequest request)
        {
            if (!Guid.TryParse(request.ServicoId, out Guid servicoGuid))
            {
                return ResponseFactory.Fail<Guid>("ServicoId inválido");
            }
            if (!Guid.TryParse(request.MedicoId, out Guid medicoGuid))
            {
                return ResponseFactory.Fail<Guid>("MedicoId inválido");
            }

            try
            {
                MargemMedicoEntity newMargemMedico = _mapper.Map(request, new MargemMedicoEntity());
                newMargemMedico.ServicoId = servicoGuid;
                newMargemMedico.MedicoId = medicoGuid;

                MargemMedicoEntity response = await _repository.CreateAsync<MargemMedicoEntity, Guid>(newMargemMedico);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateMargemMedicoAsync(UpdateMargemMedicoRequest request, Guid id)
        {
            MargemMedicoEntity margemMedicoInDb = await _repository.GetByIdAsync<MargemMedicoEntity, Guid>(id);
            if (margemMedicoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("MargemMedico não encontrado");
            }

            if (!Guid.TryParse(request.ServicoId, out Guid servicoGuid))
            {
                return ResponseFactory.Fail<Guid>("ServicoId inválido");
            }
            if (!Guid.TryParse(request.MedicoId, out Guid medicoGuid))
            {
                return ResponseFactory.Fail<Guid>("MedicoId inválido");
            }

            MargemMedicoEntity updatedMargemMedico = _mapper.Map(request, margemMedicoInDb);
            updatedMargemMedico.ServicoId = servicoGuid;
            updatedMargemMedico.MedicoId = medicoGuid;

            try
            {
                MargemMedicoEntity response = await _repository.UpdateAsync<MargemMedicoEntity, Guid>(updatedMargemMedico);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteMargemMedicoAsync(Guid id)
        {
            try
            {
                MargemMedicoEntity? margemMedico = await _repository.RemoveByIdAsync<MargemMedicoEntity, Guid>(id);
                if (margemMedico == null)
                {
                    return ResponseFactory.Fail<Guid>("MargemMedico não encontrado");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(margemMedico.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMargemMedicoAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach (Guid id in idsList)
                {
                    try
                    {
                        MargemMedicoEntity? entity = await _repository.GetByIdAsync<MargemMedicoEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"MargemMedico com ID {id} não encontrado.");
                            continue;
                        }

                        MargemMedicoEntity? deletedEntity = await _repository.RemoveByIdAsync<MargemMedicoEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"MargemMedico com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"MargemMedico com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} margens de médico.";
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, message);
                }
                else
                {
                    return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
