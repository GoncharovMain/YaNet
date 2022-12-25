using YaNet;

namespace YaNet.Features
{
	public class KeyRow : Row
	{
		protected Mark _key;

		public Mark Key => _key;


		protected KeyRow(int indent, Row row, Mark key, Type type) 
			: base(indent, row, type) => _key = key;

		protected KeyRow(int indent, Mark row, Mark key, Type type) 
			: base(indent, row, type) => _key = key;


		public KeyRow(int indent, Mark row, Mark key) 
			: base(indent, row, typeof(KeyRow)) => _key = key;

		public KeyRow(int indent, Row row, Mark key)
			: base(indent, row, typeof(KeyRow)) => _key = key;

		public override void Info() 
			=> Console.WriteLine($"[{_indent}] row: {_row} type: {Type.Name} key: {_key}");
	}
}