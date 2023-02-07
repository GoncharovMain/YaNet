using System.Text;
using YaNet.Features;
using YaNet.Exceptions;

namespace YaNet
{
    public class Parser
    {
        private StringBuilder _buffer;
        private Marker _marker;

        public Parser(StringBuilder buffer)
        {
            _buffer = buffer;
            _marker = new Marker(_buffer);
        }

        public object Parse(object obj)
        {
            //Mark[] rows = new Peeker(_buffer).Split('\n');


            Qualifier qualifier = new Qualifier(_buffer);
            //Definer definer = new Definer(_buffer, rows);

            // ignore first comments
            //definer.FirstNext();

            Collection collection = qualifier.DefineCollection();

            collection.Init(ref obj, _buffer);

            collection.Print(_buffer);

            return obj;
        }
    }
}