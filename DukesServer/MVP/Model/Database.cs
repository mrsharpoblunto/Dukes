using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Querying;

namespace DukesServer.MVP.Model
{
    public enum MediaUpdatetEventEnum
    {
        Info,
        Updated,
        Added,
        Removed
    }

    public class IndexingProgressEventArgs : EventArgs
    {
        public MediaUpdatetEventEnum Event { get; set; }
        public string Message { get; set; }
    }

    internal class Database
    {
        public static event EventHandler OnQueueChanged;

        private const String ServerQueue = "__serverQueue__";
        private const int UserQueueSize = 50;
        private static object _queueCacheLock = new object();
        private static Dictionary<string,List<QueuedSong>> _queueCache;

        static Database()
        {
            _queueCache = new Dictionary<string, List<QueuedSong>>();
        }

        public static List<QueuedSong> GetQueue()
        {
            lock (_queueCacheLock)
            {
                List<QueuedSong> result;
                if (_queueCache.TryGetValue( ServerQueue, out result)) return result;
            }

            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                List<QueuedSong> result = new List<QueuedSong>();
                List<User> users = new List<User>(unit.Users);
                if (users.Count == 0)
                    return result;

                Guid? currentUserId = users[0].Id;
                if (Player.Current.CurrentSong != null)
                {
                    var user = unit.Users.SingleOrDefault(u => u.Name == Player.Current.CurrentSong.Submitter);
                    if (user != null)
                    {
                        currentUserId = user.Id;
                    }
                }

                int userIndex = 0;
                User currentUser = users.SingleOrDefault(u => u.Id == currentUserId.Value);
                if (currentUser != null)
                {
                    userIndex = FindUserIndex(currentUser, users);
                }

                Dictionary<int, IList<QueuedSong>> userQueues = new Dictionary<int, IList<QueuedSong>>();
                Dictionary<int, bool> emptyQueues = new Dictionary<int, bool>();

                //get all the songs on the server queue
                while (!AllEmpty(emptyQueues, users))
                {
                    userIndex++;
                    userIndex = userIndex % users.Count;
                    User user = users[userIndex];
                    QueuedSong song = null;

                    //get the queue for a user if we haven't already
                    IList<QueuedSong> userQueue;
                    if (!userQueues.TryGetValue(userIndex,out userQueue))
                    {
                        userQueue = GetQueueForUser(user, UserQueueSize);
                        userQueues.Add(userIndex, userQueue);
                        emptyQueues[userIndex] = false;
                        if (userQueue!=null && userQueue.Count > 0)
                        {
                            song = userQueue[0];
                        }
                        else
                        {
                            emptyQueues[userIndex] = true;
                        }
                    }
                    //if we have thier list, remove the song we added to the server queue on the last pass
                    else
                    {
                        if (userQueue.Count > 0)
                        {
                            userQueue.RemoveAt(0);
                        }
                        if (userQueue.Count > 0)
                        {
                            song = userQueue[0];
                        }
                        else
                        {
                            emptyQueues[userIndex] = true;
                        }
                    }

                    if (song != null)
                    {
                        result.Add(song);
                    }
                }

                lock (_queueCacheLock)
                {
                    List<QueuedSong> tmp;
                    if (!_queueCache.TryGetValue(ServerQueue, out tmp))
                    {
                        _queueCache.Add(ServerQueue, result);
                    }
                    else
                    {
                        result = tmp;
                    }
                }

                return result;
            }
        }

        private static int FindUserIndex(User findUser,List<User> users)
        {
            int i = 0;
            foreach (var user in users)
            {
                if (findUser.Id == user.Id)
                {
                    return i;
                }
                ++i;
            }
            return -1;
        }

        private static bool AllEmpty(Dictionary<int, bool> emptyMap, List<User> users)
        {
            if (emptyMap.Count < users.Count)
            {
                return false;
            }

            foreach (KeyValuePair<int, bool> pair in emptyMap)
            {
                if (!pair.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public static List<QueuedSong> GetQueueForUser(User user)
        {
            List<QueuedSong> result;
            lock (_queueCacheLock)
            {
                if (_queueCache.TryGetValue(user.Id.ToString(), out result)) return result;
            }

            result = GetQueueForUser(user, UserQueueSize);

            lock (_queueCacheLock)
            {
                List<QueuedSong> tmp;
                if (!_queueCache.TryGetValue(user.Id.ToString(), out tmp))
                {
                    _queueCache.Add(user.Id.ToString(), result);
                }
                else
                {
                    result = tmp;
                }
            }
            return result;
        }

        private static List<QueuedSong> GetQueueForUser(User user,int count)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                var query = (from qs in unit.QueuedSongs where qs.UserId == user.Id orderby qs.DateAdded ascending select qs).Take(count);
                var result =  new List<QueuedSong>(query);
                return result;
            }
        }

        public static QueuedSong GetNextSong()
        {
            var queue = GetQueue();
            if (queue.Count>0)
            {
                return queue[0];
            }
            return null;
        }

        public static User GetUser(Guid token)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                return unit.Users.SingleOrDefault(u => u.Id == token);
            }
        }

        public static User GetUser(string username,string password)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                MD5 md5 = MD5.Create();
                byte[] hashedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                string passwordHash = Convert.ToBase64String(hashedBytes);
                return unit.Users.SingleOrDefault(u => u.Name == username && u.PasswordHash == passwordHash);
            }
        }

        public static IList<Song> GetSongs(string artist, string album)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {    
                Query query = new Query(typeof(Song), Entity.Attribute("Artist").Like("%" + artist + "%") && Entity.Attribute("Album").Like("%" + album + "%"));
                query.Order = Order.By("Album").AndBy("TrackNumber");
                query.Page = Page.Limit(50);
                return new List<Song>(unit.Find<Song>(query));
            }
        }

        public static IList<Song> GetSongs(string title)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                if (!string.IsNullOrEmpty(title))
                {
                    Query query = new Query(typeof (Song), Entity.Attribute("Title").Like("%" + title + "%"));
                    query.Order = Order.By("Album").AndBy("TrackNumber");
                    query.Page = Page.Limit(50);
                    return unit.Find<Song>(query);
                }
                return new List<Song>();
            }
        }

        public static void RemoveFromMyQueue(Guid userId, Guid songId)
        {
            bool queueChanged = false;
                
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                QueuedSong song = unit.QueuedSongs.SingleOrDefault(qs => qs.UserId == userId && qs.SongId == songId);
                if (song != null)
                {
                    unit.Remove(song);
                    unit.SaveChanges();
                    queueChanged = true;
                }
            }              

            if (queueChanged)
            {
                lock (_queueCacheLock)
                {
                    _queueCache.Remove(userId.ToString());
                    _queueCache.Remove(ServerQueue);
                }

                if (OnQueueChanged != null)
                {
                    OnQueueChanged(null, new EventArgs());
                }
            }
        }

        public static void ClearMyQueue(User user)
        {            
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                var songs = unit.QueuedSongs.Where(qs => qs.UserId == user.Id);
                foreach (var song in songs)
                {
                    unit.Remove(song);
                }
                unit.SaveChanges();
            }

            lock (_queueCacheLock)
            {
                _queueCache.Remove(user.Id.ToString());
                _queueCache.Remove(ServerQueue);
            }
               
            if (OnQueueChanged != null)
            {
                OnQueueChanged(null, new EventArgs());
            }       
        }

        public static void QueueMySongs(User user, List<Guid> guids)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                DateTime now = DateTime.Now;
                foreach (var id in guids)
                {
                    Song song = unit.Songs.SingleOrDefault(s => s.Id == id);
                    if (song != null)
                    {
                        //only allow one instance of a song in the queue at a time to prevent spamming
                        QueuedSong existingSong = unit.QueuedSongs.SingleOrDefault(qs => qs.SongId == song.Id);
                        if (existingSong == null)
                        {
                            QueuedSong queuedSong = new QueuedSong();
                            queuedSong.UserId = user.Id;
                            queuedSong.SongId = song.Id;
                            queuedSong.DateAdded = now;
                            now = now.AddMilliseconds(1);
                            unit.Add(queuedSong);
                        }
                    }
                }

                unit.SaveChanges();
            }

            lock (_queueCacheLock)
            {
                _queueCache.Remove(user.Id.ToString());
                _queueCache.Remove(ServerQueue);
            }

            if (OnQueueChanged != null)
            {
                OnQueueChanged(null, new EventArgs());
            }
        }

        public static List<User> GetUsers()
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                return new List<User>(unit.Users);
            }
        }

        public static List<Directory> GetRootDirectories()
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                return new List<Directory>(unit.Directories.Where(d => d.ParentDirectory==null));
            }
        }

        public static void AddUser(string username, string password)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                var existingUser = unit.Users.SingleOrDefault(u => u.Name == username);
                if (existingUser == null)
                {
                    MD5 md5 = MD5.Create();
                    byte[] hashedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                    string passwordHash = Convert.ToBase64String(hashedBytes);

                    User user = new User();
                    user.Name = username;
                    user.PasswordHash = passwordHash;

                    unit.Add(user);
                    unit.SaveChanges();
                }
            }
        }

        public static void RemoveUser(User user)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                unit.Remove(user);
                unit.SaveChanges();
            }
        }

        public static void AddRootDirectory(string source)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                DirectoryInfo di = new DirectoryInfo(source);

                if (di.Exists)
                {
                    Directory directory = new Directory(source);
                    unit.Add(directory);
                    unit.SaveChanges();
                }
            }
        }

        public static void RemoveRootDirectory(Directory directory, EventHandler<IndexingProgressEventArgs> progressNotifier)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                var dir = unit.Directories.SingleOrDefault(d => d.Id == directory.Id);
                if (dir != null)
                {
                    dir.RecursiveDelete(unit, progressNotifier);
                    unit.SaveChanges();
                }
            }
            progressNotifier(null, new IndexingProgressEventArgs { Event = MediaUpdatetEventEnum.Info, Message = "Indexing complete." });
        }

        public static void IndexDirectories(EventHandler<IndexingProgressEventArgs> progressNotifier)
        {
            using (var unit = ModelContext.Current.CreateUnitOfWork())
            {
                foreach (var directory in unit.Directories.Where(d => d.ParentDirectory == null))
                {
                    directory.Update(unit, progressNotifier);
                }
                progressNotifier(null,new IndexingProgressEventArgs{Event = MediaUpdatetEventEnum.Info,Message = "Indexing complete."});
            }
        }
    }
}