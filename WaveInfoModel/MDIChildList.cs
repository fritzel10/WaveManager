using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveInfoModel
{
	public class MDIChildList
	{
		private List<MDIChildObject> _mdichildrenlist = new List<MDIChildObject>();

		public List<MDIChildObject> MDIChildrenList
		{
			get { return _mdichildrenlist; }
			set { _mdichildrenlist = value; }
		}
		
	}
}
