using ValueType = System.Numerics.Complex;
using System;

namespace simTest
{
	class MainClass
	{
		int counter = 0;

		void Run()
		{
			Console.WriteLine("Testing RK4...");
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			// integ_0^T cos t dt = sin T 
			//TestRungeKutta(100, 0.1, 0, 0, (t, X) => Math.Cos(t), t => Math.Sin(t));
			TestRungeKutta(1000, 0.1, 0, ValueType.ImaginaryOne, (t, X) => ValueType.Cos(t), t => ValueType.Sin(t) + ValueType.ImaginaryOne);
			TestRungeKutta(100000, 0.001, 0, ValueType.ImaginaryOne, (t, X) => ValueType.Cos(t), t => ValueType.Sin(t) + ValueType.ImaginaryOne);
				//t => Math.Sin(t));
			// x' = -x, x(0) = 1 => x = e^-t
			//TestRungeKutta(100, 0.1, 0, 1, (t, X) => -X, t => Math.Exp(-t));
			var X0 = ValueType.One + ValueType.ImaginaryOne;
			TestRungeKutta(1000, 0.1, 0, X0, (t, X) => X, t => X0*Math.Exp(t)); //t => Math.Sin(t));
			TestRungeKutta(100000, 0.001, 0, X0, (t, X) => X, t => X0*Math.Exp(t)); //t => Math.Sin(t));
			sw.Stop();
			Console.WriteLine("{0} steps total {1} ms ave {2} mus",
				counter, sw.ElapsedMilliseconds, sw.ElapsedMilliseconds*1000.0/counter);
			Console.WriteLine("Testing AB4...");
			counter = 0;
			sw.Restart();
			// integ_0^T cos t dt = sin T 
			TestAdamsBashforth(1000, 0.1, 0, ValueType.ImaginaryOne, (t, X) => ValueType.Cos(t), t => ValueType.Sin(t) + ValueType.ImaginaryOne);
			TestAdamsBashforth(100000, 0.001, 0, ValueType.ImaginaryOne, (t, X) => ValueType.Cos(t), t => ValueType.Sin(t) + ValueType.ImaginaryOne);
			// x' = -x, x(0) = 1 => x = e^-t
			//TestAdamsBashforth(100, 0.1, 0, 1, (t, X) => -X, t => Math.Exp(-t));
			TestAdamsBashforth(1000, 0.1, 0, X0, (t, X) => X, t => X0*Math.Exp(t));
			TestAdamsBashforth(100000, 0.001, 0, X0, (t, X) => X, t => X0*Math.Exp(t)); //t => Math.Sin(t));
			sw.Stop();
			Console.WriteLine("{0} steps total {1} ms ave {2} mus",
				counter, sw.ElapsedMilliseconds, sw.ElapsedMilliseconds*1000d/counter);
		}

		void TestRungeKutta(int n, double dt, double t0, ValueType X0,
			Func<double, ValueType, ValueType> F, Func <double, ValueType> E)
		{
			RungeKutta rk = new RungeKutta
			{
				F = F,
				t = t0,
				dt = dt,
				X = X0,
			};
			TestSolver(rk, n, E);
		}
	
		void TestAdamsBashforth(int n, double dt, double t0, ValueType X0,
			Func<double, ValueType, ValueType> F, Func <double, ValueType> E)
		{
			AdamsBashforth ab = new AdamsBashforth
				(Function : F, step : dt, t0 : t0, X0 : X0);
			TestSolver(ab, n, E);
		}

		void TestSolver(ISolver<ValueType> solver, int n, Func <double, ValueType> E)
		{
			var tXs = solver.Evaluate(n);
			var k = n / 5;
			var i = 0;
			var maxerror = 0d;
			foreach (var tX in tXs)
			{
				bool report = (k < 2 || i++ % k == 0 || i == n);
				var t = tX.Item1;
				var X = tX.Item2;
				if (report)
					Console.Write("{0:0.###}\t{1}", t, X);
				if (E != null)
				{
					var X1 = E(t);
					var error = X == X1 ? 0.0 : (X-X1).Magnitude/(X.Magnitude+X1.Magnitude);
					if (error > maxerror)
						maxerror = error;
					if (report)
						Console.Write("\t{0}\t{1}", X1, error);
				}
				if (report)
					Console.WriteLine();
			}
			counter += n;
			var koef = maxerror / Math.Pow(solver.dt, 4);
			Console.WriteLine("max. err = {0}, koef = {1}", maxerror, koef);
		}

		public static void Main (string[] args)
		{
			new MainClass().Run();
        }
	}
}
