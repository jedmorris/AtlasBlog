using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace AtlasBlog.Services;

public class BasicEmailService : IEmailSender
{
    private readonly IConfiguration _appSettings;

    public BasicEmailService(IConfiguration appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        
    var emailSender = _appSettings["SmtpSettings:UserName"];
    
    //Compose an email based partially on the data supplied by the user
    MimeMessage newEmail = new();

    newEmail.Sender = MailboxAddress.Parse(_appSettings["SmtpSettings:UserName"]);
    newEmail.To.Add(MailboxAddress.Parse(email));
    newEmail.Subject = subject;
    
    //Email body
    BodyBuilder emailBody = new();

    emailBody.HtmlBody = htmlMessage;
    newEmail.Body = emailBody.ToMessageBody();
    
    //Configure Smtp server
    using SmtpClient smtpClient = new();

    var host = _appSettings["SmtpSettings: Host"];
    var port = Convert.ToInt32(_appSettings["SmtpSettings:Port"]);
     
    await smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls);
    await smtpClient.AuthenticateAsync(emailSender, _appSettings["SmtpSettings:Password"]);
    await smtpClient.SendAsync(newEmail);
    await smtpClient.DisconnectAsync(true);



    // throw new NotImplementedException();
    }
}