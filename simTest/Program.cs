using ValueType = System.Double;
using Complex = System.Numerics.Complex;

using System;
using System.Linq;

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

		static readonly int[] Ms = { 1, 2, 4, 8, 16, 32, 64 };

		void RunPlus()
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

		void Energy(double[] GM, double[] X, out double Energy, out Vector3 Impulse)
		{
			double K = 0.0, U = 0.0;
			Vector3 impulse = new Vector3();
			int n = GM.Length;
			for (int i = 0; i < n; i++)
			{
				var mi = GM[i] / GM[0];
				var vi = Particles.GetR(X, i + n);
				var ri = Particles.GetR(X, i);
				impulse += mi*vi;
				K += mi * vi.LengthSquared();
				for (int j = 0; j < GM.Length; j++)
				{
					if (i == j)
						continue;
					// -GM m / R
					var rj = Particles.GetR(X, j);
					var rji = (rj - ri).Length();
					U -= mi * GM[j] / rji;
				}
			}
			K /= 2;
			//Console.WriteLine("Ek = {0} Eg = {1} E = {2}", K, U, K + U);
			Energy = K + U;
			Impulse = impulse;
		}

		void RunSolarSystem(int N, bool vectors = true)
		{
			var s = new Particles
			{
				GM = new []
				{
					0.295912208285591100E-03,
					0.491248045036476000E-10,
					0.724345233264412000E-09,
					0.888769244512563400E-09 + 0.109318945074237400E-10,
					0.954954869555077000E-10,
					0.282534584083387000E-06,
					0.845970607324503000E-07,
					0.129202482578296000E-07,
					0.152435734788511000E-07,
					0.217844105197418000E-11
				}
			};
			var X0 = new []
			{
				// R
				0.00450250878464055477,  0.00076707642709100705, 0.00026605791776697764, // Sun
				0.36176271656028195477, -0.09078197215676599295,-0.08571497256275117236, // Mercury
				0.61275194083507215477, -0.34836536903362219295,-0.19527828667594382236, // Venus
				0.12051741410138465477, -0.92583847476914859295,-0.40154022645315222236, // Earth+Moon
				-0.11018607714879824523, -1.32759945030298299295,-0.60588914048429142236, // Mars
				-5.37970676855393644523, -0.83048132656339789295,-0.22482887442656542236,
				7.89439068290953155477,  4.59647805517127300705, 1.55869584283189997764,
				-18.26540225387235944523, -1.16195541867586999295,-0.25010605772133802236,
				-16.05503578023336944523,-23.94219155985470899295,-9.40015796880239402236,
				-30.48331376718383944523, -0.87240555684104999295, 8.91157617249954997764,
				// V
				-0.00000035174953607552,  0.00000517762640983341, 0.00000222910217891203,
				0.00336749397200575848,  0.02489452055768343341, 0.01294630040970409203,
				0.01095206842352823448,  0.01561768426786768341, 0.00633110570297786403,
				0.01681126830978379448,  0.00174830923073434441, 0.00075820289738312913,
				0.01448165305704756448,  0.00024246307683646861,-0.00028152072792433877,
				0.00109201259423733748, -0.00651811661280738459,-0.00282078276229867897,
				-0.00321755651650091552,  0.00433581034174662541, 0.00192864631686015503,
				0.00022119039101561468, -0.00376247500810884459,-0.00165101502742994997,
				0.00264276984798005548, -0.00149831255054097759,-0.00067904196080291327,
				0.00032220737349778078, -0.00314357639364532859,-0.00107794975959731297
			};

			//const int N = 1000;
			const double years = 1;
			const double year = 365.2564;//365.25;365,2564

			var rka = new RungeKuttaArray
			{
				t = 0,
				dt = year/N,
				X = X0.ToArray(),
				F = s.FA
			};
			var rkv = new RungeKuttaVector
			{
				t = 0,
				dt = year / N,
				X = new Vector(X0),
				F = s.FV,
			};
			var fr = new ForestRuthIntegrator
			{
				t = 0,
				dt = year / N,
				F = s.FVX,
				X = X0.Take(X0.Length/2).ToArray(),
				V = X0.Skip(X0.Length/2).ToArray(),
			};
			Console.WriteLine(N);
			//Console.WriteLine("X0 = \n{0:R}", rkv.X);
//			for (int i = 0; i < 5 /*s.GM.Length*/; i++)
//				Console.WriteLine("{0:G} {1:G} {2:G}", X0[3 * i], X0[3 * i + 1], X0[3 * i + 2]);
			double E0, E1, En, Ev, Efr;
			Vector3 p0, p1, pn, pv, pfr;
			Energy(s.GM, rka.X, out E0, out p0);
			Energy(s.GM, fr.X.Concat(fr.V).ToArray(), out Efr, out pfr);

			//SolverExtensions.Debug = true;
			//Console.WriteLine("Array");
			rka.Step();
			//Console.WriteLine("Vector");
			rkv.Step();
			fr.Step();
//			Console.WriteLine("dt = {0} {4:F}s dX/dt max = {1}, min = {2}, len = {3}",
//				rkv.dt, rkv.dXdt.Max(Math.Abs), rkv.dXdt.Min(Math.Abs), Math.Sqrt(rkv.dXdt.Sum(z => z * z)),
//				rkv.dt*86400);
			//Console.WriteLine("End");
			//SolverExtensions.Debug = false;

			Vector3 Earth1 = new Vector3(rka.X[9], rka.X[10], rka.X[11]);

			Energy(s.GM, rka.X, out E1, out p1);
			Energy(s.GM, rkv.X.ToArray(), out Ev, out pv);
			Energy(s.GM, fr.X.Concat(fr.V).ToArray(), out Efr, out pfr);

			Console.WriteLine("E0 = {0} E1a = {1} E1v = {2} E1fr = {3}", E0, E1, Ev, Efr);
			//if (p1 != pv)
			//	Console.WriteLine("p1 = {0}, p1v = {1}, diff = {2}", p1, pv, p1 - pv);
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			var last = rka.Evaluate(N).Last();
			sw.Stop();
			Console.WriteLine("array {0} steps total {1} ms ave {2} mus",
				N, sw.ElapsedMilliseconds, sw.ElapsedMilliseconds*1000.0/N);
			var X = last.Item2;
			Energy(s.GM, X, out En, out pn);
			Console.WriteLine("dE1/E = {0}", (E1 - E0) / E0);
			Console.WriteLine("dEn/E = {0}", (En - E0) / E0);
			Vector3 EarthN = new Vector3(X[9], X[10], X[11]);
			Console.WriteLine("RK4 Earth drift: {0} - {1} ->\n {2}", Earth1, EarthN, (Earth1 - EarthN).Length());  

			Earth1 = new Vector3(fr.X[9], fr.X[10], fr.X[11]);
			sw.Start();
			fr.Evaluate(N);
			sw.Stop();
			EarthN = new Vector3(fr.X[9], fr.X[10], fr.X[11]);
			Console.WriteLine("Verlet {0} steps total {1} ms ave {2} mus",
				N, sw.ElapsedMilliseconds, sw.ElapsedMilliseconds*1000.0/N);
			Energy(s.GM, fr.X.Concat(fr.V).ToArray(), out Efr, out pfr);
			Console.WriteLine("Verlet dEn/E = {0}", (Efr - E0) / E0);
			Console.WriteLine("Verlet Earth drift: {0} - {1} ->\n {2}", Earth1, EarthN, (Earth1 - EarthN).Length());  
			if (!vectors)
				return;
			sw.Restart();
			var vlast = rkv.Evaluate(N).Last();
			sw.Stop();
			Console.WriteLine("vector {0} steps total {1} ms ave {2} mus",
				N, sw.ElapsedMilliseconds, sw.ElapsedMilliseconds*1000.0/N);
			var vX = vlast.Item2;
			Energy(s.GM, vX.ToArray(), out Ev, out pv);
			if (En != Ev)
				Console.WriteLine("En = {0}, Env = {1}, diff = {2}", En, Ev, En - Ev);
			if (pn != pv)
				Console.WriteLine("pn = {0}, pnv = {1}, diff = {2}", pn, pv, pn - pv);
		}

		public static void Main (string[] args)
		{
			var app = new MainClass();
			//app.RunPlus();
			app.RunSolarSystem(10);
			app.RunSolarSystem(25);
			app.RunSolarSystem(50);
			app.RunSolarSystem(100);
			app.RunSolarSystem(200);
//			app.RunSolarSystem(250);
			app.RunSolarSystem(300);
//			app.RunSolarSystem(325);
//			app.RunSolarSystem(350);
//			app.RunSolarSystem(375);
//			app.RunSolarSystem(400);
			app.RunSolarSystem(500);
			app.RunSolarSystem(1000, false);
			app.RunSolarSystem(5000, false);
//			app.RunSolarSystem(10000, false);
//			app.RunSolarSystem(30000, false);
//			app.RunSolarSystem(100000, false);
//			app.RunSolarSystem(300000, false);
//			app.RunSolarSystem(1000000, false);
//			app.RunSolarSystem(3000000, false);
			//app.RunSolvers();
			// System.Numerics.Quaternion q;

		}
	}
}
