namespace MySystem.Models
{
	public record RegisterViewModel: RegisterInputModel
	{
		public RegisterViewModel() : base()
		{
		}

		public RegisterViewModel(RegisterInputModel inputModel) : base(inputModel)
		{
		}

		public string Id { get; set; } = string.Empty;
	}
}
