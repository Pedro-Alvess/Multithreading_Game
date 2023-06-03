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
        private PictureBox _bullet;
        public abstract void Move();
        public abstract bool IsOutOfBounds();
        public PictureBox GetPictureBox()
        {
            return _bullet;
        }

        public PictureBox creatBullet(int width, int height, string assetsPath, string assetsName, int startX, int startY)
        {
            PictureBox obj = new PictureBox();
            obj.Width = 15;
            obj.Height = 16;
            obj.Image = Image.FromFile(Path.Combine(assetsPath, "Bullet.png"));
            obj.BackColor = Color.Transparent;
            obj.Left = startX;
            obj.Top = startY;
            Fase_1.ActiveForm.Controls.Add(obj);
            obj.BringToFront();

            return obj;
        }
    }
}
