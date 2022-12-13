using System.Text;
using YaNet;

namespace YaNet.Features
{
	public class Row
	{
		private Mark _row;
		private int _position;

		public Row(Row row) : this(row._position, row._row) { }

		public Row(int position, Mark row) => (_position, _row) = (position, row);
	}

	public class KeyRow : Row
	{
		private Mark _key;

		public KeyRow(Row row, Mark key) : base(row) => _key = key;

		public KeyRow(int position, Mark row, Mark key) 
			: base(position, row) => _key = key;
	}

	public class KeyValueRow : KeyRow
	{
		private Mark _value;

		public KeyValueRow(Row row, Mark key, Mark value) 
			: base(row, key) => _value = value;

		public KeyValueRow(int position, Mark row, Mark key, Mark value) 
			: base(position, row, key) => _value = value;
	}

	public class ItemRow : Row
	{
		private Mark _item;

		public ItemRow(Row row, Mark item) : base(row) => _item = item;

		public ItemRow(int position, Mark row, Mark item) 
			: base(position, row) => _item = item;
	}

	public class ItemKeyValueRow : Row
	{
		private Mark _item;
		private Mark _key;
		private Mark _value;

		public ItemKeyValueRow(Row row, Mark item, Mark key, Mark value)
			: base(row)
		{
			_item = item;
			_key = key;
			_value = value;
		}

		public ItemKeyValueRow(int position, Mark row, Mark item, Mark key, Mark value) : base(position, row)
		{
			_item = item;
			_key = key;
			_value = value;
		}
	}
}