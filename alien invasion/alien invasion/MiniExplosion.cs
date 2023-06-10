using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alien_invasion
{
    internal class MiniExplosion
    {
        private static string _assetsPath = Fase_1.AssetPath;
        private int _disappearTime = 250;

        public MiniExplosion (Point position)
        {
            PictureBox obj = new PictureBox ();
            obj.Width = 22;
            obj.Height = 24;
            obj.Image = Image.FromFile(Path.Combine(_assetsPath, "miniExplosion.png"));
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
