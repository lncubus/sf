using System;

namespace Solvers
{
	public class RungeKuttaArray : ISolver<double[]>
	{
		private double[] X1, X2, X3, X4;
		private double[] dXdt_1, dXdt_2, dXdt_3, dXdt_4;
		private double[] dX1, dX2, dX3, dX4;

		// call F(t, X) -> X'
		public Action<double, double[], double[]> F { get; set; }
		public double t { get; set; }
		public double dt { get; set; }
		public double[] X { get; set; }
		public double[] dXdt { get { return dXdt_1; } }

		private static void Ensure(ref double[] a, int n)
		{
			if (a == null || a.Length != n)
				a = new double[n];
		}

		public void Step()
		{
			int N = X.Length;
			Ensure(ref dXdt_1, N);
			Ensure(ref dXdt_2, N);
			Ensure(ref dXdt_3, N);
			Ensure(ref dXdt_4, N);
			Ensure(ref dX1, N);
			Ensure(ref dX2, N);
			Ensure(ref dX3, N);
			Ensure(ref dX4, N);
			Ensure(ref X1, N);
			Ensure(ref X2, N);
			Ensure(ref X3, N);
			Ensure(ref X4, N);

			F(t, X, dXdt_1);
			dXdt_1.DebugPrint("dXdt");
			for (int i = 0; i < N; i++)
			{
				double dxi = dt * dXdt_1[i];
				dX1[i] = dxi;
				X1[i] = X[i] + dxi/2;
			}
			//X1.DebugPrint("X1");
			dX1.DebugPrint("dX1");

			F(t + dt/2.0, X1, dXdt_2);
			//dXdt_2.DebugPrint("dXdt_2");
			for (int i = 0; i < N; i++)
			{
				double dxi = dt * dXdt_2[i];
				dX2[i] = dxi;
				X2[i] = X[i] + dxi/2;
			}
			//X2.DebugPrint("X2");
			dX2.DebugPrint("dX2");

			F(t + dt/2.0, X2, dXdt_3);
			//dXdt_3.DebugPrint("dXdt_3");
			for (int i = 0; i < N; i++)
			{
				double dxi = dt * dXdt_3[i];
				dX3[i] = dxi;
				X3[i] = X[i] + dxi/2;
			}
			//X3.DebugPrint("X3");
			dX3.DebugPrint("dX3");

			F(t + dt, X3, dXdt_4);
			//dXdt_4.DebugPrint("dXdt_4");
			for (int i = 0; i < N; i++)
			{
				double dxi = dt * dXdt_4[i];
				dX4[i] = dxi;
				X[i] += (dX1[i] + 2.0 * (dX2[i] + dX3[i]) + dX4[i])/6.0;
			}
			dX4.DebugPrint("dX4");
			X.DebugPrint("X");
			t += dt;
		}
	}
}


