using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.Filters;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.Specifications;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService
{
    public class OdontogramaDefinitivoService : IOdontogramaDefinitivoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public OdontogramaDefinitivoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<OdontogramaDefinitivoDTO>>> GetOdontogramaDefinitivoAsync(string keyword = "")
        {
            OdontogramaDefinitivoSearchList specification = new(keyword);
            IEnumerable<OdontogramaDefinitivoDTO> list =
                await _repository.GetListAsync<OdontogramaDefinitivo, OdontogramaDefinitivoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<OdontogramaDefinitivoDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<OdontogramaDefinitivoDTO>> GetOdontogramaDefinitivoPaginatedAsync(OdontogramaDefinitivoTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword) ||
                filter.UtenteId.HasValue ||
                filter.ConsultaId.HasValue)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = (filter.Sorting != null) ? GSHelpers.GenerateOrderByString(filter) : "";
            OdontogramaDefinitivoSearchTable specification =
                new(filter.Keyword, filter.UtenteId, filter.ConsultaId, dynamicOrder);

            PaginatedResponse<OdontogramaDefinitivoDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<OdontogramaDefinitivo, OdontogramaDefinitivoDTO, Guid>(
                    filter.PageNumber,
                    filter.PageSize,
                    specification);

            return pagedResponse;
        }

        // get single by Id
        public async Task<Response<OdontogramaDefinitivoDTO>> GetOdontogramaDefinitivoAsync(Guid id)
        {
            try
            {
                OdontogramaDefinitivoDTO dto =
                    await _repository.GetByIdAsync<OdontogramaDefinitivo, OdontogramaDefinitivoDTO, Guid>(id);
                return ResponseFactory.Success<OdontogramaDefinitivoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<OdontogramaDefinitivoDTO>(ex.Message);
            }
        }

        // get list by Utente + Consulta (odontograma da consulta)
        public async Task<Response<IEnumerable<OdontogramaDefinitivoDTO>>> GetByUtenteConsultaAsync(Guid utenteId, Guid consultaId)
        {
            try
            {
                OdontogramaDefinitivoByUtenteConsultaSpecification specification =
                    new(utenteId, consultaId);

                IEnumerable<OdontogramaDefinitivoDTO> list =
                    await _repository.GetListAsync<OdontogramaDefinitivo, OdontogramaDefinitivoDTO, Guid>(specification);

                return ResponseFactory.Success<IEnumerable<OdontogramaDefinitivoDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<OdontogramaDefinitivoDTO>>(ex.Message);
            }
        }

        // create new linha de odontograma
        public async Task<Response<Guid>> CreateOdontogramaDefinitivoAsync(CreateOdontogramaDefinitivoRequest request)
        {
            // evitar duplicado lógico por consulta/dente/superfície
            OdontogramaDefinitivoMatchLinha matchLinhaSpec =
                new(request.ConsultaId, request.NumeroDente, request.CodigoSuperficie);

            bool linhaExists =
                await _repository.ExistsAsync<OdontogramaDefinitivo, Guid>(matchLinhaSpec);

            if (linhaExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe uma linha de odontograma para este dente/superfície nesta consulta.");
            }

            OdontogramaDefinitivo newLinha =
                _mapper.Map(request, new OdontogramaDefinitivo());

            try
            {
                OdontogramaDefinitivo response =
                    await _repository.CreateAsync<OdontogramaDefinitivo, Guid>(newLinha);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update linha de odontograma
        public async Task<Response<Guid>> UpdateOdontogramaDefinitivoAsync(UpdateOdontogramaDefinitivoRequest request, Guid id)
        {
            OdontogramaDefinitivo linhaInDb =
                await _repository.GetByIdAsync<OdontogramaDefinitivo, Guid>(id);

            if (linhaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            OdontogramaDefinitivo updatedLinha =
                _mapper.Map(request, linhaInDb);

            try
            {
                OdontogramaDefinitivo response =
                    await _repository.UpdateAsync<OdontogramaDefinitivo, Guid>(updatedLinha);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete linha de odontograma
        public async Task<Response<Guid>> DeleteOdontogramaDefinitivoAsync(Guid id)
        {
            try
            {
                OdontogramaDefinitivo? linha =
                    await _repository.RemoveByIdAsync<OdontogramaDefinitivo, Guid>(id);

                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(linha.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}


