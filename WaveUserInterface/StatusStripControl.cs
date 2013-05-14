using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using WaveBusiness;
using System.Runtime.InteropServices;

namespace WaveUserInterface
{
	public partial class StatusStripControl : UserControl
	{
		public StatusStripControl()
		{
			InitializeComponent();
		}

		ToolTip t = new ToolTip();

		private void OnStatusStripLoad(object sender, EventArgs e)
		{
			UIContextManager.MemoryUseUpdate += UpdateProgressBar;
			UIContextManager.WaveSampleCountUpdate += UpdateWaveSampleCount;
			UIContextManager.AddError += UpdateErrorCbx;
			UIContextManager.ChangeVolume += ChangeVolume;
			t.SetToolTip(_pgbGreenBar, "Memory usage in MB: " + _pgbGreenBar.Value);
		}

		private void ChangeVolume(ushort Volume)
		{
			this.trackBar1.Value = Volume / (ushort.MaxValue / 10);
		}
		private void UpdateErrorCbx()
		{
			_cbxActivityLog.Items.Clear();
			foreach (string Error in WaveManager.ErrorLog)
			{
				ListViewItem anItem = new ListViewItem();
				anItem.Text = Error;
				_cbxActivityLog.Items.Add(anItem);
			}
		}

		private void UpdateWaveSampleCount(int Wave, int Sample)
		{
			_lblWaves.Text = "Waves: " + WaveManager.WaveList.aWaveList.Count;
			_lblSamples.Text = "Samples: " + Sample;
		}
		private void UpdateProgressBar(double MemoryUse)
		{
			_pgbGreenBar.Value = (int)MemoryUse;
			t.SetToolTip(_pgbGreenBar, "Memory usage in MB: " + _pgbGreenBar.Value);
		}

		private void OnVolumeScroll(object sender, EventArgs e)
		{
			int newvol = ((ushort.MaxValue / 10) * trackBar1.Value);
			uint NewVolume = (((uint)newvol & 0x0000ffff) | ((uint)newvol << 16));
			FileViewControl.waveOutSetVolume(IntPtr.Zero, NewVolume);
		}

		private void OnMouseHover(object sender, EventArgs e)
		{

		}

	}
}
