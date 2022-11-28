namespace YaNet
{
	public class Counter
	{
		private char _indent;

		public Counter(char indent)
		{
			_indent = indent;
		}

		public int LevelIndent(string value)
		{
			int countIndent = 0;

			while (value[countIndent] == _indent)
			{
				countIndent++;
			}

			return countIndent;
		}
	}
}