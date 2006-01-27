using System;
using JukeBoxData;

namespace JukeBoxControls
{
	public class TrackControl : System.Windows.Forms.UserControl
	{
		private bool _positioned = false;
		private bool _selected = false;
		private bool _readonly;

		private Track _track;
		private System.Windows.Forms.Label lblArtist;
		private System.Windows.Forms.Label lblTrack;
		private System.Windows.Forms.Label lblAlbum;
		private System.Windows.Forms.PictureBox picAlbum;
		public event EventHandler TrackClicked;
		public event EventHandler TrackDoubleClicked;

		private System.ComponentModel.Container components = null;

		public bool Readonly
		{
			set
			{
				_readonly = value;
				CheckSelected();
			}	
		}

		public TrackControl()
		{
			InitializeComponent();
			Constants.SetDetailLabelPresentation(lblArtist);
			Constants.SetDetailLabelPresentation(lblAlbum);
			Constants.SetDetailLabelPresentation(lblTrack);
			Constants.SetPicturePresentation(picAlbum);
			lblTrack.MouseEnter+=new System.EventHandler(lblTrack_MouseEnter);
			lblTrack.MouseLeave+=new System.EventHandler(lblTrack_MouseLeave);
			lblArtist.Click+=new System.EventHandler(any_Click);
			lblTrack.Click+=new System.EventHandler(any_Click);
			lblAlbum.Click+=new System.EventHandler(any_Click);
			picAlbum.Click+=new System.EventHandler(any_Click);
			lblArtist.DoubleClick+=new System.EventHandler(any_DoubleClick);
			lblTrack.DoubleClick+=new System.EventHandler(any_DoubleClick);
			lblAlbum.DoubleClick+=new System.EventHandler(any_DoubleClick);
			picAlbum.DoubleClick+=new System.EventHandler(any_DoubleClick);
		}

		private void Position()
		{
			picAlbum.SetBounds(0,0,Height,Height);
			lblTrack.SetBounds(Height,0,Width-Height,Height/3);
			lblArtist.SetBounds(Height,Height/3,Width-Height,Height/3);
			lblAlbum.SetBounds(Height,(2*Height)/3,Width-Height,Height/3);
			_positioned = true;
		}

		private void CheckSelected()
		{
			if (_selected&&!_readonly)
			{
				Constants.SetLabelPresentationSelected(lblAlbum);
				Constants.SetLabelPresentationSelected(lblArtist);
				Constants.SetLabelPresentationSelected(lblTrack);
			}
			else
			{
				Constants.SetDetailLabelPresentation(lblArtist);
				Constants.SetDetailLabelPresentation(lblAlbum);
				Constants.SetDetailLabelPresentation(lblTrack);
			}
		}

		public bool MouseOver
		{
			set
			{
				if (_readonly)return;
				if (value)
				{
					Constants.SetLabelPresentationHover(lblAlbum);
					Constants.SetLabelPresentationHover(lblArtist);
					Constants.SetLabelPresentationHover(lblTrack);
				}
				else
				{
					CheckSelected();
				}
			}
		}

		public bool Selected
		{
			set
			{
				if (_readonly)return;
				_selected = value;
				CheckSelected();
			}
		}

		public Track Track
		{
			get { return _track; }
			set
			{
				if (!_positioned) Position();
				_track = value;
				if (_track==null)
				{
					lblTrack.Visible = false;
					lblAlbum.Visible = false;
					lblArtist.Visible = false;
					picAlbum.Image = null;
				}
				else
				{
					lblTrack.Text = string.Format("{0:00} {1}",_track.TrackNo,_track.Title);
					lblAlbum.Text = _track.Album;
					lblArtist.Text = _track.Artist;
					picAlbum.Image = AlbumFolderCollection.AlbumFolder(_track.Folder).AlbumImage;
					lblTrack.Visible = true;
					lblAlbum.Visible = true;
					lblArtist.Visible = true;
				}
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
			this.picAlbum = new System.Windows.Forms.PictureBox();
			this.lblArtist = new System.Windows.Forms.Label();
			this.lblTrack = new System.Windows.Forms.Label();
			this.lblAlbum = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// picAlbum
			// 
			this.picAlbum.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.picAlbum.Location = new System.Drawing.Point(8, 8);
			this.picAlbum.Name = "picAlbum";
			this.picAlbum.Size = new System.Drawing.Size(48, 40);
			this.picAlbum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picAlbum.TabIndex = 4;
			this.picAlbum.TabStop = false;
			// 
			// lblArtist
			// 
			this.lblArtist.Location = new System.Drawing.Point(64, 0);
			this.lblArtist.Name = "lblArtist";
			this.lblArtist.Size = new System.Drawing.Size(536, 24);
			this.lblArtist.TabIndex = 1;
			this.lblArtist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblArtist.UseMnemonic = false;
			// 
			// lblTrack
			// 
			this.lblTrack.Location = new System.Drawing.Point(64, 24);
			this.lblTrack.Name = "lblTrack";
			this.lblTrack.Size = new System.Drawing.Size(536, 24);
			this.lblTrack.TabIndex = 2;
			this.lblTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblTrack.UseMnemonic = false;
			// 
			// lblAlbum
			// 
			this.lblAlbum.Location = new System.Drawing.Point(64, 48);
			this.lblAlbum.Name = "lblAlbum";
			this.lblAlbum.Size = new System.Drawing.Size(536, 24);
			this.lblAlbum.TabIndex = 3;
			this.lblAlbum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblAlbum.UseMnemonic = false;
			// 
			// TrackControl
			// 
			this.Controls.Add(this.lblAlbum);
			this.Controls.Add(this.lblTrack);
			this.Controls.Add(this.lblArtist);
			this.Controls.Add(this.picAlbum);
			this.Name = "TrackControl";
			this.Size = new System.Drawing.Size(608, 72);
			this.ResumeLayout(false);

		}
		#endregion

		private void lblTrack_MouseEnter(object sender, System.EventArgs e)
		{
			MouseOver = true;
		}

		private void lblTrack_MouseLeave(object sender, System.EventArgs e)
		{
			MouseOver = false;
		}

		private void any_Click(object sender, System.EventArgs e)
		{
			if (TrackClicked!=null)
			{
				foreach(EventHandler handler in TrackClicked.GetInvocationList())
				{
					try
					{
						handler(this,null);
					}
					catch {}
				}
			}
		}

		private void any_DoubleClick(object sender, System.EventArgs e)
		{
			if (TrackDoubleClicked!=null)
			{
				foreach(EventHandler handler in TrackDoubleClicked.GetInvocationList())
				{
					try
					{
						handler(this,null);
					}
					catch {}
				}
			}
		}
	}
}