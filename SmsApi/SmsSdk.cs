using System.Net;
using SmsApi.Interfaces;
using SmsApi.Models;

namespace SmsApi;

public class SmsSdk
{
    private readonly SmsSettings _settings;
    private readonly ISmsProviderFactory _providerFactory;

    public SmsSdk(SmsSettings settings,ISmsProviderFactory providerFactory)
    {
        _settings = settings;
        _providerFactory = providerFactory;
    }
    
    public async Task<string> Send(string number, string message)
    {
        number = number.Length switch
        {
            10 => "90" + number,
            11 => "9" + number,
            _ => number.Replace("+", string.Empty).Replace(" ", string.Empty)
        };
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var provider = _providerFactory.Create(_settings.Type);
        var request = new SendSmsRequest(number,message);
        var response = await provider.SendMessage(request);
        return response.Message;
    }
}