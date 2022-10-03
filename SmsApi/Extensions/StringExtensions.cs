using System.Globalization;
using System.Text;

namespace SmsApi.Extensions;

public static class StringExtensions
{
    public static string ReplaceTrChar(this string message)
    {
        var unaccentedText  = string.Join("", message.Normalize(NormalizationForm.FormD)
            .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));

        return unaccentedText;
    }
}