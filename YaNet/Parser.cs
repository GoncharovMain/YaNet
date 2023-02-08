namespace YaNet
{
    public class Parser
    {
        private StringBuilder _buffer;

        public Parser(StringBuilder buffer)
        {
            _buffer = buffer;
        }

        public object Parse(object obj)
        {
            Qualifier qualifier = new Qualifier(_buffer);

            Collection collection = qualifier.DefineCollection();

            collection.Init(ref obj, _buffer);

            collection.Print(_buffer);

            return obj;
        }
    }
}