#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Medicos
{
  [Table("Medico", Schema = "Medicos")]
  public class Medico : EntidadePessoa
  {
    public bool Director { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public Especialidade? Especialidade { get; set; }
    public double? Margem { get; set; }
    public string? LoginPRVR { get; set; }
    public bool ComunicacaoNif { get; set; }
    public bool? ComunicacaoNifAdse { get; set; }
    public string? GrupoFuncional { get; set; }
    public string? Letra { get; set; }
    public int CartaoCidadaoMedico { get; set; }
    public Guid? IdUtilizador { get; set; }
    public bool Globalbooking { get; set; }
    public HorarioMedico? Horario { get; set; }
    

  }
}
