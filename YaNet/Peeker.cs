using System.Text;

namespace YaNet
{
	public class Peeker
	{
		
		private string  buff;
		
		public ref string BUFF => ref buff;
		

		private StringBuilder _buffer;
		
		private int _start;
		private int _end;

		private int _length;

		public Offset Offset => new Offset(_start, _end);

		public int Length => _length;
		public string Buffer => ToString();

		public Peeker(string buffer) : this(buffer, 0, buffer.Length - 1) { }
		
		public Peeker(string buffer, int start) : this(buffer, start, buffer.Length - 1) { }
		
		public Peeker(string buffer, int start, int end)
		{
			if (start > end)
				throw new Exception($"Start position [{start}] more than end position [{end}].");

			_buffer = new StringBuilder(buffer);
			_start = start;
			_end = end;

			_length = end - start + 1;
		}


		public Peeker(char[] buffer) : this(new String(buffer)) { }

		public Peeker(char[] buffer, int start) : this(new String(buffer), start) { }

		public Peeker(char[] buffer, int start, int end) : this(new String(buffer), start, end) { }

		public string Substring(int startIndex, int endIndex)
		{
			if (startIndex < 0 || startIndex > endIndex)
				throw new Exception($"Start[{startIndex}] or end[{endIndex}] index not valid.");

			return Buffer.Substring(startIndex, endIndex - startIndex);
		}

		public bool StartWith(string substring)
		{
			if (_length < substring.Length)
				throw new Exception("Length of buffer less than length of substring.");

			for (int i = _start, j = 0; i < _end && j < substring.Length; i++, j++)
			{
				if (_buffer[i] != substring[j])
				{
					return false;
				}
			}

			return true;
		}

		public int IndexOf(char symbol)
		{
			if (0 > _start || _start >= _length)
				throw new Exception($"Start index[{_start}] is out of range buffer.");

			for (int i = _start; i < _end; i++)
			{
				if (_buffer[i] == symbol)
					return i;
			}

			return _length;
		}

		public int IndexOf(char[] substring)
			=> IndexOf(new String(substring));

		public int IndexOf(string substring)
		{
			if (_length == 0 || substring.Length == 0)
				throw new Exception("Characters is empty.");

			if (_length < substring.Length)
				throw new Exception("Length buffer less than length substring.");

			bool hasSubstring = true;

			int maxLength = _end - substring.Length + 2;

			for (int i = _start; i < maxLength; i++)
			{
				for (int j = 0; j < substring.Length; j++)
				{
					if (_buffer[i + j] != substring[j])
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

		public int CountIndent(string indent)
		{
			if (_buffer.Length <= indent.Length)
				throw new Exception("The buffer length cannot be less than the indent length or equal.");

			int bias = indent.Length;

			int countIndent = 0;

			for (int i = _start; i < _buffer.Length; i += bias)
			{
				for (int j = 0; j < bias; j++)
				{
					if (_buffer[i + j] != indent[j])
					{
						return countIndent;
					}
				}
				
				countIndent++;
			}

			return countIndent;
		}

		public static bool operator ==(Peeker peeker, string line)
		{
			if (peeker._length != line.Length)
				return false;

			for (int i = peeker._start, j = 0; i < peeker._end; i++, j++)
			{
				if (peeker._buffer[i] != line[j])
					return false;
			}

			return true;
		}

		public static bool operator !=(Peeker peeker, string line)
			=> !(peeker == line);

		public override bool Equals(object obj)
			=> this == (string)obj;

		public override int GetHashCode() => Buffer.GetHashCode();
		

		public override string ToString() => _buffer.ToString(_start, _length);

		public string ToCharCode()
		{
			return String.Join("", Buffer.Select(ch => $"[{(int)ch}]"));
		}
	}
}