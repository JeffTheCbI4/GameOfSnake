using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace GameOfSnake
{
    public partial class MainMenuForm : Form
    {
        PlayingForm playingForm;
        SoundPlayer musicPlayer;
        public MainMenuForm()
        {
            InitializeComponent();
            GameMediaPlayer.LoadMedia();
            //musicPlayer = new SoundPlayer(@"Sound\Music\IntoTheJungle.wav");
            axMediaPlayer.URL = @"Sound\Music\IntoTheJungle.wav";
            axMediaPlayer.settings.playCount = 9999;
            axMediaPlayer.settings.volume = 5;
            axMediaPlayer.Ctlcontrols.play();
            //musicPlayer.PlayLooping();
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            if (playingForm == null) playingForm = new PlayingForm(this);
            playingForm.Show();
            this.Hide();
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!playingForm.IsDisposed) playingForm.Close();
        }

        private void CreditsLabel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Game made by Kirill 'JeffTheCbI4' Sergushin", 
                "Credits", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }
    }
}
