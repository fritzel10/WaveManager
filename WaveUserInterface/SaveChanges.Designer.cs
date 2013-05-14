namespace WaveUserInterface
{
	partial class SaveChanges
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveChanges));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this._btnYes = new System.Windows.Forms.Button();
			this._btnNo = new System.Windows.Forms.Button();
			this._btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(19, 17);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(42, 47);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(75, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(195, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Modified document. Save changes?";
			// 
			// _btnYes
			// 
			this._btnYes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._btnYes.Location = new System.Drawing.Point(78, 40);
			this._btnYes.Name = "_btnYes";
			this._btnYes.Size = new System.Drawing.Size(75, 24);
			this._btnYes.TabIndex = 2;
			this._btnYes.Text = "Yes";
			this._btnYes.UseVisualStyleBackColor = true;
			this._btnYes.Click += new System.EventHandler(this.OnClickYes);
			// 
			// _btnNo
			// 
			this._btnNo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._btnNo.Location = new System.Drawing.Point(160, 40);
			this._btnNo.Name = "_btnNo";
			this._btnNo.Size = new System.Drawing.Size(75, 24);
			this._btnNo.TabIndex = 3;
			this._btnNo.Text = "No";
			this._btnNo.UseVisualStyleBackColor = true;
			this._btnNo.Click += new System.EventHandler(this.OnClickNo);
			// 
			// _btnCancel
			// 
			this._btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._btnCancel.Location = new System.Drawing.Point(241, 40);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(75, 24);
			this._btnCancel.TabIndex = 4;
			this._btnCancel.Text = "Cancel";
			this._btnCancel.UseVisualStyleBackColor = true;
			this._btnCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// SaveChanges
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(328, 72);
			this.Controls.Add(this._btnCancel);
			this.Controls.Add(this._btnNo);
			this.Controls.Add(this._btnYes);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Name = "SaveChanges";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SaveChanges";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button _btnYes;
		private System.Windows.Forms.Button _btnNo;
		private System.Windows.Forms.Button _btnCancel;
	}
}