﻿using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;

namespace PEUtility
{
    public partial class MainWindow : Form
    {
        private Executable _executable;
        private const int NumRecentFiles = 10;

        public MainWindow()
        {
            InitializeComponent();

            DragEnter += MainWindow_DragEnter;
            DragDrop += MainWindow_DragDrop;

            importSearchBox.TextChanged += importSearchBox_TextChanged;
            exportSearchBox.TextChanged += exportSearchBox_TextChanged;

            ReadRecentFiles();
        }

        private void ReadRecentFiles()
        {
            var key = Registry.CurrentUser.CreateSubKey("Software\\WinDisasm");
            int i;
            for (i = 1; i <= NumRecentFiles; i++)
            {
                var value = key.GetValue("Recent" + i) as string;
                if (value != null)
                {
                    var item = new ToolStripMenuItem(value, null, ItemClick, value);
                    recentToolStripMenuItem.DropDownItems.Add(item);
                }
            }
            key.Close();

            recentToolStripMenuItem.Enabled = recentToolStripMenuItem.HasDropDownItems;
        }

        void ItemClick(object sender, EventArgs e)
        {
            OpenFile((sender as ToolStripItem).Name);
        }

        private void StoreRecentFile()
        {
            // Move current file to front
            int index = recentToolStripMenuItem.DropDownItems.IndexOfKey(_executable.Filename);
            ToolStripItem item;
            if (index != -1)
            {
                item = recentToolStripMenuItem.DropDownItems[index];
                recentToolStripMenuItem.DropDownItems.RemoveAt(index);
            }
            else
            {
                item = new ToolStripMenuItem(_executable.Filename, null, ItemClick, _executable.Filename);
            }
            recentToolStripMenuItem.DropDownItems.Insert(0, item);

            // Prune recent list
            while (recentToolStripMenuItem.DropDownItems.Count > NumRecentFiles)
            {
                recentToolStripMenuItem.DropDownItems.RemoveAt(NumRecentFiles);
            }

            // Rewrite registry entries
            var key = Registry.CurrentUser.CreateSubKey("Software\\WinDisasm");
            int i;
            for (i = 1; i <= NumRecentFiles; i++)
            {
                key.DeleteValue("Recent" + i, false);
            }
            i = 1;
            foreach (var recent in recentToolStripMenuItem.DropDownItems)
            {
                key.SetValue("Recent" + i, recent);
                i++;
            }
            key.Close();

            recentToolStripMenuItem.Enabled = true;
        }

        void importSearchBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(importSearchBox.Text))
            {
                ShowAllImports();
                return;
            }

            importsList.Nodes.Clear();
            foreach (var importEntry in _executable.ImportEntries)
            {
                var node = importsList.Nodes.Add(importEntry.Name);
                bool hasMatch = false;
                foreach (var entry in importEntry.Entries)
                {
                    if (entry.IndexOf(importSearchBox.Text, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        node.Nodes.Add(entry);
                        hasMatch = true;
                    }
                }
                if (hasMatch)
                {
                    node.ForeColor = Color.Black;
                    node.Expand();
                }
                else
                {
                    node.ForeColor = Color.Gray;
                }
            }
        }

        void exportSearchBox_TextChanged(object sender, EventArgs e)
        {
            exportsList.Clear();
            foreach (var exportEntry in _executable.ExportEntries)
            {
                if (exportEntry.Name.IndexOf(exportSearchBox.Text, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    exportsList.Items.Add(exportEntry.Name);
                }
            }
        }

        void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            OpenFile(files[0]);
        }

        void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "All Executables (*.exe, *.dll)|*.exe;*.dll|EXE files (*.exe)|*.exe|DLL files (*.dll)|*.dll|All files (*.*)|*.*";
            dialog.ValidateNames = false;
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                OpenFile(dialog.FileName);
            }
        }

        private void ShowAllImports()
        {
            importsList.Nodes.Clear();
            foreach (var importEntry in _executable.ImportEntries)
            {
                var node = importsList.Nodes.Add(importEntry.Name);
                foreach (var entry in importEntry.Entries)
                {
                    node.Nodes.Add(entry);
                }
            }
        }

        private void OpenFile(string filename)
        {
            if (_executable != null)
                _executable.Close();

            var newExecutable = new Executable(filename);
            if (!newExecutable.IsValid)
                return;

            _executable = newExecutable;
            Text = Path.GetFileName(filename) + " - PE Disassembler";

            StoreRecentFile();

            ShowAllImports();
            importSearchBox.Enabled = true;

            exportsList.Clear();
            foreach (var exportEntry in _executable.ExportEntries)
            {
                exportsList.Items.Add(exportEntry.Name);
            }
            exportSearchBox.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_executable != null)
                _executable.Close();

            Close();
        }
    }
}
