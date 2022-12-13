using System.Text;

namespace YaNet.Rows
{
	public class ScalarRow : Row
	{
		private char[] _key;
		private char[] _values;
		private char[] _references;

		public ScalarRow() { }

		public ScalarRow(StringBuilder text, Mark mark) : base(text, mark) { }
		
		public ScalarRow(string text, Mark mark) : base(new StringBuilder(text), mark) { }
	}
}