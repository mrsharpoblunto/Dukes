using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DukesServer.MVP.Model.Backup
{
    internal class DefaultDatabaseBackup: IDatabaseBackup
    {
        public string Name
        {
            get { return "Default"; }
        }

        public Stream Data
        {
            get { return Assembly.GetExecutingAssembly().GetManifestResourceStream("DukesServer.Resources.dukes-restore.db"); }
        }
    }
}