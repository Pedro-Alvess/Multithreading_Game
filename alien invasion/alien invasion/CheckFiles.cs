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
        public void CheckAssets()
        {
            string assetsFolderPath = "alien invasion\\alien invasion\\Assets";
            string[] requiredAssets = { "Bullet.png", "Player.png", "alienQueennn.png", "alienMediummm.png", "alienBasic.png" };

            bool allAssetsExist = true;

            foreach (string asset in requiredAssets)
            {
                string assetPath = Path.Combine(assetsFolderPath, asset);
                if (!File.Exists(assetPath))
                {
                    allAssetsExist = false;
                    break;                    
                }
            }

            if(!allAssetsExist)
            {
                MessageBox.Show("Os arquivos de assets estão corrompidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Task.Delay(5000).ContinueWith(t =>
                {
                    Application.Exit();
                });
            }
        }
    }
}
