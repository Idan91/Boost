﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Boost.View
{
    public delegate void LogoutEventHandler();

    public delegate void SettingsEventHandler();

    public delegate void OptionClickEventHandler();

    public partial class UserOptions : UserControl
    {
        private const int k_ButtonWidth = 200;
        private const int k_ButtonHeight = 40;
        private List<Button> m_UserOptionButtons = new List<Button>();
        public LogoutEventHandler m_LogoutEvent;
        public SettingsEventHandler m_SettingsEvent;
        public OptionClickEventHandler m_OptionClickEvent;

        public UserOptions()
        {
            InitializeComponent();
            initializeUserOptionButtons();
            m_OptionClickEvent += hideUserOptions;
        }

        private enum eUserOptions : byte
        {
            Settings,
            Logout
        }

        private void hideUserOptions()
        {
            Visible = false;
        }

        private void initializeUserOptionButtons()
        {
            int startY = 0;
            int Y = startY;
            int tabIndex = 0;

            foreach(eUserOptions option in Enum.GetValues(typeof(eUserOptions)))
            {
                Button button = new Button();
                button.AutoSize = true;
                button.Dock = System.Windows.Forms.DockStyle.Bottom;
                button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.BorderColor = BackColor;
                button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                button.Font = Stylesheet.Font_NavbarButtonDefault;
                button.ForeColor = System.Drawing.Color.White;
                button.Location = new System.Drawing.Point(0, Y);
                button.Margin = new System.Windows.Forms.Padding(0);
                button.Name = "btn" + option.ToString();
                button.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
                button.Size = new System.Drawing.Size(k_ButtonWidth, k_ButtonHeight);
                button.TabIndex = tabIndex++;
                button.Text = option.ToString();
                button.UseVisualStyleBackColor = true;
                button.Cursor = Cursors.Hand;
                Controls.Add(button);
                m_UserOptionButtons.Add(button);
                button.Click += new EventHandler(UserOptionButton_Click);

                Y += k_ButtonHeight;
            }
        }

        public void UserOptionButton_Click(object sender, EventArgs e)
        {
            selectUserOption((Button)sender);
        }

        public void AdjustUserOptionsSize()
        {
            Size = new Size(k_ButtonWidth, (k_ButtonHeight + 3) * m_UserOptionButtons.Count);
        }

        private void selectUserOption(Button i_Button) 
        {
            switch (i_Button.Name)
            {
                case "btnSettings":
                    {
                        if(m_OptionClickEvent != null)
                        {
                            m_OptionClickEvent.Invoke();
                        }

                        if(m_SettingsEvent != null)
                        {
                            m_SettingsEvent.Invoke();
                        }

                        break;
                    }

                case "btnLogout":
                    {
                        if (m_OptionClickEvent != null)
                        {
                            m_OptionClickEvent.Invoke();
                        }

                        DialogResult result = MessageBox.Show(
                            "Are you sure you want to logout?\n\nLogging out would delete any locally saved settings and cached data from your machine",
                            "Logout",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            if(m_LogoutEvent != null)
                            {
                                m_LogoutEvent.Invoke();
                            }
                        }

                        break;
                    }
            }
        }
    }
}
