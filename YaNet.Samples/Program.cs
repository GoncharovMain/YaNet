using System.Text;
using YaNet;
using YaNet.Features;
using System.Reflection;
using YaNet.Samples.Context;
using YaNet.Nodes;
using System.IO;

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
        }

        public static void Main()
        {
            string yaml = "Array:\n\t- 15\n\t- 5\n\t- 7\n\t- 22\n\t- 365\n\t- 5000\n\t- -2345";

            Deserializer deserializer = new Deserializer(yaml);

            Collect collect = deserializer.Deserialize<Collect>();

            foreach (var item in collect.Array)
            {
                Console.WriteLine(item);
            }

        }
    }
}