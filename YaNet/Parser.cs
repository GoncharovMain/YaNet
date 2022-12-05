namespace YaNet
{

	public enum LineType
	{
		Scalar,
		List,
		Object
	}

	public class Parser
	{
		private string _text;
		private string[] _lines;
		private int[] _indents;
		private LineType[] _lineTypes;

		private int _length;

		public Parser(string text)
		{
			_text = text;

			List<(int start, int end)> linePositions = new List<(int, int)>();

			int endPosition = 0;
			int startPosition = 0;

			do
			{
				endPosition = GetEndLinePosition(endPosition);
				
				linePositions.Add((startPosition, endPosition));

				startPosition = endPosition + 1;
			}
			while(endPosition < _text.Length - 1);


			linePositions.ForEach(item => Console.WriteLine(item));	
		
			_lines = linePositions
				.Select(offset => _text.Substring(offset.start, offset.end - offset.start))
				.ToArray();

			_length = _lines.Length;


			_indents = new int[_length];

			for (int i = 0; i < _indents.Length; i++)
			{
				_indents[i] = new Peeker(_lines[i]).CountIndent("\t");
			}

			_lineTypes = new LineType[_length];

			InitTypes();

		}

		private int GetEndLinePosition(int position = 0)
		{
			for (int i = position + 1; i < _text.Length; i++)
			{
				if (_text[i] == '\n')
				{
					return i;
				}
			}

			return _text.Length - 1;
		}

		public void Info()
		{
			for (int i = 0; i < _length; i++)
			{
				Console.WriteLine($"'{_lines[i]}' : {_indents[i]}");
			}
		}

		public void InitTypes()
		{

			for (int i = 0; i < _length - 1; i++)
			{

				string delimiter = ": ";

				int endPositionDelimiter = new Peeker(_lines[i]).IndexOf(delimiter);
				
				if (endPositionDelimiter == -1)
				{
					int p = new Peeker(_lines[i + 1]).IndexOf("- ");
					
					Console.WriteLine(p);

					if (p != -1)
						_lineTypes[i] = LineType.List;
					else
						_lineTypes[i] = LineType.Object;
				}
				else
				{
					_lineTypes[i] = LineType.Scalar;
				}


				Console.WriteLine($"line: {_lines[i]} endPositionDelimiter: {endPositionDelimiter} _lineTypes: {_lineTypes[i]}");
			}

		}

	}
}