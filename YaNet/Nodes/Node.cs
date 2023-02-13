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

            Type type = obj.GetType();

            // for dictionary
            if (type == typeof(object[]))
            {
                var pair = (object[])obj;

                pair[0] = Instancer.ToConvert(pair[0].GetType(), propertyName);

                Nodes.Init(ref pair[1], buffer);

                return;
            }


            PropertyInfo property = type.GetProperty(propertyName);
            Type propertyType = property.PropertyType;

            object value;

            if (propertyType.IsArray)
            {
                // repeat code in file Collection.cs

                int rankArray = propertyType.GetArrayRank();
                Type elementType = propertyType.GetElementType();

                Collection current = Nodes as Collection;

                int[] ranks = new int[rankArray];
                int numberRank = 0;
                    
                ranks[numberRank] = current.Length;

                for (numberRank++; numberRank < rankArray; numberRank++)
                {
                    current = (current.Nodes[0] as Item).Node as Collection;

                    ranks[numberRank] = current.Length;
                }

                value = Array.CreateInstance(elementType, ranks);
            }
            else
            {
                value = Instancer.Empty(propertyType);
            }


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