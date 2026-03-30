using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using TipoEntidadeFinanceiraEntity = CliCloud.Domain.Entities.TipoEntidadeFinanceira.TipoEntidadeFinanceira;
using CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.DTOs;
using CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.Filters;
using CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.Specifications;

namespace CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService
{
    public class TipoEntidadeFinanceiraService : ITipoEntidadeFinanceiraService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public TipoEntidadeFinanceiraService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<TipoEntidadeFinanceiraDTO>>> GetTipoEntidadeFinanceiraAsync(string keyword = "")
        {
            TipoEntidadeFinanceiraSearchList specification = new(keyword);
            IEnumerable<TipoEntidadeFinanceiraDTO> list = await _repository.GetListAsync<TipoEntidadeFinanceiraEntity, TipoEntidadeFinanceiraDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TipoEntidadeFinanceiraDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<TipoEntidadeFinanceiraLightDTO>>> GetTipoEntidadeFinanceiraLightAsync(string keyword = "")
        {
            TipoEntidadeFinanceiraSearchList specification = new(keyword);
            IEnumerable<TipoEntidadeFinanceiraLightDTO> list = await _repository.GetListAsync<TipoEntidadeFinanceiraEntity, TipoEntidadeFinanceiraLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TipoEntidadeFinanceiraLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<TipoEntidadeFinanceiraTableDTO>> GetTipoEntidadeFinanceiraPaginatedAsync(TipoEntidadeFinanceiraTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            TipoEntidadeFinanceiraSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<TipoEntidadeFinanceiraTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<TipoEntidadeFinanceiraEntity, TipoEntidadeFinanceiraTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all TipoEntidadeFinanceiras (non-paginated)
        public async Task<Response<IEnumerable<TipoEntidadeFinanceiraTableDTO>>> GetAllTipoEntidadeFinanceiraAsync(TipoEntidadeFinanceiraAllFilter filter)
        {
            try
            {
                filter ??= new TipoEntidadeFinanceiraAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                TipoEntidadeFinanceiraSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<TipoEntidadeFinanceiraTableDTO> list = await _repository.GetListAsync<TipoEntidadeFinanceiraEntity, TipoEntidadeFinanceiraTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<TipoEntidadeFinanceiraTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<TipoEntidadeFinanceiraTableDTO>>(ex.Message);
            }
        }

        // get single TipoEntidadeFinanceira by Id 
        public async Task<Response<TipoEntidadeFinanceiraDTO>> GetTipoEntidadeFinanceiraAsync(Guid id)
        {
            try
            {
                TipoEntidadeFinanceiraDTO dto = await _repository.GetByIdAsync<TipoEntidadeFinanceiraEntity, TipoEntidadeFinanceiraDTO, Guid>(id);
                return ResponseFactory.Success<TipoEntidadeFinanceiraDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TipoEntidadeFinanceiraDTO>(ex.Message);
            }
        }

        // get single TipoEntidadeFinanceira by Sigla
        public async Task<Response<TipoEntidadeFinanceiraDTO>> GetTipoEntidadeFinanceiraBySiglaAsync(string sigla)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(sigla))
                {
                  return ResponseFactory.Fail<TipoEntidadeFinanceiraDTO>("Sigla não pode ser vazia");
                }

                TipoEntidadeFinanceiraMatchSigla specification = new(sigla);
                IEnumerable<TipoEntidadeFinanceiraDTO> results = await _repository.GetListAsync<TipoEntidadeFinanceiraEntity, TipoEntidadeFinanceiraDTO, Guid>(specification);

                TipoEntidadeFinanceiraDTO? tipoEntidadeFinanceira = results.FirstOrDefault();
                if(tipoEntidadeFinanceira == null)
                {
                  return ResponseFactory.Fail<TipoEntidadeFinanceiraDTO>("Não foi encontrado nenhum TipoEntidadeFinanceira com a Sigla fornecida");
                }

                return ResponseFactory.Success<TipoEntidadeFinanceiraDTO>(tipoEntidadeFinanceira);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<TipoEntidadeFinanceiraDTO>(ex.Message);
            }
        }

        // create new TipoEntidadeFinanceira
        public async Task<Response<Guid>> CreateTipoEntidadeFinanceiraAsync(CreateTipoEntidadeFinanceiraRequest request)
        {
            TipoEntidadeFinanceiraMatchSigla specification = new(request.Sigla);
            bool TipoEntidadeFinanceiraExists = await _repository.ExistsAsync<TipoEntidadeFinanceiraEntity, Guid>(specification);
            if (TipoEntidadeFinanceiraExists)
            {
                return ResponseFactory.Fail<Guid>("TipoEntidadeFinanceira com esta Sigla já existe");
            }

            TipoEntidadeFinanceiraEntity newTipoEntidadeFinanceira = _mapper.Map(request, new TipoEntidadeFinanceiraEntity());

            try
            {
                TipoEntidadeFinanceiraEntity response = await _repository.CreateAsync<TipoEntidadeFinanceiraEntity, Guid>(newTipoEntidadeFinanceira);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update TipoEntidadeFinanceira
        public async Task<Response<Guid>> UpdateTipoEntidadeFinanceiraAsync(UpdateTipoEntidadeFinanceiraRequest request, Guid id)
        {
            TipoEntidadeFinanceiraEntity TipoEntidadeFinanceiraInDb = await _repository.GetByIdAsync<TipoEntidadeFinanceiraEntity, Guid>(id);
            if (TipoEntidadeFinanceiraInDb == null)
            {
                return ResponseFactory.Fail<Guid>("TipoEntidadeFinanceira não encontrado");
            }

            // Verificar se a sigla já existe noutro registo
            if (TipoEntidadeFinanceiraInDb.Sigla != request.Sigla)
            {
                TipoEntidadeFinanceiraMatchSigla specification = new(request.Sigla);
                bool SiglaExists = await _repository.ExistsAsync<TipoEntidadeFinanceiraEntity, Guid>(specification);
                if (SiglaExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe um TipoEntidadeFinanceira com esta Sigla");
                }
            }

            TipoEntidadeFinanceiraEntity updatedTipoEntidadeFinanceira = _mapper.Map(request, TipoEntidadeFinanceiraInDb);

            try
            {
                TipoEntidadeFinanceiraEntity response = await _repository.UpdateAsync<TipoEntidadeFinanceiraEntity, Guid>(updatedTipoEntidadeFinanceira);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete TipoEntidadeFinanceira
        public async Task<Response<Guid>> DeleteTipoEntidadeFinanceiraAsync(Guid id)
        {
            try
            {
                TipoEntidadeFinanceiraEntity? TipoEntidadeFinanceira = await _repository.RemoveByIdAsync<TipoEntidadeFinanceiraEntity, Guid>(id);
                if (TipoEntidadeFinanceira == null)
                {
                    return ResponseFactory.Fail<Guid>("TipoEntidadeFinanceira não encontrado");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(TipoEntidadeFinanceira.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple TipoEntidadeFinanceiras
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoEntidadeFinanceiraAsync(IEnumerable<Guid> ids)
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
                TipoEntidadeFinanceiraEntity? entity = await _repository.GetByIdAsync<TipoEntidadeFinanceiraEntity, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"TipoEntidadeFinanceira com ID {id} não encontrado.");
                  continue;
                }

                TipoEntidadeFinanceiraEntity? deletedEntity = await _repository.RemoveByIdAsync<TipoEntidadeFinanceiraEntity, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"TipoEntidadeFinanceira com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"TipoEntidadeFinanceira com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} tipos de entidade financeira.";
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
