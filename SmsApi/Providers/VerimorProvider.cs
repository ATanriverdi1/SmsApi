using System.Text;
using System.Text.Json;
using SmsApi.Constants;
using SmsApi.Interfaces;
using SmsApi.Models;
using SmsApi.Models.Provider;

namespace SmsApi.Providers;

public class VerimorProvider : ISmsProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SmsSettings _settings;

    public VerimorProvider(IHttpClientFactory httpClientFactory, SmsSettings settings)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings;
    }
    public async Task<SendSmsResponse> SendMessage(SendSmsRequest request)
    {
        var verimorRequest = GetSendMessageRequest(_settings, request);
        
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _settings.Url);
        httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(verimorRequest), Encoding.UTF8, "application/json");
        var response = await httpClient.SendAsync(httpRequestMessage);
        var message = await response.Content.ReadAsStringAsync();
        
        return new SendSmsResponse(message);
    }

    private VerimorSmsRequest GetSendMessageRequest(SmsSettings settings, SendSmsRequest request)
    {
        var messageRequest = new VerimorSmsMessageRequest(request.Message, request.Number);
        var verimorRequest = new VerimorSmsRequest(settings.Username, settings.Password, settings.Orginator,
            SmsProviderConstant.Verimor.ValidFor, SmsProviderConstant.Verimor.DataCoding,
            new List<VerimorSmsMessageRequest> {messageRequest});
        return verimorRequest;
    }
}