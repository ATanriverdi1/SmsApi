using SmsApi.Models;

namespace SmsApi.Interfaces;

public interface ISmsProvider
{
    Task<SendSmsResponse> SendMessage(SendSmsRequest request);
}