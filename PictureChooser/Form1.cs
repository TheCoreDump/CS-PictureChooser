using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureChooser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void MainListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void MainListView_DragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("Drag Drop");

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];

                foreach (string tmpFileName in fileNames)
                {
                    Debug.WriteLine($"Adding File {tmpFileName}");

                    FileInfo tmpFileInfo = new FileInfo(tmpFileName);
                    MainListView.Items.Add(new ListViewItem(tmpFileInfo.Name) { Name = tmpFileInfo.Name, Tag = tmpFileInfo });
                }
            }
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            using (PictureProcessor tmpProcessor = new PictureProcessor())
            {

                foreach (ListViewItem tmpItem in MainListView.Items)
                    tmpProcessor.ProcessPhoto(tmpItem.Tag as FileInfo);
            }
        }
    }
}
