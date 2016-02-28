using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericMathematics
{
	/// <summary>
	/// Describes the general features of the number type.
	/// </summary>
	[Flags]
	public enum MathFeatures
	{
		/// <summary>
		/// Empty flag.
		/// </summary>
		None = 0,
		/// <summary>
		/// Type has MinValue and MaxValue.
		/// </summary>
		Bounded = 1,
		/// <summary>
		/// Type is unsigned (i.e. <see cref="UInt16"/>, <see cref="UInt32"/>, etc).
		/// </summary>
		Unsigned = 2,
		/// <summary>
		/// Type is signed and has more negative values than positive values.
		/// </summary>
		TwosComplement = 4,
		/// <summary>
		/// Type is fractional (<see cref="Single"/>, <see cref="Double"/>, <see cref="Decimal"/>, etc).
		/// </summary>
		Fractional = 8,
		/// <summary>
		/// Type is floating point number (<see cref="Single"/>, <see cref="Double"/>, etc).
		/// </summary>
		FloatingPoint = 16,
	}
}
