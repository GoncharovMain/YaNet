namespace YaNet
{
    public static class Instancer
    {
        public static object ToConvert(Type type, string value)
            => type.Name switch
            {
                "Int16" => Convert.ToInt16(value),
                "UInt16" => Convert.ToUInt16(value),
                "Byte" => Convert.ToByte(value),
                "SByte" => Convert.ToSByte(value),
                "Int32" => Convert.ToInt32(value),
                "UInt32" => Convert.ToUInt32(value),
                "Int64" => Convert.ToInt64(value),
                "UInt64" => Convert.ToUInt64(value),
                "Single" => Convert.ToSingle(value),
                "Double" => Convert.ToDouble(value),
                "Decimal" => Convert.ToDecimal(value),
                "Boolean" => Convert.ToBoolean(value),
                "Char" => Convert.ToChar(value),
                "String" => value,
                _ => Activator.CreateInstance(type)
            };

        public static object Empty(Type type)
            => type.Name switch
            {
                "String" => String.Empty,
                "System.String" => String.Empty,
                _ => Activator.CreateInstance(type)
            };
    }
}