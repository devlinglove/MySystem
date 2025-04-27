namespace MySystem.Infrastructure
{
	public class AuthConfiguration
	{
		public string Key { get; set; } = string.Empty;
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public string ClientUrl { get; set; } = string.Empty;

	}
}
