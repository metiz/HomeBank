namespace HBTest
{
    partial class frmManageCategories
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
            this.lstMainCat = new System.Windows.Forms.ListBox();
            this.lstSubCat = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNewMain = new System.Windows.Forms.TextBox();
            this.txtNewSub = new System.Windows.Forms.TextBox();
            this.btnRemoveSub = new System.Windows.Forms.Button();
            this.btnRemoveMain = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstMainCat
            // 
            this.lstMainCat.DisplayMember = "Name";
            this.lstMainCat.FormattingEnabled = true;
            this.lstMainCat.ItemHeight = 28;
            this.lstMainCat.Location = new System.Drawing.Point(43, 164);
            this.lstMainCat.Name = "lstMainCat";
            this.lstMainCat.Size = new System.Drawing.Size(178, 312);
            this.lstMainCat.TabIndex = 0;
            this.lstMainCat.ValueMember = "ID";
            this.lstMainCat.SelectedIndexChanged += new System.EventHandler(this.lstMainCat_SelectedIndexChanged);
            // 
            // lstSubCat
            // 
            this.lstSubCat.DisplayMember = "Name";
            this.lstSubCat.FormattingEnabled = true;
            this.lstSubCat.ItemHeight = 28;
            this.lstSubCat.Location = new System.Drawing.Point(267, 164);
            this.lstSubCat.Name = "lstSubCat";
            this.lstSubCat.Size = new System.Drawing.Size(178, 312);
            this.lstSubCat.TabIndex = 0;
            this.lstSubCat.ValueMember = "ID";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRemoveMain);
            this.groupBox1.Controls.Add(this.btnRemoveSub);
            this.groupBox1.Controls.Add(this.txtNewSub);
            this.groupBox1.Controls.Add(this.txtNewMain);
            this.groupBox1.Controls.Add(this.cboType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lstSubCat);
            this.groupBox1.Controls.Add(this.lstMainCat);
            this.groupBox1.Location = new System.Drawing.Point(12, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(827, 590);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 28);
            this.label1.TabIndex = 1;
            this.label1.Text = "Category Type";
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "Expence",
            "Income"});
            this.cboType.Location = new System.Drawing.Point(197, 71);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(263, 36);
            this.cboType.TabIndex = 2;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(262, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sub-Category";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 28);
            this.label3.TabIndex = 1;
            this.label3.Text = "Category";
            // 
            // txtNewMain
            // 
            this.txtNewMain.Location = new System.Drawing.Point(43, 482);
            this.txtNewMain.Name = "txtNewMain";
            this.txtNewMain.Size = new System.Drawing.Size(178, 34);
            this.txtNewMain.TabIndex = 3;
            this.txtNewMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNewMain_KeyDown);
            // 
            // txtNewSub
            // 
            this.txtNewSub.Location = new System.Drawing.Point(267, 482);
            this.txtNewSub.Name = "txtNewSub";
            this.txtNewSub.Size = new System.Drawing.Size(178, 34);
            this.txtNewSub.TabIndex = 3;
            // 
            // btnRemoveSub
            // 
            this.btnRemoveSub.BackColor = System.Drawing.Color.White;
            this.btnRemoveSub.FlatAppearance.BorderSize = 0;
            this.btnRemoveSub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSub.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveSub.Location = new System.Drawing.Point(267, 522);
            this.btnRemoveSub.Name = "btnRemoveSub";
            this.btnRemoveSub.Size = new System.Drawing.Size(178, 46);
            this.btnRemoveSub.TabIndex = 5;
            this.btnRemoveSub.Text = "Remove";
            this.btnRemoveSub.UseVisualStyleBackColor = false;
            // 
            // btnRemoveMain
            // 
            this.btnRemoveMain.BackColor = System.Drawing.Color.White;
            this.btnRemoveMain.FlatAppearance.BorderSize = 0;
            this.btnRemoveMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveMain.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveMain.Location = new System.Drawing.Point(43, 522);
            this.btnRemoveMain.Name = "btnRemoveMain";
            this.btnRemoveMain.Size = new System.Drawing.Size(178, 46);
            this.btnRemoveMain.TabIndex = 5;
            this.btnRemoveMain.Text = "Remove";
            this.btnRemoveMain.UseVisualStyleBackColor = false;
            this.btnRemoveMain.Click += new System.EventHandler(this.btnRemoveMain_Click);
            // 
            // frmManageCategories
            // 
            this.ClientSize = new System.Drawing.Size(900, 720);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmManageCategories";
            this.Text = "Manage Categories";
            this.Load += new System.EventHandler(this.frmManageCategories_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstMainCat;
        private System.Windows.Forms.ListBox lstSubCat;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNewSub;
        private System.Windows.Forms.TextBox txtNewMain;
        private System.Windows.Forms.Button btnRemoveMain;
        private System.Windows.Forms.Button btnRemoveSub;

    }
}
