namespace MySystem.Models
{
	public record RegisterInputModel
	{
		public RegisterInputModel()
		{
		}
		public RegisterInputModel(RegisterInputModel model)
		{
			Email = model.Email;
			Password = model.Password;
			FullName = model.FullName;
		}

		public string Email { get; set; }
		public string FullName { get; set; }
		public string Password { get; set; }
	}
}