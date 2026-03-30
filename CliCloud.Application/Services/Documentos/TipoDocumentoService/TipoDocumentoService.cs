using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.Filters;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService
{
    public class TipoDocumentoService : ITipoDocumentoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public TipoDocumentoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<TipoDocumentoDTO>>> GetTipoDocumentoAsync(string keyword = "")
        {
            TipoDocumentoSearchList specification = new(keyword);
            IEnumerable<TipoDocumentoDTO> list = await _repository.GetListAsync<TipoDocumento, TipoDocumentoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TipoDocumentoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<TipoDocumentoLightDTO>>> GetTipoDocumentoLightAsync(string keyword = "")
        {
            TipoDocumentoSearchList specification = new(keyword);
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
            TipoDocumentoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
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
                TipoDocumentoSearchTable specification = new(tableFilters, dynamicOrder);
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
                TipoDocumentoDTO dto = await _repository.GetByIdAsync<TipoDocumento, TipoDocumentoDTO, Guid>(id);
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

                TipoDocumentoMatchAbreviatura specification = new(abreviatura);
                IEnumerable<TipoDocumentoDTO> results = await _repository.GetListAsync<TipoDocumento, TipoDocumentoDTO, Guid>(specification);

                TipoDocumentoDTO? tipoDocumento = results.FirstOrDefault();
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
            TipoDocumentoMatchAbreviatura specification = new(request.Abreviatura);
            bool TipoDocumentoExists = await _repository.ExistsAsync<TipoDocumento, Guid>(specification);
            if (TipoDocumentoExists)
            {
                return ResponseFactory.Fail<Guid>("TipoDocumento com esta Abreviatura já existe");
            }

            TipoDocumento newTipoDocumento = _mapper.Map(request, new TipoDocumento());

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
            TipoDocumento TipoDocumentoInDb = await _repository.GetByIdAsync<TipoDocumento, Guid>(id);
            if (TipoDocumentoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("TipoDocumento não encontrado");
            }

            // Verificar se a abreviatura já existe em outro registro
            if (TipoDocumentoInDb.Abreviatura != request.Abreviatura)
            {
                TipoDocumentoMatchAbreviatura specification = new(request.Abreviatura);
                bool AbreviaturaExists = await _repository.ExistsAsync<TipoDocumento, Guid>(specification);
                if (AbreviaturaExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe um TipoDocumento com esta Abreviatura");
                }
            }

            TipoDocumento updatedTipoDocumento = _mapper.Map(request, TipoDocumentoInDb);

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
                TipoDocumento? TipoDocumento = await _repository.RemoveByIdAsync<TipoDocumento, Guid>(id);
                if (TipoDocumento == null)
                {
                    return ResponseFactory.Fail<Guid>("TipoDocumento não encontrado");
                }
                await _repository.SaveChangesAsync();
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

            foreach(Guid id in idsList)
            {
              try
              {
                TipoDocumento? entity = await _repository.GetByIdAsync<TipoDocumento, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"TipoDocumento com ID {id} não encontrado.");
                  continue;
                }

                TipoDocumento? deletedEntity = await _repository.RemoveByIdAsync<TipoDocumento, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"TipoDocumento com ID {id}.");
                }
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
    }
}
