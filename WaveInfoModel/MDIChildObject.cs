using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveInfoModel
{
	public class MDIChildObject
	{
		private string _path;

		public string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		private int myVar;

		public int MyProperty
		{
			get { return myVar; }
			set { myVar = value; }
		}
		

		private bool _modified = false;

		public bool Modified
		{
			get { return _modified; }
			set { _modified = value; }
		}
		
		
	}
}
