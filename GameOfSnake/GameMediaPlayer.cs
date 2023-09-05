using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace GameOfSnake
{
    class GameMediaPlayer
    {
        public static Dictionary<string, SoundPlayer> SoundsDict { get; set; } = SoundsDict = new Dictionary<string, SoundPlayer>();

        public static void LoadMedia()
        {
            string[] sfxNames = Directory.GetFiles(@"Sound\SFX");
            foreach (string path in sfxNames)
            {
                SoundPlayer sfx = new SoundPlayer(path);
                sfx.Load();
                string name = path.Replace("Sound\\SFX\\", "").Replace(".wav", "");
                SoundsDict.Add(name, sfx);
            }
        }
    }
}
