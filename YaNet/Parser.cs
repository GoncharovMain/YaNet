using System.Text;
using YaNet.Features;
using YaNet.Exceptions;

namespace YaNet
{
	public class Cascade
	{
		private int _countEndRow;
		private int _countDelimiterKey;
		private int _countIndent;


		private Row[] _rows;

		private StringBuilder _buffer;
		private int _current;
		private Mark _mark;

		private delegate void Increaser();

		public Cascade(string text) : this(new StringBuilder(text)) { }

		public Cascade(StringBuilder text)
		{
			_countEndRow = 0;
			_countDelimiterKey = 0;
			_countIndent = 0;
			_buffer = text;

			_mark = new Mark(0, 1);

			_current = 0;
		}
		
		public Row Analize()
		{	
			int countRow = new Peeker(_buffer).Counter('\n') + 1;

			_rows = new Row[countRow];



			int currentPosition = 0;

			while(currentPosition < _buffer.Length)
			{
				currentPosition = FeatureRow(currentPosition);

				currentPosition++;				
				_current++;
			}

			// validate indent

			if (_rows[0].Indent > 0)
			{
				throw new IndentException($"'{new Peeker(_buffer, _rows[0].Mark)}' not correct indent.");
			}

			for (int i = 1; i < _rows.Length; i++)
			{
				if (_rows[i].Indent - _rows[i - 1].Indent > 1)
				{
					throw new IndentException($"'{new Peeker(_buffer, _rows[i].Mark)}' not correct indent.");
				}
			}

			return null;
		}

		private int FeatureRow(int lastPosition)
		{
			int indent = new Peeker(_buffer, lastPosition).CountIndent("\t");

			int start = lastPosition;

			Mark mark = new Mark(lastPosition, _buffer.Length - 1);

			for (; lastPosition < _buffer.Length; lastPosition++)
			{
				if (_buffer[lastPosition] == '\n')
				{
					mark = new Mark(start, lastPosition);
					break;
				}
			}

			
			TypeQualifier qualifier = new TypeQualifier(_buffer, new Row(indent, mark));

			Row row = qualifier.QualifyFeature();

			_rows[_current] = row;
			

			return lastPosition;
		}

		public Row this[int row] => _rows[row];
	}

	public class Parser
	{
		private StringBuilder _buffer;

		public Parser(StringBuilder text)
		{
			_buffer = text;
		}

		public Parser(string text) : this(new StringBuilder(text)) { }


		public void Deserialize()
		{
			
		}
	}
}