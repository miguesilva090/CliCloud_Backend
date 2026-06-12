using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.NaturezaDocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Filters;
using CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService
{
    public class NaturezaDocumentoService : INaturezaDocumentoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public NaturezaDocumentoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<NaturezaDocumentoDTO>>> GetNaturezaDocumentoAsync(string keyword = "")
        {
            NaturezaDocumentoSearchList spec = new(keyword);
            IEnumerable<NaturezaDocumentoDTO> list = await _repository.GetListAsync<NaturezaDocumento, NaturezaDocumentoDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<NaturezaDocumentoLightDTO>>> GetNaturezaDocumentoLightAsync(string keyword = "")
        {
            NaturezaDocumentoSearchList spec = new(keyword);
            IEnumerable<NaturezaDocumentoLightDTO> list = await _repository.GetListAsync<NaturezaDocumento, NaturezaDocumentoLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<NaturezaDocumentoTableDTO>> GetNaturezaDocumentoPaginatedAsync(NaturezaDocumentoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            NaturezaDocumentoSearchTable spec = new(filter.Filters ?? [], dynamicOrder);
            return await _repository.GetPaginatedResultsAsync<NaturezaDocumento, NaturezaDocumentoTableDTO, Guid>(
                filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<NaturezaDocumentoTableDTO>>> GetAllNaturezaDocumentoAsync(NaturezaDocumentoAllFilter? filter)
        {
            try
            {
                filter ??= new NaturezaDocumentoAllFilter();
                string order = filter.GetOrderByString();
                NaturezaDocumentoSearchTable spec = new(filter.Filters ?? [], order);
                IEnumerable<NaturezaDocumentoTableDTO> list = await _repository.GetListAsync<NaturezaDocumento, NaturezaDocumentoTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<NaturezaDocumentoTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<NaturezaDocumentoDTO>> GetNaturezaDocumentoAsync(Guid id)
        {
            try
            {
                NaturezaDocumentoDTO dto = await _repository.GetByIdAsync<NaturezaDocumento, NaturezaDocumentoDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<NaturezaDocumentoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateNaturezaDocumentoAsync(CreateNaturezaDocumentoRequest request)
        {
            string sigla = request.Sigla.Trim();
            if (sigla.Length != 1)
                return ResponseFactory.Fail<Guid>("A sigla deve ter exactamente 1 carácter.");

            NaturezaDocumentoMatchSigla spec = new(sigla);
            if (await _repository.ExistsAsync<NaturezaDocumento, Guid>(spec))
                return ResponseFactory.Fail<Guid>("Já existe uma Natureza de Documento com esta sigla.");

            NaturezaDocumento entity = _mapper.Map<NaturezaDocumento>(request);
            entity.Sigla = sigla;
            entity.Descricao = request.Descricao.Trim();

            try
            {
                NaturezaDocumento created = await _repository.CreateAsync<NaturezaDocumento, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateNaturezaDocumentoAsync(UpdateNaturezaDocumentoRequest request, Guid id)
        {
            NaturezaDocumento? existing = await _repository.GetByIdAsync<NaturezaDocumento, Guid>(id);
            if (existing == null)
                return ResponseFactory.Fail<Guid>("Natureza do Documento não encontrada.");

            string oldSigla = existing.Sigla;
            string newSigla = request.Sigla.Trim();
            if (newSigla.Length != 1)
                return ResponseFactory.Fail<Guid>("A sigla deve ter exactamente 1 carácter.");

            if (!string.Equals(oldSigla, newSigla, StringComparison.Ordinal))
            {
                NaturezaDocumentoMatchSigla duplicateSpec = new(newSigla, id);
                if (await _repository.ExistsAsync<NaturezaDocumento, Guid>(duplicateSpec))
                    return ResponseFactory.Fail<Guid>("Já existe uma Natureza de Documento com esta sigla.");

                TipoDocumentoByNaturezaSiglaSpec tipoSpec = new(oldSigla);
                IEnumerable<TipoDocumento> tipos = await _repository.GetListAsync<TipoDocumento, Guid>(tipoSpec);
                foreach (TipoDocumento tipo in tipos)
                {
                    tipo.Natureza = newSigla;
                    _ = await _repository.UpdateAsync<TipoDocumento, Guid>(tipo);
                }
            }

            existing.Sigla = newSigla;
            existing.Descricao = request.Descricao.Trim();

            try
            {
                _ = await _repository.UpdateAsync<NaturezaDocumento, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(existing.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteNaturezaDocumentoAsync(Guid id)
        {
            try
            {
                NaturezaDocumento? entity = await _repository.GetByIdAsync<NaturezaDocumento, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Natureza do Documento não encontrada.");

                TipoDocumentoByNaturezaSiglaSpec tipoSpec = new(entity.Sigla);
                if (await _repository.ExistsAsync<TipoDocumento, Guid>(tipoSpec))
                    return ResponseFactory.Fail<Guid>("Não é possível eliminar pois existem séries de documento com esta natureza.");

                NaturezaDocumento removed = await _repository.RemoveByIdAsync<NaturezaDocumento, Guid>(id);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(removed.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleNaturezaDocumentoAsync(IEnumerable<Guid> ids)
        {
            List<Guid> ok = [];
            foreach (Guid id in ids.ToList())
            {
                Response<Guid> result = await DeleteNaturezaDocumentoAsync(id);
                if (result.Status == ResponseStatus.Success)
                    ok.Add(id);
                else
                    _repository.ClearChangeTracker();
            }

            if (ok.Count == ids.Count())
                return ResponseFactory.Success<IEnumerable<Guid>>(ok);
            if (ok.Count > 0)
                return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {ids.Count()}.");
            return ResponseFactory.Fail<IEnumerable<Guid>>("Nenhuma natureza do documento eliminada.");
        }
    }
}
