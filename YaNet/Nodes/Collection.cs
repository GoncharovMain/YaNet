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

            // List<int>
            // List<string>
            // List<Person>


            // Dictionary<string, string> Pair[]
            // Dictionary<string, Person> (Node.Collection)[]

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

                        Console.WriteLine($"keyType: {key.GetType()} valueType: {value.GetType()}");

                        object parameters = new object[] { key, value };

                        node.Init(ref parameters, buffer);

                        object[] pair = (object[])parameters;

                        obj.GetType().GetMethod("Add").Invoke(obj, pair);
                    }

                    return;
                }

                throw new Exception("Not support generic type: '{genericType.Name}'.");
            }

            for (int i = 0; i < Nodes.Length; i++)
            {
                Mark propMark = Nodes[i] is Node node ? node.Key : ((Pair)Nodes[i]).Key;


                string prop = marker.Buffer(propMark);

                Console.WriteLine($"node prop name: {prop} type: {type.Name}");

                PropertyInfo property = type.GetProperty(prop);

                Console.WriteLine($"type prop: {property?.PropertyType?.Name}");

                object value = Instancer.Empty(property.PropertyType);

                Nodes[i].Init(ref value, buffer);

                property.SetValue(obj, value);
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