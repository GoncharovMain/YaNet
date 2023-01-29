namespace YaNet.Nodes
{
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

            property.SetValue(obj, Conv.Converter(property.PropertyType, value));
        }

        public void Print(StringBuilder buffer)
        {
            Marker marker = new Marker(buffer);

            Console.WriteLine($"'{marker.Buffer(Key)}' : '{marker.Buffer(Value)}'");
        }
    }
}