using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveInfoModel;

namespace WaveBusiness
{
	public static class MDIChildManager
	{
		private static MDIChildList _mdilist = new MDIChildList();

		public static MDIChildList MDIList
		{
			get { return _mdilist; }
			set { _mdilist = value; }
		}

		public static void AddMDIChild(string NewChildPath)
		{
			MDIChildObject NewChild = new MDIChildObject();
			NewChild.Path = NewChildPath;
			NewChild.Modified = false;
			_mdilist.MDIChildrenList.Add(NewChild);
		}

		public static bool IsMDIChildModified(string CheckChildPath)
		{
			int SearchResult = SearchForMDIChild(CheckChildPath);
			if (SearchResult != -99999)
			{
				return _mdilist.MDIChildrenList[SearchResult].Modified;
			}
			else { return false;  }
		}

		public static void MDIChildModified(string ModifiedChildPath, bool TrueFalse)
		{
			int SearchResult = SearchForMDIChild(ModifiedChildPath);
			if (SearchResult != -99999)
			{
				_mdilist.MDIChildrenList[SearchResult].Modified = TrueFalse;
				Console.WriteLine("MDI CHILD " + _mdilist.MDIChildrenList[SearchResult].Path + "MODIFIED? : " + _mdilist.MDIChildrenList[SearchResult].Modified);

			}

		}

		public static int SearchForMDIChild(string SearchChildPath)
		{
			int FoundIndex = -99999;
			for (int i = 0; i < _mdilist.MDIChildrenList.Count; i++)
			{
				if (_mdilist.MDIChildrenList[i].Path == SearchChildPath)
				{
					FoundIndex = i;
				}
			}
			Console.WriteLine("FOUNDINDEX for " + SearchChildPath + ": " + FoundIndex);
			return FoundIndex;
			
		}

	}
}
