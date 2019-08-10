namespace LPR
{
    partial class FixPlate
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
            this.lbl_CurrentPlate = new System.Windows.Forms.Label();
            this.txt_NewValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chk_AutoUpdate = new System.Windows.Forms.CheckBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.lbl_PK = new System.Windows.Forms.Label();
            this.btn_HideAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_CurrentPlate
            // 
            this.lbl_CurrentPlate.AutoSize = true;
            this.lbl_CurrentPlate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CurrentPlate.Location = new System.Drawing.Point(87, 6);
            this.lbl_CurrentPlate.Name = "lbl_CurrentPlate";
            this.lbl_CurrentPlate.Size = new System.Drawing.Size(50, 20);
            this.lbl_CurrentPlate.TabIndex = 20;
            this.lbl_CurrentPlate.Text = "Plate";
            // 
            // txt_NewValue
            // 
            this.txt_NewValue.Location = new System.Drawing.Point(108, 32);
            this.txt_NewValue.Name = "txt_NewValue";
            this.txt_NewValue.Size = new System.Drawing.Size(100, 20);
            this.txt_NewValue.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Current Plate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Change Plage To";
            // 
            // chk_AutoUpdate
            // 
            this.chk_AutoUpdate.AutoSize = true;
            this.chk_AutoUpdate.Location = new System.Drawing.Point(214, 35);
            this.chk_AutoUpdate.Name = "chk_AutoUpdate";
            this.chk_AutoUpdate.Size = new System.Drawing.Size(100, 17);
            this.chk_AutoUpdate.TabIndex = 24;
            this.chk_AutoUpdate.Text = "Auto-Update All";
            this.chk_AutoUpdate.UseVisualStyleBackColor = true;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(13, 56);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Save.TabIndex = 25;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(94, 56);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 26;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // lbl_PK
            // 
            this.lbl_PK.AutoSize = true;
            this.lbl_PK.Location = new System.Drawing.Point(187, 61);
            this.lbl_PK.Name = "lbl_PK";
            this.lbl_PK.Size = new System.Drawing.Size(21, 13);
            this.lbl_PK.TabIndex = 27;
            this.lbl_PK.Text = "PK";
            this.lbl_PK.Visible = false;
            // 
            // btn_HideAll
            // 
            this.btn_HideAll.Location = new System.Drawing.Point(240, 3);
            this.btn_HideAll.Name = "btn_HideAll";
            this.btn_HideAll.Size = new System.Drawing.Size(75, 23);
            this.btn_HideAll.TabIndex = 28;
            this.btn_HideAll.Text = "Hide All";
            this.btn_HideAll.UseVisualStyleBackColor = true;
            this.btn_HideAll.Click += new System.EventHandler(this.Btn_HideAll_Click);
            // 
            // FixPlate
            // 
            this.AcceptButton = this.btn_Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(327, 89);
            this.Controls.Add(this.btn_HideAll);
            this.Controls.Add(this.lbl_PK);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.chk_AutoUpdate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_NewValue);
            this.Controls.Add(this.lbl_CurrentPlate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FixPlate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fix Plate";
            this.Load += new System.EventHandler(this.FixPlate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_CurrentPlate;
        private System.Windows.Forms.TextBox txt_NewValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chk_AutoUpdate;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label lbl_PK;
        private System.Windows.Forms.Button btn_HideAll;
    }
}