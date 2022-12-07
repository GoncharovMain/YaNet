using YaNet.Lines;

namespace YaNet
{
	public class Offset
	{
		public int Start { get; set; }
		public int End { get; set; }
		public int Length => End - Start + 1;

		public Offset(int start, int end)
		{
			Start = start;
			End = end;
		}

		public static implicit operator String(Offset offset)
			=> offset.ToString();

		public static implicit operator Offset((int start, int end) offset)
			=> new Offset(offset.start, offset.end);

		public override string ToString() => $"Start: {Start} end: {End} length: {Length}";
	}
}