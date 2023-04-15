namespace ASP.Services.Random
{
	public class RandomService : IRandomService
	{
		private readonly System.Random _random = new();
		private readonly string codeChars = "abcdefghijklmnopqrstuvwxyz0123456789";
		private readonly string safeChars = new string(Enumerable.Range(20, 107).Select(x => (char)x).ToArray());

		public string ConfirmCode(int length)
		{
			return _MakeString(codeChars, length);
		}

		public string Random(int length)
		{
			return _MakeString(safeChars, length);
		}

		private string _MakeString(string source, int length)
		{
			char[] code = new char[length];
			for (int i = 0; i < length; i++)
			{
				code[i] = source[_random.Next(source.Length)];
			}
			return new string(code);
		}

	}
}
