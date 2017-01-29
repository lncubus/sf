using System;
using System.Collections.Generic;
using System.Linq;

namespace simTest
{
	public class Particles
	{
		public readonly double[] GM;

		private static void Ensure(ref double[] a, int n)
		{
			if (a == null || a.Length != n)
				a = new double[n];
		}

		private static Vectors.Vector3 GetR(double[] a, int i)
		{
			return new Vectors.Vector3(a[3 * i], a[3 * i + 1], a[3 * i + 2]);
		}

		private static Vectors.Vector3 GetR(Vectors.Vector a, int i)
		{
			return new Vectors.Vector3(a[3 * i], a[3 * i + 1], a[3 * i + 2]);
		}

		public Particles (int N)
		{
			GM = new double[N];
		}

		public void FA(double t, double[] sv, double[] va)
		{
			int n = GM.Length;
			int half = 3*n;
			// S' = V
			for (int i = 0; i < half; i++)
				va[i] = sv[half + i];
			// V' = A = ...
/*
    for i in range(0, n):
        ai = [0.0]*D3
        ri = x[D3*2*i:D3*(2*i+1)]
        for j in range(n-1, -1, -1):
            if i == j:
                continue
            rj = x[D3*2*j:D3*(2*j+1)]
            vect = rj - ri
            radius = numpy.linalg.norm(vect)
            vect /= radius
            power = GM[j]/(radius*radius)
#            print(i, j, power)
            ai += power*vect
        result[D3*(2*i+1):D3*2*(i+1)] = ai
*/
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
	}
}

