namespace CoffeeManagement.Utilities
{
    public static class TypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            if (!type.IsGenericType) return type.Name;
            var str = string.Join(",", type.GetGenericArguments().Select((Func<Type, string>)(t => t.Name)).ToArray());
            return type.Name.Remove(type.Name.IndexOf('`')) + "<" + str + ">";
        }

        public static string GetGenericTypeName(this object @object) => @object.GetType().GetGenericTypeName();
        public static string GetTypeName(this Type type) => type == null ? string.Empty : type.Name.Split('`')[0];
        public static bool IsNumericType(this Type type)
        {
            if (type == null) return false;

            var nonNullableType = Nullable.GetUnderlyingType(type) ?? type;

            return nonNullableType == typeof(byte)
                   || nonNullableType == typeof(sbyte)
                   || nonNullableType == typeof(short)
                   || nonNullableType == typeof(ushort)
                   || nonNullableType == typeof(int)
                   || nonNullableType == typeof(uint)
                   || nonNullableType == typeof(long)
                   || nonNullableType == typeof(ulong)
                   || nonNullableType == typeof(float)
                   || nonNullableType == typeof(double)
                   || nonNullableType == typeof(decimal);
        }

        public static bool IsDateTimeType(this Type t) => t == typeof(DateTime) || t == typeof(DateTime?);
        public static bool IsGuidType(this Type t) => t == typeof(Guid) || t == typeof(Guid?);
        public static bool IsBoolType(this Type t) => t == typeof(bool) || t == typeof(bool?);
        public static bool IsEnumType(this Type t)
        {
            if (t.IsEnum)
                return true;

            var underlyingType = Nullable.GetUnderlyingType(t);
            return underlyingType != null && underlyingType.IsEnum;
        }
    }
}
