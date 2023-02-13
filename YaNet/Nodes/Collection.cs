using System;
using System.ComponentModel;
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
                Array array = (Array)obj;

                Type elementType = type.GetElementType();

                int[] maxRanksUp = RankPosition.GetMaxRanks(array);

                RankPosition rankPositionUp = new RankPosition(maxRanksUp);


                if (elementType.IsArray)
                {
                    int rankInnerArray = elementType.GetArrayRank();

                    Collection innerCurrent = this;

                    for (int i = 0; i < rankPositionUp.Length; i++)
                    {
                        innerCurrent = (innerCurrent.Nodes[0] as Item).Node as Collection;
                    }


                    int[] ranks = new int[rankInnerArray];

                    int innerNumberRank = 0;

                    ranks[innerNumberRank] = innerCurrent.Length;

                    for (innerNumberRank++; innerNumberRank < rankInnerArray; innerNumberRank++)
                    {
                        innerCurrent = (innerCurrent.Nodes[0] as Item).Node as Collection;

                        ranks[innerNumberRank] = innerCurrent.Length;
                    }


                    do
                    {
                        Collection current = this;

                        for (int numberRank = 0; numberRank < array.Rank - 1; numberRank++)
                        {
                            current = (current.Nodes[rankPositionUp[numberRank]] as Item).Node as Collection;
                        }

                        object element = Array.CreateInstance(elementType.GetElementType(), ranks);

                        current.Nodes[rankPositionUp.Last].Init(ref element, buffer);


                        array.SetValue(element, rankPositionUp);

                    } while (rankPositionUp.MoveNext());

                    return;
                }

                do
                {
                    Collection current = this;

                    for (int numberRank = 0; numberRank < array.Rank - 1; numberRank++)
                    {
                        current = (current.Nodes[rankPositionUp[numberRank]] as Item).Node as Collection;
                    }

                    object element = Instancer.Empty(elementType);

                    current.Nodes[rankPositionUp.Last].Init(ref element, buffer);

                    array.SetValue(element, rankPositionUp);

                } while (rankPositionUp.MoveNext());

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