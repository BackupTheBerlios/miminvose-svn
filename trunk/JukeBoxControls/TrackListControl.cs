using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JukeBoxData;

namespace JukeBoxControls
{
	public delegate void TrackSelectedEventHandler(Track selectedtrack);

	public class TrackListControl : System.Windows.Forms.UserControl
	{
		private List<Track> _tracks;
		private int _currentindex = 0;
		private TrackControl _selected;
		private List<TrackControl> _resultcontrols = new List<TrackControl>();
		private bool _readonly;

		public event TrackSelectedEventHandler TrackSelected;

		private System.Windows.Forms.Label lblNext;
		private System.Windows.Forms.Label lblPrevious;

		private TrackControl userControlTrack6;
		private TrackControl userControlTrack5;
		private TrackControl userControlTrack4;
		private TrackControl userControlTrack3;
		private TrackControl userControlTrack2;
		private TrackControl userControlTrack1;
		private System.Windows.Forms.Label lblSummary;

		private System.ComponentModel.Container components = null;

		public TrackListControl()
		{
			InitializeComponent();

			_resultcontrols.Add(userControlTrack1);
			_resultcontrols.Add(userControlTrack2);
			_resultcontrols.Add(userControlTrack3);
			_resultcontrols.Add(userControlTrack4);
			_resultcontrols.Add(userControlTrack5);
			_resultcontrols.Add(userControlTrack6);

			InitialiseResultControls();

			Constants.SetLabelPresentation(lblSummary);

			Constants.SetLabelPresentation(lblNext);
			lblNext.MouseEnter+=new EventHandler(label_MouseEnter);
			lblNext.MouseLeave+=new EventHandler(label_MouseLeave);

			Constants.SetLabelPresentation(lblPrevious);
			lblPrevious.MouseEnter+=new EventHandler(label_MouseEnter);
			lblPrevious.MouseLeave+=new EventHandler(label_MouseLeave);
		}

		private void InitialiseResultControls()
		{
			foreach(TrackControl control in _resultcontrols) { InitialiseResultControl(control);}
		}

		private void InitialiseResultControl(TrackControl control)
		{
			control.TrackClicked+=new EventHandler(control_TrackClicked);
			control.TrackDoubleClicked+=new EventHandler(control_TrackDoubleClicked);
		}

		private void label_MouseEnter(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			Constants.SetLabelPresentationHover(label);
		}

		private void label_MouseLeave(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			Constants.SetDetailLabelPresentation(label);
		}

		public Track Selected
		{
			get
			{
				if (_selected==null) return null;
				return _selected.Track;
			}
		}

		public void RemoveSelected()
		{
			_tracks.Remove(_selected.Track);
			if (!userControlTrack2.Visible) PreviousPage();
			else DisplayResults();
		}

		public bool ReadOnly
		{
			set
			{
				_readonly = value;
				foreach(TrackControl ctrl in _resultcontrols) { ctrl.Readonly = value;}
			}
		}

		public List<Track> Tracks
		{
			set
			{
				_tracks = value;
				_currentindex = 0;
				DisplayResults();
			}
		}

		private void SetSelected(TrackControl control)
		{
			foreach(TrackControl ctrl in _resultcontrols) { ctrl.Selected = false;}
			if (_readonly) return;
			_selected = control;
			control.Selected = true;
		}

		private void TrackSelectedNotify()
		{
			if(TrackSelected!=null)
			{
				try
				{
					TrackSelected(_selected.Track);
				} 
				catch {}
			}
		}

		public void NextItem()
		{
			TrackControl firstcontrol = (TrackControl)_resultcontrols[0];
	
			if (_selected==null) SetSelected(firstcontrol);

			int index = _resultcontrols.IndexOf(_selected);

			index++;

			if (index==_resultcontrols.Count)
			{
				string first = firstcontrol.Track.Title;
				NextPage();
				if (!firstcontrol.Track.Title.Equals(first)) SetSelected(firstcontrol);
			}
			else
			{
				TrackControl nextcontrol = (TrackControl)_resultcontrols[index];
				if (nextcontrol.Visible) SetSelected(nextcontrol);
			}
		}

		public void PreviousItem()
		{
			TrackControl lastcontrol = (TrackControl)_resultcontrols[_resultcontrols.Count-1];
			int index = _resultcontrols.IndexOf(_selected);

			index--;

			if (index<0)
			{
				string last = string.Empty;
				if (lastcontrol.Track!=null) last = lastcontrol.Track.Title;
				PreviousPage();
				if (!lastcontrol.Track.Title.Equals(last)) SetSelected(lastcontrol);
			}
			else
			{
				TrackControl previouscontrol = (TrackControl)_resultcontrols[index];
				SetSelected(previouscontrol);
			}
		}

		public void NextPage()
		{
			if ((_currentindex+_resultcontrols.Count)>=_tracks.Count) return;
			_currentindex+=_resultcontrols.Count;
			DisplayResults();
		}

		public void PreviousPage()
		{
			if ((_currentindex-_resultcontrols.Count)<0) _currentindex=0;
			else _currentindex-=_resultcontrols.Count;
			DisplayResults();
		}

		private void ClearResults()
		{
			foreach(TrackControl control in _resultcontrols) { control.Track = null; control.Visible = false; }
		}

		private void DisplayResults()
		{
			ClearResults();
			lblNext.Visible = true;
			lblPrevious.Visible = true;
			int currentindex = _currentindex;

			foreach(TrackControl control in _resultcontrols)
			{
				if (currentindex>=_tracks.Count) break;
				control.Readonly = _readonly;
				control.Track = (Track)_tracks[currentindex++];
				control.Visible = true;
			}

			lblPrevious.Visible = (currentindex>_resultcontrols.Count);
			lblNext.Visible = (currentindex+1<=_tracks.Count);

			if (currentindex==_currentindex) lblSummary.Text = string.Empty;
			else lblSummary.Text = string.Format("{0} to {1} of {2}",_currentindex+1,currentindex,_tracks.Count);

			if (_selected==null) SetSelected((TrackControl)_resultcontrols[0]);
			else if (!_selected.Visible) SelectLastVisible();
		}

		private void SelectLastVisible()
		{
			foreach(TrackControl control in _resultcontrols)
			{
				if (control.Visible) SetSelected(_selected = control);
			}
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblNext = new System.Windows.Forms.Label();
			this.lblPrevious = new System.Windows.Forms.Label();
			this.userControlTrack6 = new JukeBoxControls.TrackControl();
			this.userControlTrack5 = new JukeBoxControls.TrackControl();
			this.userControlTrack4 = new JukeBoxControls.TrackControl();
			this.userControlTrack3 = new JukeBoxControls.TrackControl();
			this.userControlTrack2 = new JukeBoxControls.TrackControl();
			this.userControlTrack1 = new JukeBoxControls.TrackControl();
			this.lblSummary = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblNext
			// 
			this.lblNext.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblNext.Location = new System.Drawing.Point(469, 378);
			this.lblNext.Name = "lblNext";
			this.lblNext.Size = new System.Drawing.Size(69, 40);
			this.lblNext.TabIndex = 17;
			this.lblNext.Text = ">";
			this.lblNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblNext.Click += new System.EventHandler(this.lblNext_Click);
			// 
			// lblPrevious
			// 
			this.lblPrevious.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblPrevious.Location = new System.Drawing.Point(0, 378);
			this.lblPrevious.Name = "lblPrevious";
			this.lblPrevious.Size = new System.Drawing.Size(64, 40);
			this.lblPrevious.TabIndex = 16;
			this.lblPrevious.Text = "<";
			this.lblPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblPrevious.Click += new System.EventHandler(this.lblPrevious_Click);
			// 
			// userControlTrack6
			// 
			this.userControlTrack6.Location = new System.Drawing.Point(0, 310);
			this.userControlTrack6.Name = "userControlTrack6";
			this.userControlTrack6.Size = new System.Drawing.Size(728, 56);
			this.userControlTrack6.TabIndex = 15;
			this.userControlTrack6.Track = null;
			// 
			// userControlTrack5
			// 
			this.userControlTrack5.Location = new System.Drawing.Point(0, 248);
			this.userControlTrack5.Name = "userControlTrack5";
			this.userControlTrack5.Size = new System.Drawing.Size(728, 56);
			this.userControlTrack5.TabIndex = 14;
			this.userControlTrack5.Track = null;
			// 
			// userControlTrack4
			// 
			this.userControlTrack4.Location = new System.Drawing.Point(0, 186);
			this.userControlTrack4.Name = "userControlTrack4";
			this.userControlTrack4.Size = new System.Drawing.Size(728, 56);
			this.userControlTrack4.TabIndex = 13;
			this.userControlTrack4.Track = null;
			// 
			// userControlTrack3
			// 
			this.userControlTrack3.Location = new System.Drawing.Point(0, 124);
			this.userControlTrack3.Name = "userControlTrack3";
			this.userControlTrack3.Size = new System.Drawing.Size(728, 56);
			this.userControlTrack3.TabIndex = 12;
			this.userControlTrack3.Track = null;
			// 
			// userControlTrack2
			// 
			this.userControlTrack2.Location = new System.Drawing.Point(0, 62);
			this.userControlTrack2.Name = "userControlTrack2";
			this.userControlTrack2.Size = new System.Drawing.Size(728, 56);
			this.userControlTrack2.TabIndex = 11;
			this.userControlTrack2.Track = null;
			// 
			// userControlTrack1
			// 
			this.userControlTrack1.Location = new System.Drawing.Point(0, 0);
			this.userControlTrack1.Name = "userControlTrack1";
			this.userControlTrack1.Size = new System.Drawing.Size(728, 56);
			this.userControlTrack1.TabIndex = 10;
			this.userControlTrack1.Track = null;
			// 
			// lblSummary
			// 
			this.lblSummary.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblSummary.Location = new System.Drawing.Point(83, 386);
			this.lblSummary.Name = "lblSummary";
			this.lblSummary.Size = new System.Drawing.Size(368, 24);
			this.lblSummary.TabIndex = 21;
			this.lblSummary.Text = "1 to 6";
			this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TrackListControl
			// 
			this.Controls.Add(this.lblSummary);
			this.Controls.Add(this.lblNext);
			this.Controls.Add(this.lblPrevious);
			this.Controls.Add(this.userControlTrack6);
			this.Controls.Add(this.userControlTrack5);
			this.Controls.Add(this.userControlTrack4);
			this.Controls.Add(this.userControlTrack3);
			this.Controls.Add(this.userControlTrack2);
			this.Controls.Add(this.userControlTrack1);
			this.Name = "TrackListControl";
			this.Size = new System.Drawing.Size(570, 421);
			this.ResumeLayout(false);

		}
		#endregion

		private void lblNext_Click(object sender, System.EventArgs e)
		{
			NextPage();
		}

		private void lblPrevious_Click(object sender, System.EventArgs e)
		{
			PreviousPage();
		}

		private void control_TrackClicked(object sender, EventArgs e)
		{
			if (_readonly) return;
			TrackControl control = (TrackControl)sender;
			if (_selected==control)
			{
				RemoveSelected();
				TrackSelectedNotify();
			}
			else SetSelected(control);
		}

		private void control_TrackDoubleClicked(object sender, EventArgs e)
		{
			if (_readonly) return;
			TrackControl control = (TrackControl)sender;
			SetSelected(control);
			RemoveSelected();
			TrackSelectedNotify();
		}
	}
}
