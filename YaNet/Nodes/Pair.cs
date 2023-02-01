namespace YaNet.Nodes
{
    public class Pair : INode
    {
        public Mark Key { get; set; } // replace type Mark on Scalar
        public Mark Value { get; set; }

        public Pair(Mark key, Mark value)
        {
            Key = key;
            Value = value;
        }

        public void Init(ref object obj, StringBuilder buffer)
        {
            object[] parameters = (object[])obj;

            Marker marker = new Marker(buffer);

            parameters[0] = Instancer.ToConvert(parameters[0].GetType(), marker.Buffer(Key));
            parameters[1] = Instancer.ToConvert(parameters[1].GetType(), marker.Buffer(Value));
        }

        public static implicit operator Node(Pair pair)
            => new Node(pair.Key, null);

        public void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"'{marker.Buffer(Key)}' : '{marker.Buffer(Value)}'");
        }
    }
}