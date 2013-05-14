using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveInfoModel;

namespace WaveBusiness
{
	public delegate void NewDirectoryEventHandler(string NewDirectory);
	public delegate void OpenFromTreeViewEventHandler(string Path);
	public delegate void GreyoutItemsEventHandler(string Name);
	public delegate void ChangeVolumeEventHandler(ushort CalcVol);
	public delegate void MemoryUseUpdateEventHandler(double MemoryUse);
	public delegate void WaveSampleCountUpdateEventHandler(int WaveCount, int SampleCount);
	public delegate Wave MDIChildModifiedEventHandler(Wave ChangedWave);
	public delegate string GetColorDataEventHandler();
	public delegate void AddMDIChildEventHandler(string ChildName);
	public delegate void DeleteMDIChildEventHandler(string ChildName);
	public delegate void TVWFontChangeHandler();
	public delegate void TVWBackgroundColorChangeHandler();

	public delegate void AddToErrorLogEventHandler();
	public delegate void ResizePicBoxEventHandler(string ChildName);


	public static class UIContextManager
	{
		public static event ResizePicBoxEventHandler ResizePicBox = null;
		public static event ChangeVolumeEventHandler ChangeVolume = null;
		public static event TVWFontChangeHandler FontChange = null;
		public static event AddToErrorLogEventHandler AddError = null;
		public static event GreyoutItemsEventHandler GreyoutItems = null;
		public static event NewDirectoryEventHandler NewDirectoryAdded = null;
		public static event OpenFromTreeViewEventHandler OpenFromTreeView = null;
		public static event MemoryUseUpdateEventHandler MemoryUseUpdate = null;
		public static event WaveSampleCountUpdateEventHandler WaveSampleCountUpdate = null;
		public static event MDIChildModifiedEventHandler MDIChildModified = null;
		public static event GetColorDataEventHandler TVWColorData = null;
		//public static event GetColorDataEventHandler TVWFontData = null;
		public static event GetColorDataEventHandler TVWFontColorData = null;
		public static event AddMDIChildEventHandler AddMDIChild = null;
		public static event DeleteMDIChildEventHandler DeleteMDIChild = null;
		public static event TVWBackgroundColorChangeHandler TVWBackgroundColor = null;

		public static void DoResizePicBox(string ChildName)
		{
			ResizePicBox(ChildName);
		}

		public static void DoChangeVolume(ushort CalcVol)
		{
			ChangeVolume(CalcVol);
		}
		public static void DoTVWBackgroundColor()
		{
			TVWBackgroundColor();
		}
		public static void DoFontChange()
		{
			FontChange();
		}

		public static void DoGreyoutItems(string item)
		{
			GreyoutItems(item);
		}

		public static void AddNewError()
		{
			AddError();
		}

		public static void DoDeleteMDIChild(string ChildName)
		{
			DeleteMDIChild(ChildName);
		}

		public static void AddNewMDIChild(string ChildName)
		{
			AddMDIChild(ChildName);
		}

		public static string ShareTVWColorData()
		{
			string AColor = TVWColorData();
			return AColor;
		}

		public static string ShareTVWFontColorData()
		{
			string AColor = TVWFontColorData();
			return AColor;
		}

		//public static string ShareTVWFontData()
		//{

		//}

		public static Wave MDIChildHasBeenModified(Wave ChangedWave)
		{
			MDIChildModified(ChangedWave);
			return ChangedWave;
		}

		public static void NewDirectoryHasBeenAdded(String NewDirectory)
		{
			WaveManager.ActiveDirectory = NewDirectory;
			NewDirectoryAdded(NewDirectory);
		}

		public static void OpenWaveFromTreeView(String Path)
		{
			OpenFromTreeView(Path);
		}

		public static void UpdateMemoryUse(double MemoryUse)
		{
			MemoryUseUpdate(MemoryUse);
		}

		public static void UpdateWaveSampleCount(int Wave, int Sample)
		{
			WaveSampleCountUpdate(Wave, Sample);
		}

	}
}
