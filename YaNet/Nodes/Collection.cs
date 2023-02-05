using System.Xml.Linq;

namespace YaNet.Nodes
{
    public class Collection : INode
    {
        public INode[] Nodes { get; set; }

        public Collection(params INode[] nodes)
        {
            Nodes = nodes;
        }

        public void Init(ref object obj, StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Type type = obj.GetType();

            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();


                if (genericType == typeof(List<>))
                {
                    Type itemType = type.GetGenericArguments().First();

                    foreach (INode node in Nodes)
                    {
                        object item = Instancer.Empty(itemType);

                        node.Init(ref item, buffer);

                        type.GetMethod("Add").Invoke(obj, new[] { item });
                    }

                    return;
                }

                if (genericType == typeof(Dictionary<,>))
                {
                    Type[] genericArguments = type.GetGenericArguments();

                    Type keyType = genericArguments[0];
                    Type valueType = genericArguments[1];

                    foreach (INode node in Nodes)
                    {
                        object key = Instancer.Empty(keyType);
                        object value = Instancer.Empty(valueType);

                        object parameters = new object[] { key, value };

                        node.Init(ref parameters, buffer);

                        object[] pair = (object[])parameters;

                        obj.GetType().GetMethod("Add").Invoke(obj, pair);
                    }

                    return;
                }

                throw new Exception($"Not support generic type: '{genericType.Name}'.");
            }

            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Init(ref obj, buffer);
            }
        }

        public void Print(StringBuilder buffer)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Print(buffer);
            }
        }
    }
}