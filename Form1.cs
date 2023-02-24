using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ShowPressedKeyOnScreen
{
    public partial class Form1 : Form
    {
        List<string> PressedKeys = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        #region Mouse move codes
        private Point _mouseLoc;
        private void FormMouseDown(object sender, MouseEventArgs e)
        {
            _mouseLoc = e.Location;
        }
        private void FormMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.Location.X - _mouseLoc.X;
                int dy = e.Location.Y - _mouseLoc.Y;
                this.Location = new Point(this.Location.X + dx, this.Location.Y + dy);
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            Point loc = new Point(Screen.AllScreens[0].WorkingArea.Width - this.Width, Screen.AllScreens[0].WorkingArea.Height - (this.Height + 20));
            this.Location = loc;

            // Watch for keyboard  activity
            KeyboardListener.s_KeyEventHandler += new EventHandler(KeyboardListener_s_KeyEventHandler);
        }

        private void KeyboardListener_s_KeyEventHandler(object sender, EventArgs e)
        {
            KeyboardListener.UniversalKeyEventArgs eventArgs = (KeyboardListener.UniversalKeyEventArgs)e;

            if (eventArgs.m_Msg == 256) // key down and while press
            {
                //Debug.WriteLine($"EventMsg: KeyDown  KeyValue: {eventArgs.KeyValue} KeyData: {eventArgs.KeyData} KeyCode: {eventArgs.KeyCode}");
                PressedKeys.Add(eventArgs.KeyData.ToString());
                PressedKeys = RemoveDuplicates(PressedKeys);
                lblKeyPressed.Text = " ";
                foreach (var item in PressedKeys)
                {
                    lblKeyPressed.Text += KeysToString(item) + " ";
                }
            }
            else
            {
                PressedKeys.Add(eventArgs.KeyData.ToString());
                PressedKeys = RemoveDuplicates(PressedKeys);
                lblKeyPressed.Text = " ";
                foreach (var item in PressedKeys)
                {
                    lblKeyPressed.Text += KeysToString(item) + " ";
                }
            }

            if (eventArgs.m_Msg == 257) // key up
            {
                //Debug.WriteLine($"EventMsg: KeyUp  KeyValue: {eventArgs.KeyValue} KeyData: {eventArgs.KeyData} KeyCode: {eventArgs.KeyCode}");
                PressedKeys = RemoveDuplicates(PressedKeys);
                PressedKeys.Remove(eventArgs.KeyData.ToString());
                lblKeyPressed.Text = " ";
                foreach (var item in PressedKeys)
                {
                    lblKeyPressed.Text += KeysToString(item) + " ";
                }
            }
        }
        public List<string> RemoveDuplicates(List<string> list)
        {
            List<string> result = new List<string>();
            foreach (string item in list)
            {
                if (!result.Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public string KeysToString(string keyData)
        {
            KeysConverter converter = new KeysConverter();
            Keys keysdata = (Keys)converter.ConvertFromString(keyData);
            Keys key = keysdata & Keys.KeyCode;

            if (key >= Keys.D0 && key <= Keys.D9)
            {
                return (key - Keys.D0).ToString();
            }
            else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                return (key - Keys.NumPad0).ToString();
            }
            else
            {
                switch (key)
                {
                    case Keys.Oemtilde:
                        return "`";
                    case Keys.OemMinus:
                        return "-";
                    case Keys.Oemplus:
                        return "=";
                    case Keys.OemOpenBrackets:
                        return "[";
                    case Keys.OemCloseBrackets:
                        return "]";
                    case Keys.OemPipe:
                        return "\\";
                    case Keys.OemSemicolon:
                        return ";";
                    case Keys.OemQuotes:
                        return "'";
                    case Keys.Oemcomma:
                        return ",";
                    case Keys.OemPeriod:
                        return ".";
                    case Keys.OemQuestion:
                        return "/";
                    default:
                        return key.ToString();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PressedKeys.Clear();
            lblKeyPressed.Text = " ";
        }
    }
}
