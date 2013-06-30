using System;

using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Validation;
using Mindscape.LightSpeed.Linq;

namespace DukesServer.MVP.Model
{
  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [Table("Users", IdColumnName="Id")]
  [OrderBy("Name ASC")]
  public partial class User : Entity<System.Guid>
  {
    #region Fields
  
    [ValidateLength(0, 50)]
    private string _name;
    [ValidateLength(0, 255)]
    private string _passwordHash;

    #endregion
    
    #region Field attribute and view names
    
    public const string NameField = "Name";
    public const string PasswordHashField = "PasswordHash";


    #endregion
    
    #region Relationships

    [ReverseAssociation("User")]
    private readonly EntityCollection<QueuedSong> _queuedSongs = new EntityCollection<QueuedSong>();

    #endregion
    
    #region Properties

    public EntityCollection<QueuedSong> QueuedSongs
    {
      get { return Get(_queuedSongs); }
    }

    public string Name
    {
      get { return Get(ref _name); }
      set { Set(ref _name, value, "Name"); }
    }

    public string PasswordHash
    {
      get { return Get(ref _passwordHash); }
      set { Set(ref _passwordHash, value, "PasswordHash"); }
    }

    #endregion
  }

  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [Table("Directories", IdColumnName="Id")]
  public partial class Directory : Entity<System.Guid>
  {
    #region Fields
  
    [ValidateLength(0, 400)]
    private string _path;
    [ValidateLength(0, 255)]
    private string _name;
    private System.Nullable<System.DateTime> _lastModified;
    private System.Nullable<System.Guid> _parentDirectoryId;

    #endregion
    
    #region Field attribute and view names
    
    public const string PathField = "Path";
    public const string NameField = "Name";
    public const string LastModifiedField = "LastModified";
    public const string ParentDirectoryIdField = "ParentDirectoryId";


    #endregion
    
    #region Relationships

    [ReverseAssociation("ParentDirectory")]
    private readonly EntityCollection<Directory> _childDirectories = new EntityCollection<Directory>();
    [ReverseAssociation("Directory")]
    private readonly EntityCollection<Song> _songs = new EntityCollection<Song>();
    [ReverseAssociation("ChildDirectories")]
    private readonly EntityHolder<Directory> _parentDirectory = new EntityHolder<Directory>();

    #endregion
    
    #region Properties

    public EntityCollection<Directory> ChildDirectories
    {
      get { return Get(_childDirectories); }
    }

    public EntityCollection<Song> Songs
    {
      get { return Get(_songs); }
    }

    public Directory ParentDirectory
    {
      get { return Get(_parentDirectory); }
      set { Set(_parentDirectory, value); }
    }

    public string Path
    {
      get { return Get(ref _path); }
      set { Set(ref _path, value, "Path"); }
    }

    public string Name
    {
      get { return Get(ref _name); }
      set { Set(ref _name, value, "Name"); }
    }

    public System.Nullable<System.DateTime> LastModified
    {
      get { return Get(ref _lastModified); }
      set { Set(ref _lastModified, value, "LastModified"); }
    }

    public System.Nullable<System.Guid> ParentDirectoryId
    {
      get { return Get(ref _parentDirectoryId); }
      set { Set(ref _parentDirectoryId, value, "ParentDirectoryId"); }
    }

    #endregion
  }

  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [Table("Songs", IdColumnName="Id")]
  public partial class Song : Entity<System.Guid>
  {
    #region Fields
  
    [ValidateLength(0, 800)]
    private string _filename;
    private System.Nullable<System.DateTime> _lastModified;
    private System.Nullable<int> _size;
    [ValidateLength(0, 255)]
    private string _title;
    [ValidateLength(0, 255)]
    private string _artist;
    [ValidateLength(0, 255)]
    private string _album;
    private System.Nullable<int> _trackNumber;
    private System.Nullable<int> _year;
    [ValidateLength(0, 255)]
    private string _genre;
    private System.Nullable<int> _lengthInSeconds;
    private System.Guid _directoryId;

    #endregion
    
    #region Field attribute and view names
    
    public const string FilenameField = "Filename";
    public const string LastModifiedField = "LastModified";
    public const string SizeField = "Size";
    public const string TitleField = "Title";
    public const string ArtistField = "Artist";
    public const string AlbumField = "Album";
    public const string TrackNumberField = "TrackNumber";
    public const string YearField = "Year";
    public const string GenreField = "Genre";
    public const string LengthInSecondsField = "LengthInSeconds";
    public const string DirectoryIdField = "DirectoryId";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Songs")]
    private readonly EntityHolder<Directory> _directory = new EntityHolder<Directory>();
    [ReverseAssociation("Song")]
    private readonly EntityHolder<QueuedSong> _queuedSong = new EntityHolder<QueuedSong>();

    #endregion
    
    #region Properties

    public Directory Directory
    {
      get { return Get(_directory); }
      set { Set(_directory, value); }
    }

    public QueuedSong QueuedSong
    {
      get { return Get(_queuedSong); }
      set { Set(_queuedSong, value); }
    }

    public string Filename
    {
      get { return Get(ref _filename); }
      set { Set(ref _filename, value, "Filename"); }
    }

    public System.Nullable<System.DateTime> LastModified
    {
      get { return Get(ref _lastModified); }
      set { Set(ref _lastModified, value, "LastModified"); }
    }

    public System.Nullable<int> Size
    {
      get { return Get(ref _size); }
      set { Set(ref _size, value, "Size"); }
    }

    public string Title
    {
      get { return Get(ref _title); }
      set { Set(ref _title, value, "Title"); }
    }

    public string Artist
    {
      get { return Get(ref _artist); }
      set { Set(ref _artist, value, "Artist"); }
    }

    public string Album
    {
      get { return Get(ref _album); }
      set { Set(ref _album, value, "Album"); }
    }

    public System.Nullable<int> TrackNumber
    {
      get { return Get(ref _trackNumber); }
      set { Set(ref _trackNumber, value, "TrackNumber"); }
    }

    public System.Nullable<int> Year
    {
      get { return Get(ref _year); }
      set { Set(ref _year, value, "Year"); }
    }

    public string Genre
    {
      get { return Get(ref _genre); }
      set { Set(ref _genre, value, "Genre"); }
    }

    public System.Nullable<int> LengthInSeconds
    {
      get { return Get(ref _lengthInSeconds); }
      set { Set(ref _lengthInSeconds, value, "LengthInSeconds"); }
    }

    public System.Guid DirectoryId
    {
      get { return Get(ref _directoryId); }
      set { Set(ref _directoryId, value, "DirectoryId"); }
    }

    #endregion
  }

  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [Table("QueuedSongs", IdColumnName="Id")]
  [OrderBy("DateAdded DESC")]
  public partial class QueuedSong : Entity<System.Guid>
  {
    #region Fields
  
    private System.Nullable<System.DateTime> _dateAdded;
    private System.Guid _userId;
    private System.Guid _songId;

    #endregion
    
    #region Field attribute and view names
    
    public const string DateAddedField = "DateAdded";
    public const string UserIdField = "UserId";
    public const string SongIdField = "SongId";


    #endregion
    
    #region Relationships

    [EagerLoad]
    [ReverseAssociation("QueuedSongs")]
    private readonly EntityHolder<User> _user = new EntityHolder<User>();
    [EagerLoad]
    [ReverseAssociation("QueuedSong")]
    private readonly EntityHolder<Song> _song = new EntityHolder<Song>();

    #endregion
    
    #region Properties

    public User User
    {
      get { return Get(_user); }
      set { Set(_user, value); }
    }

    public Song Song
    {
      get { return Get(_song); }
      set { Set(_song, value); }
    }

    public System.Nullable<System.DateTime> DateAdded
    {
      get { return Get(ref _dateAdded); }
      set { Set(ref _dateAdded, value, "DateAdded"); }
    }

    public System.Guid UserId
    {
      get { return Get(ref _userId); }
      set { Set(ref _userId, value, "UserId"); }
    }

    public System.Guid SongId
    {
      get { return Get(ref _songId); }
      set { Set(ref _songId, value, "SongId"); }
    }

    #endregion
  }


  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  public partial class ModelUnitOfWork : Mindscape.LightSpeed.UnitOfWork
  {
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public ModelUnitOfWork()
    {
    }
    

    public System.Linq.IQueryable<User> Users
    {
      get { return this.Query<User>(); }
    }
    
    public System.Linq.IQueryable<Directory> Directories
    {
      get { return this.Query<Directory>(); }
    }
    
    public System.Linq.IQueryable<Song> Songs
    {
      get { return this.Query<Song>(); }
    }
    
    public System.Linq.IQueryable<QueuedSong> QueuedSongs
    {
      get { return this.Query<QueuedSong>(); }
    }
    
  }

  namespace Contracts.Data
  {
    [System.Runtime.Serialization.DataContract(Name="User")]
    [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
    public partial class UserDto
    {
      [System.Runtime.Serialization.DataMember]
      public string Name { get; set; }
      [System.Runtime.Serialization.DataMember]
      public string PasswordHash { get; set; }
    }
    [System.Runtime.Serialization.DataContract(Name="Directory")]
    [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
    public partial class DirectoryDto
    {
      [System.Runtime.Serialization.DataMember]
      public string Path { get; set; }
      [System.Runtime.Serialization.DataMember]
      public string Name { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Nullable<System.DateTime> LastModified { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Nullable<System.Guid> ParentDirectoryId { get; set; }
    }
    [System.Runtime.Serialization.DataContract(Name="Song")]
    [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
    public partial class SongDto
    {
      [System.Runtime.Serialization.DataMember]
      public string Filename { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Nullable<System.DateTime> LastModified { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Nullable<int> Size { get; set; }
      [System.Runtime.Serialization.DataMember]
      public string Title { get; set; }
      [System.Runtime.Serialization.DataMember]
      public string Artist { get; set; }
      [System.Runtime.Serialization.DataMember]
      public string Album { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Nullable<int> TrackNumber { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Nullable<int> Year { get; set; }
      [System.Runtime.Serialization.DataMember]
      public string Genre { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Nullable<int> LengthInSeconds { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Guid DirectoryId { get; set; }
    }
    [System.Runtime.Serialization.DataContract(Name="QueuedSong")]
    [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
    public partial class QueuedSongDto
    {
      [System.Runtime.Serialization.DataMember]
      public System.Nullable<System.DateTime> DateAdded { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Guid UserId { get; set; }
      [System.Runtime.Serialization.DataMember]
      public System.Guid SongId { get; set; }
    }

    
    [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
    public static class ModelDtoExtensions
    {
      private static void CopyUser(User entity, UserDto dto)
      {
        dto.Name = entity.Name;
        dto.PasswordHash = entity.PasswordHash;
      }
      
      private static void CopyUser(UserDto dto, User entity)
      {
        entity.Name = dto.Name;
        entity.PasswordHash = dto.PasswordHash;
      }
      
      public static UserDto AsDto(this User entity)
      {
        UserDto dto = new UserDto();
        CopyUser(entity, dto);
        return dto;
      }
      
      public static User CopyTo(this UserDto source, User entity)
      {
        CopyUser(source, entity);
        return entity;
      }
      private static void CopyDirectory(Directory entity, DirectoryDto dto)
      {
        dto.Path = entity.Path;
        dto.Name = entity.Name;
        dto.LastModified = entity.LastModified;
        dto.ParentDirectoryId = entity.ParentDirectoryId;
      }
      
      private static void CopyDirectory(DirectoryDto dto, Directory entity)
      {
        entity.Path = dto.Path;
        entity.Name = dto.Name;
        entity.LastModified = dto.LastModified;
        entity.ParentDirectoryId = dto.ParentDirectoryId;
      }
      
      public static DirectoryDto AsDto(this Directory entity)
      {
        DirectoryDto dto = new DirectoryDto();
        CopyDirectory(entity, dto);
        return dto;
      }
      
      public static Directory CopyTo(this DirectoryDto source, Directory entity)
      {
        CopyDirectory(source, entity);
        return entity;
      }
      private static void CopySong(Song entity, SongDto dto)
      {
        dto.Filename = entity.Filename;
        dto.LastModified = entity.LastModified;
        dto.Size = entity.Size;
        dto.Title = entity.Title;
        dto.Artist = entity.Artist;
        dto.Album = entity.Album;
        dto.TrackNumber = entity.TrackNumber;
        dto.Year = entity.Year;
        dto.Genre = entity.Genre;
        dto.LengthInSeconds = entity.LengthInSeconds;
        dto.DirectoryId = entity.DirectoryId;
      }
      
      private static void CopySong(SongDto dto, Song entity)
      {
        entity.Filename = dto.Filename;
        entity.LastModified = dto.LastModified;
        entity.Size = dto.Size;
        entity.Title = dto.Title;
        entity.Artist = dto.Artist;
        entity.Album = dto.Album;
        entity.TrackNumber = dto.TrackNumber;
        entity.Year = dto.Year;
        entity.Genre = dto.Genre;
        entity.LengthInSeconds = dto.LengthInSeconds;
        entity.DirectoryId = dto.DirectoryId;
      }
      
      public static SongDto AsDto(this Song entity)
      {
        SongDto dto = new SongDto();
        CopySong(entity, dto);
        return dto;
      }
      
      public static Song CopyTo(this SongDto source, Song entity)
      {
        CopySong(source, entity);
        return entity;
      }
      private static void CopyQueuedSong(QueuedSong entity, QueuedSongDto dto)
      {
        dto.DateAdded = entity.DateAdded;
        dto.UserId = entity.UserId;
        dto.SongId = entity.SongId;
      }
      
      private static void CopyQueuedSong(QueuedSongDto dto, QueuedSong entity)
      {
        entity.DateAdded = dto.DateAdded;
        entity.UserId = dto.UserId;
        entity.SongId = dto.SongId;
      }
      
      public static QueuedSongDto AsDto(this QueuedSong entity)
      {
        QueuedSongDto dto = new QueuedSongDto();
        CopyQueuedSong(entity, dto);
        return dto;
      }
      
      public static QueuedSong CopyTo(this QueuedSongDto source, QueuedSong entity)
      {
        CopyQueuedSong(source, entity);
        return entity;
      }

    }

  }
}
