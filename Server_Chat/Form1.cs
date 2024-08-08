using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Server_Chat
{
    public partial class Form1 : Form
    {
        private TcpListener listener;
        private TcpClient client;
        private NetworkStream NS;
        private StreamReader SR;
        private StreamWriter SW;
        public Form1()
        {
            InitializeComponent();
            Browse.Enabled = false;
            Send.Enabled = false;
            Disconnect.Enabled = false;
        }

        private async void Start_Click(object sender, EventArgs e)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 9050);
                listener.Start();
                client = await listener.AcceptTcpClientAsync();
                IPEndPoint ipep = (IPEndPoint)client.Client.RemoteEndPoint;
                textBox2.AppendText($"Connected wth {ipep.Address} at port {ipep.Port}" + Environment.NewLine);
                NS = client.GetStream();
                SR = new StreamReader(NS);
                SW = new StreamWriter(NS) { AutoFlush = true };
                Thread recvThread = new Thread(new ThreadStart(ReceiveMessages));
                recvThread.Start();
                Start.Enabled = false;
                Send.Enabled = true;
                Browse.Enabled = true;
                Disconnect.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error to start server: {ex.Message}");
            }
        }
        private async void ReceiveMessages()
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
                            string imagePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Server_Chat", imageName);
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
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Server_Chat", fileName);
                            await ReceiveAndSaveFile(filePath, true);
                        }
                        else if (receivedMessage.StartsWith("Compress:"))
                        {
                            Invoke(new Action(() => textBox2.AppendText($"Client: {receivedMessage}" + Environment.NewLine)));
                            string fileName = receivedMessage.Substring(9);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Server_Chat", fileName);
                            await ReceiveAndSaveFile(filePath, false);

                            string compressedFile = CompressFile(filePath);

                            await SendCompressedFile(compressedFile);

                        }
                        else if (receivedMessage.StartsWith("CompressImage:"))
                        {
                            Invoke(new Action(() => textBox2.AppendText($"Client: {receivedMessage}" + Environment.NewLine)));
                            string fileName = receivedMessage.Substring(14);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Server_Chat", fileName);
                            await ReceiveAndSaveFile(filePath, false);

                            // Compress the received image
                            string compressedImagePath = CompressImage(filePath);

                            // Send the compressed image back to the client
                            await SendCompressedImage(compressedImagePath);
                        }
                        else if (receivedMessage.StartsWith("DecompFile:"))
                        {
                            Invoke(new Action(() => textBox2.AppendText($"Client: {receivedMessage}" + Environment.NewLine)));
                            string fileName = receivedMessage.Substring(11);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Server_Chat", fileName);
                            await ReceiveAndSaveFile(filePath, false);

                            string DecompressedFile = DecompressFile(filePath);

                            await SendDecompressedFile(DecompressedFile);

                        }
                        else if (receivedMessage.StartsWith("DecompImage:"))
                        {
                            Invoke(new Action(() => textBox2.AppendText($"Client: {receivedMessage}" + Environment.NewLine)));
                            string fileName = receivedMessage.Substring(12);
                            string filePath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Server_Chat", fileName);
                            await ReceiveAndSaveFile(filePath, false);

                            string DecompressedImage = DecompressImage(filePath);

                            await SendDecompressedImage(DecompressedImage);

                        }
                        else if (receivedMessage.StartsWith("Video:"))
                        {
                            string videoName = receivedMessage.Substring(6);
                            string videoPath = Path.Combine("E:\\FCI\\FCI\\Level4\\SecondTerm\\NP\\Section\\Project\\Server_Chat", videoName);
                            await ReceiveFile(videoPath);
                            Invoke(new Action(() =>
                            {
                                axWindowsMediaPlayer1.URL = videoPath;
                                axWindowsMediaPlayer1.Ctlcontrols.play();
                            }));
                        }
                        else if (receivedMessage.StartsWith("RecvImage:"))
                        {
                            string filePath = receivedMessage.Substring(10);
                            string fileName = Path.GetFileName(filePath);
                            byte[] imageData = File.ReadAllBytes(filePath);
                            await SW.WriteLineAsync($"RecvImage:{fileName}");
                            await NS.WriteAsync(imageData, 0, imageData.Length);
                            Invoke(new Action(() => textBox2.AppendText("Client: " + receivedMessage + Environment.NewLine)));
                        }
                        else if (receivedMessage.StartsWith("RecvFile:"))
                        {
                            string filePath = receivedMessage.Substring(9);
                            string fileName = Path.GetFileName(filePath);
                            byte[] bytes = File.ReadAllBytes(filePath);
                            await SW.WriteLineAsync($"RecvFile:{fileName}");
                            await NS.WriteAsync(bytes, 0, bytes.Length);
                            Invoke(new Action(() => textBox2.AppendText("Client: " + receivedMessage + Environment.NewLine)));

                        }
                        else if (receivedMessage.StartsWith("Directory:"))
                        {
                            string path = receivedMessage.Substring(10);
                            DirectoryInfo directoryInfo = new DirectoryInfo(path);
                            FileInfo[] files = directoryInfo.GetFiles();
                            DirectoryInfo[] directories = directoryInfo.GetDirectories();
                            Invoke(new Action(async () =>
                            {
                                //textBox2.AppendText("Files:" + Environment.NewLine);
                                textBox2.AppendText($"Client: {path}" + Environment.NewLine);
                                await SW.WriteLineAsync("Files");
                                foreach (FileInfo file in files)
                                {
                                    //textBox2.AppendText($"Client: {file.Name}" + Environment.NewLine);
                                    await SW.WriteLineAsync($"{file.Name}");
                                }
                                //textBox2.AppendText("Directories:" + Environment.NewLine);
                                await SW.WriteLineAsync("Directory");
                                foreach (DirectoryInfo directory in directories)
                                {
                                    //textBox2.AppendText($"Client: {directory.Name}" + Environment.NewLine);
                                    await SW.WriteLineAsync($"{directory.Name}");
                                }
                            }));
                        }
                        else if (receivedMessage.StartsWith("Message:"))
                        {
                            // Message received
                            string message = receivedMessage.Substring(8);
                            Invoke(new Action(() => textBox2.AppendText($"Client: {message}" + Environment.NewLine)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving message : {ex.Message}");
            }
        }
        private string CompressImage(string inputImagePath)
        {
            string compressedImagePath = inputImagePath + ".QZip.Compressed";
            using (FileStream inputFileStream = new FileStream(inputImagePath, FileMode.Open, FileAccess.Read))
            using (FileStream outputFileStream = new FileStream(compressedImagePath, FileMode.Create, FileAccess.Write))
            using (GZipStream compressionStream = new GZipStream(outputFileStream, CompressionMode.Compress))
            {
                inputFileStream.CopyTo(compressionStream);
            }
            return compressedImagePath;
        }
        private async Task SendCompressedImage(string compressedImagePath)
        {
            string imageName = Path.GetFileName(compressedImagePath);
            await SW.WriteLineAsync($"CompressImage:{imageName}");
            byte[] imageData = File.ReadAllBytes(compressedImagePath);
            await NS.WriteAsync(imageData, 0, imageData.Length);
        }
        private string DecompressImage(string imagePath)
        {
            // Compress the received file
            string decompressedImagePath = imagePath + ".Decompressed";
            using (FileStream inputFileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            using (FileStream outputFileStream = new FileStream(decompressedImagePath, FileMode.Create, FileAccess.Write))
            using (GZipStream decompressionStream = new GZipStream(inputFileStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(outputFileStream);
            }
            return decompressedImagePath;
        }
        
        private async Task SendDecompressedImage(string compressedImagePath)
        {
            string imageName = Path.GetFileName(compressedImagePath);
            await SW.WriteLineAsync($"DecompImage:{imageName}");
            byte[] imageData = File.ReadAllBytes(compressedImagePath);
            await NS.WriteAsync(imageData, 0, imageData.Length);
        }
        private string CompressFile(string filePath)
        {
            // Compress the received file
            string compressedFilePath = filePath + ".QZip.Compressed";
            using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (FileStream outputFileStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write))
            using (GZipStream compressionStream = new GZipStream(outputFileStream, CompressionMode.Compress))
            {
                inputFileStream.CopyTo(compressionStream);
            }
            return compressedFilePath;
        }
        private async Task SendCompressedFile(string compressedFilePath)
        {
            string fileName = Path.GetFileName(compressedFilePath);
            await SW.WriteLineAsync($"Compress:{fileName}");
            byte[] fileData = File.ReadAllBytes(compressedFilePath);
            await NS.WriteAsync(fileData, 0, fileData.Length);
        }

        private string DecompressFile(string filePath)
        {
            // Compress the received file
            string decompressedFilePath = filePath + ".Decompressed";
            using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (FileStream outputFileStream = new FileStream(decompressedFilePath, FileMode.Create, FileAccess.Write))
            using (GZipStream decompressionStream = new GZipStream(inputFileStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(outputFileStream);
            }
            return decompressedFilePath;
        }
        private async Task SendDecompressedFile(string decompressedFilePath)
        {
            string fileName = Path.GetFileName(decompressedFilePath);
            await SW.WriteLineAsync($"DecompFile:{fileName}");
            byte[] fileData = File.ReadAllBytes(decompressedFilePath);
            await NS.WriteAsync(fileData, 0, fileData.Length);
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
                Invoke(new Action(() => textBox2.AppendText($"Client: {fileContent}" + Environment.NewLine)));
            }

        }
        private async Task ReceiveFile(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead;
                while ((bytesRead = await NS.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                }
            }
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
            SR.Close();
            SW.Close();
            client.Close();
            listener.Stop();
            textBox2.AppendText("Disconnected from client" + Environment.NewLine);

            Start.Enabled = true;
            Disconnect.Enabled = false;
            Send.Enabled = false;
            Browse.Enabled = false;
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All files (*.jpg;*.png;*.jpeg;*.bmp;*.txt;*.mp4;*.mkv)|*.jpg;*.png;*.jpeg;*.bmp;*.txt;*.mp4;*.mkv";
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
    }
}
