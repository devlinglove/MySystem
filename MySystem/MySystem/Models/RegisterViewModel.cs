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
	}
}
