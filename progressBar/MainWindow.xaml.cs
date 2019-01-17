using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace progressBar
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public  void DoWork(IProgress<int> progress)
        {
            for (int i = 0; i <= 100; i++)
            {
                progress.Report(i);
                //Simulating a long task
                Task.Delay(100).Wait();
            }
        }

        public async Task DownloadFile()
        {

            var uri = new Uri("http://ipv4.download.thinkbroadband.com/100MB.zip");
            string fileName ="largefile.zip";

            using (var client = new WebClient())
            {
                client.Headers.Add("User-Agent: Other");
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                await client.DownloadFileTaskAsync(uri, fileName);
            }
           
           
            
        }

        public  void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs args)
        {
            var downloadProgress = args.ProgressPercentage;
            progressBar2.Value = downloadProgress;
        }

        private void client_DownloadFileCompleted(object sender,AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("File  downloaded");
                progressBar2.Value = 0;
            }

           
        }


        private async Task <string> loadFile()
        {
            await Task.Delay(5000);
            using (var reader = File.OpenText("Words.txt"))
            {
                return await reader.ReadToEndAsync();
                
            }

        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            var progress = new Progress<int>(v =>
            {
                progressBar1.Value = v;
            });
            await Task.Run(() => DoWork(progress));
            MessageBox.Show("Task finished");
        }

        


        private  async void Button2_Click(object sender, RoutedEventArgs e)
        {
            progressBar2.Maximum = 100;
            progressBar2.Minimum = 0;
            try
            {
                await DownloadFile();
            }
            catch(Exception)
            {
                MessageBox.Show("Download error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private async void Button3_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "";
            try
            {
                var linesofFile = await loadFile();
                textBlock.Text = linesofFile;
            }
            catch (Exception)
            {
                MessageBox.Show("Load error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
