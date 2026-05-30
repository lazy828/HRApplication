using System.Diagnostics;

namespace HRApplication
{
    public partial class ProcessSelectorForm : Form
    {
        public ProcessSelectorForm()
        {
            InitializeComponent();
            LoadProcesses();
        }

        public string SelectedProcessName { get; private set; }
        public int SelectedProcessId { get; private set; }

        private void LoadProcesses()
        {
            listView.Items.Clear();
            var processes = Process.GetProcesses()
                .OrderBy(p => p.ProcessName)
                .Select(p => new
                {
                    Name = p.ProcessName,
                    Id = p.Id,
                    Title = p.MainWindowTitle ?? ""
                });

            foreach (var p in processes)
            {
                ListViewItem item = new ListViewItem(p.Name);
                item.SubItems.Add(p.Id.ToString());
                item.SubItems.Add(p.Title);
                item.Tag = p.Id;
                listView.Items.Add(item);
            }
        }

        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                SelectedProcessName = listView.SelectedItems[0].Text;
                SelectedProcessId = (int)listView.SelectedItems[0].Tag;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                SelectedProcessName = listView.SelectedItems[0].Text;
                SelectedProcessId = (int)listView.SelectedItems[0].Tag;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a process.");
            }
        }
    }
}
