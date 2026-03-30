#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum TipoFornecedor
  {
    [Display(Name = "Fornecedor Geral")]
    FornecedorGeral = 1,
    
    [Display(Name = "Fornecedor de Serviços")]
    FornecedorServicos = 2,
    
    [Display(Name = "Fornecedor de Produtos")]
    FornecedorProdutos = 3,
    
    [Display(Name = "Fornecedor de Equipamentos")]
    FornecedorEquipamentos = 4,
    
    [Display(Name = "Fornecedor de Medicamentos")]
    FornecedorMedicamentos = 5
  }
}
