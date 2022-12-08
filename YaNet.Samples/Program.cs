using System.Text;
using YaNet;
using YaNet.Lines;

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


	public class Program
	{
		public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

		public static string YamlText => File.ReadAllText(CurrentDirectory);

		public static string[] YamlLines => File.ReadAllLines(CurrentDirectory);

		public static void Main()
		{
			string yaml = "person:\n\tname: John\n\t\tage: 18\n\tsex: male\n\tbody:\n\t\tweight: 68\n\t\tgrowth: 180";

			Parser parser = new Parser(yaml);

			parser.Deserialize();


			Console.WriteLine(new Peeker(yaml).ToCharCode());


			
			// levelIndent, tokenIndent, tokenType, lengthTokenType, start, end, peeker, isList, isDict, isScalar

			// вычисляется отступ
			// генерируются ожидаемые разделители для определения типа

			// сравниваются поочерёдно и выбирается соответсвующий тип

			// определить точный тип строки можно только пропарсив часть буфера
			// до строки с соответствующим уровнем отступа.
			
			string yaml1 = "\t\tperson: John\n\t\tage: 18\n\t\tsex: male";
			string yaml2 = "\t\tperson:\n\t\t\tname: John\n\t\t\tage: 18\n\t\t\tsex: male";
			string yaml3 = "\t\tperson:\n\t\t\t- John\n\t\t\t- Bob\n\t\t\t- Martin";

			// для yaml1 строка "person: John" является Scalar, т.к. делимитер для следующей строки "\t\t" => "\t\t"
			// для yaml2 строка "person: John" является Object, т.к. делимитер для следующей строки отступ на 1 больше "\t\t" => "\t\t\t"
			// для yaml3 строка "person: John" является List, т.к. делимитер для следующей строки 

			yaml = "\t\trequest:\n\t\t\t- google\n\t\t\t- yandex";


			// тест для границы _end

			// взять весь текст из файла

			// храним строку не как элемент массива,
			// а как объект (Line) с ссылкой на весь текст 
			// и смещение - начальная и конечная позиции включительно
			// нумерация начинается с нуля

			// сначала считаем сколько строк во всем буфере
			// выделяем offset-ы и создаём массив объектов Line 
			//   для каждого объекта Line считаем отступ и проверяем,
			//   допустимы ли такие отступы


			// tamplates:
			// 	для scalar => $"(\t)*(N)(*)"
			// 	для list => $"(\t)*(N)\t- (*)"
			// 	для dictionary => $"(\t)*(N)(*)"

			// 	Qualifier/Token:
			//		TypeQualifier - определяет тип строки
			//		Peeker - ищет в строках подстроки и др. операции
			// 		Splitter - создаёт токены строк и создаёт на них ссылки

			//	

			TypeQualifier typeQualifier = new TypeQualifier(yaml2, (0, 14));


			string scalarTrace = typeQualifier.ScalarTrace();

			string listTrace = typeQualifier.ListTrace();

			string objectTrace = typeQualifier.ObjectTrace();



			Peeker peeker = new Peeker(yaml2, 15, 16);



			bool isScalar = peeker == scalarTrace;

			bool isList = peeker == listTrace;

			bool isObject = peeker == objectTrace;


			Console.WriteLine($"isScalar: {isScalar}");
			Console.WriteLine($"isList: {isList}");
			Console.WriteLine($"isObject: {isObject}");

			Console.WriteLine(peeker.ToCharCode());
			Console.WriteLine(new Peeker(scalarTrace).ToCharCode());
			Console.WriteLine(new Peeker(listTrace).ToCharCode());
			Console.WriteLine(new Peeker(objectTrace).ToCharCode());

		}
	}
}