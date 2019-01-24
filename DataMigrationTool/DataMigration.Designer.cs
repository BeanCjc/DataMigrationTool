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
            // DataMigration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 450);
            this.Controls.Add(this.rtxt_data);
            this.Name = "DataMigration";
            this.Text = "DataMigration";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxt_data;
    }
}