using System.Text;
using YaNet;

namespace YaNet.Lines
{
	public class ListLine : Line
	{
		private char[] _key;
		private Line[] _items;

		public ListLine() { }

		public ListLine(StringBuilder text, Offset offset) : base(text, offset) { }

		public ListLine(string text, Offset offset) : base(new StringBuilder(text), offset) { }
	}
}