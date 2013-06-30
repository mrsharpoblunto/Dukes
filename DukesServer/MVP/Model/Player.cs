using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DukesServer.MVP.Model
{
    internal class Player
    {
        public static IPlayer Current
        {
            get
            {
                return ServiceLocator.Get<IPlayer>();
            }
            set
            {
                ServiceLocator.Register<IPlayer>(value);
            }
        }

        public static IPlayer CreatePlayer()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string playerAssembly = ConfigurationManager.AppSettings["Player"]+".dll";

            string targetAssembly = Path.Combine(path, playerAssembly);
            if (!File.Exists(targetAssembly))
            {
                MessageBox.Show("The specified player '" + targetAssembly + "' was not found");
                Application.Exit();
            }

            Assembly assembly = Assembly.LoadFile(targetAssembly);
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract) continue;

                if (type.GetInterface("IPlayer")!=null)
                {
                    return Activator.CreateInstance(type) as IPlayer;
                }
            }

            MessageBox.Show("The specified player has no concrete public IPlayer instance.");
            Application.Exit();
            return null;
        }
    }
}
