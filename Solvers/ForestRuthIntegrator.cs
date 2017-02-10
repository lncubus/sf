using System;

namespace Solvers
{
	/// <summary>
	/// https://en.wikipedia.org/wiki/Symplectic_integrator#Examples
	/// </summary>
	public class ForestRuthIntegrator
	{
		private int N;
		private double[] _A;

		// call F(t, X, V) -> A
		public Action<double, double[], double[], double[]> F { get; set; }
		public double t { get; set; }
		public double dt { get; set; }
		public double[] X { get; set; }
		public double[] V { get; set; }
		public double[] A { get { return _A; } }

		/// <summary>
		/// Verlet method
		/// </summary>
		private readonly double[] C = {0, 1};
		private readonly double[] D = {0.5, 0.5};
		private readonly int n;

		public ForestRuthIntegrator()
		{
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
	}
}

