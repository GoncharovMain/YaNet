using System.Text;
using YaNet;
using YaNet.Features;
using System.Reflection;

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

	public class Person
	{
		public string Name { get; set; }
		public string Age { get; set; }
	
		public Address Address { get; set; }
	}

	public class Address
	{
		public string City { get; set; }
		public string Streen { get; set; }
	}

	public class Program
	{
		public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

		public static string YamlText => File.ReadAllText(CurrentDirectory);

		public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);

		public static void Main()
		{
			string yaml = "person:\n\tname: John\n\t\tage: 18\n\tsex: male\n\tbody:\n\t\tweight: 68\n\t\tgrowth: 180\naddress:\n\tcity: Los Angeles\naddress:\n\tcity: Los Angeles\naddress:\n\tcity: Los Angeles\nnames:\n\t- John\n\t- Bob\n\t- Martin";

			yaml = "person:\n\tpersonal data:\n\t\tfirstName: Bob\n\t\tmiddleName: John\n\t\tsecondName: Patick\n\t\tage: 18\n\t\tsex: male\n\taddress:\n\t\tcity: Moscow\n\t\tstreet: Red\n\t\thome: 3\n\tvisitCountries:\n\t\t- Russia\n\t\t- China\n\t\t- USA\n\tfriends:\n\t\t- id: 23\n\t\t  firstName: Albert\n\t\t  middleName: Allen\n\t\t  secondName: Bert\n\t\t- id: 56\n\t\t  firstName: Patrick\n\t\t  middleName: Cecil\n\t\t  secondName: Clarence\n\t\t- id: 87\n\t\t  firstName: Bob\n\t\t  middleName: Elliot\n\t\t  secondName: Elmer\n\t\t- id: 101\n\t\t  firstName: Ernie\n\t\t  middleName: Eugene\n\t\t  secondName: Fergus\n\tlanguages:\n\t\t- English\n\t\t- Russain\n\t\t- Japanese\n\t\t- Spanish\nip address:\n\tip: \"192.168.0.1\"\n\tport: 8080\n\tprotocol:\n\t\ttcp: true\n\t\tudp: true";
			
			yaml = "name: John\nage: 18\naddress:\n\tcity: Moscow\n\tstreet: Red";

			Cascade cascade = new Cascade(yaml);

			cascade.Analize();
			//cascade.Info();



			Person person = new Person();


			PropertyInfo property = person.GetType().GetProperty("Name");

			property.SetValue(person, "John");


			Console.WriteLine(person.Name);


			var keyValueRow = (KeyValueRow)cascade[0];

			string key = new Peeker(yaml, keyValueRow.Key).ToString();

			Console.WriteLine($"key: {key}");


			Parser parser = new Parser(YamlText);

			parser.Deserialize();
		}
	}
}