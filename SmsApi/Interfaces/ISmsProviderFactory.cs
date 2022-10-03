using SmsApi.Enums;

namespace SmsApi.Interfaces;

public interface ISmsProviderFactory
{
    ISmsProvider Create(SmsProviderType type);
}