using System.Text;
using YaNet;

namespace YaNet.Lines
{
	public class Line
	{

		public StringBuilder Buffer => _buffer;
		private StringBuilder _buffer;
		private string _indent;
		private int _lengthIndent;
		private int _countIndents;
		private int _levelIndent;

		private TypeQualifier _typeQualifier;

		private Offset _offset;

		public Offset Offset => _offset;

		public int CountIndent => _countIndents;
		public string Indent => _indent.Repeat(_countIndents);

		public Line()
		{
			_indent = "\t";
			_lengthIndent = _indent.Length;

			_typeQualifier = new TypeQualifier();
		}

		public Line(StringBuilder text, Offset offset) : this()
		{
			_buffer = text;
			_offset = offset;

			_countIndents = new Peeker(_buffer, offset).CountIndent(_indent);
		}

		public Line(string text, Offset offset) : this(new StringBuilder(text), offset) { }

		public override string ToString() => _buffer.ToString(_offset.Start, _offset.Length);

		public static implicit operator String(Line line)
			=> line.ToString();
	}
}