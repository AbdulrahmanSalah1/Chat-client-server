using AxWMPLib;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client_Chat
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream NS;
        private StreamReader SR;
        private StreamWriter SW;
        public Form1()
        {
            InitializeComponent();
            Send.Enabled = false;
            Browse.Enabled = false;
            Disconnect.Enabled = false;
            CompressFile.Enabled = false;
            CompressImage.Enabled = false;
            ReceiveImage.Enabled = false;
            DecompFile.Enabled = false;
            DecompImage.Enabled = false;
            RecvFile.Enabled = false;
            //axWindowsMediaPlayer1.URL = "E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat\\pull-up.mp4";
            //axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private async void Connect_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync("127.0.0.1", 9050);
                NS = client.GetStream();
                SR = new StreamReader(NS);
                SW = new StreamWriter(NS) { AutoFlush = true };
                Thread thread = new Thread(new ThreadStart(ReceiveMessage));
                thread.Start();
                //ReceiveMessage();
                Connect.Enabled = false;
                Send.Enabled = true;
                Browse.Enabled = true;
                Disconnect.Enabled = true;
                CompressFile.Enabled = true;
                CompressImage.Enabled = true;
                ReceiveImage.Enabled = true;
                DecompFile.Enabled = true;
                DecompImage.Enabled = true;
                RecvFile.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
            }
        }
        private async void ReceiveMessage()
        {
            try
            {
                while (true)
                {
                    string receivedMessage = await SR.ReadLineAsync();
                    if (receivedMessage != null)
                    {
                        if (receivedMessage.StartsWith("Image:"))
                        {
                            string imageName = receivedMessage.Substring(6);
                            string imagePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat", imageName);
                            byte[] buffer = new byte[client.ReceiveBufferSize];
                            int bytesRead = await NS.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                            {
                                MemoryStream ms = new MemoryStream(buffer, 0, bytesRead);
                                pictureBox1.Image = Image.FromStream(ms);
                                byte[] image = ms.ToArray();
                                File.WriteAllBytes(imagePath, image);
                            }
                        }
                        else if (receivedMessage.StartsWith("File:"))
                        {
                            string fileName = receivedMessage.Substring(5);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat", fileName);
                            await ReceiveAndSaveFile(filePath, true);
                        }
                        else if (receivedMessage.StartsWith("RecvFile:"))
                        {
                            string fileName = receivedMessage.Substring(9);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat", fileName);
                            await ReceiveAndSaveFile(filePath, true);
                        }
                        else if (receivedMessage.StartsWith("Compress:"))
                        {
                            string fileName = receivedMessage.Substring(9);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat", fileName);
                            await ReceiveDecompAndCompFile(filePath);
                        }
                        else if (receivedMessage.StartsWith("DecompFile:"))
                        {
                            string fileName = receivedMessage.Substring(11);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat", fileName);
                            await ReceiveDecompAndCompFile(filePath);
                        }
                        else if (receivedMessage.StartsWith("DecompImage:"))
                        {
                            string fileName = receivedMessage.Substring(12);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat", fileName);
                            await ReceiveDecompAndCompImage(filePath);
                            pictureBox1.Image = Image.FromFile(filePath);
                        }
                        else if (receivedMessage.StartsWith("CompressImage:"))
                        {
                            string fileName = receivedMessage.Substring(14);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat", fileName);
                            await ReceiveDecompAndCompImage(filePath);
                        }
                        else if (receivedMessage.StartsWith("RecvImage:"))
                        {
                            string imageName = receivedMessage.Substring(10);
                            string imagePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat", imageName);
                            byte[] buffer = new byte[client.ReceiveBufferSize];
                            int bytesRead = await NS.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                            {
                                MemoryStream ms = new MemoryStream(buffer, 0, bytesRead);
                                pictureBox1.Image = Image.FromStream(ms);
                                byte[] image = ms.ToArray();
                                File.WriteAllBytes(imagePath, image);
                                Invoke(new Action(() => textBox2.AppendText($"Received image from server" + Environment.NewLine)));
                            }
                        }
                        else if (receivedMessage.StartsWith("Video:"))
                        {
                            string videoName = receivedMessage.Substring(6);
                            //string video = Path.GetFileName(videoName);
                            string videoPath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Client_Chat", videoName);
                            await ReceiveFile(videoPath, false);
                            Invoke(new Action(() =>
                            {
                                try
                                {
                                    axWindowsMediaPlayer1.URL = videoPath;
                                    axWindowsMediaPlayer1.Ctlcontrols.play();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error playing video: {ex.Message}");
                                }
                            }));
                        }
                        
                        else if (receivedMessage.StartsWith("Message:"))
                        {
                            string message = receivedMessage.Substring(8);
                            Invoke(new Action(() => textBox2.AppendText($"Server: {message}" + Environment.NewLine)));
                        }
                        else
                        {
                            Invoke(new Action(() => textBox2.AppendText($"Server: {receivedMessage}" + Environment.NewLine)));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Receiving Message: {ex.Message}");
            }
        }
        private async Task ReceiveDecompAndCompImage(string imagePath)
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = await NS.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                await File.WriteAllBytesAsync(imagePath, buffer);
            }
            // Display a message indicating that the compressed image has been received and saved
            Invoke(new Action(() => textBox2.AppendText($"Compressed or Decompressed image received and saved at: {imagePath}" + Environment.NewLine)));
        }
        private async Task ReceiveAndSaveFile(string filePath, bool isContent)
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = await NS.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                await File.WriteAllBytesAsync(filePath, buffer);
            }
            if (isContent)
            {
                string fileContent = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Invoke(new Action(() => textBox2.AppendText($"Server: {fileContent}" + Environment.NewLine)));
            }
        }
        private async Task ReceiveFile(string filePath, bool isContent)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead;
                while ((bytesRead = await NS.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                }
                if (isContent)
                {
                    string fileContent = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Invoke(new Action(() => textBox2.AppendText($"Server: {fileContent}" + Environment.NewLine)));
                }
            }
            
        }
        private async Task ReceiveDecompAndCompFile(string filePath)
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = await NS.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                await File.WriteAllBytesAsync(filePath, buffer);
            }
            // Display a message indicating that the compressed image has been received and saved
            Invoke(new Action(() => textBox2.AppendText($"Compressed or Decompressed file received and saved at: {filePath}" + Environment.NewLine)));
        }
        private async void Send_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            string filePath = textBox1.Text;
            if (File.Exists(filePath))
            {
                string fileName = Path.GetFileName(filePath);
                if (fileName.ToLower().EndsWith(".txt"))
                {
                    await SW.WriteLineAsync($"File:{fileName}");
                }
                else if (fileName.ToLower().EndsWith(".mp4") || fileName.ToLower().EndsWith(".mkv"))
                {
                    await SW.WriteLineAsync($"Video:{fileName}");
                }
                else if (fileName.ToLower().EndsWith(".jpg") || fileName.ToLower().EndsWith(".jpeg") || fileName.ToLower().EndsWith(".png") || fileName.ToLower().EndsWith(".bmp"))
                {
                    await SW.WriteLineAsync($"Image:{fileName}");
                }
                byte[] fileData = File.ReadAllBytes(filePath);
                await NS.WriteAsync(fileData, 0, fileData.Length);
            }
            else if (Directory.Exists(filePath))
            {
                await SW.WriteLineAsync($"Directory:{filePath}");
                byte[] path = Encoding.UTF8.GetBytes(filePath);
                await NS.WriteAsync(path, 0, path.Length);
            }
            else
            {
                // Send message
                string message = textBox1.Text;
                await SW.WriteLineAsync($"Message:{message}");
            }
            textBox1.Clear();
        }
        private void Disconnect_Click(object sender, EventArgs e)
        {
            NS.Close();
            SW.Close();
            SR.Close();
            client.Close();
            textBox2.AppendText("Disconnected from server" + Environment.NewLine);

            Connect.Enabled = true;
            Disconnect.Enabled = false;
            Send.Enabled = false;
            Browse.Enabled = false;
            CompressFile.Enabled = false;
            CompressImage.Enabled = false;
            ReceiveImage.Enabled = false;
            RecvFile.Enabled = false;
            DecompImage.Enabled = false;
            DecompFile.Enabled = false;
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All files (*.jpg;*.png;*.jpeg;*.bmp;*.txt;*.mp4;*.mkv;*.QZip.Compressed)|*.jpg;*.png;*.jpeg;*.bmp;*.txt;*.mp4;*.mkv;*.QZip.Compressed";
            ofd.Title = "Select a File";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = ofd.FileName;
                    textBox1.Text = filePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private async void CompressFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            string filePath = textBox1.Text;
            string fileName = Path.GetFileName(filePath);
            if (File.Exists(filePath))
            {
                if (fileName.ToLower().EndsWith(".txt"))
                {
                    await SW.WriteLineAsync($"Compress:{filePath}");
                    byte[] fileData = File.ReadAllBytes(filePath);
                    await NS.WriteAsync(fileData, 0, fileData.Length);
                }
            }
            textBox1.Clear();
        }

        private async void CompressImage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            string filePath = textBox1.Text;
            string fileName = Path.GetFileName(filePath);
            if (File.Exists(filePath))
            {
                if (fileName.ToLower().EndsWith(".jpg") || fileName.ToLower().EndsWith(".jpeg") || fileName.ToLower().EndsWith(".png") || fileName.ToLower().EndsWith(".bmp"))
                {
                    await SW.WriteLineAsync($"CompressImage:{filePath}");
                    byte[] fileData = File.ReadAllBytes(filePath);
                    await NS.WriteAsync(fileData, 0, fileData.Length);
                }
            }
            textBox1.Clear();
        }

        private async void ReceiveImage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            string filePath = textBox1.Text;
            string fileName = Path.GetFileName(filePath);
            if (File.Exists(filePath))
            {
                if (fileName.ToLower().EndsWith(".jpg") || fileName.ToLower().EndsWith(".jpeg") || fileName.ToLower().EndsWith(".png") || fileName.ToLower().EndsWith(".bmp"))
                {
                    await SW.WriteLineAsync($"RecvImage:{fileName}");
                    byte[] fileData = File.ReadAllBytes(filePath);
                    await NS.WriteAsync(fileData, 0, fileData.Length);
                }
            }
            textBox1.Clear();
        }

        private async void RecvFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            string filePath = textBox1.Text;
            string fileName = Path.GetFileName(filePath);
            if (File.Exists(filePath))
            {
                if (fileName.ToLower().EndsWith(".txt"))
                {
                    await SW.WriteLineAsync($"RecvFile:{fileName}");
                    byte[] fileData = File.ReadAllBytes(filePath);
                    await NS.WriteAsync(fileData, 0, fileData.Length);
                }
            }
            textBox1.Clear();
        }

        private async void DecompFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            string filePath = textBox1.Text;
            string fileName = Path.GetFileName(filePath);
            if (File.Exists(filePath))
            {
                await SW.WriteLineAsync($"DecompFile:{fileName}");
                byte[] fileData = File.ReadAllBytes(filePath);
                await NS.WriteAsync(fileData, 0, fileData.Length);
                //MessageBox.Show("Compressed file sent successfully.");

            }
            textBox1.Clear();
        }

        private async void DecompImage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            string filePath = textBox1.Text;
            string fileName = Path.GetFileName(filePath);
            if (File.Exists(filePath))
            {
                await SW.WriteLineAsync($"DecompImage:{fileName}");
                byte[] fileData = File.ReadAllBytes(filePath);
                await NS.WriteAsync(fileData, 0, fileData.Length);
                //MessageBox.Show("Compressed file sent successfully.");

            }
            textBox1.Clear();
        }
    }
}
