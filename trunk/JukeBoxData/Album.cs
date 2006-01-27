using System.Collections.Generic;

namespace JukeBoxData
{
	public class Album
	{
		private string _title;
		private string _artist;
		private List<Track> _tracks = new List<Track>();

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

		public string Key
		{
			get { return string.Format("{0} ({1})",Title,Artist); }
		}

		public List<Track> Tracks
		{
			get { return _tracks; }
		}
	}
}
