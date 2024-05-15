namespace MiddleApi.Services.Configurations;

public class MailSettings
{
    public required string Server { get; set; }
    public int Port { get; set; }
    public bool EnableSsl{ get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}