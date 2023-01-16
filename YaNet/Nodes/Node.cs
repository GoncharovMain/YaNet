using System.Reflection;
using System.Text;

namespace YaNet.Nodes
{
    public interface INode
    {
        void Init(object obj, StringBuilder buffer);
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

            //Console.WriteLine($"Key: {key} value: {value}");

            PropertyInfo property = obj.GetType().GetProperty(key);

            property.SetValue(obj, Conv.Converter(property.PropertyType, value));
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
    }

    public class Collection : INode
    {
        public INode[] Nodes { get; set; }
        public Collection(INode[] nodes)
        {
            Nodes = nodes;
        }
        public void Init(object obj, StringBuilder buffer)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Init(obj, buffer);
            }
        }
    }
    public class Node : INode
    {
        public Mark Key { get; set; }
        public Collection Collection { get; set; }

        public Node(Mark key, INode[] nodes)
        {
            Key = key;
            Collection = new Collection(nodes);
        }
        public void Init(object obj, StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            string key = marker.Buffer(Key);

            PropertyInfo property = obj.GetType().GetProperty(key);


            object inner = Activator.CreateInstance(property.PropertyType);

            property.SetValue(obj, inner);

            Collection.Init(inner, buffer);
        }
    }
    public class NodeReference : Node
    {
        public Mark Reference { get; set; }

        public NodeReference(Mark Key, Mark reference, INode[] nodes)
            : base(Key, nodes)
        {
            Reference = reference;
        }

        public new void Init(object obj, StringBuilder buffer)
        {
            throw new NotImplementedException();
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