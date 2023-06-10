using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alien_invasion
{
    internal class Explosion
    {
        private static string _assetsPath = Fase_1.AssetPath;
        private int _disappearTime = 350;

        public Explosion(Point position) 
        {
            PictureBox obj = new PictureBox();
            obj.Width = 55;
            obj.Height = 50;
            obj.Image = Image.FromFile(Path.Combine(_assetsPath, "explosion.png"));
            obj.SizeMode = PictureBoxSizeMode.CenterImage;
            obj.BackColor = Color.Transparent;
            obj.Left = position.X;
            obj.Top = position.Y;


            Fase_1.ActiveForm.Invoke((MethodInvoker)(() =>
            {
                Fase_1.ActiveForm.Controls.Add(obj);
                obj.BringToFront();
            }));

            Task.Delay(_disappearTime).ContinueWith(_ => DeleteObj(obj));
        }
        private void DeleteObj(PictureBox obj)
        {
            Fase_1.ActiveForm.Invoke((MethodInvoker)(() =>
            {
                Fase_1.ActiveForm.Controls.Remove(obj);
            }));
        }
    }
}
