using System;
using System.Windows.Forms;

namespace FileExplorer.Dialogs
{
    public partial class InputDialog : Form
    {
        public InputDialog(string title, string prompt, string defaultValue)
        {
            InitializeComponent();
            Text = title;
            labelPrompt.Text = prompt;
            textBoxValue.Text = defaultValue;
        }

        public string Value
        {
            get { return textBoxValue.Text.Trim(); }
        }
    }
}
