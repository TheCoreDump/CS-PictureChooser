using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureChooser
{
    public partial class PictureChooserForm : Form
    {
        private int PictureIndex = 0;
        private Dictionary<int, FileInfo> PictureCollection = new Dictionary<int, FileInfo>();

        public PictureChooserForm()
        {
            InitializeComponent();
        }

        private void SelectFolderButton_Click(object sender, EventArgs e)
        {
            MainFolderBrowser.SelectedPath = "D:\\Google Drive\\Pictures\\Family";

            MainFolderBrowser.ShowDialog();

            SetFolder(new DirectoryInfo(MainFolderBrowser.SelectedPath));
        }

        public void SetFolder(DirectoryInfo directory)
        {
            PictureIndex = 0;
            PictureCollection.Clear();

            int counter = 0;

            foreach (FileInfo tmpFile in directory.GetFiles("*.jpg").ToList().OrderBy(fi => fi.Name))
                PictureCollection.Add(counter++, tmpFile);

            ShowCurrentPicture();
        }

        private void ShowCurrentPicture()
        {
            if (PictureCollection.ContainsKey(PictureIndex))
            {
                MainPictureBox.Image = Image.FromFile(PictureCollection[PictureIndex].FullName);
            }
        }

        private void PictureChooserForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                MoveRight();
        }

        private void PictureChooserForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Left))
            {
                e.IsInputKey = true;
            }
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {

        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            MoveRight();
        }

        public void MoveLeft()
        {

        }

        public void MoveRight()
        {
            if (PictureIndex < (PictureCollection.Count - 1))
            {
                PictureIndex++;
                ShowCurrentPicture();
            }
        }

        private void PictureChooserForm_Load(object sender, EventArgs e)
        {

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                    MoveRight();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
