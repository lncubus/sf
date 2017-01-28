/*
 * Portions copyright (c) Microsoft
 * Licensed to the .NET Foundation under one or more agreements.
 * The .NET Foundation licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
*/

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace Vectors
{
    public class Vector : IReadOnlyList<double>, IEquatable<Vector>, IFormattable
    {
		public static readonly Vector Empty = new Vector();

        protected readonly double[] Z;

        public Vector(IEnumerable<Double> x)
        {
            Z = x.ToArray();
        }

        public Vector(params double[] x)
        {
            Z = x.ToArray();
        }

		/// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            int hash = Z.Length;
            foreach (var z in Z)
                hash = HashHelpers.Combine(hash, z.GetHashCode());
            return hash;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector; False otherwise.</returns>
        public override bool Equals(object obj)
        {
			if (object.ReferenceEquals(obj, null))
				return false;
			if (object.ReferenceEquals(this, obj))
				return true;
            if (!(obj is Vector))
                return false;
            return Equals((Vector)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector is equal to this Vector instance.
        /// </summary>
        /// <param name="other">The Vector to compare this instance to.</param>
        /// <returns>True if the other Vector is equal to this instance; False otherwise.</returns>
        public bool Equals(Vector other)
        {
			if (object.ReferenceEquals(other, null))
				return false;
			if (object.ReferenceEquals(this, other))
				return true;
			if (other.Z.Length != Z.Length)
                return false;
			return Z.SequenceEqual(other.Z);
        }

		/// <summary>
		/// Returns a boolean indicating whether the given Vector has any NaN values.
		/// </summary>
		/// <returns>True if this Vector has any NaN value; False otherwise.</returns>
		public bool HasNaNs()
		{
			return Z.Any(double.IsNaN); 
		}

        /// <summary>
        /// Returns a String representing this Vector instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Vector instance, using the specified format to format individual elements.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Vector instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			separator += " ";
            string content = "(" + string.Join(separator, Z.Select(z => z.ToString(format, formatProvider))) + ")";
            return content;
        }

		/// <summary>
		/// Gets the Vector dimension.
		/// </summary>
		/// <value>The Vector dimension.</value>
		public int Dimension
		{
			get { return Z.Length; }
		}
			
		/// <summary>
		/// Gets the value at the specified index.
		/// </summary>
		/// <param name="index">The value at the specified index.</param>
		public double this[int index]
		{
			get { return Z[index]; }
		}

		/// <summary>
		/// Converts the string representation of a vector in a specified culture-specific format to
		/// its double-precision floating-point number equivalent.
		/// Returns null if parsing is failed.
		/// </summary>
		/// <returns>Parsed vector or null if parsing is failed.</returns>
		/// <param name="input">A string containing a numbers to convert.</param>
		/// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information
		/// about <paramref name="input"/>.</param>
		public static Vector TryParse(string input, IFormatProvider formatProvider)
		{
			// clear whitespaces
			input = string.Join("", input.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
			if (input.StartsWith("(") && input.EndsWith(")"))
				input = input.Substring(1, input.Length - 2);
			if (string.IsNullOrEmpty(input))
				return Empty;
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			var parts = input.Split(new[] { separator }, StringSplitOptions.None);
			var values = new List<double>(parts.Length);
			foreach (var part in parts)
			{
				double p;
				if (!Double.TryParse(part, NumberStyles.Float, formatProvider, out p))
					return null;
				values.Add(p);
			}
			return new Vector(values);
		}

        #region Public static operators
        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The summed vector.</returns>
        public static Vector operator +(Vector left, Vector right)
        {
			return new Vector(left.Z.Zip(right.Z, (l, r) => l + r));
        }

        /// <summary>
        /// Subtracts the second vector from the first.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The difference vector.</returns>
        public static Vector operator -(Vector left, Vector right)
        {
			return new Vector(left.Z.Zip(right.Z, (l, r) => l - r));
        }

        /// <summary>
        /// Multiplies a vector by the given scalar.
        /// </summary>
        /// <param name="left">The source vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector operator *(Vector left, double right)
        {
			return new Vector(left.Z.Select(l => l * right));
        }

        /// <summary>
        /// Multiplies a vector by the given scalar.
        /// </summary>
        /// <param name="left">The scalar value.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector operator *(double left, Vector right)
        {
			return new Vector(right.Z.Select(r => left * r));
        }

		/// <summary>
        /// Divides the vector by the given scalar.
        /// </summary>
        /// <param name="value1">The source vector.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        public static Vector operator /(Vector left, double right)
        {
			return (1.0/right)*left;
        }

        /// <summary>
        /// Negates a given vector.
        /// </summary>
        /// <param name="value">The source vector.</param>
        /// <returns>The negated vector.</returns>
        public static Vector operator -(Vector value)
        {
			return new Vector(value.Z.Select(v => -v));
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        public static bool operator ==(Vector left, Vector right)
        {
			if (left.Z.Length != right.Z.Length)
				return false;
			return left.Z.Zip(right.Z, (l, r) => l == r).All(eq => eq);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        public static bool operator !=(Vector left, Vector right)
        {
            return !(left == right);
        }
        #endregion Public static operators

		#region IReadOnlyList<double>
		public int Count
		{
			get { return Z.Length; }
		}
		public IEnumerator<double> GetEnumerator()
		{
			foreach (var z in Z)
				yield return z;
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Z.GetEnumerator();
		}
		#endregion
    }
}

