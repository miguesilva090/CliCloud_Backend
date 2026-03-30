using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.Specifications;
using CliCloud.Domain.Entities.ProcessoClinico.Documentos;

namespace CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService
{
    public class DocumentosFichaClinicaService : IDocumentosFichaClinicaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public DocumentosFichaClinicaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<DocumentosFichaClinicaDTO>>> GetByUtenteAsync(Guid utenteId, string? categoria = "Clinico")
        {
            var spec = new DocumentoFichaClinicaByUtenteSpec(utenteId, categoria);
            var entities = await _repository.GetListAsync<DocumentosFichaClinica, Guid>(spec);
            var dtos = _mapper.Map<IEnumerable<DocumentosFichaClinicaDTO>>(entities);
            return ResponseFactory.Success(dtos);
        }

        public async Task<Response<Guid>> CreateAsync(
            CreateDocumentosFichaClinicaRequest request,
            string nomeFicheiro,
            string caminhoRelativo,
            string terminacao,
            string tipo,
            bool isVideo)
        {
            var entity = new DocumentosFichaClinica
            {
                Id = Guid.NewGuid(),
                UtenteId = request.UtenteId,
                Descricao = request.Descricao,
                NomeFicheiro = nomeFicheiro,
                CaminhoRelativo = caminhoRelativo,
                Terminacao = terminacao,
                IsVideo = isVideo,
                Categoria = Enum.TryParse<DocumentoFichaClinicaCategoria>(request.Categoria, true, out var cat)
                    ? cat
                    : DocumentoFichaClinicaCategoria.Clinico,
                Tipo = Enum.TryParse<DocumentoFichaClinicaTipo>(tipo, true, out var t)
                    ? t
                    : DocumentoFichaClinicaTipo.Documento,
            };

            await _repository.CreateAsync<DocumentosFichaClinica, Guid>(entity);
            await _repository.SaveChangesAsync();

            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync<DocumentosFichaClinica, Guid>(id);
            if (entity == null)
            {
                return ResponseFactory.Fail<Guid>("Documento não encontrado.");
            }

            await _repository.RemoveAsync<DocumentosFichaClinica, Guid>(entity);
            await _repository.SaveChangesAsync();

            return ResponseFactory.Success(id);
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IList<Guid> ids)
        {
            var results = new List<Guid>();

            foreach (var id in ids)
            {
                var entity = await _repository.GetByIdAsync<DocumentosFichaClinica, Guid>(id);
                if (entity == null) continue;

                await _repository.RemoveAsync<DocumentosFichaClinica, Guid>(entity);
                results.Add(id);
            }

            await _repository.SaveChangesAsync();

            return ResponseFactory.Success<IEnumerable<Guid>>(results);
        }
    }
}