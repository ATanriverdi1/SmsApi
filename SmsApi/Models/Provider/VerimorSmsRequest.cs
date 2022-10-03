namespace SmsApi.Models.Provider;

public record VerimorSmsMessageRequest(string msg, string dest);

public record VerimorSmsRequest(string username, string password, string source_addr, string valid_for, string datacoding, List<VerimorSmsMessageRequest> messages);
