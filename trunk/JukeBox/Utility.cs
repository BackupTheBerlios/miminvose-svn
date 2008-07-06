using JukeBoxData;
using Microsoft.MediaPlayer.Interop;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JukeBox
{
	public class Library
	{
		private readonly IWMPPlaylist _baseplaylist;
	    private bool _shutdown;

        public bool Started { get; private set; }
        public int Total { get; private set; }
        public int Index { get; private set; }
        public TrackCollection Tracks { get; private set; }

		public Library(IWMPPlaylist playlist)
		{
			_baseplaylist = playlist;
			Tracks = new TrackCollection();
			Total = playlist.count;
		}

        public void Load()
        {
            for (var i = 0; i < _baseplaylist.count; i++)
            {
                if (_shutdown) break;
                var track = Utility.CreateTrack(_baseplaylist.get_Item(i));
                if (track != null)
                {
                    Tracks.Add(track);
                    Started = true;
                }
                if (i % 100 == 0) Application.DoEvents();
                Index = i;
            }
        }

	    public void Stop()
		{
			_shutdown = true;
		}
	}

	public static class Utility
	{
		public static Track GetRandomTrack(Random random,IWMPPlaylist list)
		{
			Track track;
			var attempts = 0;

			while (true)
			{
                track = CreateTrack(list.get_Item(random.Next(list.count)));
				if (track != null) break;
				if (attempts++ > 100) break;
			}

			return track;
		}

		public static void Randomise(IWMPPlaylist list,Queue<Track> queue)
		{
			queue.Clear();
			var random = new Random();
			var tracks = new List<Track>();
			for(var i=0;i<list.count;i++)
			{
                var t = CreateTrack(list.get_Item(i));
				if (t != null) tracks.Add(t);
			}
			while (tracks.Count > 0)
			{
				var index = random.Next(tracks.Count);
				queue.Enqueue(tracks[index]);
				tracks.RemoveAt(index);
			}
		}

		public static Track CreateTrack(IWMPMedia item)
		{
			var mediatype = item.getItemInfo("MediaType");
			if (!mediatype.Equals("audio")) return null;

			var track = new Track {Title = item.name};

		    ushort index;
			if (ushort.TryParse(item.getItemInfo("WM/TrackNumber"),out index)) track.TrackNo = index;
			
			track.Artist = item.getItemInfo("Author");
			track.AlbumArtist = item.getItemInfo("WM/AlbumArtist");
			if (IsEmpty(track.AlbumArtist)&&!IsEmpty(track.Artist)) track.AlbumArtist = track.Artist;
			if (IsEmpty(track.Artist)&&!IsEmpty(track.AlbumArtist)) track.Artist = track.AlbumArtist;
			track.Album = item.getItemInfo("WM/AlbumTitle");
			if (IsEmpty(track.Album)) track.Album = "Unknown";
			track.Duration = item.duration;
			track.URL = item.sourceURL;

			if (IsEmpty(track.Title)||IsEmpty(track.Artist)||IsEmpty(track.AlbumArtist)||IsEmpty(track.Album))
			{
				Console.WriteLine("Found empty string");
			}

			return track;
		}

		public static bool IsEmpty(string s)
		{
			return string.IsNullOrEmpty(s);
		}
	}
}
