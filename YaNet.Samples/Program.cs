namespace YaNet.Samples
{
	public static class DefaultSymbols
	{
		public const string ListDelimiter = "- ";
		public const string DictDelimiter = ": ";

		public const char Delimiter = ':';
		public const char EndLine = '\n';
		public const char SpaceIndent = ' ';
		public const char TabIndent = '\t';
	}

	public class Program
	{
		public static string YamlText => File.ReadAllText(
			Directory.GetCurrentDirectory() + "/ex1.yaml");

		public static void Main()
		{
			string yaml = YamlText;

			char delimiter = DefaultSymbols.Delimiter;
			char endLine = DefaultSymbols.EndLine;


			Peeker peeker = new Peeker(yaml);

			List<string> keys = new List<string>();
			List<string> values = new List<string>();




			int delimiterPosition = peeker.Peek(delimiter);

			string key = peeker.Substring(0, delimiterPosition);
			keys.Add(key);

			int endLinePosition = peeker.Peek(endLine, delimiterPosition);

			string value = peeker.Substring(delimiterPosition + 1, endLinePosition);
			values.Add(value);


			while (endLinePosition < yaml.Length)
			{
				delimiterPosition = peeker.Peek(delimiter, endLinePosition + 1);

				key = peeker.Substring(endLinePosition + 1, delimiterPosition);
				keys.Add(key);

				endLinePosition = peeker.Peek(endLine, delimiterPosition);

				value = peeker.Substring(delimiterPosition + 1, endLinePosition);
				values.Add(value);
			}

			Console.WriteLine();


			for (int i = 0; i < keys.Count; i++)
			{
				Console.WriteLine($"key: {$"'{keys[i]}'",15} value: {$"'{values[i]}'",15}");
			}

			Counter counter = new Counter(DefaultSymbols.TabIndent);

			for (int i = 0; i < keys.Count; i++)
			{
				key = keys[i];

				int levelIndent = counter.LevelIndent(key);

				Console.WriteLine($"key: '{key}' levelIndent: {levelIndent}");
			}

		}
	}
}