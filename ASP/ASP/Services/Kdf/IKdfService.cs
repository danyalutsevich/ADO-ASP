namespace ASP.Services.Kdf
{
	public interface IKdfService
	{
		string GetDerivedKey(string password, string salt);
	}
}
