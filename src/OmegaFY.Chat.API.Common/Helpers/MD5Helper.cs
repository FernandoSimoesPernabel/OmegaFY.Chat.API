using System.Security.Cryptography;
using System.Text;

namespace OmegaFY.Chat.API.Common.Helpers;

public static class MD5Helper
{
    private static readonly string TO_STRING_FORMAT = "x2";

    public static string ComputeStringHashFromString(string value)
    {
        byte[] md5HashFromObject = MD5.HashData(Encoding.Default.GetBytes(value));

        StringBuilder md5ValueBuilder = new StringBuilder();

        for (int i = 0; i < md5HashFromObject.Length; i++)
            md5ValueBuilder.Append(md5HashFromObject[i].ToString(TO_STRING_FORMAT));

        return md5ValueBuilder.ToString();
    }
}