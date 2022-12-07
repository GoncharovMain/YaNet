using System.Text;
using YaNet.Lines;

namespace YaNet
{
	public class TypeQualifier
	{
		// levelIndent, 
		// tokenIndent, 
		// tokenType, 
		// lengthTokenType, 
		// start, 
		// end, 
		// peeker, 
		// isList, 
		// isDict, 
		// isScalar

		private Peeker _peeker;
		private StringBuilder _buffer;
		private Offset _offset;

		public TypeQualifier() { }

		public TypeQualifier(string text) : this(new StringBuilder(text), new Offset(0, text.Length - 1)) { }

		public TypeQualifier(StringBuilder buffer) : this(buffer, new Offset(0, buffer.Length)) { }

		public TypeQualifier(string text, Offset offset) : this(new StringBuilder(text), offset) { }

		public TypeQualifier(StringBuilder buffer, Offset offset)
		{
			_peeker = new Peeker(buffer, offset.Start, offset.End);
			_buffer = buffer;
			_offset = offset;
		}
		
		public LineType Quilify()
		{
			int lastIndex = _buffer.Length - 1;

			if (_offset.End + 1 <= lastIndex)
			{
				return LineType.NoneLine;
			}

			int start = _offset.End;
			int end;

			for (end = start + 1; end < _offset.Length; end++)
			{
				if (_buffer[end] == '\n')
				{
					break;
				}
			}
			
			int countIndent = new Peeker(_buffer, start, end).CountIndent("\t");

			

			return LineType.NoneLine;
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


		public static Line GetLine(LineType typeList)
			=> typeList switch
			{
				LineType.Scalar => new ScalarLine(),
				LineType.List => new ListLine(),
				LineType.Dictionary => new DictionaryLine(),
				LineType.Object => new ObjectLine(),
				_ => new Line()
			};
	}
}