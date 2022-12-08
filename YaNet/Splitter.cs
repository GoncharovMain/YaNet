using System.Text;
using YaNet.Lines;

namespace YaNet
{
	public class Splitter
	{
		private StringBuilder _buffer;

		private Offset[] _offsets;

		public Splitter(string text) : this(new StringBuilder(text)) { }

		public Splitter(StringBuilder text) => _buffer = text;

		public Splitter(Line[] lines)
		{

			_buffer = lines[0].Buffer;
		}

		public Offset Split()
		{
			return null;
		}

		public Line this[int index]
		{
			get => new Line(_buffer, _offsets[index]);
		}
	}
}