using MySystem.Models;

namespace MySystem.Services
{
	public interface IEmailSenderAsync
	{ 
		Task SendConfirmationLinkAsync(EmailContent Content);
	}
}
