using System.Text;
using YaNet.Rows;

namespace YaNet
{
	public class Cascade
	{
		private int _countEndRow;
		private int _countDelimiterKey;
		private int _countIndent;

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
		
		public void Analize()
		{	
			// 1. отступ
			// 2. признак элемента коллекции
			//   2.1. признак элемента как объекта
			//   2.2. признак элемента как простого элемента
			// 3. признак ключ:
			//   3.1. признак ключ: значение
			//   3.2. признак ключ: ссылка
			//   3.2. признак ключ: объект

			// 1. от


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

		private Row[] _rows;


		public Parser(StringBuilder text)
		{
			_buffer = text;
		}

		public Parser(string text) : this(new StringBuilder(text)) { }

		public void PrintRows()
		{
			for (int i = 0; i < _rows.Length; i++)
			{
				Console.WriteLine($"[{i:00}] [{_rows[i].CountIndent}][{this[i]}]");
			}
		}

		public string this[int i] => _rows[i].Buffer.ToString(_rows[i].Mark.Start, _rows[i].Mark.Length - 1);

		public void Cascade()
		{

		}

		public void Deserialize()
		{
			Peeker peeker = new Peeker(_buffer);


			Mark[] marks = peeker.Split('\n');

			_rows = new Row[marks.Length];


			for (int i = 0; i < _rows.Length; i++)
			{
				_rows[i] = new Row(_buffer, marks[i]);
			}

			for (int i = 0; i < _rows.Length - 1; i++)
			{
				
				// handle variant when row is item of list
				// two spaces "  " and "- "
				//	persons:
				//		- name: John
				//			age: 18
				//		- name: Bob
				// 			age: 25
				//		- name: Patrick
				//			age: 23

				if (_rows[i].CountIndent + 1 < _rows[i + 1].CountIndent)
				{
					throw new Exception($"Row {i + 1}: '{_rows[i + 1]}' has not correct indent.");
				}
			}


			PrintRows();


			


		}

	}
}