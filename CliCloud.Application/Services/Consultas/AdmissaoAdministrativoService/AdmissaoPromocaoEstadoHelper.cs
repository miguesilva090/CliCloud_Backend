using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

/// <summary>
/// Mapeamento de estados receção/fecho alinhado com ADMISS → HIST (legado).
/// </summary>
internal static class AdmissaoPromocaoEstadoHelper
{
  public static void AplicarEstadosRecepcao(Consulta consulta, Admissao admissao, bool preservarExistentes = false)
  {
    if (!preservarExistentes || !consulta.Confirmado.HasValue)
    {
      consulta.Confirmado = admissao.Confirmado;
    }

    if (!preservarExistentes || !consulta.Efetuado.HasValue)
    {
      consulta.Efetuado = admissao.Efetuado ?? consulta.Efetuado;
    }

    bool? faltou = DerivarFaltou(admissao);
    if (faltou.HasValue && (!preservarExistentes || consulta.Faltou != true))
    {
      consulta.Faltou = faltou;
    }
  }

  public static bool? DerivarFaltou(Admissao admissao)
  {
    if (admissao.StatusConsulta is StatusConsulta.Faltou or StatusConsulta.FaltouJustificada)
    {
      return true;
    }

    if (admissao.StatusConsulta is StatusConsulta.Desmarcada or StatusConsulta.Suspensa)
    {
      return false;
    }

    return null;
  }

  /// <summary>
  /// Estado clínico/admin para histórico; não força Concluída só por fecho administrativo.
  /// </summary>
  public static StatusConsulta? ResolverStatusConsultaHistorico(Admissao admissao)
  {
    if (admissao.StatusConsulta
        is StatusConsulta.Desmarcada
        or StatusConsulta.Suspensa
        or StatusConsulta.Faltou
        or StatusConsulta.FaltouJustificada)
    {
      return admissao.StatusConsulta;
    }

    if (admissao.Efetuado == true)
    {
      return StatusConsulta.Concluida;
    }

    if (admissao.StatusConsulta
        is StatusConsulta.EmAtendimento
        or StatusConsulta.Concluida
        or StatusConsulta.Agendada
        or StatusConsulta.Pendente)
    {
      return admissao.StatusConsulta;
    }

    return null;
  }
}
