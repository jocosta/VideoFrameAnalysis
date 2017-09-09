namespace LiveCameraForm
{
    partial class Form1
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
            this.imageBase = new System.Windows.Forms.PictureBox();
            this.imageDrawing = new System.Windows.Forms.PictureBox();
            this.btnEmotion = new System.Windows.Forms.Button();
            this.CameraList = new System.Windows.Forms.ComboBox();
            this.btnNenhum = new System.Windows.Forms.Button();
            this.btnFace = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageDrawing)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBase
            // 
            this.imageBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBase.Location = new System.Drawing.Point(0, 0);
            this.imageBase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imageBase.Name = "imageBase";
            this.imageBase.Size = new System.Drawing.Size(1039, 544);
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
            this.imageDrawing.Size = new System.Drawing.Size(1039, 544);
            this.imageDrawing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageDrawing.TabIndex = 1;
            this.imageDrawing.TabStop = false;
            // 
            // btnEmotion
            // 
            this.btnEmotion.BackColor = System.Drawing.Color.Transparent;
            this.btnEmotion.CausesValidation = false;
            this.btnEmotion.Location = new System.Drawing.Point(16, 116);
            this.btnEmotion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEmotion.Name = "btnEmotion";
            this.btnEmotion.Size = new System.Drawing.Size(84, 60);
            this.btnEmotion.TabIndex = 2;
            this.btnEmotion.Text = "Emoção";
            this.btnEmotion.UseVisualStyleBackColor = false;
            this.btnEmotion.Click += new System.EventHandler(this.btnEmotion_Click);
            // 
            // CameraList
            // 
            this.CameraList.FormattingEnabled = true;
            this.CameraList.Location = new System.Drawing.Point(16, 406);
            this.CameraList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CameraList.Name = "CameraList";
            this.CameraList.Size = new System.Drawing.Size(160, 24);
            this.CameraList.TabIndex = 3;
            this.CameraList.Visible = false;
            // 
            // btnNenhum
            // 
            this.btnNenhum.BackColor = System.Drawing.Color.Transparent;
            this.btnNenhum.CausesValidation = false;
            this.btnNenhum.Location = new System.Drawing.Point(16, 37);
            this.btnNenhum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNenhum.Name = "btnNenhum";
            this.btnNenhum.Size = new System.Drawing.Size(84, 54);
            this.btnNenhum.TabIndex = 4;
            this.btnNenhum.Text = "Nenhum";
            this.btnNenhum.UseVisualStyleBackColor = false;
            this.btnNenhum.Click += new System.EventHandler(this.btnNenhum_Click);
            // 
            // btnFace
            // 
            this.btnFace.BackColor = System.Drawing.Color.Transparent;
            this.btnFace.CausesValidation = false;
            this.btnFace.Location = new System.Drawing.Point(16, 202);
            this.btnFace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFace.Name = "btnFace";
            this.btnFace.Size = new System.Drawing.Size(84, 59);
            this.btnFace.TabIndex = 5;
            this.btnFace.Text = "Face";
            this.btnFace.UseVisualStyleBackColor = false;
            this.btnFace.Click += new System.EventHandler(this.btnFace_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 544);
            this.ControlBox = false;
            this.Controls.Add(this.btnFace);
            this.Controls.Add(this.btnNenhum);
            this.Controls.Add(this.CameraList);
            this.Controls.Add(this.btnEmotion);
            this.Controls.Add(this.imageDrawing);
            this.Controls.Add(this.imageBase);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageDrawing)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imageBase;
        private System.Windows.Forms.PictureBox imageDrawing;
        private System.Windows.Forms.Button btnEmotion;
        private System.Windows.Forms.ComboBox CameraList;
        private System.Windows.Forms.Button btnNenhum;
        private System.Windows.Forms.Button btnFace;
    }
}

