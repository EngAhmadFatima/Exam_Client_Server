namespace Exam_Srever
{
    partial class ExServerForm
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
            this.displayTextBox = new System.Windows.Forms.TextBox();
            this.lbl_StudConnected = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_ClientId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // displayTextBox
            // 
            this.displayTextBox.BackColor = System.Drawing.Color.Black;
            this.displayTextBox.ForeColor = System.Drawing.Color.White;
            this.displayTextBox.Location = new System.Drawing.Point(3, 3);
            this.displayTextBox.Multiline = true;
            this.displayTextBox.Name = "displayTextBox";
            this.displayTextBox.ReadOnly = true;
            this.displayTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.displayTextBox.Size = new System.Drawing.Size(282, 215);
            this.displayTextBox.TabIndex = 0;
            // 
            // lbl_StudConnected
            // 
            this.lbl_StudConnected.AutoSize = true;
            this.lbl_StudConnected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_StudConnected.Location = new System.Drawing.Point(239, 233);
            this.lbl_StudConnected.Name = "lbl_StudConnected";
            this.lbl_StudConnected.Size = new System.Drawing.Size(15, 15);
            this.lbl_StudConnected.TabIndex = 1;
            this.lbl_StudConnected.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 233);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Students connected on server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 263);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Last Student Id used on server:";
            // 
            // lbl_ClientId
            // 
            this.lbl_ClientId.AutoSize = true;
            this.lbl_ClientId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ClientId.Location = new System.Drawing.Point(239, 263);
            this.lbl_ClientId.Name = "lbl_ClientId";
            this.lbl_ClientId.Size = new System.Drawing.Size(15, 15);
            this.lbl_ClientId.TabIndex = 3;
            this.lbl_ClientId.Text = "0";
            // 
            // ExServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(290, 380);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_ClientId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_StudConnected);
            this.Controls.Add(this.displayTextBox);
            this.Name = "ExServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExamServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExServerForm_FormClosing);
            this.Load += new System.EventHandler(this.ExServerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox displayTextBox;
        private System.Windows.Forms.Label lbl_StudConnected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_ClientId;
    }
}

