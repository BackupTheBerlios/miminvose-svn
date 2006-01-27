using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using AxMicrosoft.MediaPlayer.Interop;
using Microsoft.MediaPlayer.Interop;
using JukeBoxData;
using JukeBoxControls;

namespace JukeBox
{
	public class JukeBox : System.Windows.Forms.Form
	{
		//private static readonly int INITIALQUEUESIZE = 5; // Initial number of randomly selected tracks to throw into the queue
		private const string STOPPEDSTATUS = "Stopped";

		private enum State
		{
			Default,
			ArtistSearch,
			AlbumSearch,
			TrackSearch,
			StringListSearchComplete,
			TrackSearchComplete,
			Shutdown
		}

		private Random _random = new Random();

		private State _state = State.Default;

		private Queue<Track> _default = new Queue<Track>();
		private Queue<Track> _requests = new Queue<Track>();
		private Library _library;

		private Label _selected;
		private List<Label> _labels = new List<Label>();

		private StringBuilder _buffer = new StringBuilder();

		private Track _currenttrack;

		// Menu option labels

		// Main panel

		// Menu option panels

		private System.Windows.Forms.Timer timerStartSong;
		private Timer timerUpdateDisplay;
		private Timer timerLoadLibrary;
		private Panel panelAll;
		private TrackListControl trackListControl;
		private StringListControl stringListControl;
		private AxWindowsMediaPlayer Player;
		private Label lblCriteria;
		private Label lblSearchTrack;
		private Label lblSearchAlbum;
		private Label lblSearchArtist;
		private Label lblNowPlaying;
		private TrackControl trackControl;
		private ComponentScience.ElementsEx.LedLabel lblTime;
		private Label lblImportStatus;

		private System.ComponentModel.IContainer components;

		public JukeBox()
		{
			InitializeComponent();

			Constants.SetMainFormPresentation(this);

			_labels.Add(lblNowPlaying);
			_labels.Add(lblSearchAlbum);
			_labels.Add(lblSearchArtist);
			_labels.Add(lblSearchTrack);
			InitialiseLabels();

			Constants.SetLabelPresentation(lblCriteria);

			Load+=new EventHandler(Form1_Load);
			KeyPress+=new KeyPressEventHandler(Form1_KeyPress);
			KeyUp+=new KeyEventHandler(Form1_KeyUp);

			SetSelected(lblNowPlaying);

			//Constants.SetPanelPresentation(panelAll);
			//Constants.SetPanelPresentation(panelNowPlaying);
			//Constants.SetPanelPresentation(panelSearch);

			Player.StatusChange += new EventHandler(Player_StatusChange);

			IWMPPlaylist allitems = Player.mediaCollection.getAll();

			//for (int i = 0; i < INITIALQUEUESIZE; i++)
			//{
			//    Track track = null;
			//    track = Utility.GetRandomTrack(_random,allitems);
			//    if (track != null)_default.Enqueue(track);
			//}

			_library = new Library(allitems);

			stringListControl.StringSelected+=new StringSelectedEventHandler(stringListControl_StringSelected);
			trackListControl.TrackSelected+=new TrackSelectedEventHandler(trackListControl_TrackSelected);
			trackControl.Readonly = true;
			trackListControl.ReadOnly = true;

			SetState(State.Default);

			timerStartSong.Enabled = true;
			timerLoadLibrary.Enabled = true;
			timerUpdateDisplay.Enabled = true;
		}

		private List<Track> QueuedTracks
		{
			get
			{
				List<Track> list = new List<Track>();
				foreach (Track track in _requests) list.Add(track);
				foreach (Track track in _default) list.Add(track);
				return list;
			}
		}

		private void UpdateQueueDisplay()
		{
			if(_state==State.Default)
			{
				trackListControl.ReadOnly = true;
				trackListControl.Tracks = QueuedTracks;
				trackListControl.Visible = true;
			}
		}

		private Track GetNext()
		{
			Track track = null;

			while((track==null)||(!System.IO.File.Exists(track.URL)))
			{
				if (_requests.Count > 0) track = (Track)_requests.Dequeue();
				else if (_default.Count > 0) track = (Track)_default.Dequeue();
				else track = Utility.GetRandomTrack(_random,Player.mediaCollection.getAll());
			}

			return track;
		}

		private void PlayNext()
		{
			_currenttrack = GetNext();
			if (_currenttrack == null)
			{
				MessageBox.Show("Couldn't find any tracks to play");
				return;
			}
			Player.URL = _currenttrack.URL;
			Player.Ctlcontrols.play();
			trackControl.Track = _currenttrack;
			UpdateQueueDisplay();
		}

		private void InitialiseLabels()
		{
			foreach(Label l in _labels) { InitialiseLabel(l); }
		}

		private void InitialiseLabel(Label label)
		{
			Constants.SetLabelPresentation(label);
			label.Click+=new EventHandler(label_Click);
			label.MouseEnter+=new EventHandler(label_MouseEnter);
			label.MouseLeave+=new EventHandler(label_MouseLeave);
		}

		private void SetState(State state)
		{
			_state = state;
			stringListControl.Visible = false;
			trackListControl.Visible = false;
			switch(state)
			{
				case State.AlbumSearch:
					lblCriteria.Visible = true;
					break;
				case State.ArtistSearch:
					lblCriteria.Visible = true;
					break;
				case State.TrackSearch:
					lblCriteria.Visible = true;
					break;
				case State.Default:
					lblCriteria.Visible = false;
					trackListControl.ReadOnly = true;
					UpdateQueueDisplay();
					trackListControl.Visible = true;
					break;
				case State.StringListSearchComplete:
					stringListControl.Visible = true;
					break;
				case State.TrackSearchComplete:
					trackListControl.Visible = true;
					break;
			}
		}

		private void SetSelected(Label label)
		{
			_buffer = new StringBuilder();
			lblCriteria.Text = string.Empty;
			foreach(Label l in _labels) { Constants.SetLabelPresentation(l);}
			_selected = label;
			Constants.SetLabelPresentationSelected(label);
			if (label==lblNowPlaying) SetState(State.Default);
			if (label==lblSearchArtist) SetState(State.ArtistSearch);
			if (label==lblSearchAlbum) SetState(State.AlbumSearch);
			if (label==lblSearchTrack) SetState(State.TrackSearch);
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JukeBox));
			this.timerStartSong = new System.Windows.Forms.Timer(this.components);
			this.timerUpdateDisplay = new System.Windows.Forms.Timer(this.components);
			this.timerLoadLibrary = new System.Windows.Forms.Timer(this.components);
			this.panelAll = new System.Windows.Forms.Panel();
			this.lblImportStatus = new System.Windows.Forms.Label();
			this.trackListControl = new JukeBoxControls.TrackListControl();
			this.stringListControl = new JukeBoxControls.StringListControl();
			this.Player = new AxMicrosoft.MediaPlayer.Interop.AxWindowsMediaPlayer();
			this.lblCriteria = new System.Windows.Forms.Label();
			this.lblSearchTrack = new System.Windows.Forms.Label();
			this.lblSearchAlbum = new System.Windows.Forms.Label();
			this.lblSearchArtist = new System.Windows.Forms.Label();
			this.lblNowPlaying = new System.Windows.Forms.Label();
			this.trackControl = new JukeBoxControls.TrackControl();
			this.lblTime = new ComponentScience.ElementsEx.LedLabel();
			this.panelAll.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Player)).BeginInit();
			this.SuspendLayout();
			// 
			// timerStartSong
			// 
			this.timerStartSong.Tick += new System.EventHandler(this.timerStart_Tick);
			// 
			// timerUpdateDisplay
			// 
			this.timerUpdateDisplay.Tick += new System.EventHandler(this.timerUpdateDisplay_Tick);
			// 
			// timerLoadLibrary
			// 
			this.timerLoadLibrary.Interval = 1000;
			this.timerLoadLibrary.Tick += new System.EventHandler(this.timerLoadLibrary_Tick);
			// 
			// panelAll
			// 
			this.panelAll.Controls.Add(this.lblImportStatus);
			this.panelAll.Controls.Add(this.trackListControl);
			this.panelAll.Controls.Add(this.stringListControl);
			this.panelAll.Controls.Add(this.Player);
			this.panelAll.Controls.Add(this.lblCriteria);
			this.panelAll.Controls.Add(this.lblSearchTrack);
			this.panelAll.Controls.Add(this.lblSearchAlbum);
			this.panelAll.Controls.Add(this.lblSearchArtist);
			this.panelAll.Controls.Add(this.lblNowPlaying);
			this.panelAll.Controls.Add(this.trackControl);
			this.panelAll.Controls.Add(this.lblTime);
			this.panelAll.Location = new System.Drawing.Point(0, 1);
			this.panelAll.Name = "panelAll";
			this.panelAll.Size = new System.Drawing.Size(800, 600);
			this.panelAll.TabIndex = 0;
			// 
			// lblImportStatus
			// 
			this.lblImportStatus.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblImportStatus.Location = new System.Drawing.Point(23, 95);
			this.lblImportStatus.Name = "lblImportStatus";
			this.lblImportStatus.Size = new System.Drawing.Size(178, 33);
			this.lblImportStatus.TabIndex = 54;
			this.lblImportStatus.Text = "Current Queue";
			this.lblImportStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// trackListControl
			// 
			this.trackListControl.Location = new System.Drawing.Point(230, 149);
			this.trackListControl.Name = "trackListControl";
			this.trackListControl.Size = new System.Drawing.Size(567, 434);
			this.trackListControl.TabIndex = 53;
			// 
			// stringListControl
			// 
			this.stringListControl.Location = new System.Drawing.Point(228, 149);
			this.stringListControl.Name = "stringListControl";
			this.stringListControl.Size = new System.Drawing.Size(569, 434);
			this.stringListControl.TabIndex = 52;
			// 
			// Player
			// 
			this.Player.Enabled = true;
			this.Player.Location = new System.Drawing.Point(0, 0);
			this.Player.Name = "Player";
			this.Player.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Player.OcxState")));
			this.Player.Size = new System.Drawing.Size(35, 34);
			this.Player.TabIndex = 51;
			this.Player.Visible = false;
			// 
			// lblCriteria
			// 
			this.lblCriteria.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblCriteria.Location = new System.Drawing.Point(228, 95);
			this.lblCriteria.Name = "lblCriteria";
			this.lblCriteria.Size = new System.Drawing.Size(569, 51);
			this.lblCriteria.TabIndex = 50;
			this.lblCriteria.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblCriteria.UseMnemonic = false;
			// 
			// lblSearchTrack
			// 
			this.lblSearchTrack.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblSearchTrack.Location = new System.Drawing.Point(0, 480);
			this.lblSearchTrack.Name = "lblSearchTrack";
			this.lblSearchTrack.Size = new System.Drawing.Size(220, 37);
			this.lblSearchTrack.TabIndex = 49;
			this.lblSearchTrack.Text = "Track Search";
			this.lblSearchTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSearchAlbum
			// 
			this.lblSearchAlbum.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblSearchAlbum.Location = new System.Drawing.Point(0, 369);
			this.lblSearchAlbum.Name = "lblSearchAlbum";
			this.lblSearchAlbum.Size = new System.Drawing.Size(224, 42);
			this.lblSearchAlbum.TabIndex = 48;
			this.lblSearchAlbum.Text = "Album Search";
			this.lblSearchAlbum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSearchArtist
			// 
			this.lblSearchArtist.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblSearchArtist.Location = new System.Drawing.Point(1, 263);
			this.lblSearchArtist.Name = "lblSearchArtist";
			this.lblSearchArtist.Size = new System.Drawing.Size(221, 40);
			this.lblSearchArtist.TabIndex = 47;
			this.lblSearchArtist.Text = "Artist Search";
			this.lblSearchArtist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblNowPlaying
			// 
			this.lblNowPlaying.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblNowPlaying.Location = new System.Drawing.Point(1, 149);
			this.lblNowPlaying.Name = "lblNowPlaying";
			this.lblNowPlaying.Size = new System.Drawing.Size(222, 40);
			this.lblNowPlaying.TabIndex = 46;
			this.lblNowPlaying.Text = "Current Queue";
			this.lblNowPlaying.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// trackControl
			// 
			this.trackControl.BackColor = System.Drawing.Color.Black;
			this.trackControl.Location = new System.Drawing.Point(235, 0);
			this.trackControl.Name = "trackControl";
			this.trackControl.Size = new System.Drawing.Size(509, 92);
			this.trackControl.TabIndex = 45;
			this.trackControl.Track = null;
			// 
			// lblTime
			// 
			this.lblTime.BackColor = System.Drawing.Color.Black;
			this.lblTime.Columns = 5;
			this.lblTime.ForeColor = System.Drawing.Color.GreenYellow;
			this.lblTime.Location = new System.Drawing.Point(46, 11);
			this.lblTime.Name = "lblTime";
			this.lblTime.Segment.Size = 3;
			this.lblTime.Size = new System.Drawing.Size(174, 64);
			this.lblTime.TabIndex = 44;
			this.lblTime.Text = "88:88";
			// 
			// JukeBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(949, 641);
			this.ControlBox = false;
			this.Controls.Add(this.panelAll);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.Name = "JukeBox";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "JukeBox";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.panelAll.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.Player)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void label_MouseEnter(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			if (label==_selected) return;
			Constants.SetLabelPresentationHover(label);
		}

		private void label_MouseLeave(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			if (label==_selected) return;
			Constants.SetLabelPresentation(label);
		}

		private void label_Click(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			SetSelected(label);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			panelAll.Top = (Height - panelAll.Height) / 2;
			panelAll.Left = (Width - panelAll.Width) / 2;
		}

		private void Player_StatusChange(object sender, EventArgs e)
		{
			string status = Player.status.Trim();
			if (status.Equals(STOPPEDSTATUS))
			{
				_currenttrack = null;
				UpdateTime();
				timerStartSong.Enabled = true;
			}
		}

		private void timerStart_Tick(object sender, System.EventArgs e)
		{
			PlayNext();
			timerStartSong.Enabled = false;
		}

		private void timerUpdateDisplay_Tick(object sender, System.EventArgs e)
		{
			if (_library.Index + 1 == _library.Total) lblImportStatus.Visible = false;
			else lblImportStatus.Text = string.Format("{0}/{1}", _library.Index, _library.Total);
			UpdateTime();
		}

		private void UpdateTime()
		{
			//if (_state == State.Shutdown) return;
			if (_currenttrack==null)
			{
				lblTime.Text = string.Empty;
				return;
			}
			// TODO: Allow configuration for which time to display.
			int remaining = (int)(_currenttrack.Duration - Player.Ctlcontrols.currentPosition);
			lblTime.Text = string.Format("{0:00}:{1:00}",(remaining/60),(remaining%60));
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (_state==State.StringListSearchComplete) return;
			if (_state==State.TrackSearchComplete) return;
			if (char.IsLetter(e.KeyChar)||char.IsPunctuation(e.KeyChar)||char.IsSeparator(e.KeyChar))
			{
				e.Handled = true;
				_buffer.Append(e.KeyChar);
				lblCriteria.Text = _buffer.ToString();
			}
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case System.Windows.Forms.Keys.Enter:
					string command = _buffer.ToString();
					_buffer = new StringBuilder();
					switch(_state)
					{
						case State.Default:
						switch(command)
						{
							case "h":
								Player.Visible = !Player.Visible;
								break;
							case "p":
								string status = Player.status.Trim();
								if (status.StartsWith("Playing")) Player.Ctlcontrols.pause();
								else if (status.StartsWith("Paused")) Player.Ctlcontrols.play();
								break;
							case "kill":
								Player.Ctlcontrols.stop();
								break;
							case "die":
								_library.Stop();
								timerUpdateDisplay.Enabled = false;
								Player.Ctlcontrols.stop();
								Close();
								_state = State.Shutdown;
								break;
							//case "build":
							//    _tracks = Utility.BuildTrackCollection(Player.mediaCollection.getAll());
							//    //Utility.SaveTrackCollection(_tracks);
							//    break;
							default:
								IWMPPlaylistArray playlists = Player.playlistCollection.getByName(command);
								if (playlists.count>0)
								{
									Utility.Randomise(playlists.Item(0),_default);
									UpdateQueueDisplay();
								}
								break;
						}
							break;
						case State.ArtistSearch:
							List<string>artists = _library.Tracks.GetArtists(lblCriteria.Text);
							if (artists.Count == 0) SetState(State.ArtistSearch);
							else
							{
								stringListControl.Strings = artists;
								SetState(State.StringListSearchComplete);
							}
							break;
						case State.AlbumSearch:
							List<string> albums = _library.Tracks.GetAlbums(lblCriteria.Text);
							if (albums.Count==0) SetState(State.AlbumSearch);
							else
							{
								stringListControl.Strings = albums;
								SetState(State.StringListSearchComplete);
							}
							break;
						case State.TrackSearch:
							List<Track>tracks = _library.Tracks.GetTracks(lblCriteria.Text);
							if (tracks.Count==0) SetState(State.TrackSearch);
							else
							{
								trackListControl.ReadOnly = false;
								trackListControl.Tracks = tracks;
								SetState(State.TrackSearchComplete);
							}
							break;
						case State.StringListSearchComplete:
							ShowStringSelectResults(stringListControl.Selected);
							break;
						case State.TrackSearchComplete:
							Track track = trackListControl.Selected;
							if (track!=null)
							{
								_requests.Enqueue(track);
								trackListControl.RemoveSelected();
							}
							break;
					}
					break;
				case System.Windows.Forms.Keys.Delete:
				case System.Windows.Forms.Keys.Back:
					switch(_state)
					{
						case State.AlbumSearch:
						case State.ArtistSearch:
						case State.TrackSearch:
							if (_buffer.Length>0) _buffer.Remove(_buffer.Length-1,1);
							lblCriteria.Text = _buffer.ToString();
							break;
					}

					break;
				case System.Windows.Forms.Keys.Escape:
					_buffer = new StringBuilder();
					switch(_state)
					{
						case State.StringListSearchComplete:
							lblCriteria.Text = string.Empty;
							if(_selected==lblSearchAlbum) SetState(State.AlbumSearch);
							if(_selected==lblSearchArtist) SetState(State.ArtistSearch);
							break;
						case State.TrackSearchComplete:
							if(_selected==lblSearchTrack) SetState(State.TrackSearch);
							else SetState(State.StringListSearchComplete);
							break;
						case State.AlbumSearch:
						case State.ArtistSearch:
						case State.TrackSearch:
							lblCriteria.Text = string.Empty;
							SetSelected(lblNowPlaying);
							break;
					}
					break;
				case System.Windows.Forms.Keys.Down:
					switch(_state)
					{
						case State.Default:
							SetSelected(lblSearchArtist);
							break;
						case State.ArtistSearch:
							SetSelected(lblSearchAlbum);
							break;
						case State.AlbumSearch:
							SetSelected(lblSearchTrack);
							break;
						case State.TrackSearch:
							SetSelected(lblNowPlaying);
							break;
						case State.StringListSearchComplete:
							stringListControl.NextItem();
							break;
						case State.TrackSearchComplete:
							trackListControl.NextItem();
							break;
					}
					break;
				case System.Windows.Forms.Keys.Up:
					switch(_state)
					{
						case State.Default:
							SetSelected(lblSearchTrack);
							break;
						case State.ArtistSearch:
							SetSelected(lblNowPlaying);
							break;
						case State.AlbumSearch:
							SetSelected(lblSearchArtist);
							break;
						case State.TrackSearch:
							SetSelected(lblSearchAlbum);
							break;
						case State.StringListSearchComplete:
							stringListControl.PreviousItem();
							break;
						case State.TrackSearchComplete:
							trackListControl.PreviousItem();
							break;
					}
					break;
				case System.Windows.Forms.Keys.Left:
					if (_state==State.StringListSearchComplete) stringListControl.PreviousPage();
					if (_state==State.TrackSearchComplete) trackListControl.PreviousPage();
					if (_state==State.Default) trackListControl.PreviousPage();
					break;
				case System.Windows.Forms.Keys.Right:
					if (_state==State.StringListSearchComplete) stringListControl.NextPage();
					if (_state==State.TrackSearchComplete) trackListControl.NextPage();
					if (_state==State.Default) trackListControl.NextPage();
					break;
			}
		}

		private void stringListControl_StringSelected(string selectedstring)
		{
			 ShowStringSelectResults(selectedstring);
		}

		private void ShowStringSelectResults(string selectedstring)
		{
			List<Track> results = null;

			if (_selected == lblSearchAlbum) results = _library.Tracks.GetTracksByAlbum(selectedstring);
			else if (_selected == lblSearchArtist) results = _library.Tracks.GetTracksByArtist(selectedstring);

			if ((results==null)||(results.Count==0)) return;

			trackListControl.ReadOnly = false;
			trackListControl.Tracks = results;

			SetState(State.TrackSearchComplete);
		}

		private void trackListControl_TrackSelected(Track selectedtrack)
		{
			_requests.Enqueue(selectedtrack);
		}

		private void timerLoadLibrary_Tick(object sender, EventArgs e)
		{
			timerLoadLibrary.Enabled = false;
			_library.Load();
			MessageBox.Show("Indexing complete");
		}
	}
}
