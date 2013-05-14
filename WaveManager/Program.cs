using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaveUserInterface;

namespace WaveManager
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			SplashScreen f = new SplashScreen();
			f.Shown += new EventHandler((a, b) =>
			{
				System.Threading.Thread t = new System.Threading.Thread
				(
					() =>	{
								System.Threading.Thread.Sleep(1000);
								f.Invoke(new Action(() => { f.Close(); }));
							}
				);
				t.IsBackground = true;
				t.Start();
			});
			Application.Run(f);
			Application.Run(new FileViewControl());
		}
	}
}
