using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveInfoModel
{
	[Serializable]
	public class Wave
	{

		private byte[] _header;
		private int _numSamples;
		private byte[] _samples;

		private int _modified = 0;

		public int Modified
		{
			get { return _modified; }
			set { _modified = value; }
		}

		private bool _isfull = false;

		public bool IsFull
		{
			get { return _isfull; }
			set { _isfull = value; }
		}

		public int WidthProportion;

		private List<int> _widthHelper;

		public List<int> WidthHelper
		{
			get { return _widthHelper; }
			set { _widthHelper = value; }
		}
		

		private static Wave _backupwave =null;

		public Wave BackupWave
		{
			get { return _backupwave; }
			set { _backupwave = value; }
		}
		

		private string _wavepath;

		public string WavePath
		{
			get { return _wavepath; }
			set { _wavepath = value; }
		}


		public byte[] Header
		{
			get { return _header; }
			set { _header = value; }
		}

		public int NumSamples
		{
			get { return _numSamples; }
			set { _numSamples = value; }
		}

		public byte[] Samples
		{
			get { return _samples; }
			set { _samples = value; }
		}

		//private byte[] data;

		//public byte[] Data
		//{
		//	get { return data; }
		//	set { data = value; }
		//}

		//private int sampleSize;

		//public int SampleSize
		//{
		//	get { return sampleSize; }
		//	set { sampleSize = value; }
		//}


	}
}
