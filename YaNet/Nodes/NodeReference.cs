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

        public new void Init(object obj, StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            string key = marker.Buffer(Key);

            PropertyInfo property = obj.GetType().GetProperty(key);


            object inner = Activator.CreateInstance(property.PropertyType);

            property.SetValue(obj, inner);

            Nodes.Init(inner, buffer);
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