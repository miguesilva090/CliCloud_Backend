#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.EstadosCivis;
using CliCloud.Domain.Entities.Habilitacoes;
using CliCloud.Domain.Entities.Profissoes;
using CliCloud.Domain.Entities.Sexos;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Utility
{
  [Table("EntidadePessoa", Schema = "Utility")]
  public class EntidadePessoa : Entidade
  {
    public DateOnly? DataNascimento {get;set;}
    public Guid? SexoId { get; set; }
    public Sexo? Sexo { get; set; }
    public Guid? EstadoCivilId { get; set; }
    public Entities.EstadosCivis.EstadoCivil? EstadoCivil { get; set; }
    public Guid? HabilitacaoId { get; set; }
    public Habilitacao? Habilitacao { get; set; }
    public Guid? ProfissaoId { get; set; }
    public Profissao? Profissao { get; set; }
    public string? Nacionalidade {get;set;}
    public string? Naturalidade {get;set;}
    public string? NumeroCartaoIdentificacao {get;set;}
    public DateOnly? DataEmissaoCartaoIdentificacao {get;set;}
    public DateOnly? DataValidadeCartaoIdentificacao {get;set;}
    public string? Arquivo {get;set;}
    public string? Carteira {get;set;}
    public string? NomeUtilizador {get;set;}
    public string? UrlFotoAssinatura {get;set;}
    public string? NumeroIdentificacaoBancaria {get;set;}
  }
}