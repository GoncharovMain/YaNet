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
            /*

            Node.Collection.Pair[]

            Person Person;
            Dictionary<string, string> Person;

            person:
                name: John
                age: 18
                sex: male

            Node.Collection.(Item.Scalar)[]

            List<string> Persons;
            string[] Persons;

            persons:
                - John
                - Bob
                - Patrick


            Node.Collection.Item.Node.Collection.Pair[]

            List<Person> Persons;
            Person[] Persons;

            persons:
                -
                    name: John
                    age: 18
                    sex: male
                -
                    name: Bob
                    age: 17
                    sex: male
                -
                    name: Patrick
                    age: 22
                    sex: male


            Node.Collection.(Node.Collection.Pair[])[]
            Dictionary<string, MetaPerson> Persons;

            persons:
                John:
                    age: 18
                    sex: male
                Bob:
                    age: 17
                    sex: male
                Patrick:
                    age: 22
                    sex: male


             */

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

                        Console.WriteLine($"\tType of List<{itemType.Name}>");
                        
                        node.Init(ref obj, buffer);

                        obj.GetType().GetMethod("Add").Invoke(obj, new[] { item });
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

                        
                        Console.Write($"Type of node: {node.GetType().Name}. ");

                        object parameters = new object[] { key, value };

                        node.Init(ref parameters, buffer);


                        object[] pair = (object[])parameters;

                        Console.WriteLine($"\tType of Dictionary<{keyType.Name}, {valueType.Name}> => ['{pair[0]}', '{pair[1]}'].");

                        obj.GetType().GetMethod("Add").Invoke(obj, pair);
                    }

                    return;
                }

                throw new Exception($"Not support generic type: '{genericType.Name}'.");
            }

            for (int i = 0; i < Nodes.Length; i++)
            {
                //Mark propMark = Nodes[i] is Node node ? node.Key : ((Pair)Nodes[i]).Key;

                Console.WriteLine($"Init object in collection: {obj.GetType().Name}");

                Nodes[i].Init(ref obj, buffer);

                Console.WriteLine();

                //property.SetValue(obj, value);
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