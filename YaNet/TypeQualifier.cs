using System.Text;
using YaNet.Features;

namespace YaNet
{
	public class TypeQualifier
	{
		private Peeker _peeker;
		private StringBuilder _buffer;
		private Mark _mark;

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
		
		public RowType Quilify()
		{
			int lastIndex = _buffer.Length - 1;

			if (_mark.End + 1 <= lastIndex)
			{
				return RowType.NoneRow;
			}

			int start = _mark.End;
			int end;

			for (end = start + 1; end < _mark.Length; end++)
			{
				if (_buffer[end] == '\n')
				{
					break;
				}
			}
			
			int countIndent = new Peeker(_buffer, start, end).CountIndent("\t");

			

			return RowType.NoneRow;
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