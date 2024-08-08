namespace Server_Chat
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
            Start = new Button();
            Send = new Button();
            Disconnect = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            Browse = new Button();
            pictureBox1 = new PictureBox();
            axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer1).BeginInit();
            SuspendLayout();
            // 
            // Start
            // 
            Start.Location = new Point(521, 12);
            Start.Name = "Start";
            Start.Size = new Size(153, 41);
            Start.TabIndex = 0;
            Start.Text = "Start";
            Start.UseVisualStyleBackColor = true;
            Start.Click += Start_Click;
            // 
            // Send
            // 
            Send.Location = new Point(521, 386);
            Send.Name = "Send";
            Send.Size = new Size(153, 41);
            Send.TabIndex = 1;
            Send.Text = "Send";
            Send.UseVisualStyleBackColor = true;
            Send.Click += Send_Click;
            // 
            // Disconnect
            // 
            Disconnect.Location = new Point(521, 73);
            Disconnect.Name = "Disconnect";
            Disconnect.Size = new Size(153, 41);
            Disconnect.TabIndex = 2;
            Disconnect.Text = "Disconnect";
            Disconnect.UseVisualStyleBackColor = true;
            Disconnect.Click += Disconnect_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 397);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(493, 27);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Location = new Point(12, 12);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(493, 368);
            textBox2.TabIndex = 4;
            // 
            // Browse
            // 
            Browse.Location = new Point(521, 339);
            Browse.Name = "Browse";
            Browse.Size = new Size(153, 41);
            Browse.TabIndex = 5;
            Browse.Text = "Browse";
            Browse.UseVisualStyleBackColor = true;
            Browse.Click += Browse_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(688, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(490, 765);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // axWindowsMediaPlayer1
            // 
            axWindowsMediaPlayer1.Enabled = true;
            axWindowsMediaPlayer1.Location = new Point(12, 430);
            axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            axWindowsMediaPlayer1.OcxState = (AxHost.State)resources.GetObject("axWindowsMediaPlayer1.OcxState");
            axWindowsMediaPlayer1.Size = new Size(490, 349);
            axWindowsMediaPlayer1.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1186, 789);
            Controls.Add(axWindowsMediaPlayer1);
            Controls.Add(pictureBox1);
            Controls.Add(Browse);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(Disconnect);
            Controls.Add(Send);
            Controls.Add(Start);
            Name = "Form1";
            Text = "Server";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Start;
        private Button Send;
        private Button Disconnect;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button Browse;
        private PictureBox pictureBox1;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
    }
}
