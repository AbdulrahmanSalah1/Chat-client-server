namespace Client_Chat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            Connect = new Button();
            Send = new Button();
            Disconnect = new Button();
            Browse = new Button();
            pictureBox1 = new PictureBox();
            axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            CompressFile = new Button();
            CompressImage = new Button();
            ReceiveImage = new Button();
            RecvFile = new Button();
            DecompImage = new Button();
            DecompFile = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer1).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 361);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(525, 27);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Location = new Point(12, 12);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(525, 343);
            textBox2.TabIndex = 1;
            // 
            // Connect
            // 
            Connect.Location = new Point(543, 12);
            Connect.Name = "Connect";
            Connect.Size = new Size(145, 38);
            Connect.TabIndex = 2;
            Connect.Text = "Connect";
            Connect.UseVisualStyleBackColor = true;
            Connect.Click += Connect_Click;
            // 
            // Send
            // 
            Send.Location = new Point(543, 361);
            Send.Name = "Send";
            Send.Size = new Size(145, 38);
            Send.TabIndex = 3;
            Send.Text = "Send";
            Send.UseVisualStyleBackColor = true;
            Send.Click += Send_Click;
            // 
            // Disconnect
            // 
            Disconnect.Location = new Point(543, 65);
            Disconnect.Name = "Disconnect";
            Disconnect.Size = new Size(145, 38);
            Disconnect.TabIndex = 4;
            Disconnect.Text = "Disconnect";
            Disconnect.UseVisualStyleBackColor = true;
            Disconnect.Click += Disconnect_Click;
            // 
            // Browse
            // 
            Browse.Location = new Point(543, 405);
            Browse.Name = "Browse";
            Browse.Size = new Size(145, 38);
            Browse.TabIndex = 5;
            Browse.Text = "Browse";
            Browse.UseVisualStyleBackColor = true;
            Browse.Click += Browse_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(694, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(482, 343);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // axWindowsMediaPlayer1
            // 
            axWindowsMediaPlayer1.Enabled = true;
            axWindowsMediaPlayer1.Location = new Point(12, 394);
            axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            axWindowsMediaPlayer1.OcxState = (AxHost.State)resources.GetObject("axWindowsMediaPlayer1.OcxState");
            axWindowsMediaPlayer1.Size = new Size(525, 380);
            axWindowsMediaPlayer1.TabIndex = 7;
            // 
            // CompressFile
            // 
            CompressFile.Location = new Point(694, 405);
            CompressFile.Name = "CompressFile";
            CompressFile.Size = new Size(145, 38);
            CompressFile.TabIndex = 8;
            CompressFile.Text = "Compress File";
            CompressFile.UseVisualStyleBackColor = true;
            CompressFile.Click += CompressFile_Click;
            // 
            // CompressImage
            // 
            CompressImage.Location = new Point(845, 405);
            CompressImage.Name = "CompressImage";
            CompressImage.Size = new Size(145, 38);
            CompressImage.TabIndex = 9;
            CompressImage.Text = "Compress Image";
            CompressImage.UseVisualStyleBackColor = true;
            CompressImage.Click += CompressImage_Click;
            // 
            // ReceiveImage
            // 
            ReceiveImage.Location = new Point(845, 361);
            ReceiveImage.Name = "ReceiveImage";
            ReceiveImage.Size = new Size(145, 38);
            ReceiveImage.TabIndex = 10;
            ReceiveImage.Text = "Receive Image";
            ReceiveImage.UseVisualStyleBackColor = true;
            ReceiveImage.Click += ReceiveImage_Click;
            // 
            // RecvFile
            // 
            RecvFile.Location = new Point(694, 361);
            RecvFile.Name = "RecvFile";
            RecvFile.Size = new Size(145, 38);
            RecvFile.TabIndex = 11;
            RecvFile.Text = "Receive File";
            RecvFile.UseVisualStyleBackColor = true;
            RecvFile.Click += RecvFile_Click;
            // 
            // DecompImage
            // 
            DecompImage.Location = new Point(845, 449);
            DecompImage.Name = "DecompImage";
            DecompImage.Size = new Size(145, 38);
            DecompImage.TabIndex = 12;
            DecompImage.Text = "Decompress Image";
            DecompImage.UseVisualStyleBackColor = true;
            DecompImage.Click += DecompImage_Click;
            // 
            // DecompFile
            // 
            DecompFile.Location = new Point(694, 449);
            DecompFile.Name = "DecompFile";
            DecompFile.Size = new Size(145, 38);
            DecompFile.TabIndex = 13;
            DecompFile.Text = "Decompress File";
            DecompFile.UseVisualStyleBackColor = true;
            DecompFile.Click += DecompFile_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1185, 780);
            Controls.Add(DecompFile);
            Controls.Add(DecompImage);
            Controls.Add(RecvFile);
            Controls.Add(ReceiveImage);
            Controls.Add(CompressImage);
            Controls.Add(CompressFile);
            Controls.Add(axWindowsMediaPlayer1);
            Controls.Add(pictureBox1);
            Controls.Add(Browse);
            Controls.Add(Disconnect);
            Controls.Add(Send);
            Controls.Add(Connect);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Client";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button Connect;
        private Button Send;
        private Button Disconnect;
        private Button Browse;
        private PictureBox pictureBox1;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private Button CompressFile;
        private Button CompressImage;
        private Button ReceiveImage;
        private Button RecvFile;
        private Button DecompImage;
        private Button DecompFile;
    }
}
