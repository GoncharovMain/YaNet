using System.Reflection;
using System.Text;

namespace YaNet.Nodes
{
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