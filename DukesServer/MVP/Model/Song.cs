using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DukesServer.MVP.Model
{
	partial class Song
	{
		public Song()
		{
		}


		public Song(string filename)
		{
			Filename = filename;
			Update();			
		}

		public void Update()
		{
			FileInfo fileInfo = new FileInfo(_filename);
			Size = ((int)fileInfo.Length / 1048576);
			LastModified = fileInfo.LastWriteTime;

			try
			{
				TagLib.File mediaFile = TagLib.File.Create(_filename);

				Title = mediaFile.Tag.Title;
				Artist = mediaFile.Tag.FirstPerformer;
				Album = mediaFile.Tag.Album;
				TrackNumber = (int)mediaFile.Tag.Track;
				Year = (int)mediaFile.Tag.Year;
				Genre = mediaFile.Tag.FirstGenre;
				LengthInSeconds = (int)mediaFile.Properties.Duration.TotalSeconds;
			}
			catch (Exception ex)
			{
				Logger.Current.Write(ex,"Error reading tags for " + _filename);
			}
			finally
			{
				if (String.IsNullOrEmpty(_title))
					Title = Path.GetFileName(_filename);
			}
		}

		public bool MatchesTags(List<string> tags)
		{
			string lcasefileName = _filename.ToLower();
			foreach (string tag in tags)
			{
				if (!lcasefileName.Contains(tag))
					return false;
			}
			return true;
		}
		
		public static bool IsMediaFile(string filename)
		{
			filename = filename.ToLower();
			if (filename.EndsWith(".mp3") ||
				filename.EndsWith(".wma")
			)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
