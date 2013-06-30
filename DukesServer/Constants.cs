using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DukesServer
{
    internal static class Constants
    {
        public static string AppDir
        {
            get
            {
                return new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;                
            }
        }

        public static string DataDir
        {
            get
            {
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                var di = new DirectoryInfo(Path.Combine(localAppData, "Dukes"));
                if (!di.Exists) di.Create();

                return di.FullName;
            }
        }

        public static string DatabaseFile
        {
            get
            {
                return DataDir + "\\dukes.db";
            }
        }

        public static string LogFile
        {
            get
            {
                return DataDir + "\\log.txt";
            }
        }

        public static string SettingsFile
        {
            get
            {
                return DataDir + "\\settings.json";
            }
        }

        public static string BackupDir
        {
            get
            {
                if (!Directory.Exists(DataDir + "\\backups"))
                {
                    Directory.CreateDirectory(DataDir + "\\backups");
                }
                return DataDir + "\\backups";
            }
        }
    }
}