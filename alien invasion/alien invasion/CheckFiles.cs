using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alien_invasion
{
    internal class CheckFiles
    {
        private string AssetPath;
        public string CheckAssets()
        {
            string CurrentPath = AppDomain.CurrentDomain.BaseDirectory;
            AssetPath = Path.Combine(CurrentPath.Remove(CurrentPath.Length - 11), "Assets");

            string[] requiredAssets = { "Bullet.png", "Player.png", "alienQueennn.png", "alienMediummm.png", "alienBasic.png" };

            bool allAssetsExist = true;

            foreach (string asset in requiredAssets)
            {
                string assetPath = Path.Combine(AssetPath, asset);
                if (!File.Exists(assetPath))
                {
                    allAssetsExist = false;
                    break;                    
                }
            }

            if(!allAssetsExist)
            {
                MessageBox.Show("Os arquivos de assets está corrompido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            return AssetPath;
        }
    }
}
