using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;
using System.Drawing;

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

        #region Overridden Properties

        private List<TreeNode> selectedNodes = null;
        private TreeNode selectedNode = null;

        /// <summary>
        /// Gets the list of selected node in the current context.
        /// </summary>
        public List<TreeNode> SelectedNodes
        {
            get
            {
                return selectedNodes;
            } 

            set
            {
                ClearSelectedNodes();

                if(value != null)
                {
                    foreach(TreeNode node in selectedNodes)
                    {
                        ToggleNode(node, true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the currently selected node.
        /// </summary>
        public new TreeNode SelectedNode
        {
            get
            {
                return selectedNode;
            }
            set
            {
                ClearSelectedNodes();

                if(value != null)
                {
                    SelectNode(value);
                }
            }
        }

        #endregion

        #region Overridden Events

        protected override void OnGotFocus(EventArgs e)
        {
            try
            {
                if ( selectedNode == null && this.TopNode != null )
                {
                    ToggleNode(this.TopNode, true);
                }

                base.OnGotFocus(e);
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                base.SelectedNode = null;

                TreeNode node = this.GetNodeAt(e.Location);
                if(node != null)
                {
                    int leftOffset = node.Bounds.X;
                    int rightOffset = node.Bounds.Right + 10;

                    if(e.Location.X > leftOffset && e.Location.Y < rightOffset)
                    {
                        if (ModifierKeys == Keys.None && selectedNodes.Contains(node)) { }
                        else
                        {
                            SelectNode(node);
                        }
                    }
                }

                base.OnMouseDown(e);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                base.SelectedNode = null;

                TreeNode node = this.GetNodeAt(e.Location);
                if (node != null)
                {
                    int leftOffset = node.Bounds.X;
                    int rightOffset = node.Bounds.Right + 10;

                    if (e.Location.X > leftOffset && e.Location.Y < rightOffset)
                    {
                        if (ModifierKeys == Keys.None && selectedNodes.Contains(node))
                        {
                            SelectNode(node);
                        }
                    }
                }

                base.OnMouseUp(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            try
            {
                TreeNode node = e.Item as TreeNode;

                if (node != null)
                {
                    if (!selectedNodes.Contains(node))
                    {
                        SelectSingleNode(node);
                        ToggleNode(node, true);
                    }
                }

                base.OnItemDrag(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            try
            {
                base.SelectedNode = null;
                e.Cancel = true;

                base.OnBeforeSelect(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            try
            {
                base.OnAfterSelect(e);
                base.SelectedNode = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.ShiftKey) return;
           
            bool bShift = (ModifierKeys == Keys.Shift);

            try
            {
                if (selectedNodes == null && this.TopNode != null)
                {
                    ToggleNode(this.TopNode, true);
                }
                
                if (selectedNodes == null) return;

                if (e.KeyCode == Keys.Left)
                {
                    if (selectedNode.IsExpanded && selectedNode.Nodes.Count > 0)
                    {
                        selectedNode.Collapse();
                    }
                    else if (selectedNode.Parent != null)
                    {
                        SelectSingleNode(selectedNode.Parent);
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (!selectedNode.IsExpanded)
                    {
                        selectedNode.Expand();
                    }
                    else
                    {
                        SelectSingleNode(selectedNode.FirstNode);
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (selectedNode.PrevVisibleNode != null)
                    {
                        SelectNode(selectedNode.PrevVisibleNode);
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (selectedNode.NextVisibleNode != null)
                    {
                        SelectNode(selectedNode.NextVisibleNode);
                    }
                }
                else if (e.KeyCode == Keys.Home)
                {
                    if (bShift)
                    {
                        if (selectedNode.Parent == null)
                        {
                            if (this.Nodes.Count > 0)
                            {
                                SelectNode(this.Nodes[0]);
                            }
                        }
                        else
                        {
                            SelectNode(selectedNode.Parent.FirstNode);
                        }
                    }
                    else
                    {
                        if (this.Nodes.Count > 0)
                        {
                            SelectSingleNode(this.Nodes[0]);
                        }
                    }
                }
                else if (e.KeyCode == Keys.End)
                {
                    if (bShift)
                    {
                        if (selectedNode.Parent == null)
                        {
                            if (this.Nodes.Count > 0)
                            {
                                SelectNode(this.Nodes[this.Nodes.Count - 1]);
                            }
                        }
                        else
                        {
                            SelectNode(selectedNode.Parent.LastNode);
                        }
                    }
                    else
                    {
                        if (this.Nodes.Count > 0)
                        {
                            TreeNode ndLast = this.Nodes[0].LastNode;
                            while (ndLast.IsExpanded && (ndLast.LastNode != null))
                            {
                                ndLast = ndLast.LastNode;
                            }
                            SelectSingleNode(ndLast);
                        }
                    }
                }
                else if (e.KeyCode == Keys.PageUp)
                {
                    int nCount = this.VisibleCount;
                    TreeNode ndCurrent = selectedNode;
                    while ((nCount) > 0 && (ndCurrent.PrevVisibleNode != null))
                    {
                        ndCurrent = ndCurrent.PrevVisibleNode;
                        nCount--;
                    }
                    SelectSingleNode(ndCurrent);
                }
                else if (e.KeyCode == Keys.PageDown)
                {
                    int nCount = this.VisibleCount;
                    TreeNode ndCurrent = selectedNode;
                    while ((nCount) > 0 && (ndCurrent.NextVisibleNode != null))
                    {
                        ndCurrent = ndCurrent.NextVisibleNode;
                        nCount--;
                    }
                    SelectSingleNode(ndCurrent);
                }
                else
                {
                    string sSearch = ((char)e.KeyValue).ToString();

                    TreeNode ndCurrent = selectedNode;
                    while ((ndCurrent.NextVisibleNode != null))
                    {
                        ndCurrent = ndCurrent.NextVisibleNode;
                        if (ndCurrent.Text.StartsWith(sSearch))
                        {
                            SelectSingleNode(ndCurrent);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        #endregion

        #region Helper Methods

        private void SelectNode(TreeNode node)
        {
            try
            {
                this.BeginUpdate();

                if (selectedNode == null || ModifierKeys == Keys.Control)
                {
                    bool bIsSelected = selectedNodes.Contains(node);
                    ToggleNode(node, !bIsSelected);
                }
                else if (ModifierKeys == Keys.Shift)
                {
                    TreeNode ndStart = selectedNode;
                    TreeNode ndEnd = node;

                    if (ndStart.Parent == ndEnd.Parent)
                    {
                        if (ndStart.Index < ndEnd.Index)
                        {
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.NextVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                        else if (ndStart.Index == ndEnd.Index) { }
                        else
                        {
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.PrevVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                    }
                    else
                    {
                        TreeNode ndStartP = ndStart;
                        TreeNode ndEndP = ndEnd;
                        int startDepth = Math.Min(ndStartP.Level, ndEndP.Level);
                        
                        while (ndStartP.Level > startDepth)
                        {
                            ndStartP = ndStartP.Parent;
                        }
                        
                        while (ndEndP.Level > startDepth)
                        {
                            ndEndP = ndEndP.Parent;
                        }
                        
                        while (ndStartP.Parent != ndEndP.Parent)
                        {
                            ndStartP = ndStartP.Parent;
                            ndEndP = ndEndP.Parent;
                        }
                        
                        if (ndStartP.Index < ndEndP.Index)
                        {
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.NextVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                        else if (ndStartP.Index == ndEndP.Index)
                        {
                            if (ndStart.Level < ndEnd.Level)
                            {
                                while (ndStart != ndEnd)
                                {
                                    ndStart = ndStart.NextVisibleNode;
                                    if (ndStart == null) break;
                                    ToggleNode(ndStart, true);
                                }
                            }
                            else
                            {
                                while (ndStart != ndEnd)
                                {
                                    ndStart = ndStart.PrevVisibleNode;
                                    if (ndStart == null) break;
                                    ToggleNode(ndStart, true);
                                }
                            }
                        }
                        else
                        {
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.PrevVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                    }
                }
                else
                {
                    SelectSingleNode(node);
                }

                OnAfterSelect(new TreeViewEventArgs(selectedNode));
            }
            finally
            {
                this.EndUpdate();
            }
        }

        private void ClearSelectedNodes()
        {
            try
            {
                foreach (TreeNode node in selectedNodes)
                {
                    node.BackColor = this.BackColor;
                    node.ForeColor = this.ForeColor;
                }
            }
            finally
            {
                selectedNodes.Clear();
                selectedNode = null;
            }
        }

        private void SelectSingleNode(TreeNode node)
        {
            if (node == null)
            {
                return;
            }

            ClearSelectedNodes();
            ToggleNode(node, true);
            node.EnsureVisible();
        }

        private void ToggleNode(TreeNode node, bool bSelectNode)
        {
            if (bSelectNode)
            {
                selectedNode = node;
                if (!selectedNodes.Contains(node))
                {
                    selectedNodes.Add(node);
                }
                node.BackColor = SystemColors.Highlight;
                node.ForeColor = SystemColors.HighlightText;
            }
            else
            {
                selectedNodes.Remove(node);
                node.BackColor = this.BackColor;
                node.ForeColor = this.ForeColor;
            }
        }
        

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowTheme(this.Handle, "explorer", null);
        }

        public NVSTreeView()
        {
            this.StartingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.PopulateTreeView();

            selectedNodes = new List<TreeNode>();
            base.SelectedNode = null;
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
