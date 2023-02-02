using System.Text;
using YaNet;
using YaNet.Features;
using System.Reflection;
using YaNet.Samples.Context;
using YaNet.Nodes;
using System.IO;
using System.Runtime.Serialization;

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

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
    }

    public class Program
    {
        public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

        public static string YamlText => File.ReadAllText(CurrentDirectory);

        public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);

        public class Collect
        {
            public List<int> Array { get; set; }
            public Dictionary<string, string> Dict { get; set; }

            public Dictionary<string, Meta> Persons { get; set; }
        }

        public class Meta
        {
            public int Age { get; set; }
        }

        public static void Main()
        {
            string yaml = "Array:\n\t- 15\n\t- 5\n\t- 7\n\t- 22\n\t- 365\n\t- 5000\n\t- -2345" +
                "\nDict:\n\tName: John\n\tAge: 18\n\tSex: male\n" +
                "Persons:\n\tJohn:\n\t\tAge: 18\n\tBob:\n\t\tAge: 20";

            Console.WriteLine(yaml);
            Console.WriteLine();

            Console.WriteLine(new String('=', 40));
            Console.WriteLine();


            Deserializer deserializer = new Deserializer(yaml);

            Collect collect = deserializer.Deserialize<Collect>();

            foreach (var item in collect.Array)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("len" + collect.Dict.Count);

            foreach (var item in collect.Dict)
            {
                Console.WriteLine($"key: {item.Key} value: {item.Value}");
            }

        }
    }
}