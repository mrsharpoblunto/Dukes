using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DukesServer.MVP.Model.Backup
{
    internal interface IDatabaseBackup
    {
        string Name { get; }
        Stream Data { get; }
    }
}