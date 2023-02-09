using System;
using System.Xml.Linq;

namespace YaNet.Nodes
{
    public class Collection : INode
    {
        public INode[] Nodes { get; set; }

        public int Length => Nodes.Length;

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

                int rank = array.Rank;

                int[] position = new int[rank];


                for (int numberRank = 0; numberRank < rank; numberRank++)
                {
                    Collection current = ((Nodes[numberRank] as Item).Node as Collection);

                    int length = current.Length;

                    for (int indexRank = 0; indexRank < array.GetLength(numberRank); indexRank++)
                    {
                        for (int i = 0; i < array.GetLength(numberRank); i++)
                        {

                        }

                        position[numberRank] = indexRank;

                        if (elementType.IsArray)
                        {


                            int elementRank = elementType.GetArrayRank();


                            for (int i = 0; i < array.Length; i++)
                            {

                                // bad code !!!
                                int length0 = InnerCollection(Nodes[i]).Length;

                                object element = Activator.CreateInstance(elementType, length0);

                                Nodes[i].Init(ref element, buffer);

                                array.SetValue(element, i);
                            }

                            return;
                        }
                    }
                }

                // move to rank init
                for (int i = 0; i < Nodes.Length; i++)
                {
                    object element = Instancer.Empty(elementType);

                    Nodes[i].Init(ref element, buffer);

                    array.SetValue(element, i);
                }
                
          
                //int[] position = new int[rank];

                //// inner collection.items[0]
                //// ����� ���� �� ������� rank?

                //INode current = Nodes[0];
    
                //for (int numberRank = 0; numberRank < rank; numberRank++)
                //{
                //    current = ((current as Item).Node as Collection);


                //    for (int i = 0; i < array.GetLength(numberRank); i++)
                //    {
                //        position[numberRank] = i;

                //        object item = Instancer.Empty(elementType);

                //        current.Init(ref item, buffer);

                //        array.SetValue(item, position);
                //    }
                //}

                return;
            }

            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Init(ref obj, buffer);
            }
        }

        public INode[] InnerCollection(INode node) => ((Collection)(((Item)node).Node)).Nodes;
        public INode[] InnerCollection(int index) => ((Collection)(((Item)Nodes[index]).Node)).Nodes;

        public void Print(StringBuilder buffer)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Print(buffer);
            }
        }
    }
}