namespace JukeBoxData
{
	public class Track
	{
		public ushort _number;
		public string _title;
		public string _artist;
		public string _albumartist;
		public string _album;
		public string _albumkey;
		public string _url;
		public double _duration;

		public Track() {}

		public ushort TrackNo
		{
			get { return _number; }
			set { _number = value; }
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public string Artist
		{
			get { return _artist; }
			set { _artist = value; }
		}

		public string AlbumArtist
		{
			get { return _albumartist; }
			set { _albumartist = value; }
		}

		public string Album
		{
			get { return _album; }
			set { _album = value; }
		}

		public string AlbumKey
		{
			get { return _albumkey; }
			set { _albumkey = value; }
		}

		public double Duration
		{
			get { return _duration; }
			set { _duration = value; }
		}

		public string URL
		{
			get { return _url; }
			set { _url = value; }
		}

		public string Key
		{
			get { return string.Format("{0} ({1})",Title,Artist); }
		}

		public string Folder
		{
			get
			{
				string url = URL;
				if (url==null) return null;
				int index = URL.LastIndexOf(@"\");
				if (index>0)
				{
					return URL.Substring(0,index);
				}
				return null;
			}
		}
	}
}
