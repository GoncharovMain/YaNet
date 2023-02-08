using YaNet.Samples.Context;

namespace YaNet.Samples
{
    public class Program
    {
        public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

        public static string YamlText => File.ReadAllText(CurrentDirectory);

        public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);

        public static void Main()
        {
            //string path = Directory.GetCurrentDirectory() + "/ex2.yanet";
            string path = "C:\\Users\\yuriy.goncharov\\Desktop\\YaNet\\YaNet.Samples\\ex2.yanet";

            string yaml = File.ReadAllText(path);

            Deserializer deserializer = new Deserializer(yaml);

            Data data = deserializer.Deserialize<Data>();
        }
    }
}