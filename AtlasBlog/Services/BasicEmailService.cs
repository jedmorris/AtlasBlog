using Microsoft.AspNetCore.Identity.UI.Services;

namespace AtlasBlog.Services;

public class BasicEmailService : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        throw new NotImplementedException();
    }
}