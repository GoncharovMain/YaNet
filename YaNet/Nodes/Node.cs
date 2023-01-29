using System.Reflection;
using System.Text;

namespace YaNet.Nodes
{
    public interface INode
    {
        void Init(object obj, StringBuilder buffer);
        void Print(StringBuilder buffer);
    }


    public class Scalar : INode
    {
        public Mark Value { get; set; }

        public Scalar(Mark value) => Value = value;

        public static implicit operator Mark(Scalar scalar)
            => new Mark(scalar.Value);

        public static implicit operator String(Scalar scalar)
            => scalar.ToString();

        public void Init(object obj, StringBuilder buffer)
        {
            throw new NotImplementedException();
        }

        public void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"- '{marker.Buffer(Value)}'");
        }
    }

    public class Pair : INode
    {
        public Mark Key { get; set; } // replace type Mark on Scalar
        public Mark Value { get; set; }

        public Pair(Mark key, Mark value)
        {
            Key = key;
            Value = value;
        }

        public void Init(object obj, StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            string key = marker.Buffer(Key);
            string value = marker.Buffer(Value);


            PropertyInfo property = obj.GetType().GetProperty(key);

            Console.Write($"Key: {key} ");
            Console.Write($"value: {value} ");
            Console.Write($"type: {obj.GetType().Name} ");
            Console.WriteLine($"propType: {property.PropertyType}");


            //Console.WriteLine($"Key: {key} value: {value} type: {obj.GetType().Name} propType: {property.PropertyType}");

            property.SetValue(obj, Conv.Converter(property.PropertyType, value));
        }

        public void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"'{marker.Buffer(Key)}' : '{marker.Buffer(Value)}'");
        }
    }

    public class Item : INode
    {
        public INode Node { get; set; }
        public Item(INode node)
        {
            Node = node;
        }

        public void Init(object obj, StringBuilder buffer)
        {
            throw new NotImplementedException();
        }

        public void Print(StringBuilder buffer)
        {
            Node.Print(buffer);
        }
    }

    public class Collection : INode
    {
        public INode[] Nodes { get; set; }

        public Collection(params INode[] nodes)
        {
            Nodes = nodes;
        }
        public void Init(object obj, StringBuilder buffer)
        {
            Console.WriteLine($"objType: {obj.GetType().Name} count nodes: {Nodes.Length}");

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
    public class Node : INode
    {
        public Mark Key { get; set; }
        public INode Nodes { get; set; }

        public Node(Mark key, INode nodes)
        {
            Key = key;
            Nodes = nodes;
        }
        public void Init(object obj, StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            string key = marker.Buffer(Key);

            PropertyInfo property = obj.GetType().GetProperty(key);


            object inner = Activator.CreateInstance(property.PropertyType);

            property.SetValue(obj, inner);

            Nodes.Init(inner, buffer);
        }

        public virtual void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"'{marker.Buffer(Key)}':");

            Nodes.Print(buffer);
        }
    }
    public class NodeReference : Node
    {
        public Mark Reference { get; set; }

        public NodeReference(Mark Key, Mark reference, INode nodes)
            : base(Key, nodes)
        {
            Reference = reference;
        }

        public new void Init(object obj, StringBuilder buffer)
        {
            throw new NotImplementedException();
        }

        public static explicit operator NodeReference(Pair pair)
            => new NodeReference(pair.Key, pair.Value, null);

        public override void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"'{marker.Buffer(Key)}' : '{marker.Buffer(Reference)}'");

            Nodes.Print(buffer);
        }
    }

    public static class Conv
    {
        public static object Converter(Type type, string value)
            => type.Name switch
            {
                "Int16" => (object)Convert.ToInt16(value),
                "UInt16" => (object)Convert.ToUInt16(value),
                "Byte" => (object)Convert.ToByte(value),
                "SByte" => (object)Convert.ToSByte(value),
                "Int32" => (object)Convert.ToInt32(value),
                "UInt32" => (object)Convert.ToUInt32(value),
                "Int64" => (object)Convert.ToInt64(value),
                "UInt64" => (object)Convert.ToUInt64(value),
                "Single" => (object)Convert.ToSingle(value),
                "Double" => (object)Convert.ToDouble(value),
                "Decimal" => (object)Convert.ToDecimal(value),
                "Boolean" => (object)Convert.ToBoolean(value),
                "Char" => (object)Convert.ToChar(value),
                "String" => (object)value,
                _ => Activator.CreateInstance(type)
            };
    }
}