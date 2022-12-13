namespace YaNet
{
	public static class StringExtension
	{
		public static string Repeat(this char symbol, int count)
			=> new String(symbol, count);

		public static string Repeat(this string row, int count)
			=> String.Join(row, count);
	}
}