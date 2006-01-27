using System;
using System.Collections.Generic;

namespace JukeBoxData
{
	public class TrackCollection
	{
		public static bool IsEmpty(string s)
		{
			return ((s==null)||(s.Length==0));
		}

		private List<string> _artists = new List<string>();
		private List<Album> _albums = new List<Album>();
		private Dictionary<string, Album> _albummap = new Dictionary<string, Album>();
		private List<Track> _tracks = new List<Track>();

		public TrackCollection() {}

		public void Add(Track track)
		{
			lock (_tracks)
			{
				string artist = null;

				_tracks.Add(track);

				if (!IsEmpty(track.Artist))
				{
					artist = track.Artist;
					if (!_artists.Contains(artist)) _artists.Add(artist);
				}

				if (!IsEmpty(track.AlbumArtist))
				{
					artist = track.AlbumArtist;
					if (!_artists.Contains(artist)) _artists.Add(artist);
				}

				if (!IsEmpty(track.Album) && !IsEmpty(artist))
				{
					Album album = new Album();
					album.Artist = artist;
					album.Title = track.Album;
					track.AlbumKey = album.Key;

					if (_albummap.ContainsKey(album.Key)) album = _albummap[album.Key];
					else
					{
						_albummap[album.Key] = album;
						_albums.Add(album);
					}
					album.Tracks.Add(track);
				}
			}
		}

		public Track GetAnyTrack()
		{
			lock (_tracks)
			{
				Random random = new Random();
				return _tracks[random.Next(_tracks.Count)];
			}
		}

		public Album GetAlbumFromKey(string key)
		{
			lock (_tracks)
			{
				if (_albummap.ContainsKey(key)) return _albummap[key];
				return null;
			}
		}

		public List<string>GetArtists(string criteria)
		{
			lock (_tracks)
			{
				string lcriteria = null;
				if ((criteria != null) && (criteria.Length > 0))
				{
					lcriteria = criteria.ToLower();
				}
				if (lcriteria == null) return _artists;
				List<string> list = new List<string>();
				foreach (string artist in _artists)
				{
					if (artist.ToLower().IndexOf(lcriteria) >= 0) list.Add(artist);
				}
				return list;
			}
		}

		public List<string> GetAlbums(string criteria)
		{
			lock(_tracks)
			{
				List<string> list = new List<string>();
				string lcriteria = null;
				if (!IsEmpty(criteria)) lcriteria = criteria.ToLower();
				foreach (Album album in _albums)
				{
					if (lcriteria == null) list.Add(album.Key);
					else if (album.Key.ToLower().IndexOf(lcriteria) >= 0) list.Add(album.Key);
				}
				return list;
			}
		}

		public List<Track> GetTracks(string criteria)
		{
			lock (_tracks)
			{
				string lcriteria = null;
				if ((criteria != null) && (criteria.Length > 0))
				{
					lcriteria = criteria.ToLower();
				}
				if (lcriteria == null) return _tracks;
				List<Track> list = new List<Track>();
				foreach (Track track in _tracks)
				{
					if (track.Key.ToLower().IndexOf(lcriteria) >= 0) list.Add(track);
				}
				return list;
			}
		}

		public List<Track> GetTracksByArtist(string criteria)
		{
			lock (_tracks)
			{
				List<Track> list = new List<Track>();
				foreach (Track track in _tracks)
				{
					if (track.Artist.Equals(criteria)) list.Add(track);
					else if (track.AlbumArtist.Equals(criteria)) list.Add(track);
				}
				return list;
			}
		}

		public List<Track> GetTracksByAlbum(string albumkey)
		{
			lock (_tracks)
			{
				if (_albummap.ContainsKey(albumkey)) return _albummap[albumkey].Tracks;
				else return new List<Track>();
			}
		}
	}
}