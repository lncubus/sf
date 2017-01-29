// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using StringBuilder = System.Text.StringBuilder;

using System;
using System.Globalization;
//using System.Text;

namespace Vectors
{
	/// <summary>
	/// A structure encapsulating three single precision floating point values and provides hardware accelerated methods.
	/// </summary>
	public partial struct Vector3 : IEquatable<Vector3>, IFormattable
	{
		#region Public Static Properties
		/// <summary>
		/// The vector (0,0,0).
		/// </summary>
		public static readonly Vector3 Zero = new Vector3(0,0,0);
		/// <summary>
		/// The vector (1,0,0).
		/// </summary>
		public static readonly Vector3 UnitX = new Vector3(1.0, 0.0, 0.0);
		/// <summary>
		/// The vector (0,1,0).
		/// </summary>
		public static readonly Vector3 UnitY = new Vector3(0.0, 1.0, 0.0);
		/// <summary>
		/// The vector (0,0,1).
		/// </summary>
		public static readonly Vector3 UnitZ = new Vector3(0.0, 0.0, 1.0);
		#endregion Public Static Properties

		#region Public Instance Methods

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			int hash = this.X.GetHashCode();
			hash = HashHelpers.Combine(hash, this.Y.GetHashCode());
			hash = HashHelpers.Combine(hash, this.Z.GetHashCode());
			return hash;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Vector3))
				return false;
			return Equals((Vector3)obj);
		}

		/// <summary>
		/// Returns a String representing this Vector3 instance.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString()
		{
			return ToString("G", CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector3 instance, using the specified format to format individual elements.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector3 instance, using the specified format to format individual elements 
		/// and the given IFormatProvider.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <param name="formatProvider">The format provider to use when formatting elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			StringBuilder sb = new StringBuilder();
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			sb.Append('(');
			sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(' ');
			sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(' ');
			sb.Append(((IFormattable)this.Z).ToString(format, formatProvider));
			sb.Append(')');
			return sb.ToString();
		}

		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		/// <returns>The vector's length.</returns>
		public double Length()
		{
			return Math.Sqrt(LengthSquared());
		}

		/// <summary>
		/// Returns the length of the vector squared. This operation is cheaper than Length().
		/// </summary>
		/// <returns>The vector's length squared.</returns>
		public double LengthSquared()
		{
			return X * X + Y * Y + Z * Z;
		}
		#endregion Public Instance Methods

		#region Public Static Methods
		/// <summary>
		/// Returns the Euclidean distance between the two given points.
		/// </summary>
		/// <param name="value1">The first point.</param>
		/// <param name="value2">The second point.</param>
		/// <returns>The distance.</returns>
		public static double Distance(Vector3 value1, Vector3 value2)
		{
			return Math.Sqrt(DistanceSquared(value1, value2));
		}

		/// <summary>
		/// Returns the Euclidean distance squared between the two given points.
		/// </summary>
		/// <param name="value1">The first point.</param>
		/// <param name="value2">The second point.</param>
		/// <returns>The distance squared.</returns>
		public static double DistanceSquared(Vector3 value1, Vector3 value2)
		{
			Vector3 difference = value1 - value2;
			return difference.LengthSquared();
		}

		/// <summary>
		/// Returns a vector with the same direction as the given vector, but with a length of 1.
		/// </summary>
		/// <param name="value">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector3 Normalize(Vector3 value)
		{
			double length = value.Length();
			return value / length;
		}

		/// <summary>
		/// Computes the cross product of two vectors.
		/// </summary>
		/// <param name="vector1">The first vector.</param>
		/// <param name="vector2">The second vector.</param>
		/// <returns>The cross product.</returns>
		public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
		{
			return new Vector3(
				vector1.Y * vector2.Z - vector1.Z * vector2.Y,
				vector1.Z * vector2.X - vector1.X * vector2.Z,
				vector1.X * vector2.Y - vector1.Y * vector2.X);
		}

		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified normal.
		/// </summary>
		/// <param name="vector">The source vector.</param>
		/// <param name="normal">The normal of the surface being reflected off.</param>
		/// <returns>The reflected vector.</returns>
		public static Vector3 Reflect(Vector3 vector, Vector3 normal)
		{
			double dot = vector*normal;
			Vector3 temp = normal * dot * 2f;
			return vector - temp;
		}

		/// <summary>
		/// Restricts a vector between a min and max value.
		/// </summary>
		/// <param name="value1">The source vector.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The restricted vector.</returns>
		public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
		{
			// This compare order is very important!!!
			// We must follow HLSL behavior in the case user specified min value is bigger than max value.

			double x = value1.X;
			x = (x > max.X) ? max.X : x;
			x = (x < min.X) ? min.X : x;

			double y = value1.Y;
			y = (y > max.Y) ? max.Y : y;
			y = (y < min.Y) ? min.Y : y;

			double z = value1.Z;
			z = (z > max.Z) ? max.Z : z;
			z = (z < min.Z) ? min.Z : z;

			return new Vector3(x, y, z);
		}

		/// <summary>
		/// Linearly interpolates between two vectors based on the given weighting.
		/// </summary>
		/// <param name="value1">The first source vector.</param>
		/// <param name="value2">The second source vector.</param>
		/// <param name="amount">Value between 0 and 1 indicating the weight of the second source vector.</param>
		/// <returns>The interpolated vector.</returns>
		public static Vector3 Lerp(Vector3 value1, Vector3 value2, double amount)
		{
			Vector3 firstInfluence = value1 * (1f - amount);
			Vector3 secondInfluence = value2 * amount;
			return firstInfluence + secondInfluence;
		}
		#endregion Public Static Methods
	}
}
