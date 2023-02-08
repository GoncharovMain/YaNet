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
            Type type = obj.GetType();

            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();

                MethodInfo methodAdd = type.GetMethod("Add");

                if (genericType == typeof(List<>))
                {
                    Type itemType = type.GetGenericArguments().First();

                    foreach (INode node in Nodes)
                    {
                        object item = Instancer.Empty(itemType);

                        node.Init(ref item, buffer);

                        methodAdd.Invoke(obj, new[] { item });
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

                        methodAdd.Invoke(obj, pair);
                    }

                    return;
                }

                throw new Exception($"Not support generic type: '{genericType.Name}'.");
            }

            if (type.IsArray)
            {
                #region types arrays
                // Stepped arrays.
                // use GetType() and GetElementType()
                // array = int[][][]
                // array.GetType() => int[][][]
                // array.GetType().GetElementType() => int[][]
                // array.GetType().GetElementType().GetElementType() => int[]
                // CreateInstance(Type, Int32[], Int32[])

                // Multidimensional arrays. 
                // use Rank
                // Rank for int[] => 1
                // Rank for int[,] => 2
                // Rank for int[,,] => 3
                // Rank for int[,,,] => 4
                // CreateInstance(Type, Int32[])

                #endregion types arrays

                // For stepped array.
                Type elementType = type.GetElementType();

                Array array = (Array)obj;
                
                if (elementType.IsArray)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        // bad code !!!
                        int length = ((Collection)(((Item)Nodes[i]).Node)).Nodes.Length;

                        object element = Activator.CreateInstance(elementType, length);

                        Nodes[i].Init(ref element, buffer);

                        array.SetValue(element, i);
                    }

                    return;
                }

                for (int i = 0; i < Nodes.Length; i++)
                {
                    object element = Instancer.Empty(elementType);

                    Nodes[i].Init(ref element, buffer);

                    array.SetValue(element, i);
                }

                return;
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