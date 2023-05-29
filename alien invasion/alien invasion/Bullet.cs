using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alien_invasion
{
    internal class Bullet
    {
        private PictureBox _bullet;
        private int _speed = 5;

        public Bullet(int startX, int startY)
        {
            _bullet = new PictureBox();
            _bullet.Width = 15;
            _bullet.Height = 16;
            _bullet.Image = Image.FromFile("C:\\Users\\pepeh\\OneDrive\\Documentos\\GitHub\\Multithreading_Game\\alien invasion\\alien invasion\\Assets\\Bullet.png");
            _bullet.BackColor = Color.Transparent;
            _bullet.Left = startX - (_bullet.Width / 2);
            _bullet.Top = startY;
            //Fase_1.ActiveForm.Controls.Add(_bullet);
        }

        public void Move()
        {
            if (_bullet.InvokeRequired)
            {
                _bullet.Invoke(new MethodInvoker(Move));
            }
            else
            {
                _bullet.Top += _speed;
            }
        }

        public bool IsOutOfBounds()
        {
            return _bullet.Top + _bullet.Height < 0;
        }

        public PictureBox GetPictureBox()
        {
            return _bullet;
        }
    }
}
