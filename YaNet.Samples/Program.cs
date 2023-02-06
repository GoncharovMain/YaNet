using System.Text;
using YaNet;
using YaNet.Features;
using System.Reflection;
using YaNet.Samples.Context;
using YaNet.Nodes;
using System.IO;
using System.Runtime.Serialization;
using System.Data.Common;

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
    public class IP
    {
        public int[] Bytes { get; set; }
        public int Port { get; set; }
        
        public IP() { }

        public IP(string ip)
        {
            Port = Convert.ToInt32(ip.Split(':')[^1]);

            Bytes = ip.Split(':')[0].Split('.').Select(@byte => Convert.ToInt32(@byte)).ToArray();
        }

        public static implicit operator string(IP ip)
            => String.Join('.', ip.Bytes) + ":" + ip.Port;

        public static implicit operator IP(string ip)
            => new IP(ip);
    }

    public class Program
    {
        public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

        public static string YamlText => File.ReadAllText(CurrentDirectory);

        public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);



        public static void Main()
        {

            string path = "C:\\Users\\yuriy.goncharov\\Desktop\\YaNet\\YaNet.Samples\\ex2.yanet";

            string yaml = File.ReadAllText(path);

            Deserializer deserializer = new Deserializer(yaml);

            Data data = deserializer.Deserialize<Data>();


            for (int i = 0; i < data.Matrix.Count; i++)
            {
                Console.WriteLine(String.Join(" ", data.Matrix[i]));
            }

            Console.WriteLine(data.Ip);
            
        }
    }
}