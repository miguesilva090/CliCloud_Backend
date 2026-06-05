using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;
using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.Specifications;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Faturacao;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService;

public class FicheirosEletronicosService(IRepositoryAsync repository) : IFicheirosEletronicosService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<IEnumerable<FIcheiroEletronicoRegistoTableDTO>>> ListarAsync(
        Guid clinicaId,
        string sigla,
        CancellationToken ct = default)
    {
        if (!FicheiroEletronicoSiglaExtensions.TryParse(sigla, out FicheiroEletronicoSigla siglaEnum))
            return ResponseFactory.Fail<IEnumerable<FIcheiroEletronicoRegistoTableDTO>>("Sigla de ficheiro eletrónico inválida.");

        IEnumerable<FicheiroEletronicoRegisto> rows = await _repository.GetListAsync<FicheiroEletronicoRegisto, Guid>(
            new FicheiroEletronicoRegistosByClinicaSiglaSpec(clinicaId, siglaEnum),
            ct);

        IEnumerable<FIcheiroEletronicoRegistoTableDTO> dto = rows.Select(r => new FIcheiroEletronicoRegistoTableDTO
        {
            Id = r.Id,
            DocumentoId = r.DocumentoId,
            NumeroExibicaoDocumento = r.NumeroDocumentoExibicao,
            Sigla = r.Sigla.ToLegadoSigla(),
            DataGeracao = r.DataGeracao,
            DataDocumento = r.DataDocumento,
        });

        return ResponseFactory.Success(dto);
    }

    public async Task<Response<GerarFicheiroEletronicoResponse>> GerarAsync(
        Guid clinicaId,
        GerarFicheiroEletronicoRequest request,
        CancellationToken ct = default)
    {
        try
        {
            if (!FicheiroEletronicoSiglaExtensions.TryParse(request.Sigla, out FicheiroEletronicoSigla sigla))
                return ResponseFactory.Fail<GerarFicheiroEletronicoResponse>("Sigla de ficheiro eletrónico inválida.");

            Clinica clinica = await _repository.GetByIdAsync<Clinica, Guid>(clinicaId, cancellationToken: ct);
            Documento documento = await FicheiroEletronicoDataHelper.ObterDocumentoOrganismoAsync(_repository, clinicaId, request.DocumentoId, ct);
            Organismo organismo = await FicheiroEletronicoDataHelper.ObterOrganismoAsync(_repository, documento.OrganismoId!.Value, ct);
            DateTime agora = DateTime.Now;
            DateTime? ultima = await FicheiroEletronicoDataHelper.ObterUltimaGeracaoAsync(_repository, clinicaId, sigla, ct);

            FicheiroEletronicoGeradoDTO gerado = sigla switch
            {
                FicheiroEletronicoSigla.SadGnr => FicheiroEletronicoSadGnrHelper.Gerar(
                    documento,
                    clinica,
                    await FicheiroEletronicoDataHelper.ObterLinhasSadGnrAsync(_repository, documento.Id, organismo.Id, ct),
                    agora,
                    ultima),

                FicheiroEletronicoSigla.Adm => FicheiroEletronicoAdmHelper.Gerar(
                    documento,
                    clinica,
                    organismo,
                    await FicheiroEletronicoDataHelper.ObterLinhasAdmAsync(_repository, documento, organismo.Id, ct)),

                FicheiroEletronicoSigla.SadPsp => FicheiroEletronicoSadPspHelper.Gerar(
                    documento,
                    organismo,
                    await FicheiroEletronicoDataHelper.ObterLinhasSadPspAsync(_repository, documento.Id, organismo.Id, ct)),

                _ => throw new InvalidOperationException("Sigla não suportada."),
            };

            _ = await _repository.CreateAsync<FicheiroEletronicoRegisto, Guid>(new FicheiroEletronicoRegisto
            {
                ClinicaId = clinicaId,
                DocumentoId = documento.Id,
                NumeroDocumentoExibicao = documento.NumeroExibicao ?? string.Empty,
                Sigla = sigla,
                DataGeracao = agora,
                DataDocumento = documento.Data,
                NomeFicheiro = gerado.Nome,
            });

            return ResponseFactory.Success(new GerarFicheiroEletronicoResponse
            {
                FicheiroBase64 = Convert.ToBase64String(gerado.Bytes),
                Nome = gerado.Nome,
                NumeroExibicaoDocumento = documento.NumeroExibicao,
                DataDocumento = documento.Data,
                Erros = gerado.Erros,
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<GerarFicheiroEletronicoResponse>(ex.Message);
        }
    }

    public async Task<Response<GerarFicheiroEletronicoResponse>> JuntarAsync(
        Guid clinicaId,
        JuntarFicheirosEletronicosRequest request,
        CancellationToken ct = default)
    {
        try
        {
            Clinica clinica = await _repository.GetByIdAsync<Clinica, Guid>(clinicaId, cancellationToken: ct);
            Documento documento = await FicheiroEletronicoDataHelper.ObterDocumentoOrganismoAsync(_repository, clinicaId, request.DocumentoId, ct);
            Organismo organismo = await FicheiroEletronicoDataHelper.ObterOrganismoAsync(_repository, documento.OrganismoId!.Value, ct);

            FicheiroEletronicoGeradoDTO gerado = FicheiroEletronicoJuntarHelper.Juntar(
                request.Sigla,
                documento,
                clinica,
                organismo,
                request.Ficheiros,
                DateTime.Now);

            return ResponseFactory.Success(new GerarFicheiroEletronicoResponse
            {
                FicheiroBase64 = Convert.ToBase64String(gerado.Bytes),
                Nome = gerado.Nome,
                NumeroExibicaoDocumento = documento.NumeroExibicao,
                DataDocumento = documento.Data,
                Erros = gerado.Erros,
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<GerarFicheiroEletronicoResponse>(ex.Message);
        }
    }
}