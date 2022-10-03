using SmsApi.Enums;
using SmsApi.Interfaces;
using SmsApi.Models;
using SmsApi.Providers;

namespace SmsApi.Factories;

public class SmsProviderFactory : ISmsProviderFactory
{
    private readonly Dictionary<SmsProviderType, ISmsProvider> _providers;
    
    public SmsProviderFactory(IHttpClientFactory httpClientFactory, SmsSettings settings)
    {
        _providers = new Dictionary<SmsProviderType, ISmsProvider>
        {
            {SmsProviderType.GuvenTelekom, new GuvenTelekomProvider(httpClientFactory, settings)},
            {SmsProviderType.NetGsm, new NetGsmProvider(httpClientFactory, settings)},
            {SmsProviderType.SmartMessage, new SmartMessageProvider(httpClientFactory, settings)},
            {SmsProviderType.Verimor, new VerimorProvider(httpClientFactory, settings)}
        };
    }
    public ISmsProvider Create(SmsProviderType type)
    {
        if (_providers.ContainsKey(type))
        {
            return _providers[type];
        }

        throw new NotImplementedException();
    }
}