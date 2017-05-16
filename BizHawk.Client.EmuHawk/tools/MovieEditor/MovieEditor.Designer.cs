namespace BizHawk.Client.EmuHawk.tools.MovieEditor
{
	partial class MovieEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MovieEditor));
			this.MovieEditorStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.coolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.storyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.broToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.thingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.etcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MovieEditorInputRoll = new BizHawk.Client.EmuHawk.InputRoll();
			this.MovieEditorStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// MovieEditorStrip
			// 
			this.MovieEditorStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.fileToolStripMenuItem,
			this.thingsToolStripMenuItem,
			this.etcToolStripMenuItem});
			this.MovieEditorStrip.Location = new System.Drawing.Point(0, 0);
			this.MovieEditorStrip.Name = "MovieEditorStrip";
			this.MovieEditorStrip.Size = new System.Drawing.Size(284, 24);
			this.MovieEditorStrip.TabIndex = 1;
			this.MovieEditorStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.coolToolStripMenuItem,
			this.storyToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// coolToolStripMenuItem
			// 
			this.coolToolStripMenuItem.Name = "coolToolStripMenuItem";
			this.coolToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.coolToolStripMenuItem.Text = "cool";
			// 
			// storyToolStripMenuItem
			// 
			this.storyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.broToolStripMenuItem});
			this.storyToolStripMenuItem.Name = "storyToolStripMenuItem";
			this.storyToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.storyToolStripMenuItem.Text = "story";
			// 
			// broToolStripMenuItem
			// 
			this.broToolStripMenuItem.Name = "broToolStripMenuItem";
			this.broToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
			this.broToolStripMenuItem.Text = "bro";
			// 
			// thingsToolStripMenuItem
			// 
			this.thingsToolStripMenuItem.Name = "thingsToolStripMenuItem";
			this.thingsToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
			this.thingsToolStripMenuItem.Text = "Things";
			// 
			// etcToolStripMenuItem
			// 
			this.etcToolStripMenuItem.Name = "etcToolStripMenuItem";
			this.etcToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.etcToolStripMenuItem.Text = "etc";
			// 
			// MovieEditorInputRoll
			// 
			this.MovieEditorInputRoll.AllowColumnReorder = false;
			this.MovieEditorInputRoll.AllowColumnResize = false;
			this.MovieEditorInputRoll.allowRightClickSelecton = false;
			this.MovieEditorInputRoll.AlwaysScroll = false;
			this.MovieEditorInputRoll.CellHeightPadding = 0;
			this.MovieEditorInputRoll.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MovieEditorInputRoll.HideWasLagFrames = false;
			this.MovieEditorInputRoll.HorizontalOrientation = false;
			this.MovieEditorInputRoll.LagFramesToHide = 0;
			this.MovieEditorInputRoll.letKeysModifySelection = false;
			this.MovieEditorInputRoll.Location = new System.Drawing.Point(0, 24);
			this.MovieEditorInputRoll.MaxCharactersInHorizontal = 1;
			this.MovieEditorInputRoll.MultiSelect = false;
			this.MovieEditorInputRoll.Name = "MovieEditorInputRoll";
			this.MovieEditorInputRoll.RowCount = 0;
			this.MovieEditorInputRoll.ScrollSpeed = 1;
			this.MovieEditorInputRoll.SeekingCutoffInterval = 0;
			this.MovieEditorInputRoll.Size = new System.Drawing.Size(284, 238);
			this.MovieEditorInputRoll.suspendHotkeys = false;
			this.MovieEditorInputRoll.TabIndex = 2;
			this.MovieEditorInputRoll.Text = "inputRoll1";
			// 
			// MovieEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.MovieEditorInputRoll);
			this.Controls.Add(this.MovieEditorStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MovieEditor";
			this.Text = "Cool Movie Editor";
			this.Load += new System.EventHandler(this.MovieEditor_Load);
			this.MovieEditorStrip.ResumeLayout(false);
			this.MovieEditorStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip MovieEditorStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem coolToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem storyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem broToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem thingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem etcToolStripMenuItem;
		private InputRoll MovieEditorInputRoll;
	}
}