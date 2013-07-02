using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DukesServer.MVP.View.impl
{
    class ViewBase : Form
    {
        public new event EventHandler Shown;
        public new event EventHandler Closed;

        protected ViewBase()
        {
            base.Shown += ViewBase_Shown;
            base.Closed += ViewBase_Closed;
        }

        void ViewBase_Shown(object sender, EventArgs e)
        {
            if (Shown != null)
            {
                Shown(this, e);
            }
        }

        public void Invoke(InvokeDelegate d)
        {
            base.Invoke(d);
        }

        public void ShowView(IView parentView)
        {
            if (parentView is IWin32Window)
            {
                ShowView(parentView as IWin32Window);
            }
        }

        protected virtual void ShowView(IWin32Window owner) { Show(owner); }
        public virtual void CloseView()
        {
            Close();
        }

        private void ViewBase_Closed(object sender, EventArgs e)
        {
            if (Closed != null)
            {
                Closed(this, new EventArgs());
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewBase));
            this.SuspendLayout();
            // 
            // ViewBase
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewBase";
            this.ResumeLayout(false);

        }
    }
}
