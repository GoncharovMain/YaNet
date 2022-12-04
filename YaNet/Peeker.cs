namespace YaNet
{
	public class Peeker
	{
		private string _buffer;
		private int _length;

		public Peeker(string buffer) => (_buffer, _length) = (buffer, buffer.Length);

		public int Peek(char waitSymbol) => Peek(waitSymbol, 0);

		public int Peek(char waitSymbol, int startIndex)
		{
			if (0 > startIndex || startIndex >= _length)
				throw new Exception($"Start index[{startIndex}] is out of range buffer.");

			for (int i = startIndex; i < _length; i++)
			{
				if (_buffer[i] == waitSymbol)
					return i;
			}

			return _length;
		}

		public int Peek(string waitSubstring, int startIndex)
		{
			return -1;
		}

		public string Substring(int startIndex, int endIndex)
		{
			if (startIndex < 0 || startIndex > endIndex)
				throw new Exception($"Start[{startIndex}] or end[{endIndex}] index not valid.");

			return _buffer.Substring(startIndex, endIndex - startIndex);
		}

		public static int FindSubstring(string symbols, string substring)
			=> FindSubstring(symbols.ToCharArray(), substring.ToCharArray());

		public static int FindSubstring(char[] symbols, char[] substring)
		{
			if (substring.Length == 0 || symbols.Length == 0)
				throw new Exception("Characters is empty.");

			if (symbols.Length < substring.Length)
				throw new Exception("Length symbols less than length substring.");

			int maxLength = symbols.Length - substring.Length;
			bool hasSubstring = true;

			for (int i = 0; i < maxLength; i++)
			{
				for (int j = 0; j < substring.Length; j++)
				{
					if (symbols[i + j] != substring[j])
					{
						hasSubstring = false;
						break;
					}
				}

				if (hasSubstring)
				{
					return i;
				}

				hasSubstring = true;
			}
		
			return -1;
		}

		public static int CountIndent(string line, string indent)
			=> CountIndent(line.ToCharArray(), indent.ToCharArray());

		public static int CountIndent(char[] line, char[] indent)
		{
			if (line.Length <= indent.Length)
				throw new Exception("The line length cannot be less than the indent length or equal.");

			int bias = indent.Length;

			int countIndent = 0;

			for (int i = 0; i < line.Length; i += bias)
			{
				for (int j = 0; j < bias; j++)
				{
					if (line[i + j] != indent[j])
					{
						return countIndent;
					}
				}
				
				countIndent++;
			}

			return countIndent;
		}
	}

	public class Offset
	{
		public int NumberLine { get; set; }
		public int StartPosition { get; set; }
		public int EndPosition { get; set; }
		public int LengthLine => StartPosition - EndPosition + 1;

		public Offset(int number, int startPosition, int endPosition)
		{
			NumberLine = number;
			StartPosition = startPosition;
			EndPosition = endPosition;
		}
	}
}