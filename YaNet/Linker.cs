using System.Text;
using YaNet.Features;

namespace YaNet
{
	public class Linker
	{
		private Row[] _rows;

			
		private int _current;

		public Linker(Row[] row)
		{
			_current = 0;
			_rows = row;
		}

		public Dictionary<Row, Row> ToLink()
		{
			Dictionary<Row, Row> d = new();

			int indent = 0;

			int maxIndent = 0;

			// get max indent
			for (int i = 0; i < _rows.Length; i++)
			{
				if (_rows[i].Indent > indent)
				{
					indent = _rows[i].Indent;
				}
			}

			return d;
		}
	}
}