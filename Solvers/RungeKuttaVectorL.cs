using ValueType = Vectors.Vector;

using System;
using System.Collections.Generic;

namespace Solvers
{
	public class RungeKuttaVectorL : ISolver<ValueType>
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
			var dX1 = dt*dXdt;
			var dX2 = dt*F(t + dt/2.0, ValueType.L(0.5, dX1, X));
			var dX3 = dt*F(t + dt/2.0, ValueType.L(0.5, dX2, X));
			var dX4 = dt*F(t + dt, X + dX3);
			t += dt;
			X += ValueType.L(2.0 / 6.0, dX2 + dX3, 1.0 / 6.0, dX1 + dX4); 
		}
	}
}

