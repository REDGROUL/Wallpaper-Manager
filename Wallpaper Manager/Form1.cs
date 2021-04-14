using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using VideoLibrary;

namespace Wallpaper_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string wpid;
        string audio = "--no-audio";
        

        public void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("wallpapers")) {
                Directory.CreateDirectory("wallpapers");
            }
            if (!Directory.Exists("cahed_preview")) {
                Directory.CreateDirectory("cahed_preview");
            }
            refresh();
            
        }

        void refresh()
        {
            //превью
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            //работа с файлами
            string[] files = Directory.GetFiles("wallpapers", "*.mp4");
            
            

            foreach (string paths in files)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Width = 400;
                pictureBox.Height = 240;
                string path = Path.GetFileName(paths);
                string name = path.Substring(0, path.Length - 4);
                if (!File.Exists(@"cahed_preview\" + name + ".jpg"))
                {
                    ffMpeg.GetVideoThumbnail(paths, @"cahed_preview\" + name + ".jpg");
                }
                pictureBox.Name = path;
                pictureBox.SizeMode = pictureBox1.SizeMode;
                pictureBox.Image = Image.FromFile(@"cahed_preview\" + name + ".jpg");
                pictureBox.Text = path;
                pictureBox.Click += new System.EventHandler(this.pictureClick);
                this.flowLayoutPanel1.Controls.Add(pictureBox);
            }
        }
        void pictureClick(object sender, EventArgs e)
        {
            
            PictureBox currentpicture = (PictureBox)sender;
            setWallpapers(@"wallpapers\"+currentpicture.Name, audio);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wallpaper Manager (WeeBP GUI+MPV). \nAuthor: REDGROUL \nVersion: pre-Alpha 5\nTest edition❤");
        }
       
        string runWeebp()
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c WeeBPMpv\\wp id /t",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
            wpid = process.StandardOutput.ReadToEnd();
            return wpid;
        }
        public void setWallpapers(string name, string audio)
        {
            DisableMPV();
            string wpid = runWeebp();
            string stat = "/c WeeBPMpv\\wp run WeeBPMpv\\mpv --wid=" + wpid + " " + name + " --loop=inf --player-operation-mode=pseudo-gui --force-window=yes " + audio + " /t";
            if (debugToolStripMenuItem.Checked)
            {
                MessageBox.Show(stat);
            }
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = stat,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }); ;
        }

        public void DisableMPV()
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c taskkill /IM mpv.exe /F /t",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label1.Visible = true;
                textBox1.Visible = true;
                button1.Visible = true;
            }
            else 
            {
                label1.Visible = false;
                textBox1.Visible = false;
                button1.Visible = false;
            }
        }

        private void resetDoDefualtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableMPV();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            refresh();
        }
        private void helpToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show("Если обои не ставятся:\n1)переименуйте файл лучше всего в цифры\n2)Запустите mpv.exe" +
             //   " из папки приложения если запустился то скачайте заново Weebp с github'a\n3)Если не помогло пиши автору");
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Maximized;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
            else if (FormWindowState.Normal == this.WindowState)
            { notifyIcon1.Visible = false; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "") 
            {
                string link = textBox1.Text;
             //   SaveVideoToDisk(link);
                setWallpapers(textBox1.Text,audio);
            }
        }
        void SaveVideoToDisk(string link)
        {
            var youTube = YouTube.Default; // starting point for YouTube actions
            var video = youTube.GetVideo(link);// gets a Video object with info about the video
            File.WriteAllBytes(@"wallpapers" + video.FullName, video.GetBytes());
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                audio = "";
            }
            else {
                audio = "--no-audio";
            }
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Width = this.Width;
            flowLayoutPanel1.Height = this.Height - 160;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisableMPV();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!debugToolStripMenuItem.Checked)
            {
                debugToolStripMenuItem.Checked = true;
            }
            else 
            {
                debugToolStripMenuItem.Checked = false;
            }

        }

        private void soundToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (soundToolStripMenuItem.Checked)
            {
                soundToolStripMenuItem.Checked = false;
            }
            else {
                soundToolStripMenuItem.Checked = true;
            }

            if (soundToolStripMenuItem.Checked)
            {
                audio = "";
            }
            else
            {
                audio = "--no-audio";
            }
        }

       /* private void button1_Click(object sender, EventArgs e)
        {

        }*/

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
           string path = Directory.GetCurrentDirectory()+ @"\wallpapers";
            Process.Start(new ProcessStartInfo("explorer.exe", " /open, "+ path));
        }

        private void gitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/REDGROUL/Wallpaper-Manager");
        }
    }
}