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
            this.rtxt_data = new System.Windows.Forms.RichTextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblCardType = new System.Windows.Forms.Label();
            this.lblMaxId = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtxt_data
            // 
            this.rtxt_data.Location = new System.Drawing.Point(1, 91);
            this.rtxt_data.Name = "rtxt_data";
            this.rtxt_data.Size = new System.Drawing.Size(667, 361);
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(320, 68);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(593, 68);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 20);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "导入新小微";
            this.btnImport.UseVisualStyleBackColor = true;
            // 
            // DataMigration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 450);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblMaxId);
            this.Controls.Add(this.lblCardType);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.rtxt_data);
            this.Name = "DataMigration";
            this.Text = "DataMigration";
            this.Load += new System.EventHandler(this.DataMigration_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxt_data;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblCardType;
        private System.Windows.Forms.Label lblMaxId;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnImport;
    }
}