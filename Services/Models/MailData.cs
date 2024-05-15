namespace MiddleApi.Services.Models;

public class MailData
{
    public required string To { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}