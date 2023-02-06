namespace YaNet
{
    public static class Instancer
    {
        public static string[] SimpleTypes =
        {
            "Int16", "UInt16",
            "Byte", "SByte",
            "Int32", "UInt32",
            "Int64", "UInt64",
            "Single", "Double", "Decimal",
            "Boolean",
            "Char", "String"
        };

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
                _ => ImplicitConvert(type, value)
                //_ => Activator.CreateInstance(type)
            };

        private static object ImplicitConvert(Type type, string value)
        {
            MethodInfo method = type.GetMethod("op_Implicit", new[] { typeof(string) });

            if (method == null)
            {
                //return Activator.CreateInstance(type);
                throw new Exception($"Type {type.FullName} not support implicit method from string type.");
            }

            object obj = Activator.CreateInstance(type);

            obj = method.Invoke(obj, new[] { value });

            return obj;
        }

        public static object Empty(Type type)
            => type.Name switch
            {
                "String" => string.Empty,
                _ => Activator.CreateInstance(type)
            };

        public static bool IsSimple(this Type type)
            => SimpleTypes.Contains(type.Name);
    }
}