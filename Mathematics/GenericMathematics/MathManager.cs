using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GenericMathematics
{
	public sealed class MathManager
	{
        static Lazy<MathManager> instance = new Lazy<MathManager>(() => new MathManager());

		public static MathManager Instance
		{
			get { return instance.Value; }
		}

		Dictionary<Type, Type> maths = new Dictionary<Type, Type>()
		{
			{ typeof(Byte),       typeof(ByteMath)       },
			{ typeof(SByte),      typeof(SByteMath)      },
			{ typeof(Int16),      typeof(Int16Math)      },
			{ typeof(UInt16),     typeof(UInt16Math)     },
			{ typeof(Int32),      typeof(Int32Math)      },
			{ typeof(UInt32),     typeof(UInt32Math)     },
			{ typeof(Int64),      typeof(Int64Math)      },
			{ typeof(UInt64),     typeof(UInt64)         },
			{ typeof(Single),     typeof(SingleMath)     },
			{ typeof(Double),     typeof(DoubleMath)     },
			{ typeof(Decimal),    typeof(DecimalMath)    },
			{ typeof(BigInteger), typeof(BigIntegerMath) },
            { typeof(Complex),    typeof(ComplexMath)    },
            { typeof(Vector2<>),  typeof(Vector2Math<>)  },
            { typeof(Vector3<>),  typeof(Vector3Math<>)  },
            { typeof(Fraction),   typeof(FractionMath)   },
		};

		private MathManager() { }

        public Math<TElement> GetMath<TElement>()
        {
            return (Math<TElement>)Activator.CreateInstance(GetMathType(typeof(TElement)));
        }

        public Type GetMathType(Type elementType)
		{
			if (elementType == null)
				throw new ArgumentNullException(nameof(elementType));

			Type registeredMathType = GetRegisteredMathType(elementType);
            if (registeredMathType != null)
                return registeredMathType;

            if (elementType.IsGenericType)
            {
                Type openElementType = elementType.GetGenericTypeDefinition();
                Type openMathType = GetRegisteredMathType(openElementType);
                if (openMathType == null)
                {
                    throw new NotSupportedException(
                        string.Format(TextResources.Math_ImplementationNotFound, elementType.FullName));
                }
                if (!elementType.IsConstructedGenericType)
                    return openMathType;
                return openMathType.MakeGenericType(elementType.GetGenericArguments());
            }

			throw new NotSupportedException(
					string.Format(TextResources.Math_ImplementationNotFound, elementType.FullName));
		}

        private Type GetRegisteredMathType(Type elementType)
        {
            lock (maths)
            {
                Type mathType;
                if (maths.TryGetValue(elementType, out mathType))
                    return mathType;
                else
                    return null;
            }
        }

        // Example:
        //     class FooMath<T, U> : Math<Foo<T, U>> { ... }
        //     class XFooMath<T, U> : FooMath<T, U> { ... }
        //
        //     GetSubclassOfRawGeneric(typeof(XFooMath<int, long>), typeof(Math<>)) -> Math<Foo<int, long>>
        private static Type GetSubclassOfRawGeneric(Type toCheck, Type rawGeneric)
        {
            while (toCheck != typeof(object))
            {
                var current = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (current == rawGeneric)
                    return toCheck;

                toCheck = toCheck.BaseType;
            }
            return null;
        }
    }
}
