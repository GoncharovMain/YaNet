namespace YaNet.Samples.Context
{
    public class RequestData
    {
        public string Host { get; set; }
        public Dictionary<string, Request> Requests { get; set; }
    }
    public class Request
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public Headers Headers { get; set; }
        public MimeType MimeType { get; set; }
        public Body Body { get; set; }
    }
    
    public class Body
    {
        public User User { get; set; }
    }
    public class User
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }

    public class MimeType
    {
        public string Format { get; set; }
        public string Charset { get; set; }

        public static implicit operator MimeType(string mimeType)
        {
            string[] data = mimeType.Split(';');

            return new MimeType { Format = data[0].Trim(), Charset = data[1].Trim() };
        }

        public static implicit operator string(MimeType type)
        {
            return $"{type.Format}; {type.Charset}";
        }
    }
    public class Headers
    {
        public string Host { get; set; }
        public string UserAgent { get; set; }
        public string Referrer { get; set; }
        public string Origin { get; set; }
    }
}