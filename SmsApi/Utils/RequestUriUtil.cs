using System.Text;

namespace SmsApi.Utils;

public static class RequestUriUtil
{
    public static string GetUriWithQueryString(string requestUri, Dictionary<string, string> queryStringParams)
    {
        var startingQuestionMarkAdded = false;
        var sb = new StringBuilder();
        sb.Append(requestUri);
        foreach (var parameter in queryStringParams.Where(_ => true))
        {
            sb.Append(startingQuestionMarkAdded ? '&' : '?');
            sb.Append(parameter.Key);
            sb.Append('=');
            sb.Append(parameter.Value);
            startingQuestionMarkAdded = true;
        }

        return sb.ToString();
    }
}