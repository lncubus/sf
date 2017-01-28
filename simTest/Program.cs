using ValueType = System.Double;
using Complex = System.Numerics.Complex;

using System;

using Solvers;
using Vectors;

namespace simTest
{
	class MainClass
	{
		int counter = 0;

		void RunSolvers()
		{
			Console.WriteLine("Testing RK4...");
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			// integ_0^T cos t dt = sin T 
			//TestRungeKutta(100, 0.1, 0, 0, (t, X) => Math.Cos(t), t => Math.Sin(t));
			TestRungeKutta(1000, 0.1, 0, 1, (t, X) => Math.Cos(t), t => Math.Sin(t) + 1);
			TestRungeKutta(100000, 0.001, 0, 1, (t, X) => Math.Cos(t), t => Math.Sin(t) + 1);
				//t => Math.Sin(t));
			// x' = x, x(0) = 1 => x = e^t
			//TestRungeKutta(100, 0.1, 0, 1, (t, X) => -X, t => Math.Exp(-t));
			var X0 = 1;
			TestRungeKutta(1000, 0.1, 0, X0, (t, X) => X, t => X0*Math.Exp(t)); //t => Math.Sin(t));
			TestRungeKutta(100000, 0.001, 0, X0, (t, X) => X, t => X0*Math.Exp(t)); //t => Math.Sin(t));
			sw.Stop();
			Console.WriteLine("{0} steps total {1} ms ave {2} mus",
				counter, sw.ElapsedMilliseconds, sw.ElapsedMilliseconds*1000.0/counter);
			Console.WriteLine("Testing AB4...");
			counter = 0;
			sw.Restart();
			// integ_0^T cos t dt = sin T 
			TestAdamsBashforth(1000, 0.1, 0, 1, (t, X) => Math.Cos(t), t => Math.Sin(t) + 1);
			TestAdamsBashforth(100000, 0.001, 0, 1, (t, X) => Math.Cos(t), t => Math.Sin(t) + 1);
			// x' = x, x(0) = 1 => x = e^t
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
			var rk = new Solvers.RungeKuttaDouble
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
			var ab = new Solvers.AdamsBashforth
				(Function : F, step : dt, t0 : t0, X0 : X0);
			TestSolver(ab, n, E);
		}

		void TestSolver(Solvers.ISolver<ValueType> solver, int n, Func <double, ValueType> E)
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
					var error = X == X1 ? 0.0 : Math.Abs(X-X1)/(Math.Max(Math.Abs(X),Math.Abs(X1)));
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

		void RunVectors()
		{
			var empty = new Vectors.Vector();
			var empty2 = new Vector();
			var d1_0 = new Vectors.Vector(0);
			var d1_1 = new Vectors.Vector(1);
			var d3_0 = new Vectors.Vector(new double[] { 0, 0, 0 });
			var d3_x = new Vectors.Vector(new double[] { 1, 0, 0 });
			var d3_y = new Vectors.Vector(new double[] { 0, 1, 0 });
			var d3_z = new Vectors.Vector(new double[] { 0, 0, 1 });
			var d3_NaN = new Vectors.Vector(0, 0, Double.NaN);
			var d3_NaN_2 = new Vector(0, 0, Double.NaN);
			var all = new []
			{
				empty, d1_0, d1_1, d3_0, d3_x, d3_y, d3_z, d3_NaN
			};
			foreach (var v1 in all)
			{
				var s1 = v1.ToString("R", System.Globalization.CultureInfo.InvariantCulture);
				var v3 = Vector.TryParse(s1, System.Globalization.CultureInfo.InvariantCulture);
				if (ReferenceEquals(v3, null))
					Console.WriteLine("Can't roundtrip {0}, {1}", v1, s1);
				if (!v1.Equals(v3))
					Console.WriteLine("Roundtrip error {0} -> {1}", v1, v3);
				if (v1.HasNaNs() != v3.HasNaNs())
					Console.WriteLine("Different NaNs {0}, {1}", v1, v3);
				if (v1 != v3 && !v1.HasNaNs())
					Console.WriteLine("{0} != {1}", v1, v3);
				foreach (var v2 in all)
				{
					if (object.ReferenceEquals(v1, v2))
						continue;
					if (v1.GetHashCode() == v2.GetHashCode())
						Console.WriteLine("{0} and {1} have same hashes", v1, v2);
					if (v1.Equals(v2))
						Console.WriteLine("{0} and {1} are equal", v1, v2);
				}
			}
			if (!empty.Equals(empty2))
				Console.WriteLine("{0} and {1} are not equal", empty, empty2);
			if (!d3_NaN.Equals(d3_NaN_2))
				Console.WriteLine("{0} and {1} are not equal", d3_NaN, d3_NaN_2);
			foreach (var v in all)
			{
				Console.WriteLine("{0} ({1}) {2:X}", v, string.Join("; ", v), v.GetHashCode());
			}
		}

		static readonly int[] Ms = { 1, 2, 4, 8, 16, 32, 64, 128, 256 };

		void Run()
		{
			const int N = 1000000;
			Complex sumC = 0;
			Vector sumV = new Vector(0, 0);
			Random r = new Random(0x1E33DEAD);
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			for (int i = 0; i < N; i++)
			{
				double a = r.NextDouble() * 2 - 1;
				double b = r.NextDouble() * 2 - 1;
				Complex c = new Complex(a, b);
				sumC += c;
			}
			sw.Stop();
			Console.WriteLine("Complex plus {0} mus", sw.Elapsed.TotalSeconds);
			r = new Random(0x1E33DEAD);
			sw.Restart();
			for (int i = 0; i < N; i++)
			{
				double a = r.NextDouble() * 2 - 1;
				double b = r.NextDouble() * 2 - 1;
				Vector v = new Vector(a, b);
				sumV += v;
			}
			sw.Stop();
			Console.WriteLine("Vector plus {0} mus", sw.Elapsed.TotalSeconds);
			Console.WriteLine("{0} {1}", sumC, sumV);
			var diff = Math.Abs(sumC.Real - sumV[0]) + Math.Abs(sumC.Imaginary - sumV[1]);
			Console.WriteLine(diff);
			Quaternion sumQ = new Quaternion (0, 0, 0, 0);
			sumV = new Vector(0, 0, 0, 0);
			r = new Random(0x1E33DEAD);
			sw.Restart();
			for (int i = 0; i < N; i++)
			{
				double a = r.NextDouble() * 2 - 1;
				double b = r.NextDouble() * 2 - 1;
				double c = r.NextDouble() * 2 - 1;
				double d = r.NextDouble() * 2 - 1;
				Quaternion q = new Quaternion(a, b, c, d);
				sumQ += q;
			}
			sw.Stop();
			Console.WriteLine("Quaternion plus {0} mus", sw.Elapsed.TotalSeconds);
			r = new Random(0x1E33DEAD);
			sw.Restart();
			for (int i = 0; i < N; i++)
			{
				double a = r.NextDouble() * 2 - 1;
				double b = r.NextDouble() * 2 - 1;
				double c = r.NextDouble() * 2 - 1;
				double d = r.NextDouble() * 2 - 1;
				Vector v = new Vector(a, b, c, d);
				sumV += v;
			}
			sw.Stop();
			Console.WriteLine("Vector plus {0} mus", sw.Elapsed.TotalSeconds);
			Console.WriteLine("{0} {1}", sumQ, sumV);
			diff = Math.Abs(sumQ.X - sumV[0]) + Math.Abs(sumQ.Y - sumV[1]) +
				Math.Abs(sumQ.Z - sumV[2]) + Math.Abs(sumQ.W- sumV[3]);
			Console.WriteLine(diff);
			foreach (int M in Ms)
			{
				var sumX = new double[M];
				sumX.Initialize();
				sumV = new Vector(sumX);
				r = new Random(0x1E33DEAD);
				sw.Restart();
				for (int i = 0; i < N; i++)
				{
					for (int j = 0; j < sumX.Length; j++)
						sumX[j] += r.NextDouble() * 2 - 1;
				}
				sw.Stop();
				Console.WriteLine("Array {0} plus {1} mus", M, sw.Elapsed.TotalSeconds);
				r = new Random(0x1E33DEAD);
				var buffer = new double[M];
				buffer.Initialize();
				sw.Restart();
				for (int i = 0; i < N; i++)
				{
					for (int j = 0; j < buffer.Length; j++)
						buffer[j] = r.NextDouble() * 2 - 1;
					var v = new Vector(buffer);
					sumV += v;
				}
				sw.Stop();
				Console.WriteLine("Vector {0} plus {1} mus", M, sw.Elapsed.TotalSeconds);
				diff = 0;
				for (int j = 0; j < sumX.Length; j++)
					diff += Math.Abs(sumX[j] - sumV[j]);
				if (diff != 0)
					Console.WriteLine(diff);
			}
		}

		public static void Main (string[] args)
		{
			var app = new MainClass();
			app.Run();
			//app.RunSolvers();
			// System.Numerics.Quaternion q;

		}
	}
}
