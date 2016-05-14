using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using ZemnaCmd.Core;

namespace ZemnaCmdConfig
{
    public partial class frmConfig : Form
    {
        private bool _modified = false;

        public frmConfig()
        {
            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            XmlManager xm = new XmlManager();

            var paths = xm.LoadPaths();

            lbPath.Items.Clear();

            foreach (var path in paths)
            {
                lbPath.Items.Add(path);
            }

            EnableControls(0);

            _modified = false;
        }

        private void SaveConfig()
        {
            XmlManager xm = new XmlManager();

            List<String> paths = new List<string>();

            foreach (object item in lbPath.Items)
            {
                paths.Add(item.ToString());
            }

            xm.SavePaths(paths);

            _modified = false;
        }

        private void EnableControls(int type)
        {
            btnEdit.Enabled = type == 1 ? true : false;
            btnRemove.Enabled = type == 0 ? false : true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (lbPath.Items.Contains(folderBrowserDialog1.SelectedPath) == true)
                {
                    MessageBox.Show("The path has already existed", "Add");
                    return;
                }

                lbPath.Items.Add(folderBrowserDialog1.SelectedPath);
                
                _modified = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = lbPath.SelectedItem.ToString();

            // 폴더 선택 창 띄우기
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                // 현재 선택된 폴더가 이미 리스트에 존재할 경우
                if (lbPath.Items.Contains(folderBrowserDialog1.SelectedPath) == true)
                {
                    // 리스트에서 선택된 항목일 경우
                    if (lbPath.Items.IndexOf(folderBrowserDialog1.SelectedPath) == lbPath.SelectedIndex)
                        return;

                    MessageBox.Show("The path has already existed", "Edit");
                    return;
                }

                lbPath.Items[lbPath.SelectedIndex] = folderBrowserDialog1.SelectedPath;
                
                _modified = true;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Remove", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                for (int i = lbPath.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    lbPath.Items.RemoveAt(lbPath.SelectedIndices[i]);
                }

                _modified = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                lbPath.Items.Clear();

                _modified = true;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                LoadConfig();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();

            Application.Exit();
        }

        private void lbPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lbPath.SelectedIndices.Count)
            {
                case 0:
                    EnableControls(0);
                    break;
                case 1:
                    EnableControls(1);
                    break;
                default:
                    EnableControls(2);
                    break;
            }
        }

        private void frmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_modified == true)
            {
                DialogResult ret = MessageBox.Show("Save changes?", "Close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (ret)
                {
                    case DialogResult.Yes:
                        SaveConfig();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void lbPath_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (lbPath.SelectedItem != null)
            {
                btnEdit.PerformClick();
            }
        }
    }
}
