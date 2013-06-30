using System;
using System.Data;
using System.Configuration;
using System.Linq;
using DukesServer.MVP.Model.Service.Entities;
using System.Collections.Generic;

namespace DukesServer.MVP.Model.Service
{
    internal static class Mapper
    {

        public static Entities.Song MapFromDomain(Song domainSong)
        {
            if (domainSong == null)
                return null;

            Entities.Song song = new Entities.Song();
            song.Id = domainSong.Id;
            song.Album = domainSong.Album;
            song.Artist = domainSong.Artist;
            song.Title = domainSong.Title;
            song.FileName = domainSong.Filename;
            return song;
        }

        public static List<Entities.Song> MapFromDomain(IList<Song> domainSongs)
        {
            List<Entities.Song> songs = new List<Entities.Song>();
            foreach (Song song in domainSongs)
            {
                songs.Add(MapFromDomain(song));
            }
            return songs;
        }

        public static Entities.QueuedSong MapFromDomain(QueuedSong domainQueuedSong)
        {
            if (domainQueuedSong == null)
                return null;
            Entities.QueuedSong queuedSong = new Entities.QueuedSong();
            queuedSong.Submitter = domainQueuedSong.User.Name;
            queuedSong.Song = MapFromDomain(domainQueuedSong.Song);
            return queuedSong;
        }

        public static List<Entities.QueuedSong> MapFromDomain(IList<QueuedSong> domainQueue)
        {
            List<Entities.QueuedSong> songs = new List<Entities.QueuedSong>();
            foreach (QueuedSong domainQueuedSong in domainQueue)
            {
                songs.Add(MapFromDomain(domainQueuedSong));
            }
            return songs;
        }

        public static Entities.User MapFromDomain(User domainUser)
        {
            Entities.User user = new Entities.User();
            user.Name = domainUser.Name;
            user.Id = domainUser.Id;

            return user;
        }
    }
}