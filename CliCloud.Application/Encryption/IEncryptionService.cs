using CliCloud.Application.Common.Marker;

namespace CliCloud.Infrastructure.Encryption
{
  /// <summary>
  /// Contrato de encriptação usado por camadas "Application" e "WebApi".
  /// Mantém o namespace histórico para não espalhar mudanças nos imports.
  /// </summary>
  public interface IEncryptionService : ISingletonService
  {
    string EncryptString(string plainText);
    string DecryptString(string cipherText);
  }
}

