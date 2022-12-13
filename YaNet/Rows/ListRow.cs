using System.Text;
using YaNet;

namespace YaNet.Rows
{
	public class ListRow : Row
	{
		private char[] _key;
		private Row[] _items;

		public ListRow() { }

		public ListRow(StringBuilder text, Mark mark) : base(text, mark) { }

		public ListRow(string text, Mark mark) : base(new StringBuilder(text), mark) { }
	}
}