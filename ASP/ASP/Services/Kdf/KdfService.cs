using ASP.Services.Hash;

namespace ASP.Services.Kdf
{
	public class KdfService : IKdfService
	{
		private readonly IHashService _hashService;
		
		public KdfService(IHashService hashService)
		{
			_hashService = hashService;
		}

		
		public string GetDerivedKey(string password, string salt)
		{
			return _hashService.Hash(password + salt);
		}
	}
}
