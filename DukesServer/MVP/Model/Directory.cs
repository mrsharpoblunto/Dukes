using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Mindscape.LightSpeed;

namespace DukesServer.MVP.Model
{
	partial class Directory
	{
		public Directory()
		{
		}

		public Directory(string path)
		{
			DirectoryInfo di = new DirectoryInfo(path);
			ParentDirectoryId = null;
			Path = di.FullName;
			Name = di.Name;		
		}

		public bool Update(ModelUnitOfWork unit, EventHandler<IndexingProgressEventArgs> notifier)
		{
			if (!ParentDirectoryId.HasValue)
			{
				Notify(notifier,MediaUpdatetEventEnum.Info, "Looking at " + _path);
			}

			if (!System.IO.Directory.Exists(_path))
			{
				//dont delete media sources on update, even if they dont exist
				if (ParentDirectory==null)
				{
					return false;
				}

				if (EntityState != EntityState.Deleted)
				{
					Notify(notifier, MediaUpdatetEventEnum.Removed, "Removing " + _path);
                    RecursiveDelete(unit, notifier);
					unit.SaveChanges();
					return false;					
				}
			}

			string[] directoryNames = new string[0];
			try
			{
				directoryNames = System.IO.Directory.GetDirectories(_path);
			}
			catch (Exception)
			{
				Notify(notifier, MediaUpdatetEventEnum.Removed, "Removing " + _path);
				RecursiveDelete(unit, notifier);
				unit.SaveChanges();				
				return false;				
			}

			foreach (string directoryName in directoryNames)
			{
				if (!AlreadyIndexedDirectory(directoryName))
				{				
					Notify(notifier, MediaUpdatetEventEnum.Added, "Adding " + directoryName);
					Directory newDirectory = new Directory(directoryName);
					unit.Add(newDirectory);
					ChildDirectories.Add(newDirectory);
					unit.SaveChanges();
				}
			}

			for (int i = ChildDirectories.Count - 1; i >= 0;--i)
			{
				if (!ChildDirectories[i].Update(unit, notifier))
				{					
					ChildDirectories.RemoveAt(i);
					unit.SaveChanges();
				}
			}
			
			DirectoryInfo thisDirectory = new DirectoryInfo(_path);
			if (!LastModified.HasValue || thisDirectory.LastWriteTime > LastModified)
			{
				LastModified = thisDirectory.LastWriteTime.AddSeconds(1); //account for Sql datetime precision loss?
				unit.SaveChanges();

				Notify(notifier, MediaUpdatetEventEnum.Updated, "Updating " + _path);
				UpdateSongs(unit,notifier);
			}

			return true;
		}

		private void Notify(EventHandler<IndexingProgressEventArgs> notifier, MediaUpdatetEventEnum eventEnum, string message)
		{
			notifier(this,new IndexingProgressEventArgs{Event = eventEnum,Message = message});
		}

		private void UpdateSongs(ModelUnitOfWork unit, EventHandler<IndexingProgressEventArgs> notifier)
		{
			try
			{
				//remove all files that were removed since the last update
				RemoveNonExistantFiles(unit,notifier);

				string[] filenames = System.IO.Directory.GetFiles(_path, "*.*", SearchOption.TopDirectoryOnly);
				foreach (string filename in filenames)
				{
					try
					{
						if (Song.IsMediaFile(filename))
						{						
							Song existingSong = GetExistingSong(filename);

							//if the file has changed since we last looked at it, replace it with the new version
							if (existingSong != null && existingSong.LastModified > existingSong.LastModified)
							{
								Notify(notifier,MediaUpdatetEventEnum.Updated, " Updating " + filename);
								existingSong.Update();
							}
							//if it didn't exist before, add it to the list
							else if (existingSong == null)
							{
								Notify(notifier, MediaUpdatetEventEnum.Added, " Adding " + filename);
								var song = new Song(filename);
								unit.Add(song);
								Songs.Add(song);
							}

							unit.SaveChanges();
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		private void RemoveNonExistantFiles(ModelUnitOfWork unit, EventHandler<IndexingProgressEventArgs> notifier)
		{
			List<Song> removeSongs = new List<Song>();
			for (int i = Songs.Count - 1; i >= 0; --i)
			{
				if (!File.Exists(Songs[i].Filename))
				{
					removeSongs.Add(Songs[i]);
					Notify(notifier, MediaUpdatetEventEnum.Removed, " Removing " + Songs[i].Filename);
				}	        
			}

			if (removeSongs.Count == 0) return;
			
			foreach (var song in removeSongs)
			{
				Notify(notifier, MediaUpdatetEventEnum.Removed, " Removing " + song.Filename);
				Songs.Remove(song);
				RemoveSong(unit,song);
			}
			unit.SaveChanges();				
		}

		private Song GetExistingSong(string filename)
		{
			return Songs.Find(f => filename == f.Filename);
		}

		private bool AlreadyIndexedDirectory(string path)
		{
			return ChildDirectories.Find(d => d.Path == path) != null;
		}

		#region IEquatable<MediaDirectory> Members

		public bool Equals(Directory other)
		{
			if (Path.Length > other.Path.Length)
			{
				return Path.StartsWith(other.Path, StringComparison.InvariantCultureIgnoreCase);
			}
			else
			{
				return other.Path.StartsWith(Path, StringComparison.InvariantCultureIgnoreCase);
			}
		}

		#endregion

		public void RecursiveDelete(ModelUnitOfWork unit, EventHandler<IndexingProgressEventArgs> notifier)
		{
			for (int i = ChildDirectories.Count - 1; i >= 0; --i)
			{
				ChildDirectories[i].RecursiveDelete(unit,notifier);
			}

			for (int i = Songs.Count - 1; i >= 0;--i)
			{
                Notify(notifier, MediaUpdatetEventEnum.Removed, " Removing " + Songs[i].Filename);
				RemoveSong(unit, Songs[i]);
			}
			unit.Remove(this);
		}

		private static void RemoveSong(ModelUnitOfWork unit, Song song)
		{
			Song song1 = song;
			var queuedSongs = unit.QueuedSongs.Where(qs => qs.SongId == song1.Id);
			foreach (var queuedSong in queuedSongs)
			{
				unit.Remove(queuedSong);
			}
			unit.Remove(song);
		}
	}
}
