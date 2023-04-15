using static System.Net.Mime.MediaTypeNames;

namespace ASP.Services.Hash
{
	public class Md5HashService : IHashService
	{
		public string Hash(string data)
		{
			using var md5 = System.Security.Cryptography.MD5.Create();
			var inputBytes = System.Text.Encoding.UTF8.GetBytes(data);
			var hashBytes = md5.ComputeHash(inputBytes);
			return Convert.ToHexString(hashBytes);
		}
	}
}
