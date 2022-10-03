using SmsApi.Interfaces;
using SmsApi.Models;
using SmsApi.Utils;

namespace SmsApi.Providers;

public class SmartMessageProvider : ISmsProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SmsSettings _settings;

    public SmartMessageProvider(IHttpClientFactory httpClientFactory, SmsSettings settings)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings;
    }

    public async Task<SendSmsResponse> SendMessage(SendSmsRequest request)
    {
        var smartMessageUrl = GetSmartMessageUrlWithQuery(request);
        
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, smartMessageUrl);
        var response = await httpClient.SendAsync(httpRequestMessage);
        var message = await response.Content.ReadAsStringAsync();
        
        return new SendSmsResponse(message);
    }

    private string GetSmartMessageUrlWithQuery(SendSmsRequest request)
    {
        var query = new Dictionary<string, string>()
        {
            ["UserName"] = _settings.Username,
            ["Password"] = _settings.Password,
            ["JobId"] = _settings.Orginator.Split('|')[1],
            ["Message"] = request.Message,
            ["MobilePhone"] = request.Number,
            ["CustomerNo"] = _settings.Orginator.Split('|')[0],
            ["PlannedSendingDate"] = DateTime.Now.AddMinutes(1).ToLongDateString()
        };

        var requestUriWithQuery = RequestUriUtil.GetUriWithQueryString(_settings.Url, query);
        return requestUriWithQuery;
    }
}