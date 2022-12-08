using System.Text;
using YaNet.Lines;

namespace YaNet
{
	public class Parser
	{
		private StringBuilder _buffer;

		private Line[] _lines;


		public Parser(StringBuilder text)
		{
			_buffer = text;
		}

		public Parser(string text) : this(new StringBuilder(text)) { }


		public void Deserialize()
		{
			Peeker peeker = new Peeker(_buffer);


			Offset[] offsets = peeker.Split('\n');

			_lines = new Line[offsets.Length];


			for (int i = 0; i < _lines.Length; i++)
			{
				_lines[i] = new Line(_buffer, offsets[i]);
			}

			for (int i = 0; i < _lines.Length - 1; i++)
			{
				
				// handle variant when line is item of list
				// two spaces "  " and "- "
				//	persons:
				//		- name: John
				//			age: 18
				//		- name: Bob
				// 			age: 25
				//		- name: Patrick
				//			age: 23

				if (_lines[i].CountIndent + 1 < _lines[i + 1].CountIndent)
				{
					throw new Exception($"Line {i + 1}: '{_lines[i + 1]}' has not correct indent.");
				}
			}

			Console.WriteLine($"enter: {(int)'\n'}");

			for (int i = 0; i < _lines.Length; i++)
			{
				Console.WriteLine($"line: {new Peeker(_lines[i].Buffer, _lines[i].Offset).ToCharCode()}");
			}



			Splitter splitter = new Splitter(_lines);

			Splitter splitterNext = splitter.SplitLevel(0);


			splitterNext.Offsets.ToList().ForEach(offset => Console.WriteLine($"peek: {new Peeker(_buffer, offset).Buffer} offset: {offset}"));

			
			Line nextLine = splitterNext[0];


			splitterNext = new Splitter(nextLine); 




		}

	}
}