using System.Text;
using YaNet.Features;

namespace YaNet
{
	public class Cascade
	{
		private int _countEndRow;
		private int _countDelimiterKey;
		private int _countIndent;


		private Row[] _rows = new Row[]
		{
			new KeyRow(indent: 0, row: (0, 7), key: (0, 7)),
			new KeyRow(indent: 1, row: (2, 15), key: (0, 17)),
			new KeyValueRow(indent: 2, row: (3, 17), key: (0, 0), value: (0, 0)),
			new KeyValueRow(indent: 2, row: (0, 0), key: (0, 0), value: (0, 0)),
			// new KeyValueRow(),
			// new KeyValueRow(),
			// new KeyValueRow(),
			// new KeyRow(),
			// new KeyValueRow(),
			// new KeyValueRow(),
			// new KeyValueRow(),
			// new KeyRow(),
			new ItemRow(indent: 1, row: (0, 0), item: (0, 0)),
			// new ItemRow(),
			// new ItemRow(),
		};

		private StringBuilder _buffer;
		private int _current;
		private Mark _mark;

		private delegate void Increaser();

		private Dictionary<int, Increaser> _features;

		public Cascade(string text) : this(new StringBuilder(text)) { }

		public Cascade(StringBuilder text)
		{
			_countEndRow = 0;
			_countDelimiterKey = 0;
			_countIndent = 0;
			_buffer = text;

			_mark = new Mark(0, 1);

			_features = new Dictionary<int, Increaser>
			{
				{ 9, () => { _countIndent++; } },
				{ 10, () => { _countEndRow++; } },
				{ 58, () => { _countDelimiterKey++; } },
				{ -1, () => { return; } },
			};

			_current = 0;
		}
		
		public Row Analize()
		{	
			// '\t' => 9
			// '\n' => 10
			// ':' => 58

			// 1. отступ
			// 2. признак элемента коллекции
			//   2.1. признак элемента как объекта
			//   2.2. признак элемента как простого элемента
			// 3. признак ключ:
			//   3.1. признак ключ: значение
			//   3.2. признак ключ: ссылка
			//   3.2. признак ключ: объект

			// count row


			int countRow = new Peeker(_buffer).Counter('\n') + 1;

			Console.WriteLine(countRow);

			_rows = new Row[countRow];



			int currentPosition = 0;

			while(currentPosition < _buffer.Length)
			{
				currentPosition = FeatureRow(currentPosition + 1);

				_current++;

				Console.WriteLine(currentPosition);
			}


			return null;
		}

		private int FeatureRow(int lastPosition)
		{
			int indent = new Peeker(_buffer, lastPosition).CountIndent("\t");

			Console.WriteLine($"indent: {indent}");

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

		public void IncreaseFeature(char symbol)		
		{
			switch(symbol)
			{
				case '\n': _countEndRow++; break;
				case '\t': _countIndent++; break;
				case ':': _countDelimiterKey++; break;
				default: break;
			};
		}

		public void Info()
		{
			Console.WriteLine($"end lines: {_countEndRow} delimiters: {_countDelimiterKey} indents: {_countIndent}");
		}
		
		public void Next()
		{

		}

		public void Current()
		{

		}
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