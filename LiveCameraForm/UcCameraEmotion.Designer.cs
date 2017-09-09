namespace LiveCameraForm
{
    partial class UcCameraEmotion
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
            this.SuspendLayout();

            this.imageBase = new System.Windows.Forms.PictureBox();
            this.imageDrawing = new System.Windows.Forms.PictureBox();
            this.CameraList = new System.Windows.Forms.ComboBox();
            this.imageBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageDrawing.Dock = System.Windows.Forms.DockStyle.Fill;

            this.CameraList.FormattingEnabled = true;
            this.CameraList.Location = new System.Drawing.Point(16, 406);
            this.CameraList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CameraList.Name = "CameraList";
            this.CameraList.Size = new System.Drawing.Size(160, 24);
            this.CameraList.TabIndex = 3;
            this.CameraList.Visible = false;

            // 
            // imageBase
            // 
            this.imageBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBase.Location = new System.Drawing.Point(0, 0);
            this.imageBase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imageBase.Name = "imageBase";
            this.imageBase.Size = this.Size;
            this.imageBase.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBase.TabIndex = 0;
            this.imageBase.TabStop = false;
            this.imageBase.Visible = false;


            // 
            // imageDrawing
            // 
            this.imageDrawing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageDrawing.Location = new System.Drawing.Point(0, 0);
            this.imageDrawing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imageDrawing.Name = "imageDrawing";
            this.imageDrawing.Size = this.Size;
            this.imageDrawing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageDrawing.TabIndex = 1;
            this.imageDrawing.TabStop = false;


            this.Controls.Add(this.CameraList);
            this.Controls.Add(this.imageBase);
            this.Controls.Add(this.imageDrawing);

            // 
            // UcCameraEmotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UcCameraEmotion";
            this.Size = new System.Drawing.Size(1177, 755);
            this.Load += new System.EventHandler(this.UcCameraEmotion_Load);
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.PictureBox imageBase;
        private System.Windows.Forms.PictureBox imageDrawing;
        private System.Windows.Forms.ComboBox CameraList;
    }
}
