using System.Text;
using YaNet;

namespace YaNet.Features
{
	public class Row
	{
		private Mark _row;

		public Type Type { get; private set; }


		protected Row(Row row, Type type) : this(row._row, type) { }

		protected Row(Mark row, Type type) => (_row, Type) = (row, type);


		public Row(Row row) : this(row._row) => Type = typeof(Row);

		public Row(Mark row) => (_row, Type) = (row, typeof(Row));
	}

	public class KeyRow : Row
	{
		private Mark _key;


		protected KeyRow(Row row, Mark key, Type type) 
			: base(row, type) => _key = key;

		protected KeyRow(Mark row, Mark key, Type type) 
			: base(row, type) => _key = key;


		public KeyRow(Row row, Mark key) 
			: base(row, typeof(KeyRow)) => _key = key;

		public KeyRow(Mark row, Mark key) 
			: base(row, typeof(KeyRow)) => _key = key;
	}

	public class KeyValueRow : KeyRow
	{
		private Mark _value;


		public KeyValueRow(Row row, Mark key, Mark value) 
			: base(row, key, typeof(KeyValueRow)) => _value = value;

		public KeyValueRow(Mark row, Mark key, Mark value) 
			: base(row, key, typeof(KeyValueRow)) => _value = value;
	}

	public class ItemRow : Row
	{
		private Mark _item;
		

		public ItemRow(Row row, Mark item) 
			: base(row, typeof(ItemRow)) => _item = item;

		public ItemRow(Mark row, Mark item) 
			: base(row, typeof(ItemRow)) => _item = item;
	}

	public class ItemKeyValueRow : Row
	{
		private Mark _item;
		private Mark _key;
		private Mark _value;

		public ItemKeyValueRow(Row row, Mark item, Mark key, Mark value)
			: base(row, typeof(ItemKeyValueRow))
		{
			_item = item;
			_key = key;
			_value = value;
		}

		public ItemKeyValueRow(Mark row, Mark item, Mark key, Mark value) 
			: base(row, typeof(ItemKeyValueRow))
		{
			_item = item;
			_key = key;
			_value = value;
		}
	}
}