using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveUserInterface
{
	public partial class SaveChanges : Form
	{
		public SaveChanges()
		{
			InitializeComponent();
		}

		private void OnClickYes(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void OnClickNo(object sender, EventArgs e)
		{
			this.Close();

		}

		private void OnClickCancel(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
