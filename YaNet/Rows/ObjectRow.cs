using System.Text;

namespace YaNet.Rows
{
	public class ObjectRow : Row
	{
		private Row[] _lines;

		public ObjectRow() { }

		public ObjectRow(StringBuilder text, Mark mark) : base(text, mark) { }

		public ObjectRow(string text, Mark mark) : base(new StringBuilder(text), mark) { }
	}
}