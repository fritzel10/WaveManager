using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveUserInterface
{
	public partial class Help : Form
	{
		public Help()
		{
			InitializeComponent();
		}

		private void OnClickLink(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ProcessStartInfo s = new ProcessStartInfo("IExplore.exe");
			s.WindowStyle = ProcessWindowStyle.Maximized;
			s.Arguments = _linklbl1.Text;
			Process.Start(s);
		}

		private void OnClickClose(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
