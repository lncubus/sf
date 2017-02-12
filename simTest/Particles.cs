using System;
using System.Collections.Generic;
using System.Linq;

namespace simTest
{
	public class Particles
	{
		public double[] GM;

		private static void Ensure(ref double[] a, int n)
		{
			if (a == null || a.Length != n)
				a = new double[n];
		}

		public static Vectors.Vector3 GetR(double[] a, int i)
		{
			return new Vectors.Vector3(a[3 * i], a[3 * i + 1], a[3 * i + 2]);
		}

		public static void SetR(double[] a, int i, Vectors.Vector3 v)
		{
			a[3 * i] = v.X;
			a[3 * i + 1] = v.Y;
			a[3 * i + 2] = v.Z;
		}

		public static Vectors.Vector3 GetR(Vectors.Vector a, int i)
		{
			return new Vectors.Vector3(a[3 * i], a[3 * i + 1], a[3 * i + 2]);
		}

		public void FA(double t, double[] sv, double[] va)
		{
			int n = GM.Length;
			int half = 3*n;
			// S' = V
			for (int i = 0; i < half; i++)
				va[i] = sv[half + i];
			// V' = A = ...
			for (int i = 0; i < n; i++)
			{
				var ai = new Vectors.Vector3();
				var ri = GetR(sv, i);
				for (int j = n - 1; j >= 0; j--)
				{
					if (i == j)
						continue;
					var rj = GetR(sv, j);
					var vect = rj - ri;
					var square_radius = vect.LengthSquared();
					vect /= Math.Sqrt(square_radius);
					var power = GM[j] / square_radius;
					ai += vect * power;
				}
				va[half + 3 * i] = ai.X; 
				va[half + 3 * i + 1] = ai.Y; 
				va[half + 3 * i + 2] = ai.Z; 
			}
		}

		public Vectors.Vector FV(double t, Vectors.Vector sv)
		{
			int n = GM.Length;
			int half = 3*n;
			// S' = V
			var v = sv.Skip(half);
			var a = new List<double>(half);
			for (int i = 0; i < n; i++)
			{
				var ai = new Vectors.Vector3();
				var ri = GetR(sv, i);
				for (int j = n - 1; j >= 0; j--)
				{
					if (i == j)
						continue;
					var rj = GetR(sv, j);
					var vect = rj - ri;
					var square_radius = vect.LengthSquared();
					vect /= Math.Sqrt(square_radius);
					var power = GM[j] / square_radius;
					ai += vect * power;
				}
				a.Add(ai.X);
				a.Add(ai.Y);
				a.Add(ai.Z);
			}
			return new Vectors.Vector(v.Concat(a));
		}

		// public Action<double, double[], double[], double[]> F
		// F(t, X, V) -> A
		public void FVX(double t, double[] X, double[] V, double[] A)
		{
			int n = GM.Length;
			for (int i = 0; i < n; i++)
			{
				var ai = new Vectors.Vector3();
				var ri = GetR(X, i);
				for (int j = n - 1; j >= 0; j--)
				{
					if (i == j)
						continue;
					var rj = GetR(X, j);
					var vect = rj - ri;
					var square_radius = vect.LengthSquared();
					vect /= Math.Sqrt(square_radius);
					var power = GM[j] / square_radius;
					ai += vect * power;
				}
				SetR(A, i, ai);
			}
		}
	}
}

