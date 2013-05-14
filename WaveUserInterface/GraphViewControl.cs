using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaveBusiness;
using WaveInfoModel;

namespace WaveUserInterface
{
	public partial class GraphViewControl : Form
	{
		public GraphViewControl()
		{ InitializeComponent(); }

		private int _hasdata;
		public int HasData
		{
			get { return _hasdata; }
			set { _hasdata = value; }
		}

		private void MDIChildForm_Load(object sender, EventArgs e)
		{
			ResizeRedraw = true;
			DoubleBuffered = true;
			this.HasData = 0;
			UIContextManager.MDIChildModified += MDIChildModified;
			UIContextManager.ResizePicBox += ResizePictureBox;
		}

		private void ResizePictureBox(string ChildName)
		{
			if (this.Text == ChildName) // Execute only for the MDIchild in question.
			{
				// Switch to zoom/full view mode.
				if (WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.Text)].IsFull == false)
				{
					WaveManager.WaveList.aWaveList[WaveManager.FindWave(ChildName)].IsFull = true;
				}
				else
				{
					WaveManager.WaveList.aWaveList[WaveManager.FindWave(ChildName)].IsFull = false;
				}
				Invalidate(); 
			}
		}

		private Wave MDIChildModified(Wave aWave)
		{ return aWave; }

		private void MDIChildOnPaint(object sender, PaintEventArgs e)
		{
			if (WaveManager.WaveList.aWaveList.Count == 0)
				return;

			// Create temporary holding wave container.
			Wave MyWave = new Wave();

			// Find the right wave
			if (WaveManager.FindWave(this.Text) != -99999)
			{ MyWave = WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.Text)]; }
			else
			{ return; }
			ColorConverter aConverter = new ColorConverter();
			Color Acolor = ColorTranslator.FromHtml(UserSettings.WaveColor);
			Pen aPen = new Pen(Acolor, UserSettings.PenThickness);

			if (WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.Text)].IsFull == false)
			{
				this.AutoScroll = true;
				AutoScrollMinSize = new Size(MyWave.NumSamples, 256);
				e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

				for (int i = 0; i < MyWave.NumSamples - 1; i++)
				{ e.Graphics.DrawLine(aPen, i, MyWave.Samples[i], i + 1, MyWave.Samples[i + 1]); }
			}
			else
			{
				float x = (float)(this.Width) / (float)(MyWave.NumSamples);
				float y = (float)(this.Height) / (float)(256);
				e.Graphics.ScaleTransform(x,y);

				for (int i = 0; i < MyWave.NumSamples - 1; i++)
				{ 
					e.Graphics.DrawLine(aPen, i, MyWave.Samples[i], i + 1, MyWave.Samples[i + 1]);
				}
				this.AutoScroll = false;
			}
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{ UIContextManager.DoDeleteMDIChild(this.Text); }

		private void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			SaveChanges SaveChange = new SaveChanges();
			if (WaveManager.FindWave(this.Text) != -99999)
			{
				if (WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.Text)].Modified != 0) // wave has been modified
				{
					if (SaveChange.ShowDialog(this) != DialogResult.OK)
					{ return; }
					else
					{
						SaveFileDialog d = new SaveFileDialog();
						d.Filter = @"WAV|*.wav";
						d.DefaultExt = "wav";
						d.FileName = Path.GetFileNameWithoutExtension(this.Text);
						using (d)
						{
							if (d.ShowDialog(this) != DialogResult.OK)
								return;
						}
						byte[] Wave = WaveManager.ConvertBytesToWav(WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.Text)]);
						System.IO.File.WriteAllBytes(d.FileName, Wave);
					}
				}
				WaveManager.DeleteWave(this.Text);
			}
		}
	}
}
