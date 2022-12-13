using System.Text;
using YaNet;
using YaNet.Rows;

namespace YaNet.Samples
{
	public static class DefaultSymbols
	{
		public const string ListDelimiter = "- ";
		public const string DictDelimiter = ": ";

		public const char Delimiter = ':';
		public const char EndRow = '\n';
		public const string SpaceIndent = "  ";
		public const char TabIndent = '\t';
	}

	public enum RowBreak
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


	public static class Templates
	{
		public static string[] Scalars = new string[]
		{
			"[\t][key][: ][value][\n]",
			"[\t][key][: ][ref][\n]",
			"[\t][key][: ][mixed][\n]",
		};

		public static string[] Objects = new string[]
		{
			"[\t][key][: ][ref][\n]",
			"[\t][key][:][\n]",
		};

		public static string[] ListItems = new string[]
		{
			"[\t][- ][value][\n]",
			"[\t][- ][key][: ][value][\n]",
			"[\t][- ][key][: ][ref][\n]",
			"[\t][- ][key][: ][mixed][\n]",
			"[\t][- ][key][:][\n]",
			"[\t][-][\n]"
		};
	}


	public class Program
	{
		public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

		public static string YamlText => File.ReadAllText(CurrentDirectory);

		public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);

		public static void Main()
		{
			string yaml = "person:\n\tname: John\n\tage: 18\n\tsex: male\n\tbody:\n\t\tweight: 68\n\t\tgrowth: 180\naddress:\n\tcity: Los Angeles\naddress:\n\tcity: Los Angeles\naddress:\n\tcity: Los Angeles";


			Cascade cascade = new Cascade(yaml);

			cascade.Analize();
			cascade.Info();
			

			Parser parser = new Parser(YamlText);

			parser.Deserialize();
		}
	}
}