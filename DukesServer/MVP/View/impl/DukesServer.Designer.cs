namespace DukesServer.MVP.View.impl
{
    partial class DukesServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DukesServerForm));
            this.AdminTabControl = new System.Windows.Forms.TabControl();
            this.PlayerTab = new System.Windows.Forms.TabPage();
            this.changePortButton = new System.Windows.Forms.Button();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.playerState = new System.Windows.Forms.Label();
            this.PlayButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.currentlyPlayingTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.PlayerQueueList = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.serviceURLTextBox = new System.Windows.Forms.TextBox();
            this.playerNotificationsTextBox = new System.Windows.Forms.TextBox();
            this.UsersTab = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UsersList = new System.Windows.Forms.ListBox();
            this.removeUserButton = new System.Windows.Forms.Button();
            this.NewUserGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.addUserButton = new System.Windows.Forms.Button();
            this.MediaTab = new System.Windows.Forms.TabPage();
            this.mediaIndexNotifications = new System.Windows.Forms.TextBox();
            this.IndexButton = new System.Windows.Forms.Button();
            this.removeMediaSource = new System.Windows.Forms.Button();
            this.mediaSourcesList = new System.Windows.Forms.ListBox();
            this.addMediaSourceButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.AdminTabControl.SuspendLayout();
            this.PlayerTab.SuspendLayout();
            this.UsersTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.NewUserGroupBox.SuspendLayout();
            this.MediaTab.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AdminTabControl
            // 
            this.AdminTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdminTabControl.Controls.Add(this.PlayerTab);
            this.AdminTabControl.Controls.Add(this.UsersTab);
            this.AdminTabControl.Controls.Add(this.MediaTab);
            this.AdminTabControl.Location = new System.Drawing.Point(7, 26);
            this.AdminTabControl.Name = "AdminTabControl";
            this.AdminTabControl.SelectedIndex = 0;
            this.AdminTabControl.Size = new System.Drawing.Size(508, 450);
            this.AdminTabControl.TabIndex = 0;
            // 
            // PlayerTab
            // 
            this.PlayerTab.Controls.Add(this.changePortButton);
            this.PlayerTab.Controls.Add(this.portTextBox);
            this.PlayerTab.Controls.Add(this.label5);
            this.PlayerTab.Controls.Add(this.playerState);
            this.PlayerTab.Controls.Add(this.PlayButton);
            this.PlayerTab.Controls.Add(this.PauseButton);
            this.PlayerTab.Controls.Add(this.currentlyPlayingTextBox);
            this.PlayerTab.Controls.Add(this.label8);
            this.PlayerTab.Controls.Add(this.label7);
            this.PlayerTab.Controls.Add(this.label6);
            this.PlayerTab.Controls.Add(this.PlayerQueueList);
            this.PlayerTab.Controls.Add(this.label4);
            this.PlayerTab.Controls.Add(this.serviceURLTextBox);
            this.PlayerTab.Controls.Add(this.playerNotificationsTextBox);
            this.PlayerTab.Location = new System.Drawing.Point(4, 22);
            this.PlayerTab.Name = "PlayerTab";
            this.PlayerTab.Padding = new System.Windows.Forms.Padding(3);
            this.PlayerTab.Size = new System.Drawing.Size(500, 424);
            this.PlayerTab.TabIndex = 0;
            this.PlayerTab.Text = "Player";
            this.PlayerTab.UseVisualStyleBackColor = true;
            // 
            // changePortButton
            // 
            this.changePortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.changePortButton.Location = new System.Drawing.Point(353, 11);
            this.changePortButton.Name = "changePortButton";
            this.changePortButton.Size = new System.Drawing.Size(141, 23);
            this.changePortButton.TabIndex = 15;
            this.changePortButton.Text = "Change port";
            this.changePortButton.UseVisualStyleBackColor = true;
            this.changePortButton.Click += new System.EventHandler(this.changePortButton_Click);
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portTextBox.Location = new System.Drawing.Point(285, 13);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(62, 20);
            this.portTextBox.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(273, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = ":";
            // 
            // playerState
            // 
            this.playerState.AutoSize = true;
            this.playerState.Location = new System.Drawing.Point(276, 106);
            this.playerState.Name = "playerState";
            this.playerState.Size = new System.Drawing.Size(41, 13);
            this.playerState.TabIndex = 12;
            this.playerState.Text = "Playing";
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(195, 101);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(75, 23);
            this.PlayButton.TabIndex = 11;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Location = new System.Drawing.Point(114, 101);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(75, 23);
            this.PauseButton.TabIndex = 10;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // currentlyPlayingTextBox
            // 
            this.currentlyPlayingTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentlyPlayingTextBox.Location = new System.Drawing.Point(114, 75);
            this.currentlyPlayingTextBox.Name = "currentlyPlayingTextBox";
            this.currentlyPlayingTextBox.ReadOnly = true;
            this.currentlyPlayingTextBox.Size = new System.Drawing.Size(380, 20);
            this.currentlyPlayingTextBox.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Currently Playing";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 284);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Player Notifications";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Player Queue";
            // 
            // PlayerQueueList
            // 
            this.PlayerQueueList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayerQueueList.FormattingEnabled = true;
            this.PlayerQueueList.Location = new System.Drawing.Point(114, 143);
            this.PlayerQueueList.Name = "PlayerQueueList";
            this.PlayerQueueList.Size = new System.Drawing.Size(380, 134);
            this.PlayerQueueList.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Dukes Service URL";
            // 
            // serviceURLTextBox
            // 
            this.serviceURLTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceURLTextBox.Location = new System.Drawing.Point(114, 12);
            this.serviceURLTextBox.Name = "serviceURLTextBox";
            this.serviceURLTextBox.ReadOnly = true;
            this.serviceURLTextBox.Size = new System.Drawing.Size(156, 20);
            this.serviceURLTextBox.TabIndex = 1;
            // 
            // playerNotificationsTextBox
            // 
            this.playerNotificationsTextBox.AcceptsReturn = true;
            this.playerNotificationsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playerNotificationsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.playerNotificationsTextBox.Location = new System.Drawing.Point(114, 284);
            this.playerNotificationsTextBox.Multiline = true;
            this.playerNotificationsTextBox.Name = "playerNotificationsTextBox";
            this.playerNotificationsTextBox.ReadOnly = true;
            this.playerNotificationsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.playerNotificationsTextBox.Size = new System.Drawing.Size(380, 134);
            this.playerNotificationsTextBox.TabIndex = 0;
            // 
            // UsersTab
            // 
            this.UsersTab.Controls.Add(this.groupBox1);
            this.UsersTab.Controls.Add(this.NewUserGroupBox);
            this.UsersTab.Location = new System.Drawing.Point(4, 22);
            this.UsersTab.Name = "UsersTab";
            this.UsersTab.Padding = new System.Windows.Forms.Padding(3);
            this.UsersTab.Size = new System.Drawing.Size(500, 424);
            this.UsersTab.TabIndex = 1;
            this.UsersTab.Text = "Users";
            this.UsersTab.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.UsersList);
            this.groupBox1.Controls.Add(this.removeUserButton);
            this.groupBox1.Location = new System.Drawing.Point(6, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(488, 302);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Existing Users";
            // 
            // UsersList
            // 
            this.UsersList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UsersList.FormattingEnabled = true;
            this.UsersList.Location = new System.Drawing.Point(6, 19);
            this.UsersList.Name = "UsersList";
            this.UsersList.Size = new System.Drawing.Size(475, 251);
            this.UsersList.TabIndex = 0;
            // 
            // removeUserButton
            // 
            this.removeUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeUserButton.Location = new System.Drawing.Point(373, 273);
            this.removeUserButton.Name = "removeUserButton";
            this.removeUserButton.Size = new System.Drawing.Size(108, 23);
            this.removeUserButton.TabIndex = 3;
            this.removeUserButton.Text = "Remove user";
            this.removeUserButton.UseVisualStyleBackColor = true;
            this.removeUserButton.Click += new System.EventHandler(this.removeUserButton_Click);
            // 
            // NewUserGroupBox
            // 
            this.NewUserGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewUserGroupBox.Controls.Add(this.label2);
            this.NewUserGroupBox.Controls.Add(this.label1);
            this.NewUserGroupBox.Controls.Add(this.passwordTextBox);
            this.NewUserGroupBox.Controls.Add(this.userNameTextBox);
            this.NewUserGroupBox.Controls.Add(this.addUserButton);
            this.NewUserGroupBox.Location = new System.Drawing.Point(6, 6);
            this.NewUserGroupBox.Name = "NewUserGroupBox";
            this.NewUserGroupBox.Size = new System.Drawing.Size(488, 104);
            this.NewUserGroupBox.TabIndex = 2;
            this.NewUserGroupBox.TabStop = false;
            this.NewUserGroupBox.Text = "New User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(67, 45);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(414, 20);
            this.passwordTextBox.TabIndex = 2;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userNameTextBox.Location = new System.Drawing.Point(67, 19);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(414, 20);
            this.userNameTextBox.TabIndex = 1;
            // 
            // addUserButton
            // 
            this.addUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addUserButton.Location = new System.Drawing.Point(406, 71);
            this.addUserButton.Name = "addUserButton";
            this.addUserButton.Size = new System.Drawing.Size(75, 23);
            this.addUserButton.TabIndex = 0;
            this.addUserButton.Text = "Add user";
            this.addUserButton.UseVisualStyleBackColor = true;
            this.addUserButton.Click += new System.EventHandler(this.addUserButton_Click);
            // 
            // MediaTab
            // 
            this.MediaTab.Controls.Add(this.removeMediaSource);
            this.MediaTab.Controls.Add(this.addMediaSourceButton);
            this.MediaTab.Controls.Add(this.mediaIndexNotifications);
            this.MediaTab.Controls.Add(this.IndexButton);
            this.MediaTab.Controls.Add(this.mediaSourcesList);
            this.MediaTab.Location = new System.Drawing.Point(4, 22);
            this.MediaTab.Name = "MediaTab";
            this.MediaTab.Size = new System.Drawing.Size(500, 424);
            this.MediaTab.TabIndex = 2;
            this.MediaTab.Text = "Media Sources";
            this.MediaTab.UseVisualStyleBackColor = true;
            // 
            // mediaIndexNotifications
            // 
            this.mediaIndexNotifications.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mediaIndexNotifications.Location = new System.Drawing.Point(6, 333);
            this.mediaIndexNotifications.Multiline = true;
            this.mediaIndexNotifications.Name = "mediaIndexNotifications";
            this.mediaIndexNotifications.ReadOnly = true;
            this.mediaIndexNotifications.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mediaIndexNotifications.Size = new System.Drawing.Size(487, 88);
            this.mediaIndexNotifications.TabIndex = 3;
            // 
            // IndexButton
            // 
            this.IndexButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IndexButton.Location = new System.Drawing.Point(6, 304);
            this.IndexButton.Name = "IndexButton";
            this.IndexButton.Size = new System.Drawing.Size(130, 23);
            this.IndexButton.TabIndex = 2;
            this.IndexButton.Text = "Index media sources";
            this.IndexButton.UseVisualStyleBackColor = true;
            this.IndexButton.Click += new System.EventHandler(this.indexButton_Click);
            // 
            // removeMediaSource
            // 
            this.removeMediaSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeMediaSource.Location = new System.Drawing.Point(362, 304);
            this.removeMediaSource.Name = "removeMediaSource";
            this.removeMediaSource.Size = new System.Drawing.Size(131, 23);
            this.removeMediaSource.TabIndex = 1;
            this.removeMediaSource.Text = "Remove media source";
            this.removeMediaSource.UseVisualStyleBackColor = true;
            this.removeMediaSource.Click += new System.EventHandler(this.removeMediaSource_Click);
            // 
            // mediaSourcesList
            // 
            this.mediaSourcesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mediaSourcesList.FormattingEnabled = true;
            this.mediaSourcesList.Location = new System.Drawing.Point(6, 16);
            this.mediaSourcesList.Name = "mediaSourcesList";
            this.mediaSourcesList.Size = new System.Drawing.Size(487, 277);
            this.mediaSourcesList.TabIndex = 0;
            // 
            // addMediaSourceButton
            // 
            this.addMediaSourceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addMediaSourceButton.Location = new System.Drawing.Point(242, 304);
            this.addMediaSourceButton.Name = "addMediaSourceButton";
            this.addMediaSourceButton.Size = new System.Drawing.Size(114, 23);
            this.addMediaSourceButton.TabIndex = 2;
            this.addMediaSourceButton.Text = "Add media source";
            this.addMediaSourceButton.UseVisualStyleBackColor = true;
            this.addMediaSourceButton.Click += new System.EventHandler(this.addMediaSourceButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(-2, -1);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(45, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreDatabaseToolStripMenuItem,
            this.backupDatabaseToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.optionsToolStripMenuItem.Text = "File";
            // 
            // restoreDatabaseToolStripMenuItem
            // 
            this.restoreDatabaseToolStripMenuItem.Name = "restoreDatabaseToolStripMenuItem";
            this.restoreDatabaseToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.restoreDatabaseToolStripMenuItem.Text = "Restore Database";
            // 
            // backupDatabaseToolStripMenuItem
            // 
            this.backupDatabaseToolStripMenuItem.Name = "backupDatabaseToolStripMenuItem";
            this.backupDatabaseToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.backupDatabaseToolStripMenuItem.Text = "Backup Database";
            this.backupDatabaseToolStripMenuItem.Click += new System.EventHandler(this.backupDatabaseToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // DukesServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 481);
            this.Controls.Add(this.AdminTabControl);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(527, 482);
            this.Name = "DukesServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dukes Server Admin";
            this.AdminTabControl.ResumeLayout(false);
            this.PlayerTab.ResumeLayout(false);
            this.PlayerTab.PerformLayout();
            this.UsersTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.NewUserGroupBox.ResumeLayout(false);
            this.NewUserGroupBox.PerformLayout();
            this.MediaTab.ResumeLayout(false);
            this.MediaTab.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl AdminTabControl;
        private System.Windows.Forms.TabPage PlayerTab;
        private System.Windows.Forms.TabPage UsersTab;
        private System.Windows.Forms.TabPage MediaTab;
        private System.Windows.Forms.TextBox playerNotificationsTextBox;
        private System.Windows.Forms.GroupBox NewUserGroupBox;
        private System.Windows.Forms.ListBox UsersList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button removeUserButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Button addUserButton;
        private System.Windows.Forms.Button addMediaSourceButton;
        private System.Windows.Forms.ListBox mediaSourcesList;
        private System.Windows.Forms.Button removeMediaSource;
        private System.Windows.Forms.TextBox mediaIndexNotifications;
        private System.Windows.Forms.Button IndexButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backupDatabaseToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox serviceURLTextBox;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.TextBox currentlyPlayingTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox PlayerQueueList;
        private System.Windows.Forms.Label playerState;
        private System.Windows.Forms.Button changePortButton;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}