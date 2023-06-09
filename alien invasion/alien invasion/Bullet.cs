using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace alien_invasion
{
    internal abstract class Bullet
    {
        private static string _assetsPath = Fase_1.AssetPath;
        public abstract void Move();
        public abstract bool IsOutOfBounds();

        public PictureBox creatBullet(int width, int height, string assetsName, int startX, int startY)
        {
            PictureBox obj = new PictureBox();
            obj.Width = width;
            obj.Height = height;
            obj.Image = Image.FromFile(Path.Combine(_assetsPath, assetsName));
            obj.SizeMode = PictureBoxSizeMode.CenterImage;
            obj.BackColor = Color.Transparent;
            obj.Left = startX;
            obj.Top = startY;

            Fase_1.ActiveForm.Invoke((MethodInvoker)(() =>
            {
                Fase_1.ActiveForm.Controls.Add(obj);
                obj.BringToFront();
            }));


            return obj;
        }
    }
}
