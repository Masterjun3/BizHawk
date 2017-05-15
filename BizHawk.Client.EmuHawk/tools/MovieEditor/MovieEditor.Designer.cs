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
			this.MovieEditorStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.coolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.storyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.broToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.thingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.etcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			// MovieEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.MovieEditorStrip);
			this.Name = "MovieEditor";
			this.Text = "MovieEditor";
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
	}
}