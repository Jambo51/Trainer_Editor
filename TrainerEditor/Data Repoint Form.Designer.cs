namespace TrainerEditor
{
    partial class Data_Repoint_Form
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
            this.upDownNumberOfPokemon = new System.Windows.Forms.NumericUpDown();
            this.lblNumberOfPokemon = new System.Windows.Forms.Label();
            this.chkBxMovesets = new System.Windows.Forms.CheckBox();
            this.lblRepointNeeded = new System.Windows.Forms.Label();
            this.lblOldDataLength = new System.Windows.Forms.Label();
            this.lblNewDataLength = new System.Windows.Forms.Label();
            this.lblCurrentOffset = new System.Windows.Forms.Label();
            this.txtCurrentOffset = new System.Windows.Forms.TextBox();
            this.lblNewOffset = new System.Windows.Forms.Label();
            this.txtNewOffset = new System.Windows.Forms.TextBox();
            this.btnFindFreeSpace = new System.Windows.Forms.Button();
            this.lblFreeSpaceByte = new System.Windows.Forms.Label();
            this.txtFreeSpaceByte = new System.Windows.Forms.TextBox();
            this.btnRepoint = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkBxRemoveOriginalData = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.upDownNumberOfPokemon)).BeginInit();
            this.SuspendLayout();
            // 
            // upDownNumberOfPokemon
            // 
            this.upDownNumberOfPokemon.Location = new System.Drawing.Point(185, 12);
            this.upDownNumberOfPokemon.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.upDownNumberOfPokemon.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownNumberOfPokemon.Name = "upDownNumberOfPokemon";
            this.upDownNumberOfPokemon.Size = new System.Drawing.Size(87, 20);
            this.upDownNumberOfPokemon.TabIndex = 0;
            this.upDownNumberOfPokemon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.upDownNumberOfPokemon.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownNumberOfPokemon.ValueChanged += new System.EventHandler(this.upDownNumberOfPokemon_ValueChanged);
            // 
            // lblNumberOfPokemon
            // 
            this.lblNumberOfPokemon.AutoSize = true;
            this.lblNumberOfPokemon.Location = new System.Drawing.Point(12, 14);
            this.lblNumberOfPokemon.Name = "lblNumberOfPokemon";
            this.lblNumberOfPokemon.Size = new System.Drawing.Size(158, 13);
            this.lblNumberOfPokemon.TabIndex = 1;
            this.lblNumberOfPokemon.Text = "New Number Of Pokémon (1-6):";
            // 
            // chkBxMovesets
            // 
            this.chkBxMovesets.AutoSize = true;
            this.chkBxMovesets.Location = new System.Drawing.Point(15, 47);
            this.chkBxMovesets.Name = "chkBxMovesets";
            this.chkBxMovesets.Size = new System.Drawing.Size(72, 17);
            this.chkBxMovesets.TabIndex = 2;
            this.chkBxMovesets.Text = "Movesets";
            this.chkBxMovesets.UseVisualStyleBackColor = true;
            this.chkBxMovesets.CheckedChanged += new System.EventHandler(this.chkBxMovesets_CheckedChanged);
            // 
            // lblRepointNeeded
            // 
            this.lblRepointNeeded.AutoSize = true;
            this.lblRepointNeeded.Location = new System.Drawing.Point(164, 89);
            this.lblRepointNeeded.Name = "lblRepointNeeded";
            this.lblRepointNeeded.Size = new System.Drawing.Size(108, 13);
            this.lblRepointNeeded.TabIndex = 3;
            this.lblRepointNeeded.Text = "Repoint Not Needed!";
            // 
            // lblOldDataLength
            // 
            this.lblOldDataLength.AutoSize = true;
            this.lblOldDataLength.Location = new System.Drawing.Point(12, 78);
            this.lblOldDataLength.Name = "lblOldDataLength";
            this.lblOldDataLength.Size = new System.Drawing.Size(91, 13);
            this.lblOldDataLength.TabIndex = 4;
            this.lblOldDataLength.Text = "Old Data Length: ";
            // 
            // lblNewDataLength
            // 
            this.lblNewDataLength.AutoSize = true;
            this.lblNewDataLength.Location = new System.Drawing.Point(12, 101);
            this.lblNewDataLength.Name = "lblNewDataLength";
            this.lblNewDataLength.Size = new System.Drawing.Size(97, 13);
            this.lblNewDataLength.TabIndex = 5;
            this.lblNewDataLength.Text = "New Data Length: ";
            // 
            // lblCurrentOffset
            // 
            this.lblCurrentOffset.AutoSize = true;
            this.lblCurrentOffset.Location = new System.Drawing.Point(12, 126);
            this.lblCurrentOffset.Name = "lblCurrentOffset";
            this.lblCurrentOffset.Size = new System.Drawing.Size(75, 13);
            this.lblCurrentOffset.TabIndex = 6;
            this.lblCurrentOffset.Text = "Current Offset:";
            // 
            // txtCurrentOffset
            // 
            this.txtCurrentOffset.Enabled = false;
            this.txtCurrentOffset.Location = new System.Drawing.Point(93, 123);
            this.txtCurrentOffset.Name = "txtCurrentOffset";
            this.txtCurrentOffset.Size = new System.Drawing.Size(100, 20);
            this.txtCurrentOffset.TabIndex = 7;
            this.txtCurrentOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblNewOffset
            // 
            this.lblNewOffset.AutoSize = true;
            this.lblNewOffset.Location = new System.Drawing.Point(12, 154);
            this.lblNewOffset.Name = "lblNewOffset";
            this.lblNewOffset.Size = new System.Drawing.Size(63, 13);
            this.lblNewOffset.TabIndex = 8;
            this.lblNewOffset.Text = "New Offset:";
            // 
            // txtNewOffset
            // 
            this.txtNewOffset.Location = new System.Drawing.Point(93, 151);
            this.txtNewOffset.Name = "txtNewOffset";
            this.txtNewOffset.Size = new System.Drawing.Size(100, 20);
            this.txtNewOffset.TabIndex = 9;
            this.txtNewOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnFindFreeSpace
            // 
            this.btnFindFreeSpace.Location = new System.Drawing.Point(199, 143);
            this.btnFindFreeSpace.Name = "btnFindFreeSpace";
            this.btnFindFreeSpace.Size = new System.Drawing.Size(75, 34);
            this.btnFindFreeSpace.TabIndex = 10;
            this.btnFindFreeSpace.Text = "Find Free Space";
            this.btnFindFreeSpace.UseVisualStyleBackColor = true;
            this.btnFindFreeSpace.Click += new System.EventHandler(this.btnFindFreeSpace_Click);
            // 
            // lblFreeSpaceByte
            // 
            this.lblFreeSpaceByte.AutoSize = true;
            this.lblFreeSpaceByte.Location = new System.Drawing.Point(12, 182);
            this.lblFreeSpaceByte.Name = "lblFreeSpaceByte";
            this.lblFreeSpaceByte.Size = new System.Drawing.Size(89, 13);
            this.lblFreeSpaceByte.TabIndex = 11;
            this.lblFreeSpaceByte.Text = "Free Space Byte:";
            // 
            // txtFreeSpaceByte
            // 
            this.txtFreeSpaceByte.Location = new System.Drawing.Point(107, 179);
            this.txtFreeSpaceByte.Name = "txtFreeSpaceByte";
            this.txtFreeSpaceByte.Size = new System.Drawing.Size(86, 20);
            this.txtFreeSpaceByte.TabIndex = 12;
            this.txtFreeSpaceByte.Text = "0xFF";
            this.txtFreeSpaceByte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnRepoint
            // 
            this.btnRepoint.Location = new System.Drawing.Point(51, 227);
            this.btnRepoint.Name = "btnRepoint";
            this.btnRepoint.Size = new System.Drawing.Size(75, 23);
            this.btnRepoint.TabIndex = 13;
            this.btnRepoint.Text = "Change";
            this.btnRepoint.UseVisualStyleBackColor = true;
            this.btnRepoint.Click += new System.EventHandler(this.btnRepoint_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(149, 227);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkBxRemoveOriginalData
            // 
            this.chkBxRemoveOriginalData.AutoSize = true;
            this.chkBxRemoveOriginalData.Location = new System.Drawing.Point(12, 204);
            this.chkBxRemoveOriginalData.Name = "chkBxRemoveOriginalData";
            this.chkBxRemoveOriginalData.Size = new System.Drawing.Size(130, 17);
            this.chkBxRemoveOriginalData.TabIndex = 15;
            this.chkBxRemoveOriginalData.Text = "Remove Original Data";
            this.chkBxRemoveOriginalData.UseVisualStyleBackColor = true;
            // 
            // Data_Repoint_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.chkBxRemoveOriginalData);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRepoint);
            this.Controls.Add(this.txtFreeSpaceByte);
            this.Controls.Add(this.lblFreeSpaceByte);
            this.Controls.Add(this.btnFindFreeSpace);
            this.Controls.Add(this.txtNewOffset);
            this.Controls.Add(this.lblNewOffset);
            this.Controls.Add(this.txtCurrentOffset);
            this.Controls.Add(this.lblCurrentOffset);
            this.Controls.Add(this.lblNewDataLength);
            this.Controls.Add(this.lblOldDataLength);
            this.Controls.Add(this.lblRepointNeeded);
            this.Controls.Add(this.chkBxMovesets);
            this.Controls.Add(this.lblNumberOfPokemon);
            this.Controls.Add(this.upDownNumberOfPokemon);
            this.Name = "Data_Repoint_Form";
            this.Text = "Trainer Data Repoint";
            ((System.ComponentModel.ISupportInitialize)(this.upDownNumberOfPokemon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown upDownNumberOfPokemon;
        private System.Windows.Forms.Label lblNumberOfPokemon;
        private System.Windows.Forms.CheckBox chkBxMovesets;
        private System.Windows.Forms.Label lblRepointNeeded;
        private System.Windows.Forms.Label lblOldDataLength;
        private System.Windows.Forms.Label lblNewDataLength;
        private System.Windows.Forms.Label lblCurrentOffset;
        private System.Windows.Forms.TextBox txtCurrentOffset;
        private System.Windows.Forms.Label lblNewOffset;
        private System.Windows.Forms.TextBox txtNewOffset;
        private System.Windows.Forms.Button btnFindFreeSpace;
        private System.Windows.Forms.Label lblFreeSpaceByte;
        private System.Windows.Forms.TextBox txtFreeSpaceByte;
        private System.Windows.Forms.Button btnRepoint;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkBxRemoveOriginalData;
    }
}