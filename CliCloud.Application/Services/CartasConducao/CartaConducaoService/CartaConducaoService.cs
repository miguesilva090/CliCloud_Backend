using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using CartaConducaoEntity = CliCloud.Domain.Entities.CartaConducao.CartaConducao;
using CliCloud.Application.Services.CartasConducao.CartaConducaoService.DTOs;
using CliCloud.Application.Services.CartasConducao.CartaConducaoService.Filters;
using CliCloud.Application.Services.CartasConducao.CartaConducaoService.Specifications;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoService
{
    public class CartaConducaoService : ICartaConducaoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public CartaConducaoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<CartaConducaoDTO>>> GetCartaConducaoAsync(string keyword = "")
        {
            CartaConducaoSearchList specification = new(keyword);
            IEnumerable<CartaConducaoDTO> list = await _repository.GetListAsync<CartaConducaoEntity, CartaConducaoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<CartaConducaoDTO>>(list);
        }

        // get lightweight list
        public async Task<Response<IEnumerable<CartaConducaoLightDTO>>> GetCartaConducaoLightAsync(string keyword = "")
        {
            CartaConducaoSearchList specification = new(keyword);
            IEnumerable<CartaConducaoLightDTO> list = await _repository.GetListAsync<CartaConducaoEntity, CartaConducaoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<CartaConducaoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<CartaConducaoTableDTO>> GetCartaConducaoPaginatedAsync(CartaConducaoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            CartaConducaoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<CartaConducaoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<CartaConducaoEntity, CartaConducaoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all CartasConducao (non-paginated)
        public async Task<Response<IEnumerable<CartaConducaoTableDTO>>> GetAllCartaConducaoAsync(CartaConducaoAllFilter filter)
        {
            try
            {
                filter ??= new CartaConducaoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                CartaConducaoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<CartaConducaoTableDTO> list = await _repository.GetListAsync<CartaConducaoEntity, CartaConducaoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<CartaConducaoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<CartaConducaoTableDTO>>(ex.Message);
            }
        }

        // get single CartaConducao by Id
        public async Task<Response<CartaConducaoDTO>> GetCartaConducaoAsync(Guid id)
        {
            try
            {
                CartaConducaoDTO dto = await _repository.GetByIdAsync<CartaConducaoEntity, CartaConducaoDTO, Guid>(id);
                return ResponseFactory.Success<CartaConducaoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<CartaConducaoDTO>(ex.Message);
            }
        }

        // create new CartaConducao
        public async Task<Response<Guid>> CreateCartaConducaoAsync(CreateCartaConducaoRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.CodigoCarta))
            {
                CartaConducaoMatchCodigoCarta specification = new(request.CodigoCarta);
                bool CartaConducaoExists = await _repository.ExistsAsync<CartaConducaoEntity, Guid>(specification);
                if (CartaConducaoExists)
                {
                    return ResponseFactory.Fail<Guid>("Carta de condução com este código já existe");
                }
            }

            CartaConducaoEntity newCartaConducao = _mapper.Map(request, new CartaConducaoEntity());

            try
            {
                CartaConducaoEntity response = await _repository.CreateAsync<CartaConducaoEntity, Guid>(newCartaConducao);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update CartaConducao
        public async Task<Response<Guid>> UpdateCartaConducaoAsync(UpdateCartaConducaoRequest request, Guid id)
        {
            CartaConducaoEntity CartaConducaoInDb = await _repository.GetByIdAsync<CartaConducaoEntity, Guid>(id);
            if (CartaConducaoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Carta de condução não encontrada");
            }

            if (!string.IsNullOrWhiteSpace(request.CodigoCarta) && request.CodigoCarta != CartaConducaoInDb.CodigoCarta)
            {
                CartaConducaoMatchCodigoCarta specification = new(request.CodigoCarta);
                bool CodigoExists = await _repository.ExistsAsync<CartaConducaoEntity, Guid>(specification);
                if (CodigoExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe uma carta de condução com este código");
                }
            }

            CartaConducaoEntity updatedCartaConducao = _mapper.Map(request, CartaConducaoInDb);

            try
            {
                CartaConducaoEntity response = await _repository.UpdateAsync<CartaConducaoEntity, Guid>(updatedCartaConducao);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete CartaConducao
        public async Task<Response<Guid>> DeleteCartaConducaoAsync(Guid id)
        {
            try
            {
                CartaConducaoEntity? CartaConducao = await _repository.RemoveByIdAsync<CartaConducaoEntity, Guid>(id);
                if (CartaConducao == null)
                {
                    return ResponseFactory.Fail<Guid>("Carta de condução não encontrada");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(CartaConducao.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple CartasConducao
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleCartaConducaoAsync(IEnumerable<Guid> ids)
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
                        CartaConducaoEntity? entity = await _repository.GetByIdAsync<CartaConducaoEntity, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Carta de condução com ID {id} não encontrada.");
                            continue;
                        }

                        CartaConducaoEntity? deletedEntity = await _repository.RemoveByIdAsync<CartaConducaoEntity, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Carta de condução com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Carta de condução com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    string message = $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} cartas de condução.";
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
