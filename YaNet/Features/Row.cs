using System.Text;
using YaNet;

namespace YaNet.Features
{
	public class Row
	{
		protected Mark _row;

		protected int _indent;


		public Type Type { get; private set; }

		protected Row(int indent, Row row, Type type) : this(indent, row._row, type) { }

		protected Row(int indent, Mark row, Type type) => (_indent, _row, Type) = (indent, row, type);


		public Row(int indent, Row row) : this(indent, row._row, typeof(Row)) { }

		public Row(int indent, Mark row) : this(indent, row, typeof(Row)) { }


		public virtual void Info() 
			=> Console.WriteLine($"indent: {_indent} row: {_row} type: {Type.Name}");
	}

	public class KeyRow : Row
	{
		protected Mark _key;


		protected KeyRow(int indent, Row row, Mark key, Type type) 
			: base(indent, row, type) => _key = key;

		protected KeyRow(int indent, Mark row, Mark key, Type type) 
			: base(indent, row, type) => _key = key;


		public KeyRow(int indent, Mark row, Mark key) 
			: base(indent, row, typeof(KeyRow)) => _key = key;

		public KeyRow(int indent, Row row, Mark key)
			: base(indent, row, typeof(KeyRow)) => _key = key;

		public override void Info() 
			=> Console.WriteLine($"indent: {_indent} row: {_row} type: {Type.Name} key: {_key}");
	}

	public class KeyValueRow : KeyRow
	{
		private Mark _value;


		public KeyValueRow(int indent, Row row, Mark key, Mark value) 
			: base(indent, row, key, typeof(KeyValueRow)) => _value = value;

		public KeyValueRow(int indent, Mark row, Mark key, Mark value) 
			: base(indent, row, key, typeof(KeyValueRow)) => _value = value;

		public override void Info() 
			=> Console.WriteLine($"indent: {_indent} row: {_row} type: {Type.Name} key: {_key} value: {_value}");
	}

	public class ItemRow : Row
	{
		private Mark _item;


		public ItemRow(int indent, Row row, Mark item) 
			: base(indent, row, typeof(ItemRow)) => _item = item;

		public ItemRow(int indent, Mark row, Mark item) 
			: base(indent, row, typeof(ItemRow)) => _item = item;

		public override void Info() 
			=> Console.WriteLine($"indent: {_indent} row: {_row} type: {Type.Name} item: {_item}");
	}

	public class ItemKeyValueRow : Row
	{
		private Mark _item;
		private Mark _key;
		private Mark _value;

		public ItemKeyValueRow(int indent, Row row, Mark item, Mark key, Mark value)
			: base(indent, row, typeof(ItemKeyValueRow))
		{
			_item = item;
			_key = key;
			_value = value;
		}

		public ItemKeyValueRow(int indent, Mark row, Mark item, Mark key, Mark value) 
			: base(indent, row, typeof(ItemKeyValueRow))
		{
			_item = item;
			_key = key;
			_value = value;
		}

		public override void Info() 
			=> Console.WriteLine($"indent: {_indent} row: {_row} type: {Type.Name} item: {_item} key: {_key} value: {_value}");
	}
}