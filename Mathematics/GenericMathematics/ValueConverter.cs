using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GenericMathematics
{
    public static class ValueConverter
    {
        public static object Convert(object value, Type resultType)
        {
            object result;
            if (!TryConvert(value, resultType, out result))
            {
                throw new NotSupportedException(
                    string.Format(TextResources.ValueConverter_NotSupported,
                        value.GetType().FullName, resultType.FullName));
            }

            return result;
        }

        public static bool TryConvert(object value, Type resultType, out object result)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Type sourceType = value.GetType();
            
            try
            {
                TypeConverter c = GetConverter(resultType);
                if (c != null && c.CanConvertFrom(sourceType))
                {
                    result = c.ConvertFrom(value);
                    return true;
                }

                c = GetConverter(sourceType);
                if (c != null && c.CanConvertTo(resultType))
                {
                    result = c.ConvertTo(value, resultType);
                    return true;
                }

                // Convert.ChangeType uses IConvertible for type conversions;
                // throws InvalidCastException if type could not be converted.

                // Convert.ChangeType() doesn't handle nullable types; remove nullable if appropriate.
                if (resultType.IsGenericType &&
                    resultType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    resultType = Nullable.GetUnderlyingType(resultType);
                }

                result = System.Convert.ChangeType(value, resultType);
                return true;
            }
            catch (NotSupportedException)
            {
            }
            catch (InvalidCastException)
            {
            }

            result = null;
            return false;
        }

        private static TypeConverter GetConverter(Type type)
        {
#if !SILVERLIGHT
            return TypeDescriptor.GetConverter(type);
#else
			if (type.IsNullable())
				type = Nullable.GetUnderlyingType(type);
			var tca = type.GetCustomAttribute<TypeConverterAttribute>();
			if (tca == null)
				return null;
			if (string.IsNullOrEmpty(tca.ConverterTypeName))
				return null;
			var converterType = Type.GetType(tca.ConverterTypeName, false);
			if (converterType == null)
				return null;
			var ctor = converterType.GetConstructor(new Type[]{ typeof(Type) });
			if (ctor != null)
				return (TypeConverter)ctor.Invoke(new object[]{ type });
			return (TypeConverter)Activator.CreateInstance(converterType);
#endif
        }
    }
}
