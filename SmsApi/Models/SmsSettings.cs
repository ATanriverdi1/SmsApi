using SmsApi.Enums;

namespace SmsApi.Models;

public class SmsSettings
{
    public SmsProviderType Type { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Orginator { get; set; }

    public string Url { get; set; }
}