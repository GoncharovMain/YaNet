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

        public class Parser
        {
            private StringBuilder _buffer;
            private Marker _marker;

            public Parser(StringBuilder buffer)
            {
                _buffer = buffer;
                _marker = new Marker(buffer);
            }

            public INode[] Parse()
            {
                Mark[] rows = SplitRows();

                int[] indents = new int[rows.Length];

                for (int i = 0; i < rows.Length; i++)
                {
                    indents[i] = _marker.Count('\t', rows[i]);
                }

                return null;
            }

            public Mark[] SplitRows()
            {
                int countRow = new Peeker(_buffer).Counter('\n') + 1;

                Mark[] rows = new Mark[countRow];

                int end = 0;

                for (int i = 0, start = end; i < countRow - 1; i++, start = end)
                {
                    while (_buffer[end + 1] != '\n')
                    {
                        end++;
                    }

                    rows[i] = new Mark(start, end);

                    end = end + 2;
                }

                rows[^1] = new Mark(end, _buffer.Length - 1);

                return rows;
            }

            public INode DefineFeature(Mark current, Mark next)
            {
                return null;


                // define features for Scalar, Pair, Item, Node, NodeReference

            }
        }


        public static void Main()
        {
            string yaml = "" +
                "Age: 18\nAddress:\n\tCity: London\n\tStreet: Red\n\tHome: 3\nName: Goncharov";
            //string yaml = "Name: Goncharov\nAge: 18\nAddress:\n\tCity: London\n\tStreet: Red\n\tHome: 3\n\tSex: male\n\tHome: 3";


            StringBuilder buffer = new StringBuilder(yaml);
            Marker marker = new Marker(buffer);

            // INode[] nodes = new INode[]
            // {
            //     new Pair(new Mark(0, 3), new Mark(6, 14)),
            //     new Pair(new Mark(16, 18), new Mark(21, 22)),
            //     new Node(new Mark(24, 30),
            //         new Collection(
            //             new Pair(new Mark(34, 37), new Mark(40, 45)),
            //             new Pair(new Mark(48, 53), new Mark(56, 58)),
            //             new Pair(new Mark(61, 64), new Mark(67, 67))
            //         ))
            // };

            // Person person = new Person();

            // foreach (INode node in nodes)
            //     node.Init(person, buffer);

            // Console.WriteLine(person.Name);
            // Console.WriteLine(person.Age);

            // Console.WriteLine(person.Address.City);
            // Console.WriteLine(person.Address.Street);
            // Console.WriteLine(person.Address.Home);

            // Parser parser = new Parser(buffer);

            // parser.Parse();


            // Mark[] rows = new Mark[]
            // {
            //     new Mark(24, 31), // 0 'Address:'
            //     new Mark(33, 45), // 1 '       City: London'
            //     new Mark(47, 58), // 2 '       Street: Red'
            //     new Mark(60, 67)  // 3 '       Home: 3'
            //     new Mark(0, 14),  // 4 'Name: Goncharov'
            //     new Mark(16, 22), // 5 'Age: 18'
            // };

            Mark[] rows = new Peeker(buffer).Split('\n');



            for (int i = 0; i < rows.Length; i++)
            {
                Console.WriteLine($"i: {i} row: '{marker.Buffer(rows[i])}'");
            }


            Definer definer = new Definer(buffer, rows);

            Collection collection = definer.DefineCollection();

            Console.WriteLine($"len collection: {collection.Nodes.Length}");


            collection.Print(buffer);





            Person person = new Person();



            foreach (INode node in collection.Nodes)
                node.Init(person, buffer);

            Console.WriteLine($"Name: {person.Name}");
            Console.WriteLine($"Age: {person.Age}");


            Console.WriteLine($"Address:");
            Console.WriteLine($"\tAddress.City: {person.Address.City}");
            Console.WriteLine($"\tAddress.Street: {person.Address.Street}");
            Console.WriteLine($"\tAddress.Home: {person.Address.Home}");




            // Peeker peeker = new Peeker("name:  John: Bob: Patric:123");
            // marker = new Marker("name:  John: Bob: Patric:123");
            // Mark[] ms = peeker.Split(":  ");

            // Console.WriteLine(peeker.Counter(":  "));

            // for (int i = 0; i < ms.Length; i++)
            // {
            //     Console.WriteLine($"'{marker.Buffer(ms[i])}' {ms[i]}");
            // }
        }
    }
}