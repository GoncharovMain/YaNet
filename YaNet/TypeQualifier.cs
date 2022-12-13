using System.Text;
using YaNet.Features;

namespace YaNet
{
	public class TypeQualifier
	{
		private Peeker _peeker;
		private StringBuilder _buffer;
		private Mark _mark;

		private Row _row;

		public TypeQualifier(StringBuilder text, Row row) => (_row, _buffer) = (row, text);

		public TypeQualifier() { }

		public TypeQualifier(string text) : this(new StringBuilder(text), new Mark(0, text.Length - 1)) { }

		public TypeQualifier(StringBuilder buffer) : this(buffer, new Mark(0, buffer.Length)) { }

		public TypeQualifier(string text, Mark mark) : this(new StringBuilder(text), mark) { }

		public TypeQualifier(StringBuilder buffer, Mark mark)
		{
			_peeker = new Peeker(buffer, mark.Start, mark.End);
			_buffer = buffer;
			_mark = mark;
		}

		public Row QualifyFeature()
		{
			bool isKeyValueRow = new Peeker(_buffer, _mark).Contains(": ");

			Console.WriteLine($"isKeyValueRow: {isKeyValueRow}");

			return null;
		}


		public string ScalarTrace()
		{
			int levelIndent = _peeker.CountIndent("\t");

			string nextIndent = '\t'.Repeat(levelIndent);
		
			return $"{nextIndent}";
		}

		public string ListTrace()
		{
			int levelIndent = _peeker.CountIndent("\t");

			string nextIndent = '\t'.Repeat(levelIndent + 1);

			return $"{nextIndent}- ";
		}

		public string ObjectTrace()
		{
			int levelIndent = _peeker.CountIndent("\t");

			string nextIndent = '\t'.Repeat(levelIndent + 1);

			return nextIndent;
		}
	}
}