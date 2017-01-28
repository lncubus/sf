using System;
using System.Collections.Generic;

namespace Solvers
{
	public class RungeKuttaDouble : ISolver<double[]>
	{
		// call X' = F(t, X)
		public Func<double, double[], double[]> F { get; set; }
		public double t { get; set; }
		public double dt { get; set; }
		public double[] X { get; set; }
		public double[] dXdt { get; private set; }

		private double[] X1, X2, X3, X4;
		private double[] dX1, dX2, dX3, dX4;

		private void Ensure(ref double[] a, int n)
		{
			if (a == null || a.Length != n)
				a = new double[n];
		}

		public void Step()
		{
			int N = X.Length;
			Ensure(dX1, N);
			Ensure(dX2, N);
			Ensure(dX3, N);
			Ensure(dX4, N);
			Ensure(X1, N);
			Ensure(X2, N);
			Ensure(X3, N);
			Ensure(X4, N);

			dXdt = F(t, X);
			for (int i = 0; i < N; i++)
			{
				double dxi = dt * dXdt[i];
				dX1[i] = dxi;
				X1[i] = X[i] + dxi/2;
			}
			var dXdt_2 = F(t + dt/2.0, X1);
			for (int i = 0; i < N; i++)
			{
				double dxi = dt * dXdt_2[i];
				dX2[i] = dxi;
				X2[i] = X[i] + dxi/2;
			}
			var dXdt_3 = F(t + dt/2.0, X2);
			for (int i = 0; i < N; i++)
			{
				double dxi = dt * dXdt_2[i];
				dX2[i] = dxi;
				X2[i] = X[i] + dxi/2;
			}
			var dX3 = dt*F(t + dt/2.0, X + dX2/2.0);
			var dX4 = dt*F(t + dt, X + dX3);
			t += dt;
			X += (dX1 + 2.0 * (dX2 + dX3) + dX4)/6.0;
		}


	}
}


