using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaveBusiness;
using System.IO;

namespace WaveUserInterface
{
	public partial class WaveTreeViewControl : UserControl
	{
		public WaveTreeViewControl()
		{
			InitializeComponent();
		}

		private void OnLoadWaveTreeView(object sender, EventArgs e)
		{
			UIContextManager.NewDirectoryAdded += NewDirectory;
			UIContextManager.TVWColorData += GetTreeviewBackColor;
			UIContextManager.TVWFontColorData += GetTreeviewFontColor;
			UIContextManager.FontChange += ChangeTVWFont;
			UIContextManager.TVWBackgroundColor += ChangeTVWBackground;
		}

		void ChangeTVWBackground()
		{
			ColorDialog cd = new ColorDialog();
			if (cd.ShowDialog() == DialogResult.OK)
			{
				UserSettings.TVWBackColor = System.Drawing.ColorTranslator.ToHtml(cd.Color);
				_tvwWaveList.ForeColor = cd.Color;
				NewDirectory(UserSettings.LastDirectory);
				_tvwWaveList.Refresh();
			}
		}

		void ChangeTVWFont()
		{
			FontDialog f = new FontDialog();
			f.ShowColor = true;

			if (f.ShowDialog() != DialogResult.OK)
			{ return; }
			else
			{
				using (f)
				{
					var FontConverter = new FontConverter();
					UserSettings.TVWFont = FontConverter.ConvertToString(f.Font);
					_tvwWaveList.Font = f.Font;
					UserSettings.TVWFontColor = System.Drawing.ColorTranslator.ToHtml(f.Color);
				}
				NewDirectory(UserSettings.LastDirectory);
				_tvwWaveList.Refresh();
			}
		}
		delegate void NewDirectoryCallBack(string text);

		void NewDirectory(string DirectoryPath)
		{
			if (this._tvwWaveList.InvokeRequired)
			{
				NewDirectoryCallBack n = new NewDirectoryCallBack(NewDirectory);
				this.Invoke(n, new object[] { DirectoryPath });
			}
			else
			{
				_tvwWaveList.Nodes.Clear();
				_tvwWaveList.BackColor = ColorTranslator.FromHtml(UserSettings.TVWBackColor);
				var DirectoryInfo = new DirectoryInfo(DirectoryPath);
				 _tvwWaveList.Nodes.Add(CreateDirNode(DirectoryInfo)); 
			}
		}

		private static TreeNode CreateDirNode(DirectoryInfo DirectoryInfo)
		{
			TypeConverter ColorConverter = TypeDescriptor.GetConverter(typeof(Color));
			var FontConverter = new FontConverter();
			var DirectoryNode = new TreeNode(DirectoryInfo.Name);
			DirectoryNode.NodeFont = FontConverter.ConvertFromString(UserSettings.TVWFont) as Font;
			DirectoryNode.ForeColor = ColorTranslator.FromHtml(UserSettings.TVWFontColor);
			foreach (var Directory in DirectoryInfo.GetDirectories())
			{ DirectoryNode.Nodes.Add(CreateDirNode(Directory)); }
			foreach (var File in DirectoryInfo.GetFiles())
			{
				TreeNode aNode = new TreeNode(File.Name);
				aNode.NodeFont = FontConverter.ConvertFromString(UserSettings.TVWFont) as Font;
				aNode.ForeColor = ColorTranslator.FromHtml(UserSettings.TVWFontColor);
				if(File.Extension == ".wav") DirectoryNode.Nodes.Add(aNode);
			}
			return DirectoryNode;
		}

		private string GetTreeviewFontColor()
		{
			string TVWFontColor = System.Drawing.ColorTranslator.ToHtml(WaveTreeViewControl.DefaultForeColor);
			UserSettings.TVWFontColor = TVWFontColor;
			return UserSettings.TVWFontColor;
		}

		private string GetTreeviewBackColor()
		{
			string BackColor = System.Drawing.ColorTranslator.ToHtml(WaveTreeViewControl.DefaultBackColor);
			UserSettings.TVWBackColor = BackColor;
			return BackColor;
		}

		private void OnTreeViewDoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (_tvwWaveList.SelectedNode != null)
				{
					String SelectedFileName = _tvwWaveList.SelectedNode.Text;
					string CurrDirectoryPath = UserSettings.LastDirectory;
					CurrDirectoryPath = CurrDirectoryPath + "\\" + SelectedFileName;
					UIContextManager.OpenWaveFromTreeView(CurrDirectoryPath);
				}
			}
			catch (Exception e1)
			{
				WaveManager.AddToErrorLog(e1.Message);
				MessageBox.Show("Unable to open file.");
			}
		}

		private void OnDragEnter(object sender, DragEventArgs e)
		{
		}

		private void OnItemDrag(object sender, ItemDragEventArgs e)
		{
			DoDragDrop(e.Item, DragDropEffects.Move);
		}

		private void OnDragLeave(object sender, EventArgs e)
		{
		}

		private void OnAfterSelect(object sender, TreeViewEventArgs e)
		{
		}

		private void OnMouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}
			_cmsColorFont.Show(Cursor.Position);
		}

		private void OnClickFontChange(object sender, EventArgs e)
		{
			UIContextManager.DoFontChange();
		}

		private void OnClickBackgroundColor(object sender, EventArgs e)
		{
			UIContextManager.DoTVWBackgroundColor();
		}
	}
}
