using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveInfoModel;

namespace WaveBusiness
{
	public static class WaveManager
	{
		public static Wave testwave;

		public static List<string> ErrorLog = new List<string>();
		
		public static Wave _wave = null;

		public static WaveList WaveList = new WaveList();

		// Active directory
		public static string ActiveDirectory = "";

		// Current filename
		public static string CurrFileName = "";

		public static string TreeViewHelperFile = "";

		public const int HeaderSize = 40;

		public static Wave Load(string filename)
		{
			Wave w = new Wave();
			try
			{
				BinaryReader br = new BinaryReader(File.OpenRead(filename));
				using (br)
				{
					w.Header = br.ReadBytes(HeaderSize);
					w.NumSamples = br.ReadInt32();
					w.Samples = br.ReadBytes(w.NumSamples); // w.Samples is a byte array
					w.WavePath = filename;
				}
			}
			catch(Exception e1)
			{
				AddToErrorLog(e1.Message);
				w = null;
			}
			return w;
		}

		public static void AddToErrorLog(string Error)
		{
			WaveManager.ErrorLog.Add(Error);
			UIContextManager.AddNewError();
		}

		public static byte[] ConvertBytesToWav(Wave aWave)
		{
			var temp = new List<byte>();
			temp.AddRange(aWave.Header);
			byte[] tempArr = BitConverter.GetBytes(aWave.NumSamples);
			temp.AddRange(tempArr);
			temp.AddRange(aWave.Samples);
			byte[] merged = temp.ToArray();
			return merged;
		}

		public static int FindWave(string filename)
		{
			int wavelistsize = WaveList.aWaveList.Count();
			int FoundIndex = -99999;
			for (int x = 0; x < wavelistsize; x++)
			{
				if (WaveList.aWaveList[x].WavePath == filename)
				{
					FoundIndex = x;
				}
			}
			return FoundIndex;
		}

		public static void DeleteWave(string filename)
		{
			try
			{
				int FoundIndex = FindWave(filename);
				WaveList.aWaveList.RemoveAt(FoundIndex);
			}
			catch (Exception e1)
			{
				WaveManager.AddToErrorLog(e1.Message);
			}

		}

		public static void RotateWave(string filename)
		{
			int FoundIndex = FindWave(filename);
			if (WaveList.aWaveList[FoundIndex].Modified == 0)
			{
				Array.Reverse(WaveList.aWaveList[FoundIndex].BackupWave.Samples);
				WaveList.aWaveList[FoundIndex].Samples = WaveList.aWaveList[FoundIndex].BackupWave.Samples;
			}
			else
			{
				Array.Reverse(WaveList.aWaveList[FoundIndex].Samples);
			}
		}

		public static Wave RotateWave(Wave aWave)
		{
			byte[] ToBeFlipped = aWave.Samples;
			Array.Reverse(ToBeFlipped);
			aWave.Samples = ToBeFlipped;

			return aWave;
		}

		public static void WaveModified(string filename)
		{
		}

	}


}
