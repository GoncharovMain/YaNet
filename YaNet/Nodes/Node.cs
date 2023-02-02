namespace YaNet.Nodes
{
    public class Node : INode
    {
        public Mark Key { get; set; }
        public INode Nodes { get; set; }

        public Node(Mark key, INode nodes)
        {
            Key = key;
            Nodes = nodes;
        }
        public void Init(ref object obj, StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            string propertyName = marker.Buffer(Key);

            Console.WriteLine($"In Node, object type: {obj.GetType().Name} for property: {propertyName} of type: ");

            PropertyInfo property = obj.GetType().GetProperty(propertyName);





            object value = Instancer.Empty(property.PropertyType);

            Console.WriteLine($"Property type of {obj.GetType().Name} - {property.PropertyType.Name}");

            Nodes.Init(ref value, buffer);
        }

        public virtual void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"'{marker.Buffer(Key)}':");

            Nodes.Print(buffer);
        }
    }
}