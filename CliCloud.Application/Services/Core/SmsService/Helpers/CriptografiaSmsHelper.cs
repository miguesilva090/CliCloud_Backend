using System.Security.Cryptography;
using System.Text;

namespace CliCloud.Application.Services.Core.SmsService.Helpers
{
    public static class CriptografiaSmsHelper
    {
        private const string Iv = "PoRT_FReG_FjORGm";
        private const string Salt = "_SALT_BAE-";
        private const string Password = "PASSTESTE";

        public static string Cifrar(string textoPlano)
        {
            if(string.IsNullOrWhiteSpace(textoPlano))
                return string.Empty;

            using var csp = new AesCryptoServiceProvider();
            var transform = GetCryptoTransform(csp, true);

            var payload = Encoding.UTF8.GetBytes($"%{textoPlano}%");
            var encrypted = transform.TransformFinalBlock(payload, 0, payload.Length);
            return Convert.ToBase64String(encrypted);
        }

        public static string Decifrar(string textoCifrado)
        {
            if(string.IsNullOrWhiteSpace(textoCifrado))
                return string.Empty;

            using var csp = new AesCryptoServiceProvider();
            var transform = GetCryptoTransform(csp, false);
            
            var input = Convert.FromBase64String(textoCifrado);
            var output = transform.TransformFinalBlock(input, 0, input.Length);
            var texto = Encoding.UTF8.GetString(output);

            if(texto.StartsWith("%") && texto.EndsWith("%") && texto.Length >= 2)
                texto = texto[1..^1];
            
            return texto;
        }

        private static ICryptoTransform GetCryptoTransform(AesCryptoServiceProvider csp, bool encrypt)
        {
            csp.Mode = CipherMode.CBC;
            csp.Padding = PaddingMode.PKCS7;

            var spec = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(Password), Encoding.UTF8.GetBytes(Salt), 65536);
            var key = spec.GetBytes(16);

            csp.IV = Encoding.UTF8.GetBytes(Iv);
            csp.Key = key;

            return encrypt ? csp.CreateEncryptor() : csp.CreateDecryptor();
        }
    }
}