using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace HRApplication
{
    public partial class Form1 : Form
    {
        private Memory mem = new Memory();
        private List<FreezeItem> frozenValues = new List<FreezeItem>();
        private Thread freezeThread;
        private bool isFreezing = false;

        private List<IntPtr> scanResults = new List<IntPtr>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbValueType.SelectedIndex = 0;
            txtAddress.Text = "0x00000000";
        }

        // ====================== Process Selection ======================
        private void btnSelectProcess_Click(object sender, EventArgs e)
        {
            using (ProcessSelectorForm selector = new ProcessSelectorForm())
            {
                if (selector.ShowDialog() == DialogResult.OK)
                {
                    txtProcessName.Text = selector.SelectedProcessName;

                    if (mem.AttachToProcessById(selector.SelectedProcessId))
                    {
                        lblStatus.Text = $"✅ Attached Successfully! | {selector.SelectedProcessName}";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "❌ Failed to attach. Try running as Administrator.";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please use 'Select Process' button.", "Info");
        }

        // ====================== Read / Write ======================
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                IntPtr addr = new IntPtr(Convert.ToInt64(txtAddress.Text.Replace("0x", ""), 16));
                string type = cmbValueType.SelectedItem?.ToString() ?? "Int32";
                object value = ReadValue(addr, type);
                lblValue.Text = $"Value: {value}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read Error: " + ex.Message);
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtNewValue.Text))
            {
                MessageBox.Show("Please enter Address and New Value.");
                return;
            }

            try
            {
                IntPtr addr = new IntPtr(Convert.ToInt64(txtAddress.Text.Replace("0x", ""), 16));
                string type = cmbValueType.SelectedItem?.ToString() ?? "Int32";

                bool success = WriteValue(addr, txtNewValue.Text, type);

                if (success)
                {
                    MessageBox.Show($"✅ Successfully written {txtNewValue.Text}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Optional: Read back to verify
                    object readBack = ReadValue(addr, type);
                    lblValue.Text = $"Value: {readBack} (Verified)";
                }
                else
                {
                    MessageBox.Show("❌ Write failed. Memory might be protected.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Write Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================== Freeze Related Members (Add at the top of Form1 class) ======================

        // ====================== Updated btnFreeze_Click ======================
        private void btnFreeze_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtNewValue.Text))
            {
                MessageBox.Show("Please fill Address and New Value first.", "Warning");
                return;
            }

            try
            {
                IntPtr addr = new IntPtr(Convert.ToInt64(txtAddress.Text.Replace("0x", ""), 16));
                string type = cmbValueType.SelectedItem?.ToString() ?? "Int32";

                frozenValues.Add(new FreezeItem
                {
                    Address = addr,
                    Type = type,
                    Value = txtNewValue.Text
                });

                if (!isFreezing)
                {
                    isFreezing = true;
                    freezeThread = new Thread(FreezeLoop) { IsBackground = true };
                    freezeThread.Start();
                }

                MessageBox.Show($"✅ Value frozen at {txtAddress.Text}!", "Freeze Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding to freeze: " + ex.Message, "Error");
            }
        }

        // ====================== Freeze Loop ======================
        private void FreezeLoop()
        {
            while (isFreezing && frozenValues.Count > 0)
            {
                foreach (var item in frozenValues.ToList()) // ToList to avoid modification issues
                {
                    WriteValue(item.Address, item.Value, item.Type);
                }
                Thread.Sleep(8); // Fast freeze (adjust if needed)
            }
        }

        // ====================== Freeze Item Class ======================
        private class FreezeItem
        {
            public IntPtr Address;
            public string Type;
            public string Value;
        }

        // This was wokring
        //private void btnScan_Click(object sender, EventArgs e)
        //{
        //    if (!long.TryParse(txtScanValue.Text, out long value))
        //    {
        //        MessageBox.Show("Please enter a valid number.");
        //        return;
        //    }

        //    bool useFloat = cmbValueType.SelectedItem?.ToString() == "Float";

        //    listResults.Items.Clear();
        //    listResults.Items.Add($"Scanning for {value} as {(useFloat ? "Float" : "Int32")}...");

        //    ThreadPool.QueueUserWorkItem(_ =>
        //    {
        //        scanResults = mem.FindAllValues(value, useFloat);

        //        this.Invoke(new Action(() =>
        //        {
        //            listResults.Items.Clear();
        //            listResults.Items.Add($"Scan Complete - Found {scanResults.Count} results");

        //            if (scanResults.Count > 0)
        //            {
        //                foreach (var addr in scanResults.Take(40))
        //                    listResults.Items.Add($"0x{addr.ToString("X16")}");
        //            }
        //            else
        //            {
        //                listResults.Items.Add("0 results found.");
        //                listResults.Items.Add("Try both Float and Int32.");
        //            }
        //        }));
        //    });
        //}btnNextScan_Click

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!long.TryParse(txtScanValue.Text, out long value))
            {
                MessageBox.Show("Please enter a valid number.");
                return;
            }

            bool useFloat = cmbValueType.SelectedItem?.ToString() == "Float";

            listResults.Items.Clear();
            listResults.Items.Add("Starting scan using VirtualQuery (like Cheat Engine)...");

            ThreadPool.QueueUserWorkItem(_ =>
            {
                scanResults = mem.FindAllValues(value, useFloat);

                this.Invoke(new Action(() =>
                {
                    listResults.Items.Clear();

                    if (scanResults.Count == 0)
                    {
                        listResults.Items.Add("Scan Complete - Found 0 results");
                        listResults.Items.Add("Try changing Type to Float / Int32");
                    }
                    else
                    {
                        listResults.Items.Add($"✅ Scan Complete - Found {scanResults.Count} results");

                        foreach (var addr in scanResults.Take(50))
                        {
                            listResults.Items.Add($"0x{addr.ToString("X16")}");
                        }
                    }
                }));
            });
        }

        private void btnNextScan_Click(object sender, EventArgs e)
        {
            if (scanResults.Count == 0)
            {
                MessageBox.Show("Please perform a First Scan first.", "Info");
                return;
            }

            if (!long.TryParse(txtScanValue.Text, out long newValue))
            {
                MessageBox.Show("Please enter the new value for Next Scan.");
                return;
            }

            string selectedType = cmbValueType.SelectedItem?.ToString() ?? "Int32";

            listResults.Items.Clear();
            listResults.Items.Add("Performing Next Scan...");

            ThreadPool.QueueUserWorkItem(_ =>
            {
                List<IntPtr> filtered = new List<IntPtr>();

                foreach (var addr in scanResults)
                {
                    try
                    {
                        bool match = false;

                        if (selectedType == "Float")
                        {
                            float currentValue = BitConverter.ToSingle(mem.ReadMemory(addr, 4), 0);
                            if (Math.Abs(currentValue - newValue) < 5.0f)
                                match = true;
                        }
                        else // Int32 or others
                        {
                            int currentValue = BitConverter.ToInt32(mem.ReadMemory(addr, 4), 0);
                            if (currentValue == newValue)
                                match = true;
                        }

                        if (match)
                            filtered.Add(addr);
                    }
                    catch { }
                }

                scanResults = filtered;

                this.Invoke(new Action(() =>
                {
                    listResults.Items.Clear();
                    listResults.Items.Add($"Next Scan Complete - {scanResults.Count} results remaining");

                    foreach (var addr in scanResults.Take(40))
                    {
                        listResults.Items.Add($"0x{addr.ToString("X16")}");
                    }

                    if (scanResults.Count == 0)
                        listResults.Items.Add("No matching addresses found.");
                }));
            });
        }

        private void btnClearScan_Click(object sender, EventArgs e)
        {
            scanResults.Clear();
            listResults.Items.Clear();
        }

        // ====================== String Scanner (IMPLEMENTED) ======================
        private void btnStringScan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStringScan.Text))
            {
                MessageBox.Show("Please enter your character name.");
                return;
            }

            string searchText = txtStringScan.Text.Trim();
            listStringResults.Items.Clear();
            listStringResults.Items.Add($"Searching for '{searchText}'...");

            ThreadPool.QueueUserWorkItem(_ =>
            {
                var results = mem.FindAllStrings(searchText);

                this.Invoke(new Action(() =>
                {
                    listStringResults.Items.Clear();

                    if (results.Count == 0)
                    {
                        listStringResults.Items.Add("No results found.");
                    }
                    else
                    {
                        listStringResults.Items.Add($"Found {results.Count} result(s) for '{searchText}':");
                        listStringResults.Items.Add("────────────────────────────────");

                        foreach (var addr in results)
                        {
                            listStringResults.Items.Add($"0x{addr.ToString("X8")}");
                        }
                    }
                }));
            });
        }

        // ====================== Helper Methods ======================
        private object ReadValue(IntPtr address, string type)
        {
            try
            {
                switch (type)
                {
                    case "Int32":
                        return BitConverter.ToInt32(mem.ReadMemory(address, 4), 0);
                    case "Float":
                        return BitConverter.ToSingle(mem.ReadMemory(address, 4), 0);
                    case "Double":
                        return BitConverter.ToDouble(mem.ReadMemory(address, 8), 0);
                    default:
                        return 0;
                }
            }
            catch { return "Error"; }
        }

        private bool WriteValue(IntPtr address, string input, string type)
        {
            try
            {
                byte[] bytes;
                switch (type)
                {
                    case "Int32":
                        bytes = BitConverter.GetBytes(int.Parse(input));
                        break;
                    case "Float":
                        bytes = BitConverter.GetBytes(float.Parse(input));
                        break;
                    case "Double":
                        bytes = BitConverter.GetBytes(double.Parse(input));
                        break;
                    default:
                        return false;
                }

                return mem.WriteMemory(address, bytes);
            }
            catch
            {
                return false;
            }
        }


        private void listResults_DoubleClick(object sender, EventArgs e)
        {
            if (listResults.SelectedItem == null) return;
            string line = listResults.SelectedItem.ToString();
            if (line.Contains("0x"))
                txtAddress.Text = line.Trim();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            cmbValueType.SelectedIndex = 0;
        }

        // Double-click on String Scan Result → Show Memory Around
        private void listStringResults_DoubleClick(object sender, EventArgs e)
        {
            if (listStringResults.SelectedItem == null) return;

            string line = listStringResults.SelectedItem.ToString();

            if (line.Contains("0x"))
            {
                string addressStr = line.Trim();
                txtAddress.Text = addressStr;

                // Show memory around the selected address
                ShowMemoryAround(addressStr);

                MessageBox.Show($"Address loaded: {addressStr}\n\nCheck 'Memory around selected address' below.", "Success");
            }
        }

        // Show Memory Around Selected Address
        private void ShowMemoryAround(string hexAddress)
        {
            listMemoryView.Items.Clear();

            try
            {
                IntPtr baseAddr = new IntPtr(Convert.ToInt64(hexAddress.Replace("0x", ""), 16));

                listMemoryView.Items.Add($"Memory dump around {hexAddress}:");
                listMemoryView.Items.Add("────────────────────────────────");

                for (int i = -0x80; i <= 0x180; i += 4)
                {
                    IntPtr addr = new IntPtr(baseAddr.ToInt64() + i);
                    byte[] data = mem.ReadMemory(addr, 4);
                    int value = BitConverter.ToInt32(data, 0);

                    listMemoryView.Items.Add($"0x{addr.ToString("X8")}  =  {value}");
                }

                var sb = new System.Text.StringBuilder();

                foreach (var item in listMemoryView.Items)
                {
                    sb.AppendLine(item.ToString());
                    // Or use string.Join to get subitems like in the previous example
                }

                string allValues = sb.ToString();
            }
            catch (Exception ex)
            {
                listMemoryView.Items.Add("Error reading memory: " + ex.Message);
            }
        }
    }
}