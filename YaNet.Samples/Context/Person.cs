namespace YaNet.Samples.Context
{
    public class Data
    {
        public Person Person { get; set; }
        public IpAddress IpAddress { get; set; }
        public IP Ip { get; set; }
        public List<string> Matrices { get; set; }
        private List<int> vector { get; set; }
        public string Vector
        {
            get => String.Join(", ", vector);
            set => vector = value.Split(", ").Select(item => Convert.ToInt32(item)).ToList();
        }
        public List<List<int>> Matrix { get; set; }
        public int[][] SteppedArray { get; set; }
        public int[,][] MultiArray { get; set; }
    }

    public class Person
    {
        public PersonalData PersonalData { get; set; }
        public Address Address { get; set; }

        public List<string> VisitCountries { get; set; }

        public List<Friend> Friends { get; set; }

        public List<string> Languages { get; set; }
    }

    public enum Sex
    {
        Male, Female
    }

    public class PersonalData
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int Home { get; set; }
    }

    public class Friend
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }

    public class IpAddress
    {
        public string Ip { get; set; }
        public string Port { get; set; }

        public string Full => $"{Ip}:{Port}";
        public Dictionary<string, bool> Protocol { get; set; }
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
}