using MailKit;
using MiddleApi.Services.Models;

public interface IMailService
{
    public Task SendMailAsync(MailData mailData);
}