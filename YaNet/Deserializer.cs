namespace YaNet
{
    public class Deserializer
    {
        private Parser _parser;

        private string _path;

        private StringBuilder _buffer;

        public Deserializer(StringBuilder text)
        {
            _buffer = text;

            _parser = new Parser(_buffer);
        }
        public Deserializer(string text) : this(new StringBuilder(text)) { }

        public T Deserialize<T>()
        {
            T t = (T)Activator.CreateInstance(typeof(T));

            return (T)_parser.Parse(t);
        }
    }
}