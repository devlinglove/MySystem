
using MimeKit;
using MailKit.Net.Smtp;
using MySystem.Models;
using System.Text;


namespace MySystem.Services
{
	public class MimeKitEmailService : IEmailSenderAsync
	{
		private readonly ILogger<MimeKitEmailService> _logger;

		public MimeKitEmailService(ILogger<MimeKitEmailService> logger)
		{
			_logger = logger;
		}
		public async Task SendConfirmationLinkAsync(EmailContent content)
		{
			var htmlBuilder = new StringBuilder();
			htmlBuilder.AppendLine("<html>");
			htmlBuilder.AppendLine(@"<style type=""text/css""> 
				@import url('https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap');
				p, h1, h2, h3, h4, ol, li, ul { font-family: ""Roboto"", sans-serif; } 
			</style>");
			htmlBuilder.AppendLine("<body>");
			htmlBuilder.AppendLine(content.Body);
			htmlBuilder.AppendLine("</body>");
			htmlBuilder.AppendLine("</html>");

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Umar", content.From));
			message.To.Add(new MailboxAddress("Salman", content.To));
			message.Subject = content.Subject;
			message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = htmlBuilder.ToString()
			};

			try
			{
				using (var client = new SmtpClient())
				{
					await client.ConnectAsync("smtp.freesmtpservers.com", 25, false);
					//client.Authenticate("your.email@example.com", "your_password");
					await client.SendAsync(message);
					await client.DisconnectAsync(true);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while sending the email.");
			}
			
		}
	}
}
