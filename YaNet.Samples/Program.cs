using System.Text;
using YaNet;
using YaNet.Features;
using System.Reflection;
using YaNet.Samples.Context;
using YaNet.Structures;

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

	public class Initer
	{
		private Row[] _rows;

		private List<string> _primitiveType;

		private string indent;

		public Initer()
		{
			_primitiveType = Types.Converters.Keys.ToList();
			indent = "";
		}

		public void Init(object o)
		{
			// get all property

			PropertyInfo[] properties = o.GetType().GetProperties();

			// init property of scalar

			foreach (PropertyInfo property in properties)
			{
				Type propertyType = property.PropertyType;

				string name = propertyType.Name;


				Console.WriteLine($"{indent}Name: {property.Name} Type: {name}");

				if (name == "Int32[][]")
				{

				}
				else if (name == "List`1")
				{
					Type insideType = propertyType.GetGenericArguments()[0];

					object insideObject = Activator.CreateInstance(property.PropertyType);

					Console.WriteLine($"As list: {insideType.Name}");

					//Init(insideObject);
				}
				else if (name == "Dictionary`2")
				{
					Type[] genericArguments = propertyType.GetGenericArguments();


					Type keyType = genericArguments[0];
					Type valueType = genericArguments[1];

					Console.WriteLine($"As dictionary: {keyType.Name} {valueType.Name}");

					if (!_primitiveType.Contains(valueType.Name))
					{
						Console.WriteLine("is not primitive");
					}
				}
				else if (_primitiveType.Contains(name))
				{
					Console.Write($"is primitive`");


				}
				else
				{
					indent += "\t";

					object insideObject = Activator.CreateInstance(property.PropertyType);

					this.Init(insideObject);
				}
			}
		}
	}
	public class Program
	{
		public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

		public static string YamlText => File.ReadAllText(CurrentDirectory);

		public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);

		public static T CreateInstance<T>(Type type)
			=> (T)Activator.CreateInstance(type);

		public static void Main()
		{
			string yaml = "Name: Goncharov\nAge: 18\nAddress:\n\tCity: London\n\tStreet: Red";

			Cascade cascade = new Cascade(yaml);

			cascade.Analize();
			//cascade.Info();



			Row[] rows = cascade.Rows;


			Initer initer = new Initer();

			Data data = new Data();

			initer.Init(data);

		}
	}
}