using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using CartaConducaoRestricaoEntity = CliCloud.Domain.Entities.CartaConducao.CartaConducaoRestricao;
using CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.DTOs;
using CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.Filters;
using CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.Specifications;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService
{
    public class CartaConducaoRestricoesService : ICartaConducaoRestricoesService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public CartaConducaoRestricoesService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<CartaConducaoRestricoesDTO>>> GetCartaConducaoRestricoesAsync(string keyword = "")
        {
            CartaConducaoRestricoesSearchList specification = new(keyword);
            IEnumerable<CartaConducaoRestricoesDTO> list = await _repository.GetListAsync<CartaConducaoRestricaoEntity, CartaConducaoRestricoesDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<CartaConducaoRestricoesDTO>>(list);
        }

        // get lightweight list
        public async Task<Response<IEnumerable<CartaConducaoRestricoesLightDTO>>> GetCartaConducaoRestricoesLightAsync(string keyword = "")
        {
            CartaConducaoRestricoesSearchList specification = new(keyword);
            IEnumerable<CartaConducaoRestricoesLightDTO> list = await _repository.GetListAsync<CartaConducaoRestricaoEntity, CartaConducaoRestricoesLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<CartaConducaoRestricoesLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<CartaConducaoRestricoesTableDTO>> GetCartaConducaoRestricoesPaginatedAsync(CartaConducaoRestricoesTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            CartaConducaoRestricoesSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<CartaConducaoRestricoesTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<CartaConducaoRestricaoEntity, CartaConducaoRestricoesTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all CartaConducaoRestricoes (non-paginated)
        public async Task<Response<IEnumerable<CartaConducaoRestricoesTableDTO>>> GetAllCartaConducaoRestricoesAsync(CartaConducaoRestricoesAllFilter filter)
        {
            try
            {
                filter ??= new CartaConducaoRestricoesAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                CartaConducaoRestricoesSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<CartaConducaoRestricoesTableDTO> list = await _repository.GetListAsync<CartaConducaoRestricaoEntity, CartaConducaoRestricoesTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<CartaConducaoRestricoesTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<CartaConducaoRestricoesTableDTO>>(ex.Message);
            }
        }

        // get single CartaConducaoRestricao by Id
        public async Task<Response<CartaConducaoRestricoesDTO>> GetCartaConducaoRestricoesAsync(Guid id)
        {
            try
            {
                CartaConducaoRestricoesDTO dto = await _repository.GetByIdAsync<CartaConducaoRestricaoEntity, CartaConducaoRestricoesDTO, Guid>(id);
                return ResponseFactory.Success<CartaConducaoRestricoesDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<CartaConducaoRestricoesDTO>(ex.Message);
            }
        }

        // create new CartaConducaoRestricao
        public async Task<Response<Guid>> CreateCartaConducaoRestricoesAsync(CreateCartaConducaoRestricoesRequest request)
        {
            CartaConducaoRestricoesMatchCodigo specification = new(request.CodigoRestricao);
            bool CartaConducaoRestricaoExists = await _repository.ExistsAsync<CartaConducaoRestricaoEntity, Guid>(specification);
            if (CartaConducaoRestricaoExists)
            {
                return ResponseFactory.Fail<Guid>("Restrição com este código já existe");
            }

            CartaConducaoRestricaoEntity newCartaConducaoRestricao = _mapper.Map(request, new CartaConducaoRestricaoEntity());

            try
            {
                CartaConducaoRestricaoEntity response = await _repository.CreateAsync<CartaConducaoRestricaoEntity, Guid>(newCartaConducaoRestricao);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update CartaConducaoRestricao
        public async Task<Response<Guid>> UpdateCartaConducaoRestricoesAsync(UpdateCartaConducaoRestricoesRequest request, Guid id)
        {
            CartaConducaoRestricaoEntity CartaConducaoRestricaoInDb = await _repository.GetByIdAsync<CartaConducaoRestricaoEntity, Guid>(id);
            if (CartaConducaoRestricaoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Restrição não encontrada");
            }

            if (CartaConducaoRestricaoInDb.CodigoRestricao != request.CodigoRestricao)
            {
                CartaConducaoRestricoesMatchCodigo specification = new(request.CodigoRestricao);
                bool CodigoExists = await _repository.ExistsAsync<CartaConducaoRestricaoEntity, Guid>(specification);
                if (CodigoExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe uma restrição com este código");
                }
            }

            CartaConducaoRestricaoEntity updatedCartaConducaoRestricao = _mapper.Map(request, CartaConducaoRestricaoInDb);

            try
            {
                CartaConducaoRestricaoEntity response = await _repository.UpdateAsync<CartaConducaoRestricaoEntity, Guid>(updatedCartaConducaoRestricao);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete CartaConducaoRestricao
        public async Task<Response<Guid>> DeleteCartaConducaoRestricoesAsync(Guid id)
        {
            try
            {
                CartaConducaoRestricaoEntity? CartaConducaoRestricao = await _repository.RemoveByIdAsync<CartaConducaoRestricaoEntity, Guid>(id);
                if (CartaConducaoRestricao == null)
                {
                    return ResponseFactory.Fail<Guid>("Restrição não encontrada");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(CartaConducaoRestricao.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple CartaConducaoRestricoes
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleCartaConducaoRestricoesAsync(IEnumerable<Guid> ids)
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
                        CartaConducaoRestricaoEntity? entity = await _repository.GetByIdAsync<CartaConducaoRestricaoEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Restrição com ID {id} não encontrada.");
                            continue;
                        }

                        CartaConducaoRestricaoEntity? deletedEntity = await _repository.RemoveByIdAsync<CartaConducaoRestricaoEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Restrição com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Restrição com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    string message = $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} restrições.";
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
