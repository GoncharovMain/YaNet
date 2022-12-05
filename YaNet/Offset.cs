namespace YaNet
{
	public class Offset
	{
		public int Start { get; set; }
		public int End { get; set; }

		public Offset(int start, int end)
		{
			Start = start;
			End = end;
		}

		public static implicit operator String(Offset offset)
			=> offset.ToString();

		public override string ToString() => $"Start: {Start} end: {End}";
	}
}