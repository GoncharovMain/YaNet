using System.Text;

namespace YaNet
{
    public class Marker
    {
        private StringBuilder _buffer;

        public Marker(string buffer) : this(new StringBuilder(buffer)) { }

        public Marker(StringBuilder buffer)
            => _buffer = buffer;

        public string Buffer(Mark mark)
            => _buffer.ToString(mark.Start, mark.Length);
    }
}