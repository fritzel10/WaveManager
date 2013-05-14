using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaveInfoModel;
using WaveBusiness;
using System.Diagnostics;
using System.Media;
using System.Drawing.Printing;
using System.Xml;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace WaveUserInterface
{
	public partial class FileViewControl : Form
	{
		AppDomain currentDomain = AppDomain.CurrentDomain;
		static Process process = Process.GetCurrentProcess();
		PerformanceCounter theMemCounter = new PerformanceCounter("Process", "Private Bytes", process.ProcessName);

		[DllImport("winmm.dll")]
		public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);
		[DllImport("winmm.dll")]
		public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

		#region Non-Public Data Members

		#endregion
		public FileViewControl()
		{ InitializeComponent(); }
		FileSystemWatcher w = new FileSystemWatcher();

		private void MainForm_Load(object sender, EventArgs e)
		{
			Clipboard.Clear();
			UIContextManager.OpenFromTreeView += OpenWave;
			UIContextManager.DeleteMDIChild += DeleteAnMDIChild;
			UIContextManager.AddMDIChild += Test;
			OpenUserSettings();
			pasteToolStripButton.Enabled = false;
			pasteToolStripMenuItem.Enabled = false;

			try
			{ UIContextManager.NewDirectoryHasBeenAdded(UserSettings.LastDirectory); }
			catch (Exception e3)
			{ WaveManager.AddToErrorLog(e3.Message); }

			if (WaveManager.WaveList.aWaveList.Count == 0)
			{
				pasteToolStripButton.Enabled = false;
				copyToolStripButton.Enabled = false;
			}
			UIContextManager.UpdateWaveSampleCount(this.MdiChildren.Count(), 0);

			uint CurrentVolume;
			waveOutGetVolume(IntPtr.Zero, out CurrentVolume);
			ushort CalcVol = (ushort)(CurrentVolume & 0x0000ffff);
			UIContextManager.DoChangeVolume(CalcVol);

			if(UserSettings.LastDirectory != null && UserSettings.LastDirectory != "Default")
			{
				w.Path = UserSettings.LastDirectory;
				SystemFileWatcherHelper();
			}
			//w.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
			//   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
			//w.Filter = "*.wav";
			//// Add event handlers.
			//w.Created += new FileSystemEventHandler(OnChanged);
			//w.Changed += new FileSystemEventHandler(OnChanged);
			//w.Created += new FileSystemEventHandler(OnChanged);
			//w.Deleted += new FileSystemEventHandler(OnChanged);
			//w.Renamed += new RenamedEventHandler(OnChanged);
			//w.EnableRaisingEvents = true;
		}

		private void SystemFileWatcherHelper()
		{
			w.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
				 | NotifyFilters.FileName | NotifyFilters.DirectoryName;
			w.Filter = "*.wav";
			// Add event handlers.
			w.Created += new FileSystemEventHandler(OnChanged);
			w.Changed += new FileSystemEventHandler(OnChanged);
			w.Created += new FileSystemEventHandler(OnChanged);
			w.Deleted += new FileSystemEventHandler(OnChanged);
			w.Renamed += new RenamedEventHandler(OnChanged);
			w.EnableRaisingEvents = true;
		}

		private void OnChanged(object source, FileSystemEventArgs e)
		{
			UIContextManager.NewDirectoryHasBeenAdded(UserSettings.LastDirectory);
		}

		private void DeleteAnMDIChild(string ChildName)
		{ windowToolStripMenuItem.DropDownItems.RemoveByKey(ChildName); }

		private void Test(string ChildName)
		{
			ToolStripMenuItem anItem = new ToolStripMenuItem() { Name = ChildName };
			anItem.Text = ChildName;
			windowToolStripMenuItem.DropDownItems.Add(anItem);
		}

		private void OpenUserSettings()
		{
			string StartupPath = Environment.CurrentDirectory;
			string FileName = "\\UserSettings.xml";
			string FileLocation = StartupPath + FileName;
			XmlDocument SettingsFile = new XmlDocument();
			try
			{ SettingsFile.Load(FileLocation); }
			catch (Exception e)
			{
				WaveManager.AddToErrorLog(e.Message);
				return;
			}

			XmlNode LastDirectory = SettingsFile.SelectSingleNode("//UserSettings//LastDirectory");
			UserSettings.LastDirectory = LastDirectory.Attributes["LastDirectoryName"].Value;

			XmlNode WaveColorName = SettingsFile.SelectSingleNode("//UserSettings//WaveColor");
			UserSettings.WaveColor = WaveColorName.Attributes["WaveColorName"].Value;

			XmlNode WaveBackgroundColorName = SettingsFile.SelectSingleNode("//UserSettings//WaveBackgroundColor");
			UserSettings.WaveBackgroundColor = WaveBackgroundColorName.Attributes["WaveBackgroundColorName"].Value;

			XmlNode TVWBackColor = SettingsFile.SelectSingleNode("//UserSettings//TreeviewBackgroundColor");
			UserSettings.TVWBackColor = TVWBackColor.Attributes["TreeviewBackgroundColorName"].Value;

			XmlNode TVWFont = SettingsFile.SelectSingleNode("//UserSettings//TreeViewFont");
			UserSettings.TVWFont = TVWFont.Attributes["TreeViewFontName"].Value;

			XmlNode TVWFontColor = SettingsFile.SelectSingleNode("//UserSettings//TreeViewFontColor");
			UserSettings.TVWFontColor = TVWFontColor.Attributes["TreeViewFontColorName"].Value;

			XmlNode PenThickness = SettingsFile.SelectSingleNode("//UserSettings//PenThickness");
			UserSettings.PenThickness = int.Parse(PenThickness.Attributes["PenThicknessName"].Value);

			// Change pen thickness items

			_thicknessEight.Checked = false;
			_thicknessTwo.Checked = false;
			_thicknessFour.Checked = false;
			_thicknessOne.Checked = false;

			_tsm8.Checked = false;
			_tsm2.Checked = false;
			_tsm4.Checked = false;
			_tsm1.Checked = false;

			if (UserSettings.PenThickness == 1)
			{
				_thicknessOne.Checked = true;
				_tsm1.Checked = true;
			}
			else if (UserSettings.PenThickness == 2)
			{
				_thicknessTwo.Checked = true;
				_tsm2.Checked = true;
			}
			else if (UserSettings.PenThickness == 4)
			{
				_thicknessFour.Checked = true;
				_tsm4.Checked = true;
			}
			else if(UserSettings.PenThickness == 8)
			{
				_thicknessEight.Checked = true;
				_tsm8.Checked = true;
			}

			XmlNodeList Waves = SettingsFile.SelectNodes("//UserSettings//Wave");
			foreach (XmlNode aWave in Waves)
			{
				OpenWave(aWave.Attributes["WaveName"].Value);
				Invalidate();
			}
		}

		private void OpenWave(string aPath)
		{
			Wave aWave = new Wave();
			try
			{ aWave = WaveManager.Load(aPath); }
			catch (Exception e1)
			{
				WaveManager.AddToErrorLog(e1.Message);
				MessageBox.Show("The wave file is invalid.");
				return;
			}
			if (aWave != null)
			{
				WaveManager.CurrFileName = aPath;
				string CurrFilePath = Path.GetFullPath(WaveManager.CurrFileName);
				FileInfo DirectoryInfo = new FileInfo(CurrFilePath);
				string CurrDirectoryPath = DirectoryInfo.DirectoryName;
				UserSettings.LastDirectory = CurrDirectoryPath;
				w.Path = UserSettings.LastDirectory;
				SystemFileWatcherHelper();

				//Add new MDI child to wave
				WaveManager.WaveList.aWaveList.Add(aWave);

				// This populates the treeview with the files in the directory.
				UIContextManager.NewDirectoryHasBeenAdded(CurrDirectoryPath);

				// Open a new MDI child window
				GraphViewControl f = new GraphViewControl();
				f.MdiParent = this;

				// This displays the name of the wave file in the title bar of the MDI child window.
				f.Text = CurrFilePath;
				f.BackColor = ColorTranslator.FromHtml(UserSettings.WaveBackgroundColor);
				f.Show();
				Invalidate(); // trigger Paint event

				UIContextManager.UpdateWaveSampleCount(this.MdiChildren.Count(),
					WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)].NumSamples);
				if (WaveManager.WaveList.aWaveList.Count > 0)
				{ copyToolStripButton.Enabled = true; }
				UIContextManager.AddNewMDIChild(this.ActiveMdiChild.Text);
			}
			else
			{ MessageBox.Show("OPENWAVE: The wave file is invalid."); }
		}

		private void OnDragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(TreeNode)))
			{
				TreeNode test = (TreeNode)e.Data.GetData(typeof(TreeNode));
				string CurrDirectoryPath = UserSettings.LastDirectory;
				CurrDirectoryPath = CurrDirectoryPath + "\\" + test.Text;
				OpenWave(CurrDirectoryPath);
			}
			else if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
				foreach (string s in files)
				{
					OpenWave(s);
				} 
			}
			else
			{
				MessageBox.Show("DragDrop Error: Wrong file format.");
			}
			return;
		}

		private void OnDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(TreeNode)) || e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Move;
			}
			else
			{
				MessageBox.Show("DragEnter Error: Invalid file. Enter error.");
				e.Effect = DragDropEffects.None;
			}
			return;
		}

		private void OnClickNew(object sender, EventArgs e)
		{
			GraphViewControl f = new GraphViewControl();
			f.MdiParent = this;
			UIContextManager.AddNewMDIChild(f.Text);
			f.Show();
			OnMDIChildFocusChange(this, e);

		}

		private void OnClickCascade(object sender, EventArgs e)
		{ this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade); }

		private void OnClickTileHorizontally(object sender, EventArgs e)
		{ this.LayoutMdi(System.Windows.Forms.MdiLayout.TileHorizontal); }

		private void OnClickTileVertically(object sender, EventArgs e)
		{ this.LayoutMdi(System.Windows.Forms.MdiLayout.TileVertical); }

		private void OnOpenClick(object sender, EventArgs e)
		{
			OpenFileDialog o = new OpenFileDialog();
			o.Multiselect = true;
			using (o)
			{ if (o.ShowDialog(this) != DialogResult.OK) return; }

			foreach (String aWave in o.FileNames)
			{ OpenWave(aWave); }
		}

		private void OnClickPlay(object sender, EventArgs e)
		{
			// See which MDI child is currently open.
			Form ActiveChild = this.ActiveMdiChild;
			string WaveFileToPlay = ActiveChild.Text;
			Wave PlayWave = new Wave();
			try
			{
				PlayWave = WaveManager.WaveList.aWaveList[WaveManager.FindWave(WaveFileToPlay)];
			}
			catch (Exception e3)
			{ 
				WaveManager.AddToErrorLog(e3.Message);
				return;			
			}

			// 1: Check to see whether wave has been modified.
			if (PlayWave.Modified > 0)
			{
				SaveChanges s = new SaveChanges();

				using (s)
				{
					if (s.ShowDialog(this) != DialogResult.OK) return;
					SaveFileDialog d = new SaveFileDialog();
					d.Filter = @"WAV|*.wav";
					d.DefaultExt = "wav";
					d.FileName = this.ActiveMdiChild.Text;
					byte[] Wave = WaveManager.ConvertBytesToWav(WaveManager.WaveList.aWaveList[WaveManager.FindWave(WaveFileToPlay)]);
					System.IO.File.WriteAllBytes(d.FileName, Wave);
					WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)] = WaveManager.Load(d.FileName);
					UIContextManager.DoDeleteMDIChild(this.ActiveMdiChild.Text);
					this.ActiveMdiChild.Text = d.FileName;
					UIContextManager.AddNewMDIChild(this.ActiveMdiChild.Text);
					SoundPlayer PlaySound = new SoundPlayer(d.FileName);
					PlaySound.Play();
				}
			}
			else // 1a: If wave was not modified
			{
				// Just play the sound!
				SoundPlayer PlaySound = new SoundPlayer(this.ActiveMdiChild.Text);
				PlaySound.Play();
			}
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			// Get memory usage.
			process = Process.GetCurrentProcess();
			float MemoryUsage = this.theMemCounter.NextValue();

			//long MemoryUsage = process.PrivateMemorySize64;
			double MemoryUse = 0.000000953674316 * MemoryUsage;

			// Update the status bar. The name is _pgbGreenBar
			UIContextManager.UpdateMemoryUse(MemoryUse);
		}

		private void OnToolBarClick(object sender, EventArgs e)
		{
			if (toolStrip1.Visible) { toolStrip1.Hide(); }
			else toolStrip1.Show();
		}

		private void OnClickStatusBar(object sender, EventArgs e)
		{
			if (statusStripControl2.Visible) { statusStripControl2.Hide(); }
			else statusStripControl2.Show();
		}

		private void OnClickModulate(object sender, EventArgs e)
		{
			// Find the currently active MDIChild and the filepath.
			Form ActiveChild = this.ActiveMdiChild;
			string WaveFileToModulate = ActiveChild.Text;

			// Grab the wave.
			int WaveIndex = WaveManager.FindWave(WaveFileToModulate);

			// Save backup of current state of wave
			try
			{
				WaveManager.WaveList.aWaveList[WaveIndex].BackupWave = WaveManager.WaveList.aWaveList[WaveIndex];
			}
			catch (Exception e2)
			{
				WaveManager.AddToErrorLog(e2.Message);
				return;
			}
	
			WaveManager.testwave = WaveManager.WaveList.aWaveList[WaveIndex];

			// Modulate function.
			for (int x = 0; x < WaveManager.WaveList.aWaveList[WaveIndex].NumSamples; x++)
			{ WaveManager.WaveList.aWaveList[WaveIndex].Samples[x] += (byte)(Math.Sin(x + 3.2f) * 20); }

			// Set indicator for wave to show that it has been changed.
			WaveManager.WaveList.aWaveList[WaveIndex].Modified += 1;
			OnMDIChildFocusChange(this, e);

			// Display modulated wave.
			ActiveChild.Refresh();
			Invalidate(); // trigger Paint event
		}

		private void OnClickClose(object sender, EventArgs e)
		{
			SaveChanges SaveChange = new SaveChanges();
			if (WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)].Modified != 0)
			{
				if (SaveChange.ShowDialog(this) != DialogResult.OK)
				{ return; }
				else
				{
					SaveFileDialog d = new SaveFileDialog();
					d.Filter = @"WAV|*.wav";
					d.DefaultExt = "wav";
					d.FileName = Path.GetFileNameWithoutExtension(this.ActiveMdiChild.Text);
					using (d)
					{ if (d.ShowDialog(this) != DialogResult.OK) return; }
					byte[] Wave = WaveManager.ConvertBytesToWav(WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)]);
					System.IO.File.WriteAllBytes(d.FileName, Wave);
					WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)].Modified = 0;
				}
			}
			windowToolStripMenuItem.DropDownItems.RemoveByKey(this.ActiveMdiChild.Text);
			WaveManager.DeleteWave(this.ActiveMdiChild.Text);
			this.ActiveMdiChild.Close();
			if (this.MdiChildren.Count() == 0)
			{ copyToolStripButton.Enabled = false; }
		}

		private void OnClickCloseAll(object sender, EventArgs e)
		{
			foreach (Form item in this.MdiChildren)
			{
				WaveManager.DeleteWave(item.Text);
				item.Close();
				copyToolStripButton.Enabled = false;
			}
		}

		private static PageSettings _pagesettings = new PageSettings();

		private void OnClickPageSetup(object sender, EventArgs e)
		{
			PageSetupDialog p = new PageSetupDialog();
			using (p)
			{
				p.PageSettings = _pagesettings;
				p.ShowDialog();
			}
		}
		void OnPrintPage(object sender, PrintPageEventArgs e)
		{

			Wave MyWave = new Wave();
			MyWave = WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)];
			float pageWidth = e.PageSettings.PrintableArea.Width;
			Color Acolor = ColorTranslator.FromHtml(UserSettings.WaveColor);
			Pen aPen = new Pen(Acolor, UserSettings.PenThickness);

			if (MyWave.IsFull == false)
			{
				for (int i = 0; i < (MyWave.NumSamples) - 1; i++)
				{
					e.Graphics.DrawLine(aPen, i, MyWave.Samples[i], i + 1, MyWave.Samples[i + 1]);
				}
			}
			else
			{
				float x = e.PageSettings.PrintableArea.Width / (float)(MyWave.NumSamples);
				float y = e.PageSettings.PrintableArea.Height / (float)(256);
				e.Graphics.ScaleTransform(x, y);

				for (int i = 0; i < MyWave.NumSamples - 1; i++)
				{
					e.Graphics.DrawLine(aPen, i, MyWave.Samples[i], i + 1, MyWave.Samples[i + 1]);
				}
			}
		}

		private void OnClickPrintPreview(object sender, EventArgs e)
		{
			PrintDocument pd = new PrintDocument();
			pd.DefaultPageSettings = _pagesettings;
			pd.PrintPage += OnPrintPage;
			PrintPreviewDialog ppd = new PrintPreviewDialog();
			((Form)ppd).WindowState = FormWindowState.Maximized;
			ppd.Document = pd;
			using (ppd)
			{ ppd.ShowDialog(this); }
		}

		private void OnClickPrint(object sender, EventArgs e)
		{
			PrintDocument pd = new PrintDocument();
			pd.PrintPage += OnPrintPage;
			pd.Print();
		}

		private void OnCopyBitmap(object sender, EventArgs e)
		{
			Wave aWave = WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)];
			Bitmap aBitmap = new Bitmap(aWave.NumSamples, this.ActiveMdiChild.Height);
			Rectangle r = new Rectangle(0, 0, aWave.NumSamples, this.ActiveMdiChild.Height);
			this.ActiveMdiChild.DrawToBitmap(aBitmap, r);
			Clipboard.SetData(DataFormats.Bitmap, aBitmap);
		}

		private void OnClickFullNormal(object sender, EventArgs e)
		{
			UIContextManager.DoResizePicBox(this.ActiveMdiChild.Text);
		}

		private void OnClickColorBackground(object sender, EventArgs e)
		{
			ColorDialog cd = new ColorDialog();
			if (cd.ShowDialog() == DialogResult.OK)
			{
				foreach (Form x in this.MdiChildren)
				{ x.BackColor = cd.Color; }
				UserSettings.WaveBackgroundColor = System.Drawing.ColorTranslator.ToHtml(cd.Color);
			}
		}

		private void OnClickWaveColor(object sender, EventArgs e)
		{
			ColorDialog cd = new ColorDialog();
			if (cd.ShowDialog() == DialogResult.OK)
			{
				UserSettings.WaveColor = System.Drawing.ColorTranslator.ToHtml(cd.Color);
				Invalidate();
				foreach (Form x in this.MdiChildren)
				{ x.Refresh(); }
			}
		}

		private void OnClickTwo(object sender, EventArgs e)
		{
			UserSettings.PenThickness = int.Parse(_thicknessTwo.Text);
			_thicknessTwo.Checked = true;
			_thicknessOne.Checked = false;
			_thicknessFour.Checked = false;
			_thicknessEight.Checked = false;

			_tsm1.Checked = false;
			_tsm2.Checked = true;
			_tsm4.Checked = false;
			_tsm8.Checked = false;
			Invalidate();
			foreach (Form x in this.MdiChildren)
			{ x.Refresh(); }
		}

		private void OnClickFour(object sender, EventArgs e)
		{
			UserSettings.PenThickness = int.Parse(_thicknessFour.Text);
			_thicknessTwo.Checked = false; ;
			_thicknessOne.Checked = false;
			_thicknessFour.Checked = true;
			_thicknessEight.Checked = false;

			_tsm1.Checked = false;
			_tsm2.Checked = false;
			_tsm4.Checked = true;
			_tsm8.Checked = false;

			Invalidate();
			foreach (Form x in this.MdiChildren)
			{ x.Refresh(); }
		}

		private void OnClickEight(object sender, EventArgs e)
		{
			UserSettings.PenThickness = int.Parse(_thicknessEight.Text);
			_thicknessTwo.Checked = false; ;
			_thicknessOne.Checked = false;
			_thicknessFour.Checked = false;
			_thicknessEight.Checked = true;

			_tsm1.Checked = false;
			_tsm2.Checked = false;
			_tsm4.Checked = false;
			_tsm8.Checked = true;

			Invalidate();
			foreach (Form x in this.MdiChildren)
			{ x.Refresh(); }
		}

		private void OnClickOne(object sender, EventArgs e)
		{
			UserSettings.PenThickness = int.Parse(_thicknessOne.Text);
			_thicknessTwo.Checked = false; ;
			_thicknessOne.Checked = true;
			_thicknessFour.Checked = false;
			_thicknessEight.Checked = false;

			_tsm1.Checked = true;
			_tsm2.Checked = false;
			_tsm4.Checked = false;
			_tsm8.Checked = false;

			Invalidate();
			foreach (Form x in this.MdiChildren)
			{ x.Refresh(); }
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			XmlDocument Settings = UserSettings.SaveSettingsXML();
			string StartupPath = Environment.CurrentDirectory;
			string SettingsFile = "\\UserSettings.xml";
			Settings.Save(StartupPath + SettingsFile);
		}

		private void OnClickSaveAs(object sender, EventArgs e)
		{
			SaveFileDialog d = new SaveFileDialog();
			d.Filter = @"WAV|*.wav|PNG|*.png";
			d.DefaultExt = "wav";
			using (d)
			{ if (d.ShowDialog(this) != DialogResult.OK) return; }
			if (Path.GetExtension(d.FileName) == ".png")
			{
				Wave aWave = WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)];
				Bitmap aBitmap = new Bitmap(aWave.NumSamples, this.ActiveMdiChild.Height);
				Rectangle r = new Rectangle(0, 0, aWave.NumSamples, this.ActiveMdiChild.Height);
				this.ActiveMdiChild.DrawToBitmap(aBitmap, r);

				aBitmap.Save(d.FileName);
			}
			else if (Path.GetExtension(d.FileName) == ".wav")
			{
				byte[] Wave = WaveManager.ConvertBytesToWav(WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)]);
				System.IO.File.WriteAllBytes(d.FileName, Wave);
				WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)] = WaveManager.Load(d.FileName);
				UIContextManager.DoDeleteMDIChild(this.ActiveMdiChild.Text);
				this.ActiveMdiChild.Text = d.FileName;
				UIContextManager.AddNewMDIChild(this.ActiveMdiChild.Text);
			}

		}

		private void OnClickSave(object sender, EventArgs e)
		{
			Form ActiveChild = this.ActiveMdiChild;
			string WaveFileToPlay = ActiveChild.Text;
			if (WaveManager.FindWave(WaveFileToPlay) != -99999) // File exists
			{
				Wave PlayWave = WaveManager.WaveList.aWaveList[WaveManager.FindWave(WaveFileToPlay)];
				if (PlayWave.Modified > 0)
				{
					SaveFileDialog d = new SaveFileDialog();
					d.Filter = @"WAV|*.wav";
					d.DefaultExt = "wav";
					d.FileName = this.ActiveMdiChild.Text;
					byte[] Wave = WaveManager.ConvertBytesToWav(WaveManager.WaveList.aWaveList[WaveManager.FindWave(WaveFileToPlay)]);
					System.IO.File.WriteAllBytes(d.FileName, Wave);
					WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)] = WaveManager.Load(d.FileName);
					UIContextManager.DoDeleteMDIChild(this.ActiveMdiChild.Text);
					this.ActiveMdiChild.Text = d.FileName;
					UIContextManager.AddNewMDIChild(this.ActiveMdiChild.Text);
				}
			}
		}

		private void OnClickDelete(object sender, EventArgs e)
		{
			if (this.ActiveMdiChild != null)
			{
				DeleteAnMDIChild(this.ActiveMdiChild.Text);
				this.ActiveMdiChild.Text = "";
				this.ActiveMdiChild.Refresh();
				OnMDIChildFocusChange(this, e);
			}
		}

		private void OnClickTreeviewBackground(object sender, EventArgs e)
		{ UIContextManager.DoTVWBackgroundColor(); }

		private void OnClickCopy(object sender, EventArgs e)
		{
			Wave MyWave = new Wave();
			MyWave = WaveManager.Load(this.ActiveMdiChild.Text);
			DataFormats.Format format = DataFormats.GetFormat(typeof(Wave).FullName);
			IDataObject d = new DataObject();
			d.SetData(format.Name, false, MyWave);
			Clipboard.SetDataObject(d);
			this.pasteToolStripButton.Enabled = true;
			this.pasteToolStripMenuItem.Enabled = true;
		}

		private void OnClickCut(object sender, EventArgs e)
		{
			Wave MyWave = new Wave();
			DeleteAnMDIChild(this.ActiveMdiChild.Text);
			MyWave = WaveManager.Load(this.ActiveMdiChild.Text);
			DataFormats.Format format = DataFormats.GetFormat(typeof(Wave).FullName);
			IDataObject d = new DataObject();
			d.SetData(format.Name, false, MyWave);
			Clipboard.SetDataObject(d);
			WaveManager.WaveList.aWaveList.RemoveAt(WaveManager.FindWave(this.ActiveMdiChild.Text));
			this.ActiveMdiChild.Text = "";
			this.ActiveMdiChild.Refresh();
			this.pasteToolStripButton.Enabled = true;
			this.pasteToolStripMenuItem.Enabled = true;
			this.saveAsToolStripMenuItem.Enabled =false;


		}

		private void OnClickPaste(object sender, EventArgs e)
		{
			Wave aWave = new Wave();
			IDataObject ObtainedObject = Clipboard.GetDataObject();
			string tempName = this.ActiveMdiChild.Text;
			string format = typeof(Wave).FullName;
			aWave = ObtainedObject.GetData(format) as Wave;
			if (this.ActiveMdiChild != null && aWave != null)
			{
				string temp = this.ActiveMdiChild.Text;
				this.ActiveMdiChild.Text = aWave.WavePath;
				WaveManager.WaveList.aWaveList.Add(aWave);
				windowToolStripMenuItem.DropDownItems.RemoveByKey(tempName);
				UIContextManager.AddNewMDIChild(this.ActiveMdiChild.Text);

				// This displays the name of the wave file in the title bar of the MDI child window.
				this.ActiveMdiChild.BackColor = ColorTranslator.FromHtml(UserSettings.WaveBackgroundColor);
				this.ActiveMdiChild.Refresh();

				Invalidate(); // trigger Paint event
				if (WaveManager.WaveList.aWaveList.Count > 0)
				{ copyToolStripButton.Enabled = true; }
				this.saveAsToolStripMenuItem.Enabled = true;
				this.saveToolStripButton.Enabled = true;
			}
			else
			{
				MessageBox.Show("Paste Error: Problem with pasting wave.");
			}
			return;
		}

		private void OnClickRotate(object sender, EventArgs e)
		{
			int WaveIndex = WaveManager.FindWave(this.ActiveMdiChild.Text);
			try
			{
				WaveManager.WaveList.aWaveList[WaveIndex].BackupWave = WaveManager.WaveList.aWaveList[WaveIndex];

			}
			catch (Exception e99)
			{ WaveManager.AddToErrorLog(e99.Message);
			return;

			}

			//WaveManager.WaveList.aWaveList[WaveIndex].BackupWave = WaveManager.WaveList.aWaveList[WaveIndex];
			WaveManager.RotateWave(this.ActiveMdiChild.Text);

			// Set indicator for wave to show that it has been changed.
			WaveManager.WaveList.aWaveList[WaveIndex].Modified += 1;
			OnMDIChildFocusChange(this, e);

			// Display modulated wave.
			this.ActiveMdiChild.Refresh();
			Invalidate(); // trigger Paint event
		}

		private void OnClickUndo(object sender, EventArgs e)
		{
			if (WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)].BackupWave != null) // Wave has been modified.
			{
				WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)] =
					WaveManager.Load(this.ActiveMdiChild.Text);
				Invalidate();
				this.ActiveMdiChild.Refresh();
				WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)].BackupWave = null;
				OnMDIChildFocusChange(this, e);
			}
		}

		private void OnClickHelpAbout(object sender, EventArgs e)
		{
			Help h = new Help();
			h.Show();
		}

		private void OnClickTreeviewFont(object sender, EventArgs e)
		{ UIContextManager.DoFontChange(); }

		private void OnMDIChildFocusChange(object sender, EventArgs e)
		{
			if (MdiChildren.Length != 0 && ActiveMdiChild != null)
			{
				try
				{
					int x = WaveManager.FindWave(ActiveMdiChild.Text);
					if (x != -99999)
					{
						this.saveToolStripButton.Enabled = true;
						this.saveToolStripMenuItem.Enabled = true;
						this.saveAsToolStripMenuItem.Enabled = true;
						this.undoToolStripMenuItem.Enabled = true;
						this.cutToolStripButton.Enabled = true;
						this.copyToolStripButton.Enabled = true;
						this.copyToolStripMenuItem.Enabled = true;
						this._btnDelete.Enabled = true;
						this._tsmDelete.Enabled = true;
						IDataObject data = System.Windows.Forms.Clipboard.GetDataObject();
						if (WaveManager.WaveList.aWaveList[x].Modified == 0)
						{
							this.saveToolStripButton.Enabled = false;
							this.saveToolStripMenuItem.Enabled = false;
						}
						if (WaveManager.WaveList.aWaveList[x].BackupWave != null)
						{ this.undoToolStripMenuItem.Enabled = true; }
						else
						{ this.undoToolStripMenuItem.Enabled = false; }
					}
					else
					{
						this.saveToolStripButton.Enabled = false;
						this.saveToolStripMenuItem.Enabled = false;
						this.saveAsToolStripMenuItem.Enabled = false;
						this.undoToolStripMenuItem.Enabled = false;
						this.cutToolStripButton.Enabled = false;
						this.copyToolStripButton.Enabled = false;
						this.copyToolStripMenuItem.Enabled = false;
						this._btnDelete.Enabled = false;
						this._tsmDelete.Enabled = false;
						IDataObject data = System.Windows.Forms.Clipboard.GetDataObject();
					}

				}
				catch (Exception e2)
				{
					WaveManager.AddToErrorLog(e2.Message);
					this.saveToolStripButton.Enabled = false;
					this.saveToolStripMenuItem.Enabled = false;
					this.undoToolStripMenuItem.Enabled = false;
					this.cutToolStripButton.Enabled = false;
					this.copyToolStripButton.Enabled = false;
					this.copyToolStripMenuItem.Enabled = false;
					this._btnDelete.Enabled = false;
					this._tsmDelete.Enabled = false;
					
				}
			}
			if (this.ActiveMdiChild != null && WaveManager.FindWave(this.ActiveMdiChild.Text) != -99999)
			{
				UIContextManager.UpdateWaveSampleCount(WaveManager.WaveList.aWaveList.Count,
				WaveManager.WaveList.aWaveList[WaveManager.FindWave(this.ActiveMdiChild.Text)].NumSamples);
			}
			else
			{ UIContextManager.UpdateWaveSampleCount(WaveManager.WaveList.aWaveList.Count, 0); }
		}
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileViewControl));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._tsmClose = new System.Windows.Forms.ToolStripMenuItem();
			this._tsmCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
			this.pageSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._tsmCopyBitmap = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._tsmDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
			this.fullNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.waveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.thicknessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._tsm1 = new System.Windows.Forms.ToolStripMenuItem();
			this._tsm2 = new System.Windows.Forms.ToolStripMenuItem();
			this._tsm4 = new System.Windows.Forms.ToolStripMenuItem();
			this._tsm8 = new System.Windows.Forms.ToolStripMenuItem();
			this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.backgroundToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tileHorizontallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tileVerticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this._btnDelete = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this._btnFullNormal = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this._btnPlay = new System.Windows.Forms.ToolStripButton();
			this._btnModulate = new System.Windows.Forms.ToolStripButton();
			this._btnRotate = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this._btnColor = new System.Windows.Forms.ToolStripDropDownButton();
			this.waveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.backgroundToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this._btnThickness = new System.Windows.Forms.ToolStripDropDownButton();
			this._thicknessOne = new System.Windows.Forms.ToolStripMenuItem();
			this._thicknessTwo = new System.Windows.Forms.ToolStripMenuItem();
			this._thicknessFour = new System.Windows.Forms.ToolStripMenuItem();
			this._thicknessEight = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
			this._btnAbout = new System.Windows.Forms.ToolStripButton();
			this.splitter3 = new System.Windows.Forms.Splitter();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modulateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			this.waveTreeViewControl1 = new WaveUserInterface.WaveTreeViewControl();
			this.statusStripControl2 = new WaveUserInterface.StatusStripControl();
			this.menuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.formatToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(934, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this._tsmClose,
            this._tsmCloseAll,
            this.toolStripSeparator18,
            this.pageSetupToolStripMenuItem,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
			this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.newToolStripMenuItem.Text = "&New";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
			this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.OnOpenClick);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(152, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
			this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.OnClickSaveAs);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
			// 
			// _tsmClose
			// 
			this._tsmClose.Name = "_tsmClose";
			this._tsmClose.Size = new System.Drawing.Size(155, 22);
			this._tsmClose.Text = "Close";
			this._tsmClose.Click += new System.EventHandler(this.OnClickClose);
			// 
			// _tsmCloseAll
			// 
			this._tsmCloseAll.Name = "_tsmCloseAll";
			this._tsmCloseAll.Size = new System.Drawing.Size(155, 22);
			this._tsmCloseAll.Text = "Close All";
			this._tsmCloseAll.Click += new System.EventHandler(this.OnClickCloseAll);
			// 
			// toolStripSeparator18
			// 
			this.toolStripSeparator18.Name = "toolStripSeparator18";
			this.toolStripSeparator18.Size = new System.Drawing.Size(152, 6);
			// 
			// pageSetupToolStripMenuItem
			// 
			this.pageSetupToolStripMenuItem.Name = "pageSetupToolStripMenuItem";
			this.pageSetupToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.pageSetupToolStripMenuItem.Text = "Page Setup...";
			this.pageSetupToolStripMenuItem.Click += new System.EventHandler(this.OnClickPageSetup);
			// 
			// printToolStripMenuItem
			// 
			this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
			this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.printToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.printToolStripMenuItem.Text = "&Print";
			this.printToolStripMenuItem.Click += new System.EventHandler(this.OnClickPrint);
			// 
			// printPreviewToolStripMenuItem
			// 
			this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
			this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
			this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
			this.printPreviewToolStripMenuItem.Click += new System.EventHandler(this.OnClickPrintPreview);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(152, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this._tsmCopyBitmap,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this._tsmDelete,
            this.toolStripSeparator4});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.undoToolStripMenuItem.Text = "&Undo";
			this.undoToolStripMenuItem.Click += new System.EventHandler(this.OnClickUndo);
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.redoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.redoToolStripMenuItem.Text = "&Redo";
			this.redoToolStripMenuItem.Click += new System.EventHandler(this.OnClickRedo);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
			this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.cutToolStripMenuItem.Text = "Cu&t";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.OnClickCut);
			// 
			// _tsmCopyBitmap
			// 
			this._tsmCopyBitmap.Name = "_tsmCopyBitmap";
			this._tsmCopyBitmap.Size = new System.Drawing.Size(152, 22);
			this._tsmCopyBitmap.Text = "Copy Bitmap";
			this._tsmCopyBitmap.Click += new System.EventHandler(this.OnCopyBitmap);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.copyToolStripMenuItem.Text = "&Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.OnClickCopy);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
			this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.pasteToolStripMenuItem.Text = "&Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.OnClickPaste);
			// 
			// _tsmDelete
			// 
			this._tsmDelete.Name = "_tsmDelete";
			this._tsmDelete.Size = new System.Drawing.Size(152, 22);
			this._tsmDelete.Text = "Delete";
			this._tsmDelete.Click += new System.EventHandler(this.OnClickDelete);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBarToolStripMenuItem,
            this.statusBarToolStripMenuItem,
            this.toolStripSeparator17,
            this.fullNormalToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// toolBarToolStripMenuItem
			// 
			this.toolBarToolStripMenuItem.Checked = true;
			this.toolBarToolStripMenuItem.CheckOnClick = true;
			this.toolBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.toolBarToolStripMenuItem.Name = "toolBarToolStripMenuItem";
			this.toolBarToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.toolBarToolStripMenuItem.Text = "Tool Bar";
			this.toolBarToolStripMenuItem.Click += new System.EventHandler(this.OnToolBarClick);
			// 
			// statusBarToolStripMenuItem
			// 
			this.statusBarToolStripMenuItem.Checked = true;
			this.statusBarToolStripMenuItem.CheckOnClick = true;
			this.statusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
			this.statusBarToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.statusBarToolStripMenuItem.Text = "Status Bar";
			this.statusBarToolStripMenuItem.Click += new System.EventHandler(this.OnClickStatusBar);
			// 
			// toolStripSeparator17
			// 
			this.toolStripSeparator17.Name = "toolStripSeparator17";
			this.toolStripSeparator17.Size = new System.Drawing.Size(135, 6);
			// 
			// fullNormalToolStripMenuItem
			// 
			this.fullNormalToolStripMenuItem.Name = "fullNormalToolStripMenuItem";
			this.fullNormalToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.fullNormalToolStripMenuItem.Text = "Full/Normal";
			this.fullNormalToolStripMenuItem.Click += new System.EventHandler(this.OnClickFullNormal);
			// 
			// formatToolStripMenuItem
			// 
			this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.waveToolStripMenuItem});
			this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
			this.formatToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.formatToolStripMenuItem.Text = "Format";
			// 
			// waveToolStripMenuItem
			// 
			this.waveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorToolStripMenuItem,
            this.thicknessToolStripMenuItem,
            this.backgroundToolStripMenuItem});
			this.waveToolStripMenuItem.Name = "waveToolStripMenuItem";
			this.waveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.waveToolStripMenuItem.Text = "Wave";
			// 
			// colorToolStripMenuItem
			// 
			this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
			this.colorToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.colorToolStripMenuItem.Text = "Color...";
			this.colorToolStripMenuItem.Click += new System.EventHandler(this.OnClickWaveColor);
			// 
			// thicknessToolStripMenuItem
			// 
			this.thicknessToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsm1,
            this._tsm2,
            this._tsm4,
            this._tsm8});
			this.thicknessToolStripMenuItem.Name = "thicknessToolStripMenuItem";
			this.thicknessToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.thicknessToolStripMenuItem.Text = "Thickness";
			// 
			// _tsm1
			// 
			this._tsm1.Checked = true;
			this._tsm1.CheckOnClick = true;
			this._tsm1.CheckState = System.Windows.Forms.CheckState.Checked;
			this._tsm1.Name = "_tsm1";
			this._tsm1.Size = new System.Drawing.Size(80, 22);
			this._tsm1.Text = "1";
			this._tsm1.Click += new System.EventHandler(this.OnClickOne);
			// 
			// _tsm2
			// 
			this._tsm2.CheckOnClick = true;
			this._tsm2.Name = "_tsm2";
			this._tsm2.Size = new System.Drawing.Size(80, 22);
			this._tsm2.Text = "2";
			this._tsm2.Click += new System.EventHandler(this.OnClickTwo);
			// 
			// _tsm4
			// 
			this._tsm4.CheckOnClick = true;
			this._tsm4.Name = "_tsm4";
			this._tsm4.Size = new System.Drawing.Size(80, 22);
			this._tsm4.Text = "4";
			this._tsm4.Click += new System.EventHandler(this.OnClickFour);
			// 
			// _tsm8
			// 
			this._tsm8.CheckOnClick = true;
			this._tsm8.Name = "_tsm8";
			this._tsm8.Size = new System.Drawing.Size(80, 22);
			this._tsm8.Text = "8";
			this._tsm8.Click += new System.EventHandler(this.OnClickEight);
			// 
			// backgroundToolStripMenuItem
			// 
			this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
			this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.backgroundToolStripMenuItem.Text = "Background...";
			this.backgroundToolStripMenuItem.Click += new System.EventHandler(this.OnClickColorBackground);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.rotateToolStripMenuItem,
            this.modulateToolStripMenuItem,
            this.toolStripSeparator15,
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// customizeToolStripMenuItem
			// 
			this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
			this.customizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.customizeToolStripMenuItem.Text = "&Customize";
			this.customizeToolStripMenuItem.Click += new System.EventHandler(this.OnClickCustomize);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontToolStripMenuItem,
            this.backgroundToolStripMenuItem2});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.optionsToolStripMenuItem.Text = "&Options";
			// 
			// fontToolStripMenuItem
			// 
			this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
			this.fontToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.fontToolStripMenuItem.Text = "Font";
			this.fontToolStripMenuItem.Click += new System.EventHandler(this.OnClickTreeviewFont);
			// 
			// backgroundToolStripMenuItem2
			// 
			this.backgroundToolStripMenuItem2.Name = "backgroundToolStripMenuItem2";
			this.backgroundToolStripMenuItem2.Size = new System.Drawing.Size(138, 22);
			this.backgroundToolStripMenuItem2.Text = "Background";
			this.backgroundToolStripMenuItem2.Click += new System.EventHandler(this.OnClickTreeviewBackground);
			// 
			// windowToolStripMenuItem
			// 
			this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tileHorizontallyToolStripMenuItem,
            this.tileVerticallyToolStripMenuItem,
            this.cascadeToolStripMenuItem,
            this.toolStripSeparator16});
			this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
			this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
			this.windowToolStripMenuItem.Text = "Window";
			// 
			// tileHorizontallyToolStripMenuItem
			// 
			this.tileHorizontallyToolStripMenuItem.Name = "tileHorizontallyToolStripMenuItem";
			this.tileHorizontallyToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.tileHorizontallyToolStripMenuItem.Text = "Tile Horizontally";
			this.tileHorizontallyToolStripMenuItem.Click += new System.EventHandler(this.OnClickTileHorizontally);
			// 
			// tileVerticallyToolStripMenuItem
			// 
			this.tileVerticallyToolStripMenuItem.Name = "tileVerticallyToolStripMenuItem";
			this.tileVerticallyToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.tileVerticallyToolStripMenuItem.Text = "Tile Vertically";
			this.tileVerticallyToolStripMenuItem.Click += new System.EventHandler(this.OnClickTileVertically);
			// 
			// cascadeToolStripMenuItem
			// 
			this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
			this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.cascadeToolStripMenuItem.Text = "Cascade";
			this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.OnClickCascade);
			// 
			// toolStripSeparator16
			// 
			this.toolStripSeparator16.Name = "toolStripSeparator16";
			this.toolStripSeparator16.Size = new System.Drawing.Size(157, 6);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indexToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// indexToolStripMenuItem
			// 
			this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
			this.indexToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.indexToolStripMenuItem.Text = "&Index";
			this.indexToolStripMenuItem.Click += new System.EventHandler(this.OnClickIndex);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(113, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnClickHelpAbout);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator9,
            this.toolStripSeparator6,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this._btnDelete,
            this.toolStripSeparator10,
            this.toolStripSeparator8,
            this._btnFullNormal,
            this.toolStripSeparator7,
            this.toolStripSeparator11,
            this._btnPlay,
            this._btnModulate,
            this._btnRotate,
            this.toolStripSeparator12,
            this.toolStripSeparator13,
            this._btnColor,
            this._btnThickness,
            this.toolStripSeparator14,
            this.toolStripSeparator19,
            this._btnAbout});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(934, 38);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// newToolStripButton
			// 
			this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
			this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripButton.Name = "newToolStripButton";
			this.newToolStripButton.Size = new System.Drawing.Size(35, 35);
			this.newToolStripButton.Text = "&New";
			this.newToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.newToolStripButton.Click += new System.EventHandler(this.OnClickNew);
			// 
			// openToolStripButton
			// 
			this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
			this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripButton.Name = "openToolStripButton";
			this.openToolStripButton.Size = new System.Drawing.Size(40, 35);
			this.openToolStripButton.Text = "&Open";
			this.openToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.openToolStripButton.Click += new System.EventHandler(this.OnOpenClick);
			// 
			// saveToolStripButton
			// 
			this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
			this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripButton.Name = "saveToolStripButton";
			this.saveToolStripButton.Size = new System.Drawing.Size(35, 35);
			this.saveToolStripButton.Text = "&Save";
			this.saveToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.saveToolStripButton.Click += new System.EventHandler(this.OnClickSave);
			// 
			// printToolStripButton
			// 
			this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
			this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printToolStripButton.Name = "printToolStripButton";
			this.printToolStripButton.Size = new System.Drawing.Size(36, 35);
			this.printToolStripButton.Text = "&Print";
			this.printToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.printToolStripButton.Click += new System.EventHandler(this.OnClickPrint);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(6, 38);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 38);
			// 
			// cutToolStripButton
			// 
			this.cutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripButton.Image")));
			this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripButton.Name = "cutToolStripButton";
			this.cutToolStripButton.Size = new System.Drawing.Size(30, 35);
			this.cutToolStripButton.Text = "C&ut";
			this.cutToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.cutToolStripButton.Click += new System.EventHandler(this.OnClickCut);
			// 
			// copyToolStripButton
			// 
			this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
			this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripButton.Name = "copyToolStripButton";
			this.copyToolStripButton.Size = new System.Drawing.Size(39, 35);
			this.copyToolStripButton.Text = "&Copy";
			this.copyToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.copyToolStripButton.Click += new System.EventHandler(this.OnClickCopy);
			// 
			// pasteToolStripButton
			// 
			this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
			this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripButton.Name = "pasteToolStripButton";
			this.pasteToolStripButton.Size = new System.Drawing.Size(39, 35);
			this.pasteToolStripButton.Text = "&Paste";
			this.pasteToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.pasteToolStripButton.Click += new System.EventHandler(this.OnClickPaste);
			// 
			// _btnDelete
			// 
			this._btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("_btnDelete.Image")));
			this._btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._btnDelete.Name = "_btnDelete";
			this._btnDelete.Size = new System.Drawing.Size(44, 35);
			this._btnDelete.Text = "Delete";
			this._btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this._btnDelete.Click += new System.EventHandler(this.OnClickDelete);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(6, 38);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 38);
			// 
			// _btnFullNormal
			// 
			this._btnFullNormal.Image = ((System.Drawing.Image)(resources.GetObject("_btnFullNormal.Image")));
			this._btnFullNormal.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._btnFullNormal.Name = "_btnFullNormal";
			this._btnFullNormal.Size = new System.Drawing.Size(75, 35);
			this._btnFullNormal.Text = "Full/Normal";
			this._btnFullNormal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this._btnFullNormal.Click += new System.EventHandler(this.OnClickFullNormal);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 38);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(6, 38);
			// 
			// _btnPlay
			// 
			this._btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("_btnPlay.Image")));
			this._btnPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._btnPlay.Name = "_btnPlay";
			this._btnPlay.Size = new System.Drawing.Size(33, 35);
			this._btnPlay.Text = "Play";
			this._btnPlay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this._btnPlay.Click += new System.EventHandler(this.OnClickPlay);
			// 
			// _btnModulate
			// 
			this._btnModulate.Image = ((System.Drawing.Image)(resources.GetObject("_btnModulate.Image")));
			this._btnModulate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._btnModulate.Name = "_btnModulate";
			this._btnModulate.Size = new System.Drawing.Size(62, 35);
			this._btnModulate.Text = "Modulate";
			this._btnModulate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this._btnModulate.Click += new System.EventHandler(this.OnClickModulate);
			// 
			// _btnRotate
			// 
			this._btnRotate.Image = ((System.Drawing.Image)(resources.GetObject("_btnRotate.Image")));
			this._btnRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._btnRotate.Name = "_btnRotate";
			this._btnRotate.Size = new System.Drawing.Size(45, 35);
			this._btnRotate.Text = "Rotate";
			this._btnRotate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this._btnRotate.Click += new System.EventHandler(this.OnClickRotate);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new System.Drawing.Size(6, 38);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(6, 38);
			// 
			// _btnColor
			// 
			this._btnColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.waveToolStripMenuItem1,
            this.backgroundToolStripMenuItem1});
			this._btnColor.Image = ((System.Drawing.Image)(resources.GetObject("_btnColor.Image")));
			this._btnColor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._btnColor.Name = "_btnColor";
			this._btnColor.Size = new System.Drawing.Size(49, 35);
			this._btnColor.Text = "Color";
			this._btnColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// waveToolStripMenuItem1
			// 
			this.waveToolStripMenuItem1.Name = "waveToolStripMenuItem1";
			this.waveToolStripMenuItem1.Size = new System.Drawing.Size(147, 22);
			this.waveToolStripMenuItem1.Text = "Wave...";
			this.waveToolStripMenuItem1.Click += new System.EventHandler(this.OnClickWaveColor);
			// 
			// backgroundToolStripMenuItem1
			// 
			this.backgroundToolStripMenuItem1.Name = "backgroundToolStripMenuItem1";
			this.backgroundToolStripMenuItem1.Size = new System.Drawing.Size(147, 22);
			this.backgroundToolStripMenuItem1.Text = "Background...";
			this.backgroundToolStripMenuItem1.Click += new System.EventHandler(this.OnClickColorBackground);
			// 
			// _btnThickness
			// 
			this._btnThickness.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._thicknessOne,
            this._thicknessTwo,
            this._thicknessFour,
            this._thicknessEight});
			this._btnThickness.Image = ((System.Drawing.Image)(resources.GetObject("_btnThickness.Image")));
			this._btnThickness.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._btnThickness.Name = "_btnThickness";
			this._btnThickness.Size = new System.Drawing.Size(72, 35);
			this._btnThickness.Text = "Thickness";
			this._btnThickness.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// _thicknessOne
			// 
			this._thicknessOne.Checked = true;
			this._thicknessOne.CheckOnClick = true;
			this._thicknessOne.CheckState = System.Windows.Forms.CheckState.Checked;
			this._thicknessOne.Name = "_thicknessOne";
			this._thicknessOne.Size = new System.Drawing.Size(80, 22);
			this._thicknessOne.Text = "1";
			this._thicknessOne.Click += new System.EventHandler(this.OnClickOne);
			// 
			// _thicknessTwo
			// 
			this._thicknessTwo.CheckOnClick = true;
			this._thicknessTwo.Name = "_thicknessTwo";
			this._thicknessTwo.Size = new System.Drawing.Size(80, 22);
			this._thicknessTwo.Text = "2";
			this._thicknessTwo.Click += new System.EventHandler(this.OnClickTwo);
			// 
			// _thicknessFour
			// 
			this._thicknessFour.CheckOnClick = true;
			this._thicknessFour.Name = "_thicknessFour";
			this._thicknessFour.Size = new System.Drawing.Size(80, 22);
			this._thicknessFour.Text = "4";
			this._thicknessFour.Click += new System.EventHandler(this.OnClickFour);
			// 
			// _thicknessEight
			// 
			this._thicknessEight.CheckOnClick = true;
			this._thicknessEight.Name = "_thicknessEight";
			this._thicknessEight.Size = new System.Drawing.Size(80, 22);
			this._thicknessEight.Text = "8";
			this._thicknessEight.Click += new System.EventHandler(this.OnClickEight);
			// 
			// toolStripSeparator14
			// 
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			this.toolStripSeparator14.Size = new System.Drawing.Size(6, 38);
			// 
			// toolStripSeparator19
			// 
			this.toolStripSeparator19.Name = "toolStripSeparator19";
			this.toolStripSeparator19.Size = new System.Drawing.Size(6, 38);
			// 
			// _btnAbout
			// 
			this._btnAbout.Image = ((System.Drawing.Image)(resources.GetObject("_btnAbout.Image")));
			this._btnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._btnAbout.Name = "_btnAbout";
			this._btnAbout.Size = new System.Drawing.Size(44, 35);
			this._btnAbout.Text = "About";
			this._btnAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this._btnAbout.Click += new System.EventHandler(this.OnClickHelpAbout);
			// 
			// splitter3
			// 
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter3.Location = new System.Drawing.Point(0, 62);
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(934, 3);
			this.splitter3.TabIndex = 3;
			this.splitter3.TabStop = false;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Left;
			this.splitContainer1.Location = new System.Drawing.Point(0, 65);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.waveTreeViewControl1);
			this.splitContainer1.Panel2Collapsed = true;
			this.splitContainer1.Size = new System.Drawing.Size(208, 318);
			this.splitContainer1.SplitterDistance = 85;
			this.splitContainer1.TabIndex = 8;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 10000;
			this.timer1.Tick += new System.EventHandler(this.OnTimerTick);
			// 
			// playToolStripMenuItem
			// 
			this.playToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("playToolStripMenuItem.Image")));
			this.playToolStripMenuItem.Name = "playToolStripMenuItem";
			this.playToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.playToolStripMenuItem.Text = "Play";
			this.playToolStripMenuItem.Click += new System.EventHandler(this.OnClickPlay);
			// 
			// modulateToolStripMenuItem
			// 
			this.modulateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("modulateToolStripMenuItem.Image")));
			this.modulateToolStripMenuItem.Name = "modulateToolStripMenuItem";
			this.modulateToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.modulateToolStripMenuItem.Text = "Modulate";
			this.modulateToolStripMenuItem.Click += new System.EventHandler(this.OnClickModulate);
			// 
			// rotateToolStripMenuItem
			// 
			this.rotateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("rotateToolStripMenuItem.Image")));
			this.rotateToolStripMenuItem.Name = "rotateToolStripMenuItem";
			this.rotateToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.rotateToolStripMenuItem.Text = "Rotate";
			this.rotateToolStripMenuItem.Click += new System.EventHandler(this.OnClickRotate);
			// 
			// toolStripSeparator15
			// 
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			this.toolStripSeparator15.Size = new System.Drawing.Size(127, 6);
			// 
			// waveTreeViewControl1
			// 
			this.waveTreeViewControl1.Dock = System.Windows.Forms.DockStyle.Left;
			this.waveTreeViewControl1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.waveTreeViewControl1.Location = new System.Drawing.Point(0, 0);
			this.waveTreeViewControl1.Name = "waveTreeViewControl1";
			this.waveTreeViewControl1.Size = new System.Drawing.Size(208, 318);
			this.waveTreeViewControl1.TabIndex = 0;
			// 
			// statusStripControl2
			// 
			this.statusStripControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.statusStripControl2.Location = new System.Drawing.Point(0, 383);
			this.statusStripControl2.Name = "statusStripControl2";
			this.statusStripControl2.Size = new System.Drawing.Size(934, 31);
			this.statusStripControl2.TabIndex = 4;
			// 
			// FileViewControl
			// 
			this.AllowDrop = true;
			this.ClientSize = new System.Drawing.Size(934, 414);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStripControl2);
			this.Controls.Add(this.splitter3);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip1);
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "FileViewControl";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Wave Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.MdiChildActivate += new System.EventHandler(this.OnMDIChildFocusChange);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void OnClickRedo(object sender, EventArgs e)
		{
			MessageBox.Show("Not implemented.");
		}

		private void OnClickIndex(object sender, EventArgs e)
		{
			MessageBox.Show("Not implemented.");

		}

		private void OnClickCustomize(object sender, EventArgs e)
		{
			MessageBox.Show("Not implemented.");
		}




	}
}
