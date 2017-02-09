using System;

namespace Solvers
{
	public class ForestRuthIntegrator
	{
		//private double[] X1, X2, X3, X4;
		//private double[] dXdt_1, dXdt_2, dXdt_3, dXdt_4;
		//private double[] dX1, dX2, dX3, dX4;

		private int N;
		private double[] _A;

		// call F(t, X, V) -> A
		public Action<double, double[], double[], double[]> F { get; set; }
		public double t { get; set; }
		public double dt { get; set; }
		public double[] X { get; set; }
		public double[] V { get; set; }
		public double[] A { get { return _A; } }

		public static readonly double[] C = {};
		public static readonly double[] D = {};

		private static void Ensure(ref double[] a, int n)
		{
			if (a == null || a.Length != n)
				a = new double[n];
		}

		private void StepC(double c)
		{
			double c_dt = c*dt;
			for (int i = 0; i < N; i++)
				X[i] += V[i]*c_dt;
		}

		private void StepD(double d)
		{
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

		}
	}
}

