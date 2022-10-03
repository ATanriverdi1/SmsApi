using System.Text;
using System.Text.Json;
using System.Xml;
using SmsApi.Extensions;
using SmsApi.Interfaces;
using SmsApi.Models;

namespace SmsApi.Providers;

public class GuvenTelekomProvider : ISmsProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SmsSettings _settings;

    public GuvenTelekomProvider(IHttpClientFactory httpClientFactory, SmsSettings settings)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings;
    }

    public async Task<SendSmsResponse> SendMessage(SendSmsRequest request)
    {
        var guvenTelekomRequest = GetGuvenTelekomRequest(request);
        
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _settings.Url);
        httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(guvenTelekomRequest), Encoding.UTF8,
            "application/x-www-form-urlencoded");
        var response = await httpClient.SendAsync(httpRequestMessage);
        var message = await response.Content.ReadAsStringAsync();
        
        return new SendSmsResponse(message);
    }

    private string GetGuvenTelekomRequest(SendSmsRequest request)
    {
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.Unicode,
            Indent = true,
            IndentChars = ("	")
        };
        
        using (var writer = XmlWriter.Create(sb, settings))
        {
            writer.WriteStartElement("sms");
            writer.WriteElementString("username", _settings.Username);
            writer.WriteElementString("password", _settings.Password);
            writer.WriteElementString("header", _settings.Orginator);
            writer.WriteElementString("validity", "2880");
            writer.WriteStartElement("message");
            writer.WriteStartElement("gsm");
            writer.WriteElementString("no", request.Number);
            writer.WriteEndElement(); //gsm
            writer.WriteStartElement("msg");
            writer.WriteCData(request.Message.ReplaceTrChar());
            writer.WriteEndElement(); //msg 
            writer.WriteEndElement(); //message 
            writer.WriteEndElement(); // sms 
            writer.Flush();
        }
        return sb.ToString();
    }
}