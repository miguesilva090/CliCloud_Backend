using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums.Documentos;

namespace CliCloud.Application.Services.Documentos.MotorDocumentalService;

public class MotorDocumentalService(
    IRepositoryAsync repository,
    ICurrentClinicaService currentClinicaService
) : IMotorDocumentalService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    private async Task<Guid> ObterClinicaIdAsync()
    {
        await _currentClinicaService.SetClinicaAsync();
        if (Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId) && clinicaId != Guid.Empty)
        {
            return clinicaId;
        }

        throw new InvalidOperationException("Clínica atual inválida.");
    }

    public async Task<Response<IEnumerable<ModeloDocumentoDTO>>> ObterModelosAsync(string keyword = "")
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            IEnumerable<ModeloDocumento> todos = await _repository.GetListAsync<ModeloDocumento, Guid>();

            IEnumerable<ModeloDocumento> query = todos.Where(x => x.ClinicaId == clinicaId);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string k = keyword.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    x.Nome.ToLower().Contains(k) || x.Codigo.ToLower().Contains(k)
                );
            }

            List<ModeloDocumentoDTO> data = query
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new ModeloDocumentoDTO
                {
                    Id = x.Id,
                    Codigo = x.Codigo,
                    Nome = x.Nome,
                    Tipo = x.Tipo,
                    Versao = x.Versao,
                    Ativo = x.Ativo,
                    ConteudoHtml = x.ConteudoHtml
                })
                .ToList();

            return ResponseFactory.Success<IEnumerable<ModeloDocumentoDTO>>(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<ModeloDocumentoDTO>>(ex.Message);
        }
    }

    public async Task<Response<ModeloDocumentoDTO>> ObterModeloPorIdAsync(Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            ModeloDocumento entity = await _repository.GetByIdAsync<ModeloDocumento, Guid>(id);

            if (entity == null || entity.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<ModeloDocumentoDTO>("Modelo não encontrado.");
            }

            return ResponseFactory.Success(new ModeloDocumentoDTO
            {
                Id = entity.Id,
                Codigo = entity.Codigo,
                Nome = entity.Nome,
                Tipo = entity.Tipo,
                Versao = entity.Versao,
                Ativo = entity.Ativo,
                ConteudoHtml = entity.ConteudoHtml
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<ModeloDocumentoDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CriarModeloAsync(CriarModeloDocumentoRequest request)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            IEnumerable<ModeloDocumento> todos = await _repository.GetListAsync<ModeloDocumento, Guid>();

            string codigo = request.Codigo.Trim().ToUpperInvariant();
            bool existe = todos.Any(x => x.ClinicaId == clinicaId && x.Codigo == codigo && x.Versao == 1);
            if (existe)
            {
                return ResponseFactory.Fail<Guid>("Já existe um modelo com esse código (versão 1).");
            }

            ModeloDocumento entity = new()
            {
                Id = Guid.NewGuid(),
                ClinicaId = clinicaId,
                Codigo = codigo,
                Nome = request.Nome.Trim(),
                Tipo = request.Tipo,
                Estado = EstadoModeloDocumento.Rascunho,
                Versao = 1,
                Ativo = true,
                ConteudoHtml = string.IsNullOrWhiteSpace(request.ConteudoHtml)
                    ? "<p></p>"
                    : request.ConteudoHtml
            };

            _ = await _repository.CreateAsync<ModeloDocumento, Guid>(entity);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(entity.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> AtualizarModeloAsync(AtualizarModeloDocumentoRequest request, Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            ModeloDocumento entity = await _repository.GetByIdAsync<ModeloDocumento, Guid>(id);

            if (entity == null || entity.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<Guid>("Modelo não encontrado.");
            }

            entity.Nome = request.Nome.Trim();
            entity.ConteudoHtml = request.ConteudoHtml;
            entity.Ativo = request.Ativo;
            entity.Estado = EstadoModeloDocumento.Rascunho;

            _ = await _repository.UpdateAsync<ModeloDocumento, Guid>(entity);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(entity.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> PublicarNovaVersaoModeloAsync(Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            ModeloDocumento atual = await _repository.GetByIdAsync<ModeloDocumento, Guid>(id);

            if (atual == null || atual.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<Guid>("Modelo não encontrado.");
            }

            ModeloDocumento novaVersao = new()
            {
                Id = Guid.NewGuid(),
                ClinicaId = atual.ClinicaId,
                Codigo = atual.Codigo,
                Nome = atual.Nome,
                Tipo = atual.Tipo,
                Estado = EstadoModeloDocumento.Publicado,
                Versao = atual.Versao + 1,
                Ativo = true,
                ConteudoHtml = atual.ConteudoHtml
            };

            _ = await _repository.CreateAsync<ModeloDocumento, Guid>(novaVersao);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(novaVersao.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<InstanciaDocumentoDTO>> GerarInstanciaAsync(GerarInstanciaDocumentoRequest request)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            ModeloDocumento modelo = await _repository.GetByIdAsync<ModeloDocumento, Guid>(request.ModeloDocumentoId);

            if (modelo == null || modelo.ClinicaId != clinicaId || !modelo.Ativo)
            {
                return ResponseFactory.Fail<InstanciaDocumentoDTO>("Modelo inválido ou inativo.");
            }

            string html = modelo.ConteudoHtml;

            if (request.Marcadores != null)
            {
                foreach ((string key, string value) in request.Marcadores)
                {
                    html = SubstituirMarcador(html, key, value ?? string.Empty);
                }
            }

            if (request.UtenteId.HasValue)
            {
                Utente utente = await _repository.GetByIdAsync<Utente, Guid>(request.UtenteId.Value);
                if (utente != null)
                {
                    html = SubstituirMarcador(html, "UtenteNome", utente.Nome ?? string.Empty);
                    html = SubstituirMarcador(html, "NomeUtente", utente.Nome ?? string.Empty);
                    html = SubstituirMarcador(html, "UtenteContribuinte", utente.NumeroContribuinte ?? string.Empty);
                    html = SubstituirMarcador(html, "UtenteDataNascimento", utente.DataNascimento?.ToString("dd/MM/yyyy") ?? string.Empty);
                    html = SubstituirMarcador(html, "UtenteEmail", utente.Email ?? string.Empty);
                    html = SubstituirMarcador(html, "UtenteNumero", utente.NumeroUtente ?? string.Empty);
                    html = SubstituirMarcador(html, "UtenteNumeroCC", utente.NumeroCartaoIdentificacao ?? string.Empty);
                }
            }

            Clinica clinica = await _repository.GetByIdAsync<Clinica, Guid>(clinicaId);
            string clinicaLocalidade = !string.IsNullOrWhiteSpace(clinica?.Localidade)
                ? clinica.Localidade
                : (clinica?.CCPostal ?? string.Empty);
            html = SubstituirMarcador(html, "ClinicaNome", clinica?.Nome ?? string.Empty);
            html = SubstituirMarcador(html, "ClinicaLocalidade", clinicaLocalidade);
            html = SubstituirMarcador(html, "DataNormal", DateTime.Now.ToString("dd/MM/yyyy"));
            html = SubstituirMarcador(html, "DataPorExtenso", DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("pt-PT")));

            InstanciaDocumento instancia = new()
            {
                Id = Guid.NewGuid(),
                ClinicaId = clinicaId,
                ModeloDocumentoId = modelo.Id,
                VersaoModelo = modelo.Versao,
                UtenteId = request.UtenteId,
                Titulo = string.IsNullOrWhiteSpace(request.Titulo)
                    ? $"{modelo.Nome} - {DateTime.Now:dd/MM/yyyy HH:mm}"
                    : request.Titulo.Trim(),
                ConteudoHtml = html,
                Assinado = false
            };

            _ = await _repository.CreateAsync<InstanciaDocumento, Guid>(instancia);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(new InstanciaDocumentoDTO
            {
                Id = instancia.Id,
                ModeloDocumentoId = instancia.ModeloDocumentoId,
                Titulo = instancia.Titulo,
                ConteudoHtml = instancia.ConteudoHtml,
                UtenteId = instancia.UtenteId,
                Assinado = instancia.Assinado,
                CreatedOn = instancia.CreatedOn
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<InstanciaDocumentoDTO>(ex.Message);
        }
    }

    public async Task<Response<byte[]>> ExportarModeloDocxAsync(Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            ModeloDocumento modelo = await _repository.GetByIdAsync<ModeloDocumento, Guid>(id);

            if (modelo == null || modelo.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<byte[]>("Modelo não encontrado.");
            }

            byte[] docxBytes = GerarDocxDeHtml(modelo.ConteudoHtml, modelo.Nome);
            return ResponseFactory.Success(docxBytes);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<byte[]>(ex.Message);
        }
    }

    public async Task<Response<byte[]>> ExportarInstanciaDocxAsync(Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            InstanciaDocumento instancia = await _repository.GetByIdAsync<InstanciaDocumento, Guid>(id);

            if (instancia == null || instancia.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<byte[]>("Instância não encontrada.");
            }

            byte[] docxBytes = GerarDocxDeHtml(instancia.ConteudoHtml, instancia.Titulo);
            return ResponseFactory.Success(docxBytes);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<byte[]>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<InstanciaDocumentoDTO>>> ObterInstanciasAsync(Guid? modeloId = null)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            IEnumerable<InstanciaDocumento> todos = await _repository.GetListAsync<InstanciaDocumento, Guid>();

            IEnumerable<InstanciaDocumento> query = todos.Where(x => x.ClinicaId == clinicaId);

            if (modeloId.HasValue)
            {
                query = query.Where(x => x.ModeloDocumentoId == modeloId.Value);
            }

            List<InstanciaDocumentoDTO> data = query
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new InstanciaDocumentoDTO
                {
                    Id = x.Id,
                    ModeloDocumentoId = x.ModeloDocumentoId,
                    Titulo = x.Titulo,
                    ConteudoHtml = x.ConteudoHtml,
                    UtenteId = x.UtenteId,
                    Assinado = x.Assinado,
                    CreatedOn = x.CreatedOn
                })
                .ToList();

            return ResponseFactory.Success<IEnumerable<InstanciaDocumentoDTO>>(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<InstanciaDocumentoDTO>>(ex.Message);
        }
    }

    public async Task<Response<Guid>> EliminarModeloAsync(Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            ModeloDocumento modelo = await _repository.GetByIdAsync<ModeloDocumento, Guid>(id);

            if (modelo == null || modelo.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<Guid>("Modelo não encontrado.");
            }

            var removed = await _repository.RemoveByIdAsync<ModeloDocumento, Guid>(id);
            if (removed == null)
                return ResponseFactory.Fail<Guid>("Não foi possível eliminar o modelo.");

            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<FicheiroDocumentoDTO>> RegistarFicheiroInstanciaAsync(RegistarFicheiroDocumentoRequest request)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            InstanciaDocumento instancia = await _repository.GetByIdAsync<InstanciaDocumento, Guid>(request.InstanciaDocumentoId);

            if (instancia == null || instancia.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<FicheiroDocumentoDTO>("Instância não encontrada.");
            }

            FicheiroDocumento ficheiro = new()
            {
                Id = Guid.NewGuid(),
                ClinicaId = clinicaId,
                InstanciaDocumentoId = request.InstanciaDocumentoId,
                NomeOriginal = request.NomeOriginal.Trim(),
                NomeArmazenamento = request.NomeArmazenamento.Trim(),
                CaminhoRelativo = request.CaminhoRelativo.Trim(),
                TipoMime = string.IsNullOrWhiteSpace(request.TipoMime)
                    ? "application/octet-stream"
                    : request.TipoMime.Trim(),
                TamanhoBytes = request.TamanhoBytes,
                ChecksumSha256 = request.ChecksumSha256
            };

            _ = await _repository.CreateAsync<FicheiroDocumento, Guid>(ficheiro);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(new FicheiroDocumentoDTO
            {
                Id = ficheiro.Id,
                InstanciaDocumentoId = ficheiro.InstanciaDocumentoId,
                NomeOriginal = ficheiro.NomeOriginal,
                NomeArmazenamento = ficheiro.NomeArmazenamento,
                CaminhoRelativo = ficheiro.CaminhoRelativo,
                TipoMime = ficheiro.TipoMime,
                TamanhoBytes = ficheiro.TamanhoBytes,
                ChecksumSha256 = ficheiro.ChecksumSha256,
                CreatedOn = ficheiro.CreatedOn
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<FicheiroDocumentoDTO>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<FicheiroDocumentoDTO>>> ObterFicheirosInstanciaAsync(Guid instanciaId)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            IEnumerable<FicheiroDocumento> todos = await _repository.GetListAsync<FicheiroDocumento, Guid>();

            List<FicheiroDocumentoDTO> data = todos
                .Where(x => x.ClinicaId == clinicaId && x.InstanciaDocumentoId == instanciaId)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new FicheiroDocumentoDTO
                {
                    Id = x.Id,
                    InstanciaDocumentoId = x.InstanciaDocumentoId,
                    NomeOriginal = x.NomeOriginal,
                    NomeArmazenamento = x.NomeArmazenamento,
                    CaminhoRelativo = x.CaminhoRelativo,
                    TipoMime = x.TipoMime,
                    TamanhoBytes = x.TamanhoBytes,
                    ChecksumSha256 = x.ChecksumSha256,
                    CreatedOn = x.CreatedOn
                })
                .ToList();

            return ResponseFactory.Success<IEnumerable<FicheiroDocumentoDTO>>(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<FicheiroDocumentoDTO>>(ex.Message);
        }
    }

    public async Task<Response<FicheiroDocumentoDTO>> ObterFicheiroPorIdAsync(Guid ficheiroId)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            FicheiroDocumento entity = await _repository.GetByIdAsync<FicheiroDocumento, Guid>(ficheiroId);

            if (entity == null || entity.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<FicheiroDocumentoDTO>("Ficheiro não encontrado.");
            }

            return ResponseFactory.Success(new FicheiroDocumentoDTO
            {
                Id = entity.Id,
                InstanciaDocumentoId = entity.InstanciaDocumentoId,
                NomeOriginal = entity.NomeOriginal,
                NomeArmazenamento = entity.NomeArmazenamento,
                CaminhoRelativo = entity.CaminhoRelativo,
                TipoMime = entity.TipoMime,
                TamanhoBytes = entity.TamanhoBytes,
                ChecksumSha256 = entity.ChecksumSha256,
                CreatedOn = entity.CreatedOn
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<FicheiroDocumentoDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> EliminarFicheiroAsync(Guid ficheiroId)
    {
        try
        {
            Guid clinicaId = await ObterClinicaIdAsync();
            FicheiroDocumento ficheiro = await _repository.GetByIdAsync<FicheiroDocumento, Guid>(ficheiroId);

            if (ficheiro == null || ficheiro.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<Guid>("Ficheiro não encontrado.");
            }

            var removed = await _repository.RemoveByIdAsync<FicheiroDocumento, Guid>(ficheiroId);
            if (removed == null)
            {
                return ResponseFactory.Fail<Guid>("Não foi possível eliminar o ficheiro.");
            }

            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(ficheiroId);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    /// <summary>
    /// Substitui um marcador no HTML, procurando tanto a versao raw &lt;&lt;Campo&gt;&gt;
    /// como a versao HTML-encoded &amp;lt;&amp;lt;Campo&amp;gt;&amp;gt; (produzida pelo TipTap).
    /// </summary>
    private static string SubstituirMarcador(string html, string campo, string valor)
    {
        html = html.Replace($"<<{campo}>>", valor);
        html = html.Replace($"&lt;&lt;{campo}&gt;&gt;", valor);
        return html;
    }

    private static string RemoverCaracteresInvalidosXml(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var sb = new System.Text.StringBuilder(input.Length);
        foreach (char c in input)
        {
            if (c == 0x9 || c == 0xA || c == 0xD ||
                (c >= 0x20 && c <= 0xD7FF) ||
                (c >= 0xE000 && c <= 0xFFFD))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    private static byte[] GerarDocxDeHtml(string html, string titulo)
    {
        using var memoryStream = new System.IO.MemoryStream();

        using (var wordDocument = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Create(
            memoryStream,
            DocumentFormat.OpenXml.WordprocessingDocumentType.Document,
            autoSave: true))
        {
            var mainPart = wordDocument.AddMainDocumentPart();
            mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(
                new DocumentFormat.OpenXml.Wordprocessing.Body()
            );

            var converter = new HtmlToOpenXml.HtmlConverter(mainPart);

            string wrappedHtml = string.IsNullOrWhiteSpace(html)
                ? "<p></p>"
                : RemoverCaracteresInvalidosXml(html);

            var composites = converter.Parse(wrappedHtml);
            foreach (var element in composites)
            {
                mainPart.Document.Body!.Append(element);
            }

            mainPart.Document.Save();
        }

        return memoryStream.ToArray();
    }
}
