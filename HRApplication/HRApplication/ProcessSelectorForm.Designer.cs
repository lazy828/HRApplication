namespace HRApplication
{
    partial class ProcessSelectorForm
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
            this.Text = "Select Process";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            listView = new ListView();
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Dock = DockStyle.Fill;
            listView.DoubleClick += ListView_DoubleClick;

            listView.Columns.Add("Process Name", 250);
            listView.Columns.Add("PID", 100);
            listView.Columns.Add("Window Title", 300);

            this.Controls.Add(listView);

            // OK Button
            Button btnOK = new Button();
            btnOK.Text = "Attach";
            btnOK.Location = new Point(520, 430);
            btnOK.Size = new Size(120, 35);
            btnOK.Click += BtnOK_Click;
            this.Controls.Add(btnOK);
        }

        private ListView listView;

        #endregion
    }
}