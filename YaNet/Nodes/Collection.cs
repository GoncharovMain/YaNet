namespace YaNet.Nodes
{
    public class Collection : INode
    {
        public INode[] Nodes { get; set; }

        public Collection(params INode[] nodes)
        {
            Nodes = nodes;
        }
        public void Init(object obj, StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            if (Nodes[0] is Scalar)
            {
                for (int i = 0; i < Nodes.Length; i++)
                {
                    Scalar scalar = Nodes[i] as Scalar;

                    string value = marker.Buffer(scalar.Value);

                    obj.GetType().GetGenericTypeDefinition();
                }
            }


            
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


            Type type = obj.GetType();

            Console.WriteLine($"init obj type: {obj.GetType().Name}");

            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();

                // as List<>
                if (genericType == typeof(List<>))
                {
                    
                    
                    foreach (INode node in Nodes)
                    {

                        Scalar scalar = node as Scalar;                    
    
                        string value = marker.Buffer(scalar.Value);

                        Type typeItem = type.GetGenericArguments().Single();

                        object item = Conv.Converter(typeItem, value);

                        obj.GetType().GetMethod("Add").Invoke(obj, new[] { item });
                    }
                    
                    Console.WriteLine($"this is List");
                    
                    return;
                }

                // as Dictionary<,>
                if (genericType == typeof(Dictionary<,>))
                {
                    foreach (INode node in Nodes)
                    {
                        Pair pair = node as Pair;

                        Type[] genericArguments = type.GetGenericArguments();

                        Type keyType = genericArguments[0];
                        Type valueType = genericArguments[1];

                        object key = Conv.Converter(keyType, marker.Buffer(pair.Key));

                        object value = Conv.Converter(valueType, marker.Buffer(pair.Value));

                        obj.GetType().GetMethod("Add").Invoke(obj, new[] { key, value });
                    }

                    Console.WriteLine($"this is dictionary");

                    return;
                }
            }


            if (type.IsArray)
            {
                // as one dimensional array
                
                type = obj.GetType().GetElementType();

                object array = Array.CreateInstance(type, Nodes.Length);

                return;
                //return;
            }

            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Init(obj, buffer);
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