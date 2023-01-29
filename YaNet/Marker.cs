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

        public int Count(char symbol, Mark mark)
        {
            int count = 0;

            for (int i = mark.Start; i <= mark.End; i++)
            {
                if (_buffer[i] == symbol)
                    count++;
            }

            return count;
        }
    }
}