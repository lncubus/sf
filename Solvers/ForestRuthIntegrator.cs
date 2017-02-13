using System;

namespace Solvers
{
	/// <summary>
	/// https://en.wikipedia.org/wiki/Symplectic_integrator#Examples
	/// </summary>
	public class ForestRuthIntegrator
	{
		/// <summary>
		/// Verlet method
		/// </summary>
		public static readonly double[] Verlet_C = {0, 1.0};
		public static readonly double[] Verlet_D = {0.5, 0.5};

		/// <summary>
		/// c_{1}&=1,&c_{2}&=-{\tfrac {2}{3}},&c_{3}&={\tfrac {2}{3}},
		/// </summary>
		public static readonly double[] Ruth_C = {1.0, -2.0/3.0, 2.0/3.0};
		/// <summary>
		/// d_{1}&=-{\tfrac {1}{24}},&d_{2}&={\tfrac {3}{4}},&d_{3}&={\tfrac {7}{24}}.
		/// </summary>
		public static readonly double[] Ruth_D = {-1.0/24.0, 3.0/4.0, 7.0/24.0};

		private int N;
		private double[] _A;

		// call F(t, X, V) -> A
		public Action<double, double[], double[], double[]> F { get; set; }
		public double t { get; set; }
		public double dt { get; set; }
		public double[] X { get; set; }
		public double[] V { get; set; }
		public double[] A { get { return _A; } }


		private readonly double[] C;
		private readonly double[] D;
		private readonly int n;

		public ForestRuthIntegrator(double[] c, double[] d)
		{
			C = c;
			D = d;
			if (C.Length != D.Length)
				throw new InvalidOperationException("C and D length are inconsistent");
			n = C.Length;
		}

		private static void Ensure(ref double[] a, int n)
		{
			if (a == null || a.Length != n)
				a = new double[n];
		}

		private void StepC(double c)
		{
			if (c == 0)
				return;
			double c_dt = c*dt;
			for (int i = 0; i < N; i++)
				X[i] += V[i]*c_dt;
		}

		private void StepD(double d)
		{
			if (d == 0)
				return;
			F(t, X, V, A);
			double d_dt = d*dt;
			for (int i = 0; i < N; i++)
				V[i] += A[i]*d_dt;
		}

		public void Step()
		{
			if (V.Length != X.Length)
				throw new InvalidOperationException("X and V length are inconsistent");
			N = X.Length;
			Ensure(ref _A, N);
			for (int i = 0; i < n; i++)
			{
				StepC(C[i]);
				StepD(D[i]);
			}
			t += dt;
		}

		public void Evaluate(int n)
		{
			for (int i = 0; i < n; i++)
				Step();
		}
	}
}

