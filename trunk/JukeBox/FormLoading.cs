using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JukeBox
{
	public partial class FormLoading : Form
	{
		uint _totaltracks;
		uint _tracks;

		public FormLoading(uint totaltracks)
		{
			InitializeComponent();
		}

		public uint Tracks
		{
			get { return _tracks; }
			set { _tracks = value; }
		}
	}
}