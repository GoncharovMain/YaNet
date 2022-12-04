namespace YaNet.Samples
{
	public static class DefaultSymbols
	{
		public const string ListDelimiter = "- ";
		public const string DictDelimiter = ": ";

		public const char Delimiter = ':';
		public const char EndLine = '\n';
		public const string SpaceIndent = "  ";
		public const char TabIndent = '\t';
	}

	public enum LineBreak
	{
		LF, CRLF
	}

	public class FeatureTypeToken
	{
		private string _list => ":\n\t- ";
		private string _dict => ":\n\t";
		private string _object => ":\n\t";

		public bool AsList(string value)
		{
			return value.Contains(_list);
		}

		public bool AsDict(string value)
		{
			return value.Contains(_dict);
		}

		public bool AsObject(string value)
		{
			return value.Contains(_object);
		}
	}

	public class Delimiters
	{
		public string ScalarDelimiter => ": ";
		public string ListDelimiter => "\n\t- ";
		public string DictDelimiter => "\n\t";
		public string ObjectDelimiter => "\n\t";

		// public string Indent => DefaultSymbols.TabIndent;

		// public int LevelIndent = 1;

		// public string ListDelimiter => $"\n{Indent}- ";
	}

	public class ScalarLine : Line
	{
		private char[] _key;
		private char[] _values;
		private char[] _references;

		public ScalarLine(int number, int startPosition, int endPosition) : base(number, startPosition, endPosition)
		{

		}
	}

	public class ListLine : Line
	{
		private char[] _key;
		private Line[] _items;

		public ListLine(int number, int startPosition, int endPosition) : base(number, startPosition, endPosition)
		{

		}
	}

	public class ObjectLine : Line
	{
		private Line[] _lines;

		public ObjectLine(int number, int startPosition, int endPosition) : base(number, startPosition, endPosition)
		{

		}
	}

	public class Line
	{
		private char[] _text;
		private char[] _indent;
		private int _lengthIndent;
		private int _levelIndent;

		private Offset _offset;

		public Line()
		{
			_indent = new char[] { '\t' };
			_lengthIndent = _indent.Length;
		}

		public Line(char text, Offset offset)
		{
			_offset = offset;
		}

		public Line(int number, int startPosition, int endPosition)
		{
			_offset = new Offset(number, startPosition, endPosition);
			_lengthIndent = endPosition - startPosition;
		}

		public override string ToString()
			=> new String(String.Empty);

		public static implicit operator Line(char[] line)
			=> new Line();

		public static implicit operator Line(string line)
			=> new Line();
	}

	public class Program
	{
		public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

		public static string YamlText => File.ReadAllText(CurrentDirectory);

		public static string[] YamlLines => File.ReadAllLines(CurrentDirectory);

		public static void ParseWithPeek()
		{
			string yaml = YamlText;

			char delimiter = DefaultSymbols.Delimiter;
			char endLine = DefaultSymbols.EndLine;


			Peeker peeker = new Peeker(yaml);

			List<string> keys = new List<string>();
			List<string> values = new List<string>();



			int delimiterPosition, endLinePosition;
			string key, value;


			delimiterPosition = peeker.Peek(delimiter);

			key = peeker.Substring(0, delimiterPosition);
			keys.Add(key);

			endLinePosition = peeker.Peek(endLine, delimiterPosition);

			value = peeker.Substring(delimiterPosition + 1, endLinePosition);
			values.Add(value);


			while (endLinePosition < yaml.Length)
			{
				delimiterPosition = peeker.Peek(delimiter, endLinePosition + 1);

				key = peeker.Substring(endLinePosition + 1, delimiterPosition);
				keys.Add(key);

				endLinePosition = peeker.Peek(endLine, delimiterPosition);

				value = peeker.Substring(delimiterPosition + 1, endLinePosition);
				values.Add(value);
			}

			Console.WriteLine();


			for (int i = 0; i < keys.Count; i++)
			{
				Console.WriteLine($"key: {$"'{keys[i]}'",15} value: {$"'{values[i]}'",15}");
			}

			Counter counter = new Counter(DefaultSymbols.TabIndent);

			for (int i = 0; i < keys.Count; i++)
			{
				key = keys[i];

				int levelIndent = counter.LevelIndent(key);

				Console.WriteLine($"key: '{key}' levelIndent: {levelIndent}");
			}
		}

		public static void Main()
		{
			string[] yamlLines = YamlLines;

			Line[] lines = new Line[yamlLines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				lines[i] = yamlLines[i];
			}


			foreach (Line line in lines)
			{

				Console.WriteLine($"{line.ToString()}");
			}

		}
	}
}