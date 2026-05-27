#nullable enable 

using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CliCloud.Application.Utility;

public static class SaftHash
{
    private const string PrivateKeyString = 
        "<RSAKeyValue><Modulus>vPjBf4OTssqtbjwwSc/W8KsD6Kqa6aH3elk6vTGCR+scmlm4Ob5ghI/Rv/XsAsk8dzuowWTkQ0pzqwU+25p3baspc2TuVpq5HXT20KPZV0ME78HCiz5VBJrdSqwe+8MIc8c26340C3eClof0asJfwX1Lwo7jq7NrTmfXDC8ZXtk=</Modulus><Exponent>AQAB</Exponent><P>4Mf1uZE2YShTenqNOSZq5Ei1uGJhlqwEjPvfpyuQV8I+OFfreL6CtxPQFJUd0dM46WW5916EzxjtN+KYvcVaHw==</P><Q>1zeZ8Lj9yQpWrC4lwlQJVyhIcWFwWsgOcdpAwuDz8LWLP0liLvzmDHrbo9LUrei3qWCaNWnDoSlD8qN3258YBw==</Q><DP>L3ZJTkt5Hf6z0bK6ywxvmZA4vpwXWwcrVtGAIf4y1jTviloWIEKpuxp130K1Ly7YX3/ZSoAsrsXmoNw5wChBpQ==</DP><DQ>W1DFKWVkkQuo8M93x/deKzP1YN4mDO67akVfmxBVkLmGxqh9V1CIz5zFWsJW6R9H5j8Nz0H79oKKyQXhEjdzuw==</DQ><InverseQ>HYtl++EZbozA96kuGuauHF8KsN2SktKnePgeFI1k5JMG8wOWNKu1e2F3OlIgdTvLChjDqthma8iBlDfFutljUA==</InverseQ><D>c1Tm06o8NHyaSJMXbZUZu5zEL/YfwcOGRPe8b8eNmdMWi+WCGEvmoEEeZKf0uOaUrriMkCvBXIhN++yhpmmpxQqNYF2C2zoN9MWvy039S1ZOpr57SiBvkUYG4XWdy6nULP2RdNl2muZ1UbQGKUeSr2jSssHRsU1GBIBtsn52A5k=</D></RSAKeyValue>";

    public const int VersaoChave = 1;

    public static string GerarHash(
        DateTime data,
        DateTime dataSistema,
        int codigoTipoDoc,
        string serie,
        int numeroDocumento,
        decimal totalDocumento,
        string hashDocAnterior
    )
    {
        string hash = Assinar(data, dataSistema, codigoTipoDoc, serie, numeroDocumento, totalDocumento, hashDocAnterior);
        return hash;
    }

    public static bool VerificaChave(
        DateTime data,
        DateTime dataSistema,
        int codigoTipoDoc,
        string serie,
        int numeroDocumento,
        decimal totalDocumento,
        string hashDocAtual,
        string hashDocAnterior
    )
    {
        if(string.IsNullOrWhiteSpace(hashDocAtual))
            throw new ArgumentNullException(nameof(hashDocAtual));
        
        string hash = Assinar(data, dataSistema, codigoTipoDoc, serie, numeroDocumento, totalDocumento, hashDocAnterior);
        return hashDocAtual == hash;
    }

    private static string Assinar(
        DateTime data,
        DateTime dataSistema,
        int codigoTipoDoc,
        string serie, 
        int numeroDocumento,
        decimal totalDocumento,
        string hashDocAnterior
    )
    {
        string textoAssinar = TextoAssinar(data, dataSistema, codigoTipoDoc, serie, numeroDocumento, totalDocumento, hashDocAnterior);
        byte[] signature = Assinar(textoAssinar);
        return Convert.ToBase64String(signature);
    }

    private static string TextoAssinar(
        DateTime data,
        DateTime dataSistema,
        int codigoTipoDoc,
        string serie,
        int numeroDocumento,
        decimal totalDocumento,
        string hashDocAnterior
    )
    {
        if(string.IsNullOrWhiteSpace(serie))
            throw new ArgumentNullException(nameof(serie));

        return string.Format(
            "{0};{1};{2} {3}/{4};{5};{6}",
            data.ToString("yyyy-MM-dd"),
            dataSistema.ToString("yyyy-MM-ddTHH:mm:ss"),
            codigoTipoDoc,
            serie,
            numeroDocumento,
            totalDocumento.ToString("0.00", CultureInfo.InvariantCulture),
            hashDocAnterior
        );
    }

    private static byte[] Assinar(string textoAssinar)
    {
        byte[] str = Encoding.GetEncoding("Windows-1252").GetBytes(textoAssinar);

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(PrivateKeyString);

        return rsa.SignData(str, SHA1.Create());
    }
}