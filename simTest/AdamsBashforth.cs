using ValueType = System.Numerics.Complex;

using System;
using System.Collections.Generic;

namespace simTest
{
	public class AdamsBashforth : ISolver<ValueType>
	{
		// call X' = F(t, X)
		public Func<double, ValueType, ValueType> F { get; private set; }
		public double t { get; private set; }
		public double dt { get; private set; }
		public ValueType X { get; private set; }
		public ValueType dXdt { get { return pF[0]; } }
		private readonly ValueType[] pF;
		private static readonly double[] k =
		{
			1901d/720d,  // * F(t_n, X_n)
			-1387d/360d, // * F(t_n-1, X_n-1)
			109d/30d,    // * F(t_n-2, X_n-2)
			-637d/360d,  // * F(t_n-3, X_n-3)
			251d/720d,   // * F(t_n-4, X_n-4)
		};

		public AdamsBashforth(Func<double, ValueType, ValueType> Function, double step, double t0, ValueType X0)
		{
			F = Function;
			dt = step;
			t = t0;
			X = X0;
			pF = new ValueType[4];
			Start();
		}

		/// <summary>
		/// Running <see cref="RungeKutta"/> to get previous F(t,X) values 
		/// </summary>
		private void Start()
		{
			RungeKutta rk = new RungeKutta
			{
				F = this.F,
				t = this.t,
				X = this.X,
				dt = -dt
			};
			rk.Step();
			rk.Step();
			// F(t_-1, X_-1)
			pF[0] = rk.dXdt;
			rk.Step();
			// F(t_-2, X_-2)
			pF[1] = rk.dXdt;
			rk.Step();
			// F(t_-3, X_-3)
			pF[2] = rk.dXdt;
			rk.Step();
			// F(t_-4, X_-4)
			pF[3] = rk.dXdt;
		}

		/// <summary>
		/// X_n+1 = X_n + dt*(
		///   1901/720 * F(t_n, X_n)
		///  -1387/360 * F(t_n-1, X_n-1)
		///  + 109/30  * F(t_n-2, X_n-2)
		///  - 637/360 * F(t_n-3, X_n-3)
		///  + 251/720 * F(t_n-4, X_n-4)
		/// )
		/// </summary>
		public void Step()
		{
			var Fn = F(t, X);
			var sum = k[0] * Fn + k[1] * pF[0] + k[2] * pF[1] + k[3] * pF[2] + k[4] * pF[3];
			t += dt;
			X += dt * sum;
			pF[3] = pF[2]; pF[2] = pF[1]; pF[1] = pF[0]; pF[0] = Fn;
		}
	}
}

