namespace WaveUserInterface
{
	partial class WaveTreeViewControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaveTreeViewControl));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._btnBrowse = new System.Windows.Forms.Button();
			this._tvwWaveList = new System.Windows.Forms.TreeView();
			this._cmsColorFont = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.backgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			this._cmsColorFont.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this._btnBrowse, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._tvwWaveList, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.04899F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.95101F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(216, 347);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _btnBrowse
			// 
			this._btnBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
			this._btnBrowse.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._btnBrowse.Location = new System.Drawing.Point(3, 311);
			this._btnBrowse.Name = "_btnBrowse";
			this._btnBrowse.Size = new System.Drawing.Size(210, 33);
			this._btnBrowse.TabIndex = 0;
			this._btnBrowse.Text = "Browse";
			this._btnBrowse.UseVisualStyleBackColor = true;
			// 
			// _tvwWaveList
			// 
			this._tvwWaveList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tvwWaveList.Location = new System.Drawing.Point(3, 3);
			this._tvwWaveList.Name = "_tvwWaveList";
			this._tvwWaveList.Size = new System.Drawing.Size(210, 302);
			this._tvwWaveList.TabIndex = 1;
			this._tvwWaveList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnItemDrag);
			this._tvwWaveList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelect);
			this._tvwWaveList.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
			this._tvwWaveList.DragLeave += new System.EventHandler(this.OnDragLeave);
			this._tvwWaveList.DoubleClick += new System.EventHandler(this.OnTreeViewDoubleClick);
			this._tvwWaveList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
			// 
			// _cmsColorFont
			// 
			this._cmsColorFont.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontToolStripMenuItem,
            this.backgroundColorToolStripMenuItem});
			this._cmsColorFont.Name = "_cmsColorFont";
			this._cmsColorFont.Size = new System.Drawing.Size(180, 70);
			// 
			// fontToolStripMenuItem
			// 
			this.fontToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fontToolStripMenuItem.Image")));
			this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
			this.fontToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
			this.fontToolStripMenuItem.Text = "Font...";
			this.fontToolStripMenuItem.Click += new System.EventHandler(this.OnClickFontChange);
			// 
			// backgroundColorToolStripMenuItem
			// 
			this.backgroundColorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("backgroundColorToolStripMenuItem.Image")));
			this.backgroundColorToolStripMenuItem.Name = "backgroundColorToolStripMenuItem";
			this.backgroundColorToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
			this.backgroundColorToolStripMenuItem.Text = "Background Color...";
			this.backgroundColorToolStripMenuItem.Click += new System.EventHandler(this.OnClickBackgroundColor);
			// 
			// WaveTreeViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "WaveTreeViewControl";
			this.Size = new System.Drawing.Size(216, 347);
			this.Load += new System.EventHandler(this.OnLoadWaveTreeView);
			this.tableLayoutPanel1.ResumeLayout(false);
			this._cmsColorFont.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button _btnBrowse;
		private System.Windows.Forms.TreeView _tvwWaveList;
		private System.Windows.Forms.ContextMenuStrip _cmsColorFont;
		private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem backgroundColorToolStripMenuItem;
	}
}
