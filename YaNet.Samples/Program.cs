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
            string path = "C:\\Users\\yuriy.goncharov\\Desktop\\YaNet\\YaNet.Samples\\ex3.yaml";

            string yaml = File.ReadAllText(path);

            Deserializer deserializer = new Deserializer(yaml);

            Data data = deserializer.Deserialize<Data>();

            //Array array = new int[,,][][,][][][,,,] { };

            Array array = new int[,]
            {
                { 85, 84, 20, -1 },
                { 75, 74, 10, -11 }
            };


            System.Collections.IEnumerator myEnumerator = array.GetEnumerator();

            int i = 0;

            while ((myEnumerator.MoveNext()) && (myEnumerator.Current != null))
                Console.WriteLine("[{0}] {1}", i++, myEnumerator.Current);



            //Console.WriteLine($"3, 4, 2");

            //RankPosition rankPosition = new RankPosition(3, 4, 5);

            //do
            //{
            //    Console.WriteLine(rankPosition);
            //} while (rankPosition.MoveNext());

        }
    }
}