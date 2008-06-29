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
		private bool _started;
		private readonly TrackCollection _tracks;
		private readonly int _total;
		private int _index;
		private bool _shutdown;

		public Library(IWMPPlaylist playlist)
		{
			_baseplaylist = playlist;
			_tracks = new TrackCollection();
			_total = playlist.count;
		}

		public void Load()
		{
			ImportTrackCollection();
		}

		public bool Started
		{
			get { return _started; }
		}

		public int Total
		{
			get { return _total; }
		}

		public int Index
		{
			get { return _index; }
		}

		public void Stop()
		{
			_shutdown = true;
		}

		private void ImportTrackCollection()
		{
			for (var i = 0; i < _baseplaylist.count; i++)
			{
				if (_shutdown) break;
				var track = Utility.CreateTrack(_baseplaylist.get_Item(i));
				if (track != null)
				{
					_tracks.Add(track);
					_started = true;
				}
				if (i % 100 == 0) Application.DoEvents();
				_index = i;
			}
		}

		public TrackCollection Tracks
		{
			get { return _tracks; }
		}
	}

	public static class Utility
	{
		public static Track GetRandomTrack(Random random,IWMPPlaylist list)
		{
			Track track = null;
			var attempts = 0;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
			while ((track == null) || (!System.IO.File.Exists(track.URL)))
// ReSharper restore ConditionIsAlwaysTrueOrFalse
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
//			if (item.name.Equals("jo44"))
//			{
//				for(int i=0;i<item.attributeCount;i++)
//				{
//					string name = item.getAttributeName(i);
//					string att = item.getItemInfo(name);
//					Console.WriteLine(string.Format("{0}={1}",name,att));
//				}
//			}

			var mediatype = item.getItemInfo("MediaType");
			if (!mediatype.Equals("audio")) return null;

			var track = new Track {Title = item.name};
		    ushort index = 0;
			try
			{
				index = ushort.Parse(item.getItemInfo("WM/TrackNumber"));
// ReSharper disable EmptyGeneralCatchClause
			} catch (Exception) {}
// ReSharper restore EmptyGeneralCatchClause
			track.TrackNo = index;
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
