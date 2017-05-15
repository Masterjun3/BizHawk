using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BizHawk.Client.EmuHawk.tools.MovieEditor
{
	public partial class MovieEditor : ToolFormBase, IToolFormAutoConfig
	{
		public MovieEditor()
		{
			InitializeComponent();
		}

		public bool UpdateBefore
		{
			get { return false; }
		}

		public bool AskSaveChanges()
		{
			return true;
		}

		public void FastUpdate()
		{
			return;
		}

		public void NewUpdate(ToolFormUpdateType type)
		{
			return;
		}

		public void Restart()
		{
			return;
		}

		public void UpdateValues()
		{
			return;
		}
	}
}
