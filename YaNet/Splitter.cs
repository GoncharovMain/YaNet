using System.Text;
using YaNet.Lines;

namespace YaNet
{
	public class Splitter
	{
		private StringBuilder _buffer;

		private Offset[] _offsets;

		public Offset[] Offsets => _offsets;

		private Line[] _lines;

		private int[] _lengthIndents;

		public int Length => _lines.Length;

		public Splitter(string text) : this(new StringBuilder(text)) { }

		public Splitter(StringBuilder text) => _buffer = text;

		public Splitter(StringBuilder text, Offset[] offsets)
		{
			_buffer = text;
			_offsets = offsets;

			_lines = new Line[offsets.Length];

			for (int i = 0; i < offsets.Length; i++)
			{
				_lines[i] = new Line(_buffer, offsets[i]);
			}
		}

		public Splitter(Line line) : this(new Line[] { line }) { }


		public Splitter(Line[] lines)
		{
			_buffer = lines[0].Buffer;

			_lines = lines;

			_offsets = lines.Select(line => line.Offset).ToArray();

			_lengthIndents = lines.Select(line => line.CountIndent).ToArray();
		}



		public Splitter SplitLevel(int levelIndent)
		{
			int countLevel = 0;

			for (int i = 0; i < _lines.Length; i++)
			{
				if (_lines[i].CountIndent == levelIndent)
				{
					countLevel++;
				}
			}

			if (countLevel == 0)
			{
				return null;
			}


			int[] indexes = new int[countLevel];

			for (int i = 0, j = 0; i < _offsets.Length; i++)
			{
				if (_lines[i].CountIndent == levelIndent)
				{
					indexes[j++] = i;
				}
			}

			Offset[] offsetsLevel = new Offset[countLevel];

			for (int i = 0; i < indexes.Length - 1; i++)
			{
				offsetsLevel[i] = new Offset(_offsets[indexes[i]].Start, _offsets[indexes[i + 1]].Start - 1);
			}

			offsetsLevel[^1] = new Offset(_offsets[indexes[^1]].Start, _offsets[^1].End);


			return new Splitter(_buffer, offsetsLevel);
		}

		public Line this[int index] => new Line(_buffer, _offsets[index]);
		
	}
}