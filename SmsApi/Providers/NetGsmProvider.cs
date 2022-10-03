using System.Text;
using System.Text.Json;
using SmsApi.Interfaces;
using SmsApi.Models;

namespace SmsApi.Providers;

public class NetGsmProvider : ISmsProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SmsSettings _settings;

    public NetGsmProvider(IHttpClientFactory httpClientFactory, SmsSettings settings)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings;
    }

    public async Task<SendSmsResponse> SendMessage(SendSmsRequest request)
    {
        var netGsmRequest = GetNetGsmRequest(request);
        
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _settings.Url);
        httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(netGsmRequest), Encoding.UTF8, "application/x-www-form-urlencoded");
        var response = await httpClient.SendAsync(httpRequestMessage);
        var message = await response.Content.ReadAsStringAsync();

        return new SendSmsResponse(message);
    }

    private string GetNetGsmRequest(SendSmsRequest request)
    {
        var requestXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <mainbody>
                                <header>
                                    <company dil=""TR"" bayikodu=""11111"">Ticimax</company>
                                    <usercode>" + _settings.Username + @"</usercode>
                                    <password>" + _settings.Password + @"</password>
                                    <startdate></startdate>
                                    <stopdate></stopdate>
                                    <type>1:n</type>
                                    <msgheader>" + _settings.Orginator + @"</msgheader>
                                </header>
                                <body>
                                    <msg><![CDATA[" + request.Message + @"]]></msg>
                                    <no>" + request.Number + @"</no>
                                </body>
                            </mainbody>";

        return requestXml;
    }
}