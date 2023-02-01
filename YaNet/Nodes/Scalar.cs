namespace YaNet.Nodes
{
    public class Scalar : INode
    {
        public Mark Value { get; set; }

        public Scalar(Mark value) => Value = value;

        public static implicit operator Mark(Scalar scalar)
            => new Mark(scalar.Value);

        public static implicit operator String(Scalar scalar)
            => scalar.ToString();

        public void Init(ref object obj, StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            string value = marker.Buffer(Value);

            obj = Instancer.ToConvert(obj.GetType(), value);
        }

        public void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"- '{marker.Buffer(Value)}'");
        }
    }
}