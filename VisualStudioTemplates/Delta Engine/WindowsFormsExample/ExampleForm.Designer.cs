namespace $safeprojectname$
{
	partial class ExampleForm
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			this.panel1.Anchor = 
				((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | 
					System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | 
						System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(12, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(359, 319);
			this.panel1.TabIndex = 0;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(383, 343);
			this.Controls.Add(this.panel1);
			this.Name = "ExampleForm";
			this.Text = "ExampleForm";
			this.ResumeLayout(false);
		}
		#endregion
		private System.Windows.Forms.Panel panel1;
	}
}