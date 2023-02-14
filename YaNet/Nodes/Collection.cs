using System;

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
                    InitAsList(ref obj, type, methodAdd, buffer);

                    return;
                }

                if (genericType == typeof(Dictionary<,>))
                {
                    InitAsDictionary(ref obj, type, methodAdd, buffer);

                    return;
                }

                throw new Exception($"Not support generic type: '{genericType.Name}'.");
            }

            if (type.IsArray)
            {
                InitAsArray(ref obj, type.GetElementType(), buffer);

                return;
            }

            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Init(ref obj, buffer);
            }
        }

        private void InitAsList(ref object obj, Type type, MethodInfo methodAdd, StringBuilder buffer)
        {
            Type itemType = type.GetGenericArguments().First();

            foreach (INode node in Nodes)
            {
                object item = Instancer.Empty(itemType);

                node.Init(ref item, buffer);

                methodAdd.Invoke(obj, new[] { item });
            }
        }

        private void InitAsDictionary(ref object obj, Type type, MethodInfo methodAdd, StringBuilder buffer)
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
        }

        private void InitAsArray(ref object obj, Type elementType, StringBuilder buffer)
        {
            Array array = (Array)obj;

            int[] maxRanks = RankPosition.GetMaxRanks(array);

            RankPosition rankPosition = new RankPosition(maxRanks);


            if (elementType.IsArray)
            {
                do
                {
                    #region get rank for stepped array

                    Collection current = this;

                    for (int i = 0; i < rankPosition.Length; i++)
                    {
                        current = (current.Nodes[rankPosition[i]] as Item).Node as Collection;
                    }

                    int rankInnerArray = elementType.GetArrayRank();

                    int[] innerRanks = new int[rankInnerArray];

                    int innerNumberRank = 0;

                    innerRanks[innerNumberRank] = current.Length;

                    for (innerNumberRank++; innerNumberRank < rankInnerArray; innerNumberRank++)
                    {
                        current = (current.Nodes[innerNumberRank] as Item).Node as Collection;

                        innerRanks[innerNumberRank] = current.Length;
                    }

                    #endregion get rank for inner array

                    Collection innerCurrent = this;

                    for (int numberRank = 0; numberRank < array.Rank - 1; numberRank++)
                    {
                        innerCurrent = (innerCurrent.Nodes[rankPosition[numberRank]] as Item).Node as Collection;
                    }

                    object element = Array.CreateInstance(elementType.GetElementType(), innerRanks);

                    innerCurrent.Nodes[rankPosition.Last].Init(ref element, buffer);

                    array.SetValue(element, rankPosition);

                } while (rankPosition.MoveNext());

                return;
            }

            do
            {
                Collection current = this;

                for (int numberRank = 0; numberRank < array.Rank - 1; numberRank++)
                {
                    current = (current.Nodes[rankPosition[numberRank]] as Item).Node as Collection;
                }

                object element = Instancer.Empty(elementType);

                current.Nodes[rankPosition.Last].Init(ref element, buffer);

                array.SetValue(element, rankPosition);

            } while (rankPosition.MoveNext());
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