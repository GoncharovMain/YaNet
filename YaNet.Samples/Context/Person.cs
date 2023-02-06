namespace YaNet.Samples.Context
{
    public class Data
    {
        public Person Person { get; set; }
        public IpAddress IpAddress { get; set; }

        //public IP ip { get; set; }
        public IP Ip { get; set; }

        //public int[][] Matrix { get; set; }

        public List<string> Matrices { get; set; }
        private List<int> vector { get; set; }
        public string Vector
        {
            get => String.Join(", ", vector);
            set => vector = value.Split(", ").Select(item => Convert.ToInt32(item)).ToList();
        }

        public List<int> GetVector() => vector;
        public List<List<int>> Matrix { get; set; }
    }

    public class Person
    {
        public PersonalData PersonalData { get; set; }
        public Address Address { get; set; }

        public List<string> VisitCountries { get; set; }

        public List<Friend> Friends { get; set; }

        //public List<string> Languages { get; set; }
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
        public string Sex { get; set; }
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
}