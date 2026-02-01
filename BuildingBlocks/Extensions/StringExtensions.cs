using System.Text;

namespace WT.B2C.API.BuildingBlocks.Extensions;

public static class StringExtensions
{
    public static string RemovePostfix(this string str, params string[] postfixes)
    {
        if (str == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        if (!postfixes.Any())
        {
            return str;
        }

        foreach (string text in postfixes)
        {
            if (str.EndsWith(text))
            {
                return str.Left(str.Length - text.Length);
            }
        }

        return str;
    }

    public static string Left(this string str, int len)
    {
        if (str == null)
        {
            throw new ArgumentNullException("str");
        }

        if (str.Length < len)
        {
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        }

        return str.Substring(0, len);
    }

    public static string DecodeBase64String(this string base64Encoded)
    {
        byte[] base64EncodedBytes = Convert.FromBase64String(base64Encoded);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
}