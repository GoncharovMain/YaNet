namespace YaNet
{
	public class Peeker
	{
		private string _buffer;
		private int _length;

		public Peeker(string buffer) => (_buffer, _length) = (buffer, buffer.Length);

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
	}
}