using System;
using System.Windows.Forms;

namespace Pvax.App.Views.TestBench
{
	/// <summary>
	/// Description of Entry.
	/// </summary>
	class Entry
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
#if NET20
			Application.SetCompatibleTextRenderingDefault(false);
#endif
			Application.DoEvents();
			Application.Run(new MainForm());
		}
	}
}
