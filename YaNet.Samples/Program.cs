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
		public delegate object Quilify(string value);

		public static Dictionary<string, Type> KeyValueType = new ()
		{
      { "short", typeof(short) }, 
      { "ushort", typeof(ushort) },
      { "byte", typeof(byte) }, 
      { "sbyte", typeof(sbyte) },
      { "int", typeof(int) }, 
      { "uint", typeof(uint) },
      { "long", typeof(long) }, 
      { "ulong", typeof(ulong) },
      { "float", typeof(float) },
      { "double", typeof(double) },
      { "decimal", typeof(decimal) },
      { "string", typeof(string) },
      { "char", typeof(char) },
      { "bool", typeof(bool) },

      { "Int16", typeof(short) }, 
      { "UInt16", typeof(ushort) },
      { "Byte", typeof(byte) }, 
      { "SByte", typeof(sbyte) },
      { "Int32", typeof(int) }, 
      { "UInt32", typeof(uint) },
      { "Int64", typeof(long) }, 
      { "UInt64", typeof(ulong) },
      { "Single", typeof(float) },
      { "Double", typeof(double) },
      { "Decimal", typeof(decimal) },
      { "String", typeof(string) },
      { "Char", typeof(char) },
      { "Boolean", typeof(bool) },
		};

		public static object Converter(Type type, string value)
			=> type.Name switch 
				{
					"Int16" => (object)Convert.ToInt16(value),
					"UInt16" => (object)Convert.ToUInt16(value),
					"Byte" => (object)Convert.ToByte(value),
					"SByte" => (object)Convert.ToSByte(value),
					"Int32" => (object)Convert.ToInt32(value),
					"UInt32" => (object)Convert.ToUInt32(value),
					"Int64" => (object)Convert.ToInt64(value),
					"UInt64" => (object)Convert.ToUInt64(value),
					"Single" => (object)Convert.ToSingle(value),
					"Double" => (object)Convert.ToDouble(value),
					"Decimal" => (object)Convert.ToDecimal(value),
					"Boolean" => (object)Convert.ToBoolean(value),
					"Char" => (object)Convert.ToChar(value),
					"String" => (object)value,
					_ => Activator.CreateInstance(type)
				};

		public static Dictionary<string, Quilify> Converters = new ()
		{
			{ "Int16", value => (object)Convert.ToInt16(value) },
			{ "UInt16", value => (object)Convert.ToUInt16(value) },
			{ "Byte", value => (object)Convert.ToByte(value) },
			{ "SByte", value => (object)Convert.ToSByte(value) },
			{ "Int32", value => (object)Convert.ToInt32(value) },
			{ "UInt32", value => (object)Convert.ToUInt32(value) },
			{ "Int64", value => (object)Convert.ToInt64(value) },
			{ "UInt64", value => (object)Convert.ToUInt64(value) },
			{ "Single", value => (object)Convert.ToSingle(value) },
			{ "Double", value => (object)Convert.ToDouble(value) },
			{ "Decimal", value => (object)Convert.ToDecimal(value) },
			{ "Boolean", value => (object)Convert.ToBoolean(value) },
			{ "Char", value => (object)Convert.ToChar(value) },
			{ "String", value => (object)value },
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


	public interface IType
	{
		public void Init();
	}

	public class Scalar : IType
	{
		private KeyValueRow _keyValueRow;
		private Marker _marker;
		private PropertyInfo _propertyInfo;
		private object _obj;

		private List<string> _scalarTypes = new()
		{
			"Int16", "UInt16",
			"Byte", "SByte",
			"Int32", "UInt32",
			"Int64", "UInt64",
			"Single", "Double", "Decimal",
			"Boolean",
			"Char", "String",
		};

		public Scalar(StringBuilder buffer, Row row, object obj)
		{
			_keyValueRow = row as KeyValueRow;

			_obj = obj;

			_marker = new Marker(buffer);
			
			string propertyName = _marker.Buffer(_keyValueRow.Key);

			_propertyInfo = _obj.GetType().GetProperty(propertyName);
		}

		public void Init()
		{
			string substring = _marker.Buffer(_keyValueRow.Value);

			object value = Types.Converter(_propertyInfo.PropertyType, substring);

			_propertyInfo.SetValue(_obj, value);
		}
	}

	public class ObjectType : IType
	{
		private KeyRow _keyRow;
		private Marker _marker;
		private PropertyInfo _propertyInfo;
		private object _obj;

		public ObjectType(StringBuilder buffer, Row row, object obj)
		{
			_keyRow = row as KeyRow;

			_marker = new Marker(buffer);

			_obj = obj;

			string propertyName = _marker.Buffer(_keyRow.Key);

			_propertyInfo = _obj.GetType().GetProperty(propertyName);

		}

		public void Init()
		{

		}
	}

	public class Program
	{
		public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

		public static string YamlText => File.ReadAllText(CurrentDirectory);

		public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);

		public static T CreateInstance<T>(Type type)
			=> (T)Activator.CreateInstance(type);
		
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

		public static void Main()
		{
			string yaml = "person:\n\tname: John\n\t\tage: 18\n\tsex: male\n\tbody:\n\t\tweight: 68\n\t\tgrowth: 180\naddress:\n\tcity: Los Angeles\naddress:\n\tcity: Los Angeles\naddress:\n\tcity: Los Angeles\nnames:\n\t- John\n\t- Bob\n\t- Martin";

			yaml = "person:\n\tpersonal data:\n\t\tfirstName: Bob\n\t\tmiddleName: John\n\t\tsecondName: Patick\n\t\tage: 18\n\t\tsex: male\n\taddress:\n\t\tcity: Moscow\n\t\tstreet: Red\n\t\thome: 3\n\tvisitCountries:\n\t\t- Russia\n\t\t- China\n\t\t- USA\n\tfriends:\n\t\t- id: 23\n\t\t  firstName: Albert\n\t\t  middleName: Allen\n\t\t  secondName: Bert\n\t\t- id: 56\n\t\t  firstName: Patrick\n\t\t  middleName: Cecil\n\t\t  secondName: Clarence\n\t\t- id: 87\n\t\t  firstName: Bob\n\t\t  middleName: Elliot\n\t\t  secondName: Elmer\n\t\t- id: 101\n\t\t  firstName: Ernie\n\t\t  middleName: Eugene\n\t\t  secondName: Fergus\n\tlanguages:\n\t\t- English\n\t\t- Russain\n\t\t- Japanese\n\t\t- Spanish\nip address:\n\tip: \"192.168.0.1\"\n\tport: 8080\n\tprotocol:\n\t\ttcp: true\n\t\tudp: true";
			
			yaml = "Name: Goncharov\nAge: 18\nAddress:\n\tCity: London\n\tStreet: Red";

			Cascade cascade = new Cascade(yaml);

			cascade.Analize();
			//cascade.Info();

			Row[] rows = cascade.Rows;

			// 0[0]KeyValueRow: 'name' : 'Goncharov'
			// 1[0]KeyValueRow: 'age' : '18'
			// 2[0]KeyRow: 'address':
			// 3[1]KeyValueRow: 'city' : 'Moscow'
			// 4[1]KeyValueRow: 'street' : 'Red'

			// 0[0] row: [0:15:16] type: KeyValueRow key: [0:3:4] value: [6:14:9]
			// 1[0] row: [16:23:8] type: KeyValueRow key: [16:18:3] value: [21:22:2]
			// 2[0] row: [24:32:9] type: KeyRow key: [24:30:7]
			// 3[1] row: [33:46:14] type: KeyValueRow key: [34:37:4] value: [40:45:6]
			// 4[1] row: [47:58:12] type: KeyValueRow key: [48:53:6] value: [56:58:3]

			for (int i = 0; i < rows.Length; i++)
			{
				rows[i].Info();
			}
			
			StringBuilder buffer = new StringBuilder(yaml);


			Dictionary<string, PropertyInfo> properties = typeof(Person)
				.GetProperties()
				.ToDictionary(prop => prop.Name, prop => prop);

			foreach (var k in properties.Keys)
			{
				Console.WriteLine($"{k} {properties[k].PropertyType.Name}");
			}

			

			#region Init

			Person person = new Person();

			string key = String.Empty;
			object value;

			Marker marker = new Marker(yaml);

			#endregion Init



			// scalar key value type

			Scalar scalarName = new Scalar(buffer, rows[0], person);
			scalarName.Init();


			// scalar key value type with convert

			Scalar scalarAge = new Scalar(buffer, rows[1], person);
			scalarAge.Init();


			// object type
			ObjectType objectAddress = new ObjectType(buffer, rows[2], person);



			key = marker.Buffer((rows[2] as KeyRow).Key);

			value = Types.Converter(properties[key].PropertyType, "${Person.Address}");

			// init object in object

			object address = value;

			properties[key].SetValue(person, value);

			Dictionary<string, PropertyInfo> addressProperties = address
				.GetType()
				.GetProperties()
				.ToDictionary(prop => prop.Name, prop => prop);



			// scalar key value type

			Scalar scalarCity = new Scalar(buffer, rows[3], address);

			scalarCity.Init();


			// scalar key value type

			Scalar scalarStreet = new Scalar(buffer, rows[4], address);

			scalarStreet.Init();

			

			Console.WriteLine($"person:");
			Console.WriteLine($"\tName: {person.Name}");
			Console.WriteLine($"\tAge: {person.Age}");
			Console.WriteLine($"\tAddress:");
			Console.WriteLine($"\t\tCity: {person.Address.City}");
			Console.WriteLine($"\t\tStreet: {person.Address.Street}");

		}
	}
}