using System;
using System.Collections.Generic;

namespace Solvers
{
	public interface ISolver<ValueType>
	{
		// Func<double, ValueType, ValueType> F { get; }
		double t { get; }
		double dt { get; }
		ValueType X { get; }
		ValueType dXdt { get; }

		void Step();
	}

	public static class SolverExtensions
	{
		public static IEnumerable<Tuple<double, ValueType>> Evaluate<ValueType>(this ISolver<ValueType> that, int n)
		{
			for (int i = 0; i < n; i++)
			{
				that.Step();
				yield return Tuple.Create(that.t,that.X);
			}
		}
	}
}

