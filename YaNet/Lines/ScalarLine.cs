using System.Text;

namespace YaNet.Lines
{
	public class ScalarLine : Line
	{
		private char[] _key;
		private char[] _values;
		private char[] _references;

		public ScalarLine() { }

		public ScalarLine(StringBuilder text, Offset offset) : base(text, offset) { }
		
		public ScalarLine(string text, Offset offset) : base(new StringBuilder(text), offset) { }
	}
}