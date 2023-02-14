using YaNet.Samples.Context;

namespace YaNet.Samples
{
    public class Program
    {
        public static string CurrentDirectory => Directory.GetCurrentDirectory() + "/ex1.yaml";

        public static string YamlText => File.ReadAllText(CurrentDirectory);

        public static string[] YamlRows => File.ReadAllLines(CurrentDirectory);

        public class References
        {
            public References()
            {

            }

            public static string[] ParseCascade(string cascade)
            {
                char delimiterIndexesLeft = '[';
                char delimiterIndexesRight = ']';
                char delimiterNames = '.';

                return cascade.Split(new char[] { delimiterIndexesLeft, delimiterIndexesRight, delimiterNames }, 
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public static void Main()
        {
            //string path = Directory.GetCurrentDirectory() + "/ex2.yanet";
            string path = "C:\\Users\\yuriy.goncharov\\Desktop\\YaNet\\YaNet.Samples\\ex3.yaml";

            string yaml = File.ReadAllText(path);


            string[] parsersCascade =
            {
                String.Join(':', References.ParseCascade("Requests.GoogleGet.Headers.UserAgent")),
                String.Join(':', References.ParseCascade("Requests.GoogleGet.Headers[1].UserAgent")),
                String.Join(':', References.ParseCascade("Requests. GoogleGet.Headers.1.UserAgent")),
            };

            foreach (var cascade in parsersCascade)
            {
                Console.WriteLine($"'{cascade}'");
            }

            //Deserializer deserializer = new Deserializer(yaml);

            ////RequestData data = deserializer.Deserialize<RequestData>();

            //Data data = deserializer.Deserialize<Data>();

        }
    }
}