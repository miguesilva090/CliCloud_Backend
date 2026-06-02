using CliCloud.Application.Common;

using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

using CliCloud.Domain.Entities.Consultas;

using CliCloud.Domain.Entities.Documentos;

using CliCloud.Domain.Enums;



namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;



internal static class DocumentoEmissaoClinicaSyncHelper

{

  public static async Task SincronizarAposEmissaoAsync(

    IRepositoryAsync repository,

    Guid documentoId,

    Guid tipoDocumentoId,

    IEnumerable<Guid> admissaoServicoIds,

    ModuloOrigemDocumento? moduloOrigemRequest,

    bool faturado = true,

    bool pago = false,

    CancellationToken ct = default

  )

  {

    List<Guid> ids = admissaoServicoIds.Where(x => x != Guid.Empty).Distinct().ToList();

    if (ids.Count == 0)

    {

      return;

    }



    List<AdmissaoServico> servicosDocumento = (

      await repository.GetListAsync<AdmissaoServico, Guid>(

        new AdmissaoServicoByIdsSpec(ids),

        ct

      )

    ).ToList();



    HashSet<Guid> idsNesteDocumento = servicosDocumento.Select(s => s.Id).ToHashSet();



    List<Guid> admissaoIds = servicosDocumento

      .Select(s => s.AdmissaoId)

      .Where(id => id != Guid.Empty)

      .Distinct()

      .ToList();



    ModuloOrigemDocumento modulo =

      moduloOrigemRequest ?? ModuloOrigemDocumento.Consultas;



    foreach (Guid admissaoId in admissaoIds)

    {

      bool jaExiste = (

        await repository.GetListAsync<DocumentoOrigemClinica, Guid>(

          new DocumentoOrigemClinicaByDocumentoAdmissaoSpec(documentoId, admissaoId),

          ct

        )

      ).Any();



      if (!jaExiste)

      {

        Consulta? consultaOrigem = (

          await repository.GetListAsync<Consulta, Guid>(

            new ConsultaPorAdmissaoSpec(admissaoId),

            ct

          )

        ).FirstOrDefault();



        _ = await repository.CreateAsync<DocumentoOrigemClinica, Guid>(

          new DocumentoOrigemClinica

          {

            Id = Guid.NewGuid(),

            DocumentoId = documentoId,

            ModuloOrigem = modulo,

            AdmissaoId = admissaoId,

            ConsultaId = consultaOrigem?.Id,

          }

        );

      }



      List<Admissao> admList = (

        await repository.GetListAsync<Admissao, Guid>(

          new AdmissaoByIdWithServicosSpec(admissaoId),

          ct

        )

      ).ToList();



      Admissao? admissao = admList.FirstOrDefault();

      if (admissao == null)

      {

        continue;

      }



      List<AdmissaoServico> todosServicos = (admissao.Servicos ?? []).ToList();

      if (todosServicos.Count == 0)

      {

        continue;

      }



      List<Guid> todosServicoIds = todosServicos.Select(s => s.Id).ToList();

      HashSet<Guid> jaEmDocumentos = (

        await repository.GetListAsync<DocumentoLinha, Guid>(

          new DocumentoLinhaByAdmissaoServicoIdsSpec(todosServicoIds),

          ct

        )

      )

        .Where(l => l.AdmissaoServicoId.HasValue)

        .Select(l => l.AdmissaoServicoId!.Value)

        .ToHashSet();



      bool admissaoTotalmenteFaturada = todosServicos.All(s =>

        idsNesteDocumento.Contains(s.Id) || jaEmDocumentos.Contains(s.Id)

      );



      bool marcarFaturado = faturado && admissaoTotalmenteFaturada;

      admissao.Faturado = marcarFaturado;

      admissao.Pago = pago;

      _ = await repository.UpdateAsync<Admissao, Guid>(admissao);



      Consulta? consulta = (

        await repository.GetListAsync<Consulta, Guid>(

          new ConsultaPorAdmissaoSpec(admissaoId),

          ct

        )

      ).FirstOrDefault();



      if (consulta != null)

      {

        await AdmissaoFaturacaoPromocaoHelper.SincronizarComDocumentoAsync(

          repository,

          consulta.Id,

          documentoId,

          tipoDocumentoId,

          pago,

          marcarFaturado,

          ct

        );

      }

    }

  }

}

