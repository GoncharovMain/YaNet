using System.Text;
using YaNet;
using YaNet.Features;
using System.Reflection;
using YaNet.Samples.Context;
using YaNet.Nodes;

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






    public class Program
    {
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Address Address { get; set; }
        }
        public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

        public static string YamlText => File.ReadAllText(CurrentDirectory);

        public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);

        public static T CreateInstance<T>(Type type)
                => (T)Activator.CreateInstance(type);

        public class Parser
        {
            private StringBuilder _buffer;

            public Parser(StringBuilder _buffer)
            {

            }
        }

        public static void Main()
        {
            string yaml = "Name: Goncharov\nAge: 18\nAddress:\n\tCity: London\n\tStreet: Red\n\tHome: 3";


            StringBuilder buffer = new StringBuilder(yaml);
            Marker marker = new Marker(buffer);

            INode[] nodes = new INode[]
            {
                new Pair(new Mark(0, 3), new Mark(6, 14)),
                new Pair(new Mark(16, 18), new Mark(21, 22)),
                new Node(new Mark(24, 30),
                    new INode[]
                    {
                        new Pair(new Mark(34, 37), new Mark(40, 45)),
                        new Pair(new Mark(48, 53), new Mark(56, 58)),
                        new Pair(new Mark(61, 64), new Mark(67, 67)),
                    })
            };

            Person person = new Person();

            foreach (INode node in nodes)
                node.Init(person, buffer);

            Console.WriteLine(person.Name);
            Console.WriteLine(person.Age);

            Console.WriteLine(person.Address.City);
            Console.WriteLine(person.Address.Street);
            Console.WriteLine(person.Address.Home);


        }
    }
}