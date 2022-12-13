using System.Text;
using YaNet.Rows;

namespace YaNet
{
	public class Cascade
	{

		// ищем признак конца строки
		// 

		private int _countEndRow;
		private int _countDelimiterKey;
		private int _countIndent;

		private StringBuilder _buffer;
		private int _current;
		private Mark _marks;

		public Cascade(string text) : this(new StringBuilder(text)) { }

		public Cascade(StringBuilder text)
		{
			_countEndRow = 0;
			_countDelimiterKey = 0;
			_countIndent = 0;
			_buffer = text;
		}
		
		public void Analize()
		{
			for (int i = 0; i < _buffer.Length; i++)
			{
				if (_buffer[i] == '\n')
				{
					_countEndRow++;
					continue;
				}

				if (_buffer[i] == ':')
				{
					if (_buffer[i + 1] == ' ')
					{
						_countDelimiterKey++;
						continue;
					}
				}

				if (_buffer[i] == '\t')
				{
					_countIndent++;
					continue;
				}
			}
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