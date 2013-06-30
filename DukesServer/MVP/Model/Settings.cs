using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DukesServer.MVP.Model
{
    public class Settings
    {
        public int Port;
        public bool RestoreDB;
        public bool BackupDB;
        public string RestoreBackup;

        public static Settings Load()
        {
            var settings = new FileInfo(Constants.SettingsFile);
            if (!settings.Exists) return null;

            try
            {
                using (var fileStream = new FileStream(settings.FullName,FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var reader = new BinaryReader(fileStream))
                    {
                        int length = reader.ReadInt32();
                        string raw = Encoding.UTF8.GetString(reader.ReadBytes(length));
                        return JsonConvert.DeserializeObject<Settings>(raw);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex, "Failed to load settings");
                return null;
            }
        }

        public void Save()
        {
            var settings = new FileInfo(Constants.SettingsFile);
            using (var fileStream = new FileStream(settings.FullName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                string serialized = JsonConvert.SerializeObject(this);
                using (var writer = new BinaryWriter(fileStream))
                {
                    writer.Write(serialized.Length);
                    writer.Write(Encoding.UTF8.GetBytes(serialized));
                }
            }
        }
    }
}
