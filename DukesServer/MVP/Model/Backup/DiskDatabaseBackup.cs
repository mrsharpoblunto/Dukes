using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DukesServer.MVP.Model.Backup
{
    internal class DiskDatabaseBackup: IDatabaseBackup
    {
        private string _name;
        private string _fullPath;

        public string Name
        {
            get { return _name; }
        }

        public Stream Data
        {
            get { return new FileStream(_fullPath, FileMode.Open, FileAccess.Read); }
        }

        private DiskDatabaseBackup(string filename)
        {
            _name = filename;
            _fullPath = Constants.BackupDir + "\\" + Name;
        }

        public static IDatabaseBackup CreateDiskBackup()
        {
            string filename = DateTime.Today.ToString("dd-MM-yyyy");

            using (Stream input = new FileStream(Constants.DatabaseFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (Stream output = new FileStream(Constants.BackupDir + "\\" + filename, FileMode.Create, FileAccess.Write))
                {
                    input.CopyTo(output);
                }
            }

            return new DiskDatabaseBackup(filename);
        }

        public static List<IDatabaseBackup> LoadDiskBackups()
        {
            List<IDatabaseBackup> backups = new List<IDatabaseBackup>();
            DirectoryInfo di = new DirectoryInfo(Constants.BackupDir);
            foreach (FileInfo backup in di.GetFiles())
            {
                backups.Add(new DiskDatabaseBackup(backup.Name));
            }
            return backups;
        }
    }
}