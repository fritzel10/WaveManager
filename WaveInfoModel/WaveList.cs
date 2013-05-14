using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveInfoModel
{
	public class WaveList
	{
		private List<Wave> _awavelist = new List<Wave>();

		public List<Wave> aWaveList
		{
			get { return _awavelist; }
			set { _awavelist = value; }
		}
		

	}
}
