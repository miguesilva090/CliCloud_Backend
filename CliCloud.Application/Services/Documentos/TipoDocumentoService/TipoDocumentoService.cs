using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.Filters;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications;
using CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Specifications;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService
{
    public class TipoDocumentoService : ITipoDocumentoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentClinicaService _currentClinicaService;

        public TipoDocumentoService(IRepositoryAsync repository, IMapper mapper, ICurrentClinicaService currentClinicaService)
        {
            _repository = repository;
            _mapper = mapper;
            _currentClinicaService = currentClinicaService;
        }

        // get full List
        public async Task<Response<IEnumerable<TipoDocumentoDTO>>> GetTipoDocumentoAsync(string keyword = "")
        {
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
            {
                return ResponseFactory.Fail<IEnumerable<TipoDocumentoDTO>>("Clínica atual não definida");
            }

            TipoDocumentoSearchList specification = new(keyword, clinicaId.Value);
            IEnumerable<TipoDocumentoDTO> list = await _repository.GetListAsync<TipoDocumento, TipoDocumentoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TipoDocumentoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<TipoDocumentoLightDTO>>> GetTipoDocumentoLightAsync(string keyword = "")
        {
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
            {
                return ResponseFactory.Fail<IEnumerable<TipoDocumentoLightDTO>>("Clínica atual não definida");
            }

            TipoDocumentoSearchList specification = new(keyword, clinicaId.Value);
            IEnumerable<TipoDocumentoLightDTO> list = await _repository.GetListAsync<TipoDocumento, TipoDocumentoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TipoDocumentoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<TipoDocumentoTableDTO>> GetTipoDocumentoPaginatedAsync(TipoDocumentoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
            {
                return new PaginatedResponse<TipoDocumentoTableDTO>([], 0, filter.PageNumber, filter.PageSize);
            }

            TipoDocumentoSearchTable specification = new(filter.Filters ?? [], clinicaId.Value, dynamicOrder);
            PaginatedResponse<TipoDocumentoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<TipoDocumento, TipoDocumentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all TipoDocumentos (non-paginated)
        public async Task<Response<IEnumerable<TipoDocumentoTableDTO>>> GetAllTipoDocumentoAsync(TipoDocumentoAllFilter filter)
        {
            try
            {
                filter ??= new TipoDocumentoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                Guid? clinicaId = await ResolveClinicaIdAsync();
                if (!clinicaId.HasValue)
                {
                    return ResponseFactory.Fail<IEnumerable<TipoDocumentoTableDTO>>("Clínica atual não definida");
                }

                TipoDocumentoSearchTable specification = new(tableFilters, clinicaId.Value, dynamicOrder);
                IEnumerable<TipoDocumentoTableDTO> list = await _repository.GetListAsync<TipoDocumento, TipoDocumentoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<TipoDocumentoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<TipoDocumentoTableDTO>>(ex.Message);
            }
        }

        // get single TipoDocumento by Id 
        public async Task<Response<TipoDocumentoDTO>> GetTipoDocumentoAsync(Guid id)
        {
            try
            {
                Guid? clinicaId = await ResolveClinicaIdAsync();
                if (!clinicaId.HasValue)
                {
                    return ResponseFactory.Fail<TipoDocumentoDTO>("Clínica atual não definida");
                }

                TipoDocumentoByIdClinicaSpec specification = new(id, clinicaId.Value);
                TipoDocumentoDTO? dto = (await _repository.GetListAsync<TipoDocumento, TipoDocumentoDTO, Guid>(specification)).FirstOrDefault();
                if (dto == null)
                {
                    return ResponseFactory.Fail<TipoDocumentoDTO>("TipoDocumento não encontrado");
                }
                return ResponseFactory.Success<TipoDocumentoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TipoDocumentoDTO>(ex.Message);
            }
        }

        // get single TipoDocumento by Abreviatura
        public async Task<Response<TipoDocumentoDTO>> GetTipoDocumentoByAbreviaturaAsync(string abreviatura)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(abreviatura))
                {
                  return ResponseFactory.Fail<TipoDocumentoDTO>("Abreviatura não pode ser vazia");
                }

                Guid? clinicaId = await ResolveClinicaIdAsync();
                if (!clinicaId.HasValue)
                {
                    return ResponseFactory.Fail<TipoDocumentoDTO>("Clínica atual não definida");
                }

                TipoDocumentoMatchAbreviatura specification = new(abreviatura, clinicaId.Value);
                IEnumerable<TipoDocumentoDTO> results = await _repository.GetListAsync<TipoDocumento, TipoDocumentoDTO, Guid>(specification);

                TipoDocumentoDTO? tipoDocumento = results
                    .OrderBy(x => x.TipoSerie == "N" ? 0 : 1)
                    .ThenBy(x => x.NumeroSerie)
                    .FirstOrDefault();
                if(tipoDocumento == null)
                {
                  return ResponseFactory.Fail<TipoDocumentoDTO>("Não foi encontrado nenhum TipoDocumento com a Abreviatura fornecida");
                }

                return ResponseFactory.Success<TipoDocumentoDTO>(tipoDocumento);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<TipoDocumentoDTO>(ex.Message);
            }
        }

        // create new TipoDocumento
        public async Task<Response<Guid>> CreateTipoDocumentoAsync(CreateTipoDocumentoRequest request)
        {
            Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
            if (!clinicaIdOpt.HasValue)
            {
                return ResponseFactory.Fail<Guid>("Clínica atual inválida");
            }

            Guid clinicaId = clinicaIdOpt.Value;

            if (string.IsNullOrWhiteSpace(request.NumeroSerie))
            {
                return ResponseFactory.Fail<Guid>("NumeroSerie é obrigatório");
            }

            string numeroSerie = request.NumeroSerie.Trim();
            TipoDocumentoMatchNumeroSerie specSerie = new(numeroSerie, clinicaId);
            if (await _repository.ExistsAsync<TipoDocumento, Guid>(specSerie))
            {
                return ResponseFactory.Fail<Guid>("Já existe um TipoDocumento com este NumeroSerie na clínica");
            }
            // validate natureza sigla
            Response<Guid>? naturezaError = await ValidateNaturezaSiglaAsync(request.Natureza);
            if (naturezaError != null)
                return naturezaError;

            TipoDocumento newTipoDocumento = _mapper.Map(request, new TipoDocumento());
            newTipoDocumento.NumeroSerie = numeroSerie;
            newTipoDocumento.ClinicaId = clinicaId;
            newTipoDocumento.TipoSerie = string.IsNullOrWhiteSpace(newTipoDocumento.TipoSerie)
                ? "N"
                : newTipoDocumento.TipoSerie.Trim();
            newTipoDocumento.PermiteMovimento ??= 1;

            try
            {
                TipoDocumento response = await _repository.CreateAsync<TipoDocumento, Guid>(newTipoDocumento);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update TipoDocumento
        public async Task<Response<Guid>> UpdateTipoDocumentoAsync(UpdateTipoDocumentoRequest request, Guid id)
        {
            Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
            if (!clinicaIdOpt.HasValue)
            {
                return ResponseFactory.Fail<Guid>("Clínica atual inválida");
            }

            Guid clinicaId = clinicaIdOpt.Value;
            TipoDocumentoByIdClinicaSpec getSpec = new(id, clinicaId);
            TipoDocumento? TipoDocumentoInDb = (await _repository.GetListAsync<TipoDocumento, Guid>(getSpec)).FirstOrDefault();
            if (TipoDocumentoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("TipoDocumento não encontrado");
            }

            if (string.IsNullOrWhiteSpace(request.NumeroSerie))
            {
                return ResponseFactory.Fail<Guid>("NumeroSerie é obrigatório");
            }

            string numeroSerie = request.NumeroSerie.Trim();
            if (!string.Equals(TipoDocumentoInDb.NumeroSerie, numeroSerie, StringComparison.Ordinal))
            {
                TipoDocumentoMatchNumeroSerie specSerie = new(numeroSerie, clinicaId);
                bool serieExists = await _repository.ExistsAsync<TipoDocumento, Guid>(specSerie);
                if (serieExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe um TipoDocumento com este NumeroSerie na clínica");
                }
            }

            // validate natureza sigla
            Response<Guid>? naturezaError = await ValidateNaturezaSiglaAsync(request.Natureza);
            if (naturezaError != null)
                return naturezaError;
            

            TipoDocumento updatedTipoDocumento = _mapper.Map(request, TipoDocumentoInDb);
            updatedTipoDocumento.NumeroSerie = numeroSerie;
            updatedTipoDocumento.ClinicaId = clinicaId;

            try
            {
                TipoDocumento response = await _repository.UpdateAsync<TipoDocumento, Guid>(updatedTipoDocumento);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete TipoDocumento
        public async Task<Response<Guid>> DeleteTipoDocumentoAsync(Guid id)
        {
            try
            {
                Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
                if (!clinicaIdOpt.HasValue)
                {
                    return ResponseFactory.Fail<Guid>("Clínica atual inválida");
                }

                Guid clinicaId = clinicaIdOpt.Value;
                TipoDocumentoByIdClinicaSpec getSpec = new(id, clinicaId);
                TipoDocumento? TipoDocumento = (await _repository.GetListAsync<TipoDocumento, Guid>(getSpec)).FirstOrDefault();
                if (TipoDocumento == null)
                {
                    return ResponseFactory.Fail<Guid>("TipoDocumento não encontrado");
                }

                // validate if there are any documentos associated with this tipo documento
                DocumentoByTipoDocumentoIdSpec docSpec = new(TipoDocumento.Id);
                if (await _repository.ExistsAsync<Documento, Guid>(docSpec))
                    return ResponseFactory.Fail<Guid>("Não é possível eliminar pois existem documentos emitidos com esta série");
                await _repository.RemoveAsync<TipoDocumento, Guid>(TipoDocumento);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(TipoDocumento.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple TipoDocumentos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoDocumentoAsync(IEnumerable<Guid> ids)
        {
          try
          {
            List<Guid> idsList = ids.ToList();
            List<Guid> successfullyDeletedIds = [];
            List<string> failedDeletions = [];
            Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
            if (!clinicaIdOpt.HasValue)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>("Clínica atual inválida");
            }

            Guid clinicaId = clinicaIdOpt.Value;

            foreach(Guid id in idsList)
            {
              try
              {
                TipoDocumentoByIdClinicaSpec getSpec = new(id, clinicaId);
                TipoDocumento? entity = (await _repository.GetListAsync<TipoDocumento, Guid>(getSpec)).FirstOrDefault();
                if(entity == null)
                {
                  failedDeletions.Add($"TipoDocumento com ID {id} não encontrado.");
                  continue;
                }

                DocumentoByTipoDocumentoIdSpec docSpec = new(entity.Id);
                if (await _repository.ExistsAsync<Documento, Guid>(docSpec))
                {
                  failedDeletions.Add($"TipoDocumento com ID {id} tem documentos emitidos.");
                  continue;
                }

                await _repository.RemoveAsync<TipoDocumento, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                successfullyDeletedIds.Add(id);
              }
              catch(Exception)
              {
                failedDeletions.Add($"TipoDocumento com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} tipos de documento.";
              return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, message);
            }
            else
            {
              return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ",failedDeletions));
            }
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
          }
        }

        private async Task<Guid?> ResolveClinicaIdAsync()
        {
            await _currentClinicaService.SetClinicaAsync();
            if (Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId) && clinicaId != Guid.Empty)
            {
                return clinicaId;
            }

            return null;
        }

        private async Task<Response<Guid>?> ValidateNaturezaSiglaAsync(string? natureza)
        {
            if (string.IsNullOrWhiteSpace(natureza))
                return null;

            string sigla = natureza.Trim();
            if (sigla.Length != 1)
                return ResponseFactory.Fail<Guid>("A natureza deve ter exatamente 1 caractere");

            NaturezaDocumentoMatchSigla spec = new(sigla);
            if (!await _repository.ExistsAsync<NaturezaDocumento, Guid>(spec))
                return ResponseFactory.Fail<Guid>("Natureza do documento não encontrada");
            
            return null;
        } 
    }
}
