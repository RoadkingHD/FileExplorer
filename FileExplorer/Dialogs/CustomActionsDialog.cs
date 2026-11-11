using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using FileExplorer.Models;
using FileExplorer.Services;

namespace FileExplorer.Dialogs
{
    public partial class CustomActionsDialog : Form
    {
        private readonly BindingList<CustomAction> _actions;

        public CustomActionsDialog()
        {
            InitializeComponent();
            _actions = new BindingList<CustomAction>(CustomActionStore.Load());
            dataGridViewActions.DataSource = _actions;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            _actions.Add(new CustomAction
            {
                Name = "New action",
                ExtensionFilter = ".txt",
                FilePath = "notepad.exe",
                Arguments = "{file}",
                RunPerFile = true
            });

            dataGridViewActions.CurrentCell = dataGridViewActions.Rows[dataGridViewActions.Rows.Count - 1].Cells[0];
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridViewActions.CurrentRow != null && dataGridViewActions.CurrentRow.Index >= 0 && dataGridViewActions.CurrentRow.Index < _actions.Count)
            {
                _actions.RemoveAt(dataGridViewActions.CurrentRow.Index);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            dataGridViewActions.EndEdit();

            CustomAction invalid = _actions.FirstOrDefault(a => string.IsNullOrWhiteSpace(a.Name));

            if (invalid != null)
            {
                MessageBox.Show(this, "Every custom action needs a name.", "Custom actions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            try
            {
                CustomActionStore.Save(_actions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not save the custom actions.\r\n\r\n" + ex.Message, "Custom actions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
        }
    }
}
