using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DukesServer.MVP.Model;
using DukesServer.MVP.Presenter;
using DukesServer.MVP.View.impl;
using DukesServer.Properties;
using Mindscape.LightSpeed;

namespace DukesServer
{
    internal static class Program
    {
        private static DukesServerForm _form;
        private static DukesServerPresenter _presenter;
        private static NotifyIcon _notifyIcon;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Player.Current = Player.CreatePlayer();
            Logger.Current = new Logger();

            var context = new LightSpeedContext<ModelUnitOfWork>();
            context.ConnectionString = "Data Source=" + Constants.DatabaseFile;
            context.DataProvider = DataProvider.SQLite3;
            context.PluralizeTableNames = true;
            context.IdentityMethod = IdentityMethod.Guid;
            ModelContext.Current = context;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += ApplicationThreadException;

            _notifyIcon = new NotifyIcon
                              {
                                  Icon = Resources.dukes_connected,
                                  Text = "The Dukebox of Hazzard",
                                  Visible = true,
                                  ContextMenu = GetContextMenu()
                              };
            _notifyIcon.DoubleClick += NotifyIconDoubleClick;

            _form = new DukesServerForm();
            _form.Shown += FormShown;
            _form.Closed += FormClosed;
            Application.Run(_form);
        }

        static void ApplicationThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger.Current.Write(e.Exception, e.Exception.Message);
            MessageBox.Show("Dukes Server encountered an error, please check the logs for more details", "Dukes Error");
        }

        static void NotifyIconDoubleClick(object sender, EventArgs e)
        {
            _form.Show();
            _form.BringToFront();
        }

        static void FormClosed(object sender, EventArgs e)
        {
            if (_notifyIcon!=null) _notifyIcon.Dispose();
            if (Player.Current!=null) Player.Current.Dispose();
        }

        static void FormShown(object sender, EventArgs e)
        {
            _presenter = new DukesServerPresenter(_form);
        }

        private static ContextMenu GetContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            menu.MenuItems.Add(0, new MenuItem("Exit", ExitClick));
            return menu;
        }

        private static void ExitClick(object sender, EventArgs e)
        {
            _form.ForceClose = true;
            _form.Close();
        }
    }
}