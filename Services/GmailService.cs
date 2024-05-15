using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using MiddleApi.Services.Configurations;
using MiddleApi.Services.Models;

public class GmailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly SmtpClient _smtpClient;

    public GmailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
        _smtpClient = new SmtpClient(_mailSettings.Server, _mailSettings.Port)
        {
            Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password),
            EnableSsl = _mailSettings.EnableSsl
        };
    }

    public async Task SendMailAsync(MailData mailData)
    {
        using (var mailToSend = new MailMessage(from: _mailSettings.UserName, to: mailData.To, subject: mailData.Subject, body: mailData.Body))
        {
            await _smtpClient.SendMailAsync(mailToSend);
        }
    }
}