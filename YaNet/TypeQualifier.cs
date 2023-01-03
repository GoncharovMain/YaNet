using System.Text;
using YaNet.Features;
using YaNet.Exceptions;

namespace YaNet
{
	public class QualifyType
	{
		public QualifyType()
		{
			
		}
	}

	public class TypeQualifier
	{
		private Peeker _peeker;
		private StringBuilder _buffer;
		private Mark _mark;

		private int _indent;

		private Row _row;

		public TypeQualifier(StringBuilder text, Row row) : this(text, row.Mark) { }

		public TypeQualifier() { }

		public TypeQualifier(string text) : this(new StringBuilder(text), new Mark(0, text.Length - 1)) { }

		public TypeQualifier(StringBuilder buffer) : this(buffer, new Mark(0, buffer.Length)) { }

		public TypeQualifier(string text, Mark mark) : this(new StringBuilder(text), mark) { }

		public TypeQualifier(StringBuilder buffer, Mark mark)
		{
			_peeker = new Peeker(buffer, mark.Start, mark.End);
			_buffer = buffer;
			_mark = mark;
			_indent = _peeker.Counter("\t");
		}

		public Row QualifyFeature()
		{
			bool isItemRow = _peeker.Contains("- ");
			bool isKeyRow = _peeker.Contains(":\n");
			bool isKeyValueRow = _peeker.Contains(": ");

			bool isItemKeyRow = isItemRow && isKeyRow;
			bool isItemKeyValueRow = isItemRow && isKeyValueRow;

			Console.Write($"{isItemRow}\t{isKeyRow}\t{isKeyValueRow}\t{isItemKeyRow}\t{isItemKeyValueRow}\t");

			if (isItemKeyValueRow)
			{
				return QualifyType<ItemKeyValueRow>();
			}

			if (isItemKeyRow)
			{
				return QualifyType<ItemKeyRow>();
			}

			if (isItemRow)
			{
				return QualifyType<ItemRow>();
			}

			if (isKeyValueRow)
			{
				return QualifyType<KeyValueRow>();
			}

			if (isKeyRow)
			{
				return QualifyType<KeyRow>();
			}


			bool hasNotFeature = !(isItemRow || isKeyRow || isKeyValueRow || isItemKeyValueRow || isItemKeyRow);

			if (hasNotFeature)
			{
				throw new SyntaxException($"'{_peeker}' has not features of type.");
			}

			return null;
		}

		public Row QualifyType<T>() where T : Row
		{
			// * KeyValueRow
			// * ItemRow
			// * KeyRow
			// * ItemKeyRow
			// * ItemKeyValueRow

			if (typeof(T) == typeof(KeyValueRow))
			{
				string delimiter = ": ";

				int startKey = _mark.Start + _indent;

				int endKey = _peeker.IndexOf(delimiter) - 1;


				int startValue = endKey + delimiter.Length + 1;

				int endValue = _peeker.IndexOf("\n") - 1;

				if (endValue == -2)
					endValue = _mark.End;

				Console.WriteLine($"[{_indent}]KeyValueRow: '{new Peeker(_buffer, startKey, endKey)}' : '{new Peeker(_buffer, startValue, endValue)}'");

				return new KeyValueRow(_indent, _mark, (startKey, endKey), (startValue, endValue));
			}

			if (typeof(T) == typeof(ItemRow))
			{
				int startItem = _mark.Start + _indent + "- ".Length;

				int endItem = _peeker.IndexOf("\n") - 1;

				if (endItem == -2)
					endItem = _mark.End;

				Console.WriteLine($"[{_indent}]ItemRow: - '{new Peeker(_buffer, startItem, endItem)}'");

				return new ItemRow(_indent, _mark, (startItem, endItem));
			}

			if (typeof(T) == typeof(KeyRow))
			{
				int startKey = _mark.Start + _indent;

				int endKey = _peeker.IndexOf(":") - 1;

				Console.WriteLine($"[{_indent}]KeyRow: '{new Peeker(_buffer, startKey, endKey)}':");
			
				return new KeyRow(_indent, _mark, (startKey, endKey));
			}

			if (typeof(T) == typeof(ItemKeyRow))
			{
				int startKey = _mark.Start + _indent + "- ".Length;

				int endKey = _peeker.IndexOf(":") - 1;

				Console.WriteLine($"[{_indent}]ItemKeyRow: - '{new Peeker(_buffer, startKey, endKey)}':");

				return new ItemKeyRow(_indent, _mark, (startKey, endKey));
			}

			if (typeof(T) == typeof(ItemKeyValueRow))
			{
				string delimiter = ": ";

				int startKey = _mark.Start + _indent + "- ".Length;

				int endKey = _peeker.IndexOf(delimiter) - 1;

				
				int startValue = endKey + delimiter.Length + 1;

				int endValue = _peeker.IndexOf("\n") - 1;

				if (endValue == -2)
					endValue = _mark.End;


				Console.WriteLine($"[{_indent}]ItemKeyValueRow: - '{new Peeker(_buffer, startKey, endKey)}': '{new Peeker(_buffer, startValue, endValue)}'");

				return new ItemKeyValueRow(_indent, _mark, (startKey, endKey), (startValue, endValue));
			}


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