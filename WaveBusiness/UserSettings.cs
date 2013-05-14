using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
using WaveInfoModel;

namespace WaveBusiness
{
	public static class UserSettings
	{
		private static string _lastdirectory = "Default";

		public static string LastDirectory
		{
			get { return _lastdirectory; }
			set { _lastdirectory = value; }
		}

		private static string _wavecolor = "Black";

		public static string WaveColor
		{
			get { return _wavecolor; }
			set { _wavecolor = value; }
		}

		private static string _wavebackgroundcolor = "White";

		public static string WaveBackgroundColor
		{
			get { return _wavebackgroundcolor; }
			set { _wavebackgroundcolor = value; }
		}

		private static string _tvwfont = "Arial, 8.25pt";

		public static string TVWFont
		{
			get { return _tvwfont; }
			set { _tvwfont = value; }
		}

		private static string _tvwfontcolor;

		public static string TVWFontColor
		{
			get { return _tvwfontcolor; }
			set { _tvwfontcolor = value; }
		}


		private static string _tvwbackcolor;

		public static string TVWBackColor
		{
			get { return _tvwbackcolor; }
			set { _tvwbackcolor = value; }
		}

		private static int _penthickness = 1;

		public static int PenThickness
		{
			get { return _penthickness; }
			set { _penthickness = value; }
		}

		public static void OpenSettingsXML() // parse that mofo
		{
		}
		public static XmlDocument SaveSettingsXML()
		{
			// save wave color, background color, pen thickness
			XmlDocument Settings = new XmlDocument();

			XmlElement Root = Settings.CreateElement("UserSettings");
			Settings.AppendChild(Root);

			//Last Directory
			XmlElement LastDirectory = Settings.CreateElement("LastDirectory");
			XmlAttribute Color = Settings.CreateAttribute("LastDirectoryName");
			Color.Value = UserSettings.LastDirectory;
			LastDirectory.SetAttributeNode(Color);
			Root.AppendChild(LastDirectory);

			//Wave colour
			XmlElement PenThickness = Settings.CreateElement("PenThickness");
			Color = Settings.CreateAttribute("PenThicknessName");
			Color.Value = UserSettings.PenThickness.ToString();
			PenThickness.SetAttributeNode(Color);
			Root.AppendChild(PenThickness);

			//Wave colour
			XmlElement WaveColor = Settings.CreateElement("WaveColor");
			Color = Settings.CreateAttribute("WaveColorName");
			Color.Value = UserSettings.WaveColor;
			WaveColor.SetAttributeNode(Color);
			Root.AppendChild(WaveColor);

			//Wave background colour
			XmlElement WaveBackgroundColor = Settings.CreateElement("WaveBackgroundColor");
			Color = Settings.CreateAttribute("WaveBackgroundColorName");
			Color.Value = UserSettings.WaveBackgroundColor;
			WaveBackgroundColor.SetAttributeNode(Color);
			Root.AppendChild(WaveBackgroundColor);

			//Treeview Font
			XmlElement TreeViewFont = Settings.CreateElement("TreeViewFont");
			Color = Settings.CreateAttribute("TreeViewFontName");
			Color.Value = UserSettings.TVWFont;
			TreeViewFont.SetAttributeNode(Color);
			Root.AppendChild(TreeViewFont);

			//Treeview Font colour
			XmlElement TreeViewFontColor = Settings.CreateElement("TreeViewFontColor");
			Color = Settings.CreateAttribute("TreeViewFontColorName");
			Color.Value = UserSettings.TVWFontColor;
			TreeViewFontColor.SetAttributeNode(Color);
			Root.AppendChild(TreeViewFontColor);

			//Treeview background colour
			XmlElement TreeviewBackgroundColor = Settings.CreateElement("TreeviewBackgroundColor");
			Color = Settings.CreateAttribute("TreeviewBackgroundColorName");
			Color.Value = UserSettings.TVWBackColor;
			TreeviewBackgroundColor.SetAttributeNode(Color);
			Root.AppendChild(TreeviewBackgroundColor);

			// Need to save the list of waves that were open.
			foreach (Wave aWave in WaveManager.WaveList.aWaveList)
			{
				XmlElement Wave = Settings.CreateElement("Wave");
				Color = Settings.CreateAttribute("WaveName");
				Color.Value = aWave.WavePath;
				Wave.SetAttributeNode(Color);
				Root.AppendChild(Wave);
			}

			return Settings;
		}


		//public XmlDocument createXMLFile(List<Group> groups)
		//{
		//	XmlDocument MyKeyPass = new XmlDocument();

		//	XmlElement root = MyKeyPass.CreateElement("MyKeyPass");
		//	MyKeyPass.AppendChild(root);

		//	foreach (Group aGroup in groups)
		//	{
		//		XmlElement group = MyKeyPass.CreateElement("group");
		//		XmlAttribute Name = MyKeyPass.CreateAttribute("groupName");
		//		Name.Value = aGroup.GroupName;
		//		group.SetAttributeNode(Name);
		//		root.AppendChild(group);

		//		// Fetch keys associated with a group.
		//		List<Key> keys = KeyPassManager.getKey(aGroup.GroupName);
		//		foreach (Key aKey in keys)
		//		{
		//			XmlElement key = MyKeyPass.CreateElement("key");

		//			XmlAttribute Title = MyKeyPass.CreateAttribute("Title");
		//			XmlAttribute Username = MyKeyPass.CreateAttribute("Username");
		//			XmlAttribute Password = MyKeyPass.CreateAttribute("Password");
		//			XmlAttribute URL = MyKeyPass.CreateAttribute("URL");
		//			XmlAttribute Notes = MyKeyPass.CreateAttribute("Notes");

		//			Title.Value = aKey.Title;
		//			Username.Value = aKey.Username;
		//			Password.Value = aKey.Password;
		//			URL.Value = aKey.URL;
		//			Notes.Value = aKey.Notes;

		//			key.SetAttributeNode(Notes);
		//			key.SetAttributeNode(URL);
		//			key.SetAttributeNode(Password);
		//			key.SetAttributeNode(Username);
		//			key.SetAttributeNode(Title);

		//			group.AppendChild(key);
		//		}
		//	}
		//	return MyKeyPass;
		//}

	}
}
