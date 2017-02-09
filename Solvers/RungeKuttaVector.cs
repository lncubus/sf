using ValueType = Vectors.Vector;

using System;
using System.Collections.Generic;

namespace Solvers
{
	public class RungeKuttaVector : ISolver<ValueType>
	{
		// call X' = F(t, X)
		public Func<double, ValueType, ValueType> F { get; set; }
		public double t { get; set; }
		public double dt { get; set; }
		public ValueType X { get; set; }
		public ValueType dXdt { get; private set; }

		public void Step()
		{
			dXdt = F(t, X);
			dXdt.DebugPrint("dXdt");
			var dX1 = dt*dXdt;
			dX1.DebugPrint("dX1");
			var dX2 = dt*F(t + dt/2.0, X + dX1/2.0);
			dX2.DebugPrint("dX2");
			var dX3 = dt*F(t + dt/2.0, X + dX2/2.0);
			dX3.DebugPrint("dX3");
			var dX4 = dt*F(t + dt, X + dX3);
			dX4.DebugPrint("dX4");
			t += dt;
			X += (dX1 + 2.0 * (dX2 + dX3) + dX4)/6.0;
			X.DebugPrint("X");
		}
	}
}

