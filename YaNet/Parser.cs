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
			// new ItemRow(),
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
				// '\t' => 9
				// '\n' => 10
				// ':' => 58

				{ 9, () => { _countIndent++; } },
				{ 10, () => { _countEndRow++; } },
				{ 58, () => { _countDelimiterKey++; } },
				{ -1, () => { return; } },
			};
		}
		
		public Row Analize()
		{	
			// 1. отступ
			// 2. признак элемента коллекции
			//   2.1. признак элемента как объекта
			//   2.2. признак элемента как простого элемента
			// 3. признак ключ:
			//   3.1. признак ключ: значение
			//   3.2. признак ключ: ссылка
			//   3.2. признак ключ: объект



			for (int i = 0; i < _rows.Length; i++)
			{
				_rows[i].Info();
			}


			Mark mark;

			int start = 0;
			int end = 1;

			for (int i = _mark.Start; i < _buffer.Length; i++)
			{
				if (_buffer[i] == '\t')
				{
					_countIndent++;
					continue;
				}

				if (_buffer[i] == '-' && _buffer[i + 1] == ' ')
				{
					//isElement = true;
				}

				if (_buffer[i] == ':' && _buffer[i + 1] == ' ')
				{
					_countDelimiterKey++;
					//isKey = true;
					continue;
				}

				if (_buffer[i] == '\n')
				{
					_countEndRow++;
					continue;
				}
			}

			return null;
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