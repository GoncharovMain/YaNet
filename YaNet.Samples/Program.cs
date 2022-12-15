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

	public static class Types
	{
		public static Dictionary<string, Type> KeyValueType = new ()
		{
            { "byte", typeof(byte) }, 
            { "sbyte", typeof(sbyte) },
            { "short", typeof(short) }, 
            { "ushort", typeof(ushort) },
            { "int", typeof(int) }, 
            { "uint", typeof(uint) },
            { "long", typeof(long) }, 
            { "ulong", typeof(ulong) },
            { "float", typeof(float) },
            { "double", typeof(double) },
            { "decimal", typeof(decimal) },
            { "string", typeof(string) },
            { "char", typeof(char) },
            { "uint", typeof(uint) },
            { "bool", typeof(bool) },
		};



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
		public int Age { get; set; }
	
		public Address Address { get; set; }
	}

	public class Address
	{
		public string City { get; set; }
		public string Street { get; set; }
	}

	public class KeyReflection<T> where T : new()
	{
		private Dictionary<string, PropertyInfo> _properties;

		public Dictionary<string, PropertyInfo> Properties => _properties;

		private T _value;

		public T Value => _value;

		public KeyReflection() : this(new T()) { }

		public KeyReflection(T obj)
		{
			_value = obj == null ? default : obj;

			PropertyInfo[] properties = obj.GetType().GetProperties();
			
			_properties = new Dictionary<string, PropertyInfo>();

			for (int i = 0; i < properties.Length; i++)
			{
				_properties.Add(properties[i].Name.ToLower(), properties[i]);
			}
		}

		public object this[string prop]
		{
			get
			{
				return _properties[prop].GetValue(_value);
			}
			set
			{
				_properties[prop].SetValue(_value, value);
			}
		}
	}

	// public class InitReflection<T> where T : new()
	// {
	// 	private KeyReflection<T> _keyReflection;

	// 	private Row[] _rows;

	// 	private T _value;

	// 	public T Value => _value;

	// 	public InitReflection(Row[] rows)
	// 	{
	// 		_keyReflection = new KeyReflection<T>();
			
	// 		_value = _keyReflection.Value;

	// 		_rows = rows;

	// 		foreach (var prop in _keyReflection.Properties)
	// 		{
	// 			if (prop.TypeName == typeof(int))
	// 			{
	// 				KeyValueRow kvr = row as KeyValueRow;

	// 				prop[new Peeker(_buffer, kvr.Key)].SetValue(new Peeker(_buffer, kvr.Value));
	// 			}

	// 		}

	// 	}
	// }

	public class Program
	{
		public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

		public static string YamlText => File.ReadAllText(CurrentDirectory);

		public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);

		public static T CreateInstance<T>(T value)
		{
			return (T)value;
		}

		public void foo()
		{


			if (_keyReflection["age"].Name == typeof(int))
			{
				_keyReflection["age"] = (int)CreateInstance<int>(18);
			}
			
			if (_keyReflection["age"].Name == typeof(double))
			{
				_keyReflection["age"] = (double)CreateInstance<double>(3.14);
			}

			if (_keyReflection["age"].Name == typeof(string))
			{
				_keyReflection["age"] = (string)CreateInstance<string>("18");
			}

		}

		public static void Main()
		{
			string yaml = "person:\n\tname: John\n\t\tage: 18\n\tsex: male\n\tbody:\n\t\tweight: 68\n\t\tgrowth: 180\naddress:\n\tcity: Los Angeles\naddress:\n\tcity: Los Angeles\naddress:\n\tcity: Los Angeles\nnames:\n\t- John\n\t- Bob\n\t- Martin";

			yaml = "person:\n\tpersonal data:\n\t\tfirstName: Bob\n\t\tmiddleName: John\n\t\tsecondName: Patick\n\t\tage: 18\n\t\tsex: male\n\taddress:\n\t\tcity: Moscow\n\t\tstreet: Red\n\t\thome: 3\n\tvisitCountries:\n\t\t- Russia\n\t\t- China\n\t\t- USA\n\tfriends:\n\t\t- id: 23\n\t\t  firstName: Albert\n\t\t  middleName: Allen\n\t\t  secondName: Bert\n\t\t- id: 56\n\t\t  firstName: Patrick\n\t\t  middleName: Cecil\n\t\t  secondName: Clarence\n\t\t- id: 87\n\t\t  firstName: Bob\n\t\t  middleName: Elliot\n\t\t  secondName: Elmer\n\t\t- id: 101\n\t\t  firstName: Ernie\n\t\t  middleName: Eugene\n\t\t  secondName: Fergus\n\tlanguages:\n\t\t- English\n\t\t- Russain\n\t\t- Japanese\n\t\t- Spanish\nip address:\n\tip: \"192.168.0.1\"\n\tport: 8080\n\tprotocol:\n\t\ttcp: true\n\t\tudp: true";
			
			yaml = "name: John\nage: 18\naddress:\n\tcity: Moscow\n\tstreet: Red";

			Cascade cascade = new Cascade(yaml);

			cascade.Analize();
			//cascade.Info();


			KeyValueRow kvr = cascade[0] as KeyValueRow;

			Console.WriteLine("key: " + new Peeker(yaml, kvr.Key));

			Console.WriteLine("value: " + new Peeker(yaml, kvr.Value));


			KeyReflection<Person> keyPerson = new KeyReflection<Person>();


			keyPerson["name"] = "John";
			keyPerson["age"] = 18;

			KeyReflection<Address> keyAddress = new KeyReflection<Address>();
			
			keyAddress["street"] = "Red";
			keyAddress["city"] = "Moscow";
			
			keyPerson["address"] = keyAddress.Value;


			Person person = keyPerson.Value;

			Console.WriteLine(person.Name);
			Console.WriteLine(person.Age);
			Console.WriteLine(person.Address.City);
			Console.WriteLine(person.Address.Street);



			int a = (int)Activator.CreateInstance(typeof(int));


		}
	}
}