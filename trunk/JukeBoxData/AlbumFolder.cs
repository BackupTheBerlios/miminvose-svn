using System.Collections;
using System.Drawing;

namespace JukeBoxData
{
	public class AlbumFolderCollection
	{
		private static Hashtable _albumfolders = new Hashtable();

		public static AlbumFolder AlbumFolder(string path)
		{
			AlbumFolder folder = (AlbumFolder)_albumfolders[path];

			if (folder==null)
			{
				folder = new AlbumFolder(path);
				_albumfolders.Add(path,folder);
			}

			return folder;
		}
	}

	public class AlbumFolder
	{
		private string _path;
		private string _imagepath;
		private bool _imageretrieved = false;
		private Image _image;

		public AlbumFolder(string path)
		{
			_path = path;
			_imagepath = _path + @"\Folder.jpg";
		}

		public string Path
		{
			get { return _path; }
		}

		public Image AlbumImage
		{
			get
			{
				if (!_imageretrieved)
				{
					if (System.IO.File.Exists(_imagepath))
					{
						_image = System.Drawing.Image.FromFile(_imagepath);
					}
					_imageretrieved = true;
				}
				return _image;
			}
		}
	}
}
