namespace YaNet.Nodes
{
    public class NodeReference : Node
    {
        public Mark Reference { get; set; }

        public NodeReference(Mark Key, Mark reference, INode nodes)
            : base(Key, nodes)
        {
            Reference = reference;
        }

        public new void Init(ref object obj, StringBuilder buffer)
        {
            base.Init(ref obj, buffer);
        }

        public static explicit operator NodeReference(Pair pair)
            => new NodeReference(pair.Key, pair.Value, null);

        public override void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"'{marker.Buffer(Key)}' : '{marker.Buffer(Reference)}'");

            Nodes.Print(buffer);
        }
    }
}