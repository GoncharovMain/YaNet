namespace YaNet
{
    public struct Mark
    {
        public int Start { get; private set; }
        public int End { get; private set; }
        public int Length => End - Start + 1;

        public Mark(int start, int end)
        {
            Start = start;
            End = end;
        }

        public Mark(Mark mark)
        {
            Start = mark.Start;
            End = mark.End;
        }

        public static implicit operator String(Mark mark)
            => mark.ToString();

        public static implicit operator Mark((int start, int end) mark)
            => new Mark(mark.start, mark.end);

        public override string ToString() => $"[{Start}:{End}:{Length}]";
    }
}