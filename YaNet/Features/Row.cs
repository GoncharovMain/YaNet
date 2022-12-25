using YaNet;

namespace YaNet.Features
{
	public class Row
	{
		protected Mark _row;

		protected int _indent;

		public Structures Structure = Structures.Unknown;

		public Dictionary<int, Row> References { get; private set; }

		public int Indent => _indent;

		public Mark Mark => _row;


		public Type Type { get; private set; }

		protected Row(int indent, Row row, Type type) : this(indent, row._row, type) { }

		protected Row(int indent, Mark row, Type type)
		{
			_indent = indent;
			_row = row;
			Type = type;

			References = new Dictionary<int, Row>();
		}


		public Row(int indent, Row row) : this(indent, row._row, typeof(Row)) { }

		public Row(int indent, Mark row) : this(indent, row, typeof(Row)) { }


		public virtual void Info() 
			=> Console.WriteLine($"[{_indent}] row: {_row} type: {Type.Name}");
	}
}