namespace ASP.Services.Random
{
	public interface IRandomService
	{

		string Random(int length);
		string ConfirmCode(int length);

	}
}
