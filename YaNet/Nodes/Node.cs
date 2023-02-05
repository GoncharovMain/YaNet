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

            // for dictionary
            if (obj.GetType() == typeof(object[]))
            {
                var pair = (object[])obj;

                pair[0] = Instancer.ToConvert(pair[0].GetType(), propertyName);

                Nodes.Init(ref pair[1], buffer);

                return;
            }

            PropertyInfo property = obj.GetType().GetProperty(propertyName);

            object value = Instancer.Empty(property.PropertyType);

            Nodes.Init(ref value, buffer);

            property.SetValue(obj, value);
        }

        public virtual void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"'{marker.Buffer(Key)}':");

            Nodes.Print(buffer);
        }
    }
}