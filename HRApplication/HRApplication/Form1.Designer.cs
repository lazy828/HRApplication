namespace HRApplication
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;



        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblProcess = new Label();
            txtProcessName = new TextBox();
            btnSelectProcess = new Button();
            btnAttach = new Button();
            lblStatus = new Label();
            lblAddress = new Label();
            txtAddress = new TextBox();
            lblType = new Label();
            cmbValueType = new ComboBox();
            btnRead = new Button();
            lblValue = new Label();
            btnFreeze = new Button();
            lblNewValue = new Label();
            txtNewValue = new TextBox();
            btnWrite = new Button();
            lblScanValue = new Label();
            txtScanValue = new TextBox();
            btnScan = new Button();
            btnNextScan = new Button();
            btnClearScan = new Button();
            listResults = new ListBox();
            btnStringScan = new Button();
            listStringResults = new ListBox();
            lblMemoryView = new Label();
            listMemoryView = new ListBox();
            label1 = new Label();
            txtStringScan = new TextBox();
            SuspendLayout();
            // 
            // lblProcess
            // 
            lblProcess.AutoSize = true;
            lblProcess.Location = new Point(20, 20);
            lblProcess.Name = "lblProcess";
            lblProcess.Size = new Size(50, 15);
            lblProcess.TabIndex = 0;
            lblProcess.Text = "Process:";
            // 
            // txtProcessName
            // 
            txtProcessName.Location = new Point(90, 17);
            txtProcessName.Name = "txtProcessName";
            txtProcessName.ReadOnly = true;
            txtProcessName.Size = new Size(280, 23);
            txtProcessName.TabIndex = 1;
            // 
            // btnSelectProcess
            // 
            btnSelectProcess.Location = new Point(380, 15);
            btnSelectProcess.Name = "btnSelectProcess";
            btnSelectProcess.Size = new Size(110, 30);
            btnSelectProcess.TabIndex = 2;
            btnSelectProcess.Text = "Select Process";
            btnSelectProcess.Click += btnSelectProcess_Click;
            // 
            // btnAttach
            // 
            btnAttach.Location = new Point(500, 15);
            btnAttach.Name = "btnAttach";
            btnAttach.Size = new Size(90, 30);
            btnAttach.TabIndex = 3;
            btnAttach.Text = "Attach";
            btnAttach.Click += btnAttach_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatus.ForeColor = Color.Green;
            lblStatus.Location = new Point(20, 55);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 19);
            lblStatus.TabIndex = 4;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(20, 90);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(83, 15);
            lblAddress.TabIndex = 5;
            lblAddress.Text = "Address (Hex):";
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(120, 87);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(160, 23);
            txtAddress.TabIndex = 6;
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Location = new Point(300, 90);
            lblType.Name = "lblType";
            lblType.Size = new Size(35, 15);
            lblType.TabIndex = 7;
            lblType.Text = "Type:";
            // 
            // cmbValueType
            // 
            cmbValueType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbValueType.Items.AddRange(new object[] { "Int32", "Float", "Double", "Int64", "Byte" });
            cmbValueType.Location = new Point(350, 87);
            cmbValueType.Name = "cmbValueType";
            cmbValueType.Size = new Size(110, 23);
            cmbValueType.TabIndex = 8;
            // 
            // btnRead
            // 
            btnRead.Location = new Point(20, 125);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(80, 35);
            btnRead.TabIndex = 9;
            btnRead.Text = "Read";
            btnRead.Click += btnRead_Click;
            // 
            // lblValue
            // 
            lblValue.AutoSize = true;
            lblValue.Font = new Font("Segoe UI", 10F);
            lblValue.Location = new Point(120, 130);
            lblValue.Name = "lblValue";
            lblValue.Size = new Size(0, 19);
            lblValue.TabIndex = 10;
            // 
            // btnFreeze
            // 
            btnFreeze.Location = new Point(480, 125);
            btnFreeze.Name = "btnFreeze";
            btnFreeze.Size = new Size(110, 35);
            btnFreeze.TabIndex = 11;
            btnFreeze.Text = "Freeze Value";
            btnFreeze.Click += btnFreeze_Click;
            // 
            // lblNewValue
            // 
            lblNewValue.AutoSize = true;
            lblNewValue.Location = new Point(20, 170);
            lblNewValue.Name = "lblNewValue";
            lblNewValue.Size = new Size(65, 15);
            lblNewValue.TabIndex = 12;
            lblNewValue.Text = "New Value:";
            // 
            // txtNewValue
            // 
            txtNewValue.Location = new Point(120, 167);
            txtNewValue.Name = "txtNewValue";
            txtNewValue.Size = new Size(160, 23);
            txtNewValue.TabIndex = 13;
            // 
            // btnWrite
            // 
            btnWrite.Location = new Point(320, 165);
            btnWrite.Name = "btnWrite";
            btnWrite.Size = new Size(90, 35);
            btnWrite.TabIndex = 14;
            btnWrite.Text = "Write";
            btnWrite.Click += btnWrite_Click;
            // 
            // lblScanValue
            // 
            lblScanValue.AutoSize = true;
            lblScanValue.Location = new Point(20, 210);
            lblScanValue.Name = "lblScanValue";
            lblScanValue.Size = new Size(84, 15);
            lblScanValue.TabIndex = 15;
            lblScanValue.Text = "Scan for Value:";
            // 
            // txtScanValue
            // 
            txtScanValue.Location = new Point(120, 207);
            txtScanValue.Name = "txtScanValue";
            txtScanValue.Size = new Size(130, 23);
            txtScanValue.TabIndex = 16;
            // 
            // btnScan
            // 
            btnScan.Location = new Point(260, 205);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(80, 35);
            btnScan.TabIndex = 17;
            btnScan.Text = "Scan";
            btnScan.Click += btnScan_Click;
            // 
            // btnNextScan
            // 
            btnNextScan.Location = new Point(350, 205);
            btnNextScan.Name = "btnNextScan";
            btnNextScan.Size = new Size(90, 35);
            btnNextScan.TabIndex = 18;
            btnNextScan.Text = "Next Scan";
            btnNextScan.Click += btnNextScan_Click;
            // 
            // btnClearScan
            // 
            btnClearScan.Location = new Point(450, 205);
            btnClearScan.Name = "btnClearScan";
            btnClearScan.Size = new Size(80, 35);
            btnClearScan.TabIndex = 19;
            btnClearScan.Text = "Clear";
            btnClearScan.Click += btnClearScan_Click;
            // 
            // listResults
            // 
            listResults.ItemHeight = 15;
            listResults.Location = new Point(20, 250);
            listResults.Name = "listResults";
            listResults.Size = new Size(510, 139);
            listResults.TabIndex = 20;
            listResults.DoubleClick += listResults_DoubleClick;
            // 
            // btnStringScan
            // 
            btnStringScan.Location = new Point(272, 407);
            btnStringScan.Name = "btnStringScan";
            btnStringScan.Size = new Size(120, 35);
            btnStringScan.TabIndex = 21;
            btnStringScan.Text = "Scan String";
            btnStringScan.Click += btnStringScan_Click;
            // 
            // listStringResults
            // 
            listStringResults.ItemHeight = 15;
            listStringResults.Location = new Point(20, 448);
            listStringResults.Name = "listStringResults";
            listStringResults.Size = new Size(510, 124);
            listStringResults.TabIndex = 22;
            listStringResults.DoubleClick += listStringResults_DoubleClick;
            // 
            // lblMemoryView
            // 
            lblMemoryView.AutoSize = true;
            lblMemoryView.Location = new Point(20, 582);
            lblMemoryView.Name = "lblMemoryView";
            lblMemoryView.Size = new Size(185, 15);
            lblMemoryView.TabIndex = 23;
            lblMemoryView.Text = "Memory around selected address:";
            // 
            // listMemoryView
            // 
            listMemoryView.ItemHeight = 15;
            listMemoryView.Location = new Point(20, 607);
            listMemoryView.Name = "listMemoryView";
            listMemoryView.Size = new Size(510, 154);
            listMemoryView.TabIndex = 24;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 417);
            label1.Name = "label1";
            label1.Size = new Size(84, 15);
            label1.TabIndex = 25;
            label1.Text = "Scan for Value:";
            // 
            // txtStringScan
            // 
            txtStringScan.Location = new Point(120, 414);
            txtStringScan.Name = "txtStringScan";
            txtStringScan.Size = new Size(130, 23);
            txtStringScan.TabIndex = 26;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(714, 791);
            Controls.Add(label1);
            Controls.Add(txtStringScan);
            Controls.Add(lblProcess);
            Controls.Add(txtProcessName);
            Controls.Add(btnSelectProcess);
            Controls.Add(btnAttach);
            Controls.Add(lblStatus);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(lblType);
            Controls.Add(cmbValueType);
            Controls.Add(btnRead);
            Controls.Add(lblValue);
            Controls.Add(btnFreeze);
            Controls.Add(lblNewValue);
            Controls.Add(txtNewValue);
            Controls.Add(btnWrite);
            Controls.Add(lblScanValue);
            Controls.Add(txtScanValue);
            Controls.Add(btnScan);
            Controls.Add(btnNextScan);
            Controls.Add(btnClearScan);
            Controls.Add(listResults);
            Controls.Add(btnStringScan);
            Controls.Add(listStringResults);
            Controls.Add(lblMemoryView);
            Controls.Add(listMemoryView);
            Name = "Form1";
            Text = "C# Memory Editor - RF Online";
            Load += Form1_Load_1;
            ResumeLayout(false);
            PerformLayout();
        }

        // Controls
        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.TextBox txtProcessName;
        private System.Windows.Forms.Button btnSelectProcess;
        private System.Windows.Forms.Button btnAttach;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbValueType;

        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Button btnFreeze;

        private System.Windows.Forms.Label lblNewValue;
        private System.Windows.Forms.TextBox txtNewValue;
        private System.Windows.Forms.Button btnWrite;

        private System.Windows.Forms.Label lblScanValue;
        private System.Windows.Forms.TextBox txtScanValue;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnNextScan;
        private System.Windows.Forms.Button btnClearScan;
        private System.Windows.Forms.ListBox listResults;

        private System.Windows.Forms.Button btnStringScan;
        private System.Windows.Forms.ListBox listStringResults;

        private System.Windows.Forms.Label lblMemoryView;
        private System.Windows.Forms.ListBox listMemoryView;
        private Label label1;
        private TextBox txtStringScan;
    }
}
