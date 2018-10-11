using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;

namespace NVS.Components
{
    /// <summary>
    /// A custom version of the TreeView components which 
    /// automatically loads and displays all the directories
    /// and its respective files.
    /// </summary>
    public class NVSTreeView : TreeView
    {

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowTheme(this.Handle, "explorer", null);
        }

        public NVSTreeView()
        {
            this.StartingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.PopulateTreeView();
        }

        /// <summary>
        /// Gets or sets the directory which will be the 
        /// root of the treeview.
        /// </summary>
        [Description("Gets or sets the root path of the working directory.")]
        [Category("Data")]
        public string StartingDirectory { get; set; }

        /// <summary>
        /// Populates the treeview with all the directories
        /// and files contained within.
        /// </summary>
        public void PopulateTreeView()
        {
            this.ListFilesAndDirectories(this, this.StartingDirectory);
        }
        
        private void ListFilesAndDirectories(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();

            DirectoryInfo rootDir = new DirectoryInfo(path);
            treeView.Nodes.Add(GetFileTree(rootDir));
        }

        private TreeNode GetFileTree(DirectoryInfo dir)
        {
            TreeNode root = new TreeNode(dir.Name);
            foreach(DirectoryInfo directory in dir.GetDirectories())
            {
                root.Nodes.Add(GetFileTree(directory));
            }

            foreach(FileInfo file in dir.GetFiles())
            {
                root.Nodes.Add(new TreeNode(file.Name));
            }

            return root;
        }

    }
}
