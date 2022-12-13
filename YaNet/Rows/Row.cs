using System.Text;
using YaNet;

namespace YaNet.Rows
{
	public class Row
	{

		private StringBuilder _buffer;
		private Mark _mark;
		private string _indent;
		
		private int _lengthIndent;
		private int _countIndents;
		private int _levelIndent;



		public StringBuilder Buffer => _buffer;
		public Mark Mark => _mark;
		public int CountIndent => _countIndents;
		public string Indent => _indent.Repeat(_countIndents);

		public Row()
		{
			_indent = "\t";
			_lengthIndent = _indent.Length;
		}

		public Row(StringBuilder text, Mark mark) : this()
		{
			_buffer = text;
			_mark = mark;

			_countIndents = new Peeker(_buffer, mark).CountIndent(_indent);
		}

		public Row(string text, Mark mark) : this(new StringBuilder(text), mark) { }

		public override string ToString() => _buffer.ToString(_mark.Start, _mark.Length);

		public static implicit operator String(Row row)
			=> row.ToString();
	}
}