namespace WaveUserInterface
{
	partial class Help
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Help));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this._linklbl1 = new System.Windows.Forms.LinkLabel();
			this._btnClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(24, 24);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(88, 63);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// _linklbl1
			// 
			this._linklbl1.AutoSize = true;
			this._linklbl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._linklbl1.Location = new System.Drawing.Point(140, 36);
			this._linklbl1.Name = "_linklbl1";
			this._linklbl1.Size = new System.Drawing.Size(111, 15);
			this._linklbl1.TabIndex = 1;
			this._linklbl1.TabStop = true;
			this._linklbl1.Text = "www.bu.edu/csmet";
			this._linklbl1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickLink);
			// 
			// _btnClose
			// 
			this._btnClose.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._btnClose.Location = new System.Drawing.Point(176, 64);
			this._btnClose.Name = "_btnClose";
			this._btnClose.Size = new System.Drawing.Size(75, 23);
			this._btnClose.TabIndex = 2;
			this._btnClose.Text = "Close";
			this._btnClose.UseVisualStyleBackColor = true;
			this._btnClose.Click += new System.EventHandler(this.OnClickClose);
			// 
			// Help
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(276, 106);
			this.Controls.Add(this._btnClose);
			this.Controls.Add(this._linklbl1);
			this.Controls.Add(this.pictureBox1);
			this.Name = "Help";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Help";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.LinkLabel _linklbl1;
		private System.Windows.Forms.Button _btnClose;
	}
}