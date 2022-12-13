using System.Text;
using YaNet.Rows;

namespace YaNet
{
	public class Splitter
	{
		private StringBuilder _buffer;

		private Mark[] _marks;

		public Mark[] Marks => _marks;

		private Row[] _rows;

		private int[] _lengthIndents;

		public int Length => _rows.Length;

		public Splitter(string text) : this(new StringBuilder(text)) { }

		public Splitter(StringBuilder text) => _buffer = text;

		public Splitter(StringBuilder text, Mark[] marks)
		{
			_buffer = text;
			_marks = marks;

			_rows = new Row[marks.Length];

			for (int i = 0; i < marks.Length; i++)
			{
				_rows[i] = new Row(_buffer, marks[i]);
			}
		}

		public Splitter(Row row) : this(new Row[] { row }) { }


		public Splitter(Row[] rows)
		{
			_buffer = rows[0].Buffer;

			_rows = rows;

			_marks = rows.Select(row => row.Mark).ToArray();

			_lengthIndents = rows.Select(row => row.CountIndent).ToArray();
		}



		public Splitter SplitLevel(int levelIndent)
		{
			int countLevel = 0;

			for (int i = 0; i < _rows.Length; i++)
			{
				if (_rows[i].CountIndent == levelIndent)
				{
					countLevel++;
				}
			}

			if (countLevel == 0)
			{
				return null;
			}


			int[] indexes = new int[countLevel];

			for (int i = 0, j = 0; i < _marks.Length; i++)
			{
				if (_rows[i].CountIndent == levelIndent)
				{
					indexes[j++] = i;
				}
			}

			Mark[] marksLevel = new Mark[countLevel];

			for (int i = 0; i < indexes.Length - 1; i++)
			{
				marksLevel[i] = new Mark(_marks[indexes[i]].Start, _marks[indexes[i + 1]].Start - 1);
			}

			marksLevel[^1] = new Mark(_marks[indexes[^1]].Start, _marks[^1].End);


			return new Splitter(_buffer, marksLevel);
		}

		public Row this[int index] => new Row(_buffer, _marks[index]);
		
	}
}