namespace WaveUserInterface
{
	partial class StatusStripControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._pgbGreenBar = new System.Windows.Forms.ProgressBar();
			this._cbxActivityLog = new System.Windows.Forms.ComboBox();
			this._lblSamples = new System.Windows.Forms.Label();
			this._lblWaves = new System.Windows.Forms.Label();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this._tmrTimer = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 6;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.50259F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.49741F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
			this.tableLayoutPanel1.Controls.Add(this._pgbGreenBar, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._cbxActivityLog, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._lblSamples, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this._lblWaves, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.trackBar1, 5, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(639, 33);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _pgbGreenBar
			// 
			this._pgbGreenBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pgbGreenBar.Location = new System.Drawing.Point(3, 3);
			this._pgbGreenBar.Name = "_pgbGreenBar";
			this._pgbGreenBar.Size = new System.Drawing.Size(76, 27);
			this._pgbGreenBar.TabIndex = 0;
			this._pgbGreenBar.MouseHover += new System.EventHandler(this.OnMouseHover);
			// 
			// _cbxActivityLog
			// 
			this._cbxActivityLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this._cbxActivityLog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cbxActivityLog.FormattingEnabled = true;
			this._cbxActivityLog.Location = new System.Drawing.Point(85, 3);
			this._cbxActivityLog.Name = "_cbxActivityLog";
			this._cbxActivityLog.Size = new System.Drawing.Size(295, 21);
			this._cbxActivityLog.TabIndex = 1;
			// 
			// _lblSamples
			// 
			this._lblSamples.AutoSize = true;
			this._lblSamples.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lblSamples.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblSamples.Location = new System.Drawing.Point(451, 0);
			this._lblSamples.Name = "_lblSamples";
			this._lblSamples.Size = new System.Drawing.Size(91, 33);
			this._lblSamples.TabIndex = 3;
			this._lblSamples.Text = "Samples:";
			this._lblSamples.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _lblWaves
			// 
			this._lblWaves.AutoSize = true;
			this._lblWaves.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lblWaves.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblWaves.Location = new System.Drawing.Point(386, 0);
			this._lblWaves.Name = "_lblWaves";
			this._lblWaves.Size = new System.Drawing.Size(59, 33);
			this._lblWaves.TabIndex = 2;
			this._lblWaves.Text = "Waves: ";
			this._lblWaves.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// trackBar1
			// 
			this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trackBar1.Location = new System.Drawing.Point(548, 3);
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(88, 27);
			this.trackBar1.TabIndex = 4;
			this.trackBar1.Scroll += new System.EventHandler(this.OnVolumeScroll);
			// 
			// StatusStripControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "StatusStripControl";
			this.Size = new System.Drawing.Size(639, 33);
			this.Load += new System.EventHandler(this.OnStatusStripLoad);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ProgressBar _pgbGreenBar;
		private System.Windows.Forms.ComboBox _cbxActivityLog;
		private System.Windows.Forms.Label _lblSamples;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label _lblWaves;
		private System.Windows.Forms.Timer _tmrTimer;
	}
}
