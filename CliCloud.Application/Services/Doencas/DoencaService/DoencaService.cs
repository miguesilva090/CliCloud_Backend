using AutoMapper;
using CliCloud.Application.Services.Doencas.DoencaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Doencas;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Doencas.DoencaService.Filters;
using CliCloud.Application.Services.Doencas.DoencaService.Specifications;

namespace CliCloud.Application.Services.Doencas.DoencaService
{
    public class DoencaService : IDoencaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public DoencaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<DoencaDTO>>> GetDoencasAsync(string keyword = "")
        {
            DoencaSearchList specification = new(keyword);
            IEnumerable<DoencaDTO> list = await _repository.GetListAsync<Doenca, DoencaDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<DoencaDTO>>(list);
        }

        public async Task<PaginatedResponse<DoencaDTO>> GetDoencasPaginatedAsync(DoencaTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            DoencaSearchTable specification = new(filter?.Keyword ?? "", dynamicOrder, filter?.ParentId);
            PaginatedResponse<DoencaDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Doenca, DoencaDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<DoencaDTO>> GetDoencaAsync(Guid id)
        {
            try
            {
                DoencaDTO dto = await _repository.GetByIdAsync<Doenca, DoencaDTO, Guid>(id);
                return ResponseFactory.Success<DoencaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<DoencaDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateDoencaAsync(UpdateDoencaRequest request, Guid id)
        {
            try
            {
                Doenca entity = await _repository.GetByIdAsync<Doenca, Guid>(id);
                _ = _mapper.Map(request, entity);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteDoencaAsync(Guid id)
        {
            try
            {
                Doenca? entity = await _repository.RemoveByIdAsync<Doenca, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Doença não encontrada");
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}
