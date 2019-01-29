namespace DataMigrationTool
{
    partial class DataMigration
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
            this.components = new System.ComponentModel.Container();
            this.rtxt_data = new System.Windows.Forms.RichTextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblCardType = new System.Windows.Forms.Label();
            this.lblMaxId = new System.Windows.Forms.Label();
            this.txtMaxId = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.rtxtInfo = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cancelmenuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxt_data
            // 
            this.rtxt_data.ContextMenuStrip = this.contextMenuStrip1;
            this.rtxt_data.Location = new System.Drawing.Point(1, 260);
            this.rtxt_data.Name = "rtxt_data";
            this.rtxt_data.Size = new System.Drawing.Size(667, 355);
            this.rtxt_data.TabIndex = 0;
            this.rtxt_data.Text = "";
            this.rtxt_data.TextChanged += new System.EventHandler(this.rtxt_data_TextChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(61, 68);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(137, 20);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblCardType
            // 
            this.lblCardType.Location = new System.Drawing.Point(0, 68);
            this.lblCardType.Name = "lblCardType";
            this.lblCardType.Size = new System.Drawing.Size(56, 20);
            this.lblCardType.TabIndex = 2;
            this.lblCardType.Text = "卡类型:";
            this.lblCardType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMaxId
            // 
            this.lblMaxId.Location = new System.Drawing.Point(223, 68);
            this.lblMaxId.Name = "lblMaxId";
            this.lblMaxId.Size = new System.Drawing.Size(92, 20);
            this.lblMaxId.TabIndex = 3;
            this.lblMaxId.Text = "旧小微最大ID:";
            this.lblMaxId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaxId
            // 
            this.txtMaxId.Location = new System.Drawing.Point(320, 68);
            this.txtMaxId.Multiline = true;
            this.txtMaxId.Name = "txtMaxId";
            this.txtMaxId.Size = new System.Drawing.Size(100, 20);
            this.txtMaxId.TabIndex = 4;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(593, 68);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 20);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "导入新小微";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // rtxtInfo
            // 
            this.rtxtInfo.ContextMenuStrip = this.contextMenuStrip1;
            this.rtxtInfo.Location = new System.Drawing.Point(1, 91);
            this.rtxtInfo.Name = "rtxtInfo";
            this.rtxtInfo.Size = new System.Drawing.Size(667, 170);
            this.rtxtInfo.TabIndex = 6;
            this.rtxtInfo.Text = "";
            this.rtxtInfo.TextChanged += new System.EventHandler(this.rtxtInfo_TextChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.White;
            this.progressBar1.ForeColor = System.Drawing.Color.Black;
            this.progressBar1.Location = new System.Drawing.Point(2, 610);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(666, 5);
            this.progressBar1.TabIndex = 7;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cancelmenuitem,
            this.testToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // cancelmenuitem
            // 
            this.cancelmenuitem.Name = "cancelmenuitem";
            this.cancelmenuitem.Size = new System.Drawing.Size(100, 22);
            this.cancelmenuitem.Text = "取消";
            this.cancelmenuitem.Click += new System.EventHandler(this.cancelmenuitem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.testToolStripMenuItem.Text = "test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // DataMigration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 616);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.rtxtInfo);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtMaxId);
            this.Controls.Add(this.lblMaxId);
            this.Controls.Add(this.lblCardType);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.rtxt_data);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DataMigration";
            this.Text = "DataMigration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataMigration_FormClosing);
            this.Load += new System.EventHandler(this.DataMigration_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxt_data;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblCardType;
        private System.Windows.Forms.Label lblMaxId;
        private System.Windows.Forms.TextBox txtMaxId;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.RichTextBox rtxtInfo;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cancelmenuitem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
    }
}