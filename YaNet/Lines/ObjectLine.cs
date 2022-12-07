using System.Text;

namespace YaNet.Lines
{
	public class ObjectLine : Line
	{
		private Line[] _lines;

		public ObjectLine() { }

		public ObjectLine(StringBuilder text, Offset offset) : base(text, offset) { }

		public ObjectLine(string text, Offset offset) : base(new StringBuilder(text), offset) { }
	}
}