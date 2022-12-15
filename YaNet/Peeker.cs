using System.Text;

namespace YaNet
{
	public class Peeker
	{
		private StringBuilder _buffer;
		
		private int _start;
		private int _end;

		private int _length;

		public Mark Mark => new Mark(_start, _end);

		private Mark _mark;

		public int Length => _length;
		public string Buffer => ToString();

		public Peeker(string buffer) : this(buffer, 0, buffer.Length - 1) { }
		
		public Peeker(string buffer, int start) : this(buffer, start, buffer.Length - 1) { }
		
		public Peeker(string buffer, int start, int end) : this(new StringBuilder(buffer), start, end) { }

		public Peeker(string buffer, Mark mark) : this(new StringBuilder(buffer), mark.Start, mark.End) { }


		public Peeker(char[] buffer) : this(new String(buffer)) { }

		public Peeker(char[] buffer, int start) : this(new String(buffer), start) { }

		public Peeker(char[] buffer, int start, int end) : this(new String(buffer), start, end) { }

		public Peeker(StringBuilder buffer) : this(buffer, 0, buffer.Length - 1) { }

		public Peeker(StringBuilder buffer, int start) : this(buffer, start, buffer.Length - 1) { }

		public Peeker(StringBuilder buffer, int start, int end)
		{
			if (start > end)
				throw new Exception($"Start position [{start}] more than end position [{end}].");

			_buffer = buffer;
			_start = start;
			_end = end;

			_mark = new Mark(_start, _end);

			_length = end - start + 1;
		}

		public Peeker(StringBuilder buffer, Mark mark) : this(buffer, mark.Start, mark.End) { }

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

			for (int i = _start, j = 0; i <= _end && j < substring.Length; i++, j++)
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

			for (int i = _start; i <= _end; i++)
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

		public bool Contains(char symbol)
		{
			for (int i = _start; i <= _end; i++)
			{
				if (_buffer[i] == symbol)
				{
					return true;
				}
			}

			return false;
		}

		public bool Contains(string substring)
		{
			bool contain;

			int maxLength = _end - substring.Length + 1;

			for (int i = _start; i <= maxLength; i++)
			{
				contain = true;

				for (int j = 0; j < substring.Length; j++)
				{
					if (_buffer[i + j] != substring[j])
					{
						contain = false;
					}
				}

				if (contain)
					return true;
			}

			return false;
		}

		public Mark[] Split(char delimiter = '\n')
		{
			int countRows = Counter(delimiter) + 1;
			
			Mark[] marks = new Mark[countRows];

			int i = 0;

			int start, end;

			for (start = _start, end = _start; end <= _end; end++)
			{
				if (_buffer[end] == delimiter)
				{
					marks[i++] = new Mark(start, end);
					start = end + 1;
				}
			}

			marks[^1] = new Mark(start, _end);

			return marks;
		}

		public int Counter(char symbol)
		{
			int count = 0;

			for (int i = _start; i <= _end; i++)
			{
				if (_buffer[i] == symbol)
				{
					count++;
				}
			}

			return count;
		}

		public int Counter(string substring)
		{
			int count = 0;

			bool isEqual = true;

			int maxLength = _end - substring.Length;

			for (int i = _start; i <= maxLength; i++)
			{
				for (int j = 0; j < substring.Length; j++)
				{
					if (_buffer[i] != substring[j])
					{
						isEqual = false;
					}
				}
				
				if (isEqual)
				{
					count++;
				}

				isEqual = true;
			}

			return count;
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

		public static bool operator ==(Peeker peeker, string row)
		{
			if (peeker._length != row.Length)
				return false;

			for (int i = peeker._start, j = 0; i <= peeker._end; i++, j++)
			{
				if (peeker._buffer[i] != row[j])
					return false;
			}

			return true;
		}

		public static bool operator !=(Peeker peeker, string row)
			=> !(peeker == row);

		public override bool Equals(object obj)
			=> this == (string)obj;

		public override int GetHashCode() => Buffer.GetHashCode();
		

		public override string ToString() => _buffer.ToString(_start, _length);

		public string ToCharCode()
		{
			int idx = _start;
			return String.Join("", Buffer.Select(ch => $"[{idx++}:{(int)ch}]"));
		}
	}
}