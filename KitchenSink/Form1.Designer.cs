namespace KitchenSink
{
    partial class Form1
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node0");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node5");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node2", new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node3");
            this.nvsTreeView1 = new NVS.Components.NVSTreeView();
            this.SuspendLayout();
            // 
            // nvsTreeView1
            // 
            this.nvsTreeView1.Location = new System.Drawing.Point(12, 12);
            this.nvsTreeView1.Name = "nvsTreeView1";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Node0";
            treeNode2.Name = "Node4";
            treeNode2.Text = "Node4";
            treeNode3.Name = "Node1";
            treeNode3.Text = "Node1";
            treeNode4.Name = "Node5";
            treeNode4.Text = "Node5";
            treeNode5.Name = "Node2";
            treeNode5.Text = "Node2";
            treeNode6.Name = "Node3";
            treeNode6.Text = "Node3";
            this.nvsTreeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3,
            treeNode5,
            treeNode6});
            this.nvsTreeView1.Size = new System.Drawing.Size(776, 426);
            this.nvsTreeView1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.nvsTreeView1);
            this.Name = "Form1";
            this.Text = "Kitchen Sink";
            this.ResumeLayout(false);

        }

        #endregion

        private NVS.Components.NVSTreeView nvsTreeView1;
    }
}

