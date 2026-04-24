using CliCloud.Application.Common.Marker;

namespace CliCloud.Infrastructure.Encryption
{
  public interface IEncryptionService : ISingletonService
  {
    string EncryptString(string plainText);
    string DecryptString(string cipherText);
  }
}
