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
    internal class PlayerBullet: Bullet
    {
        private PictureBox _bullet;
        private int _speed = 5;

        public PlayerBullet(int startX, int startY)
        {
            //Chama o metodo da classe abstrata para criar a bullet
            _bullet = base.creatBullet(15,16,"Bullet.png",startX - 16/2,startY);
        }
        public override void Move()
        {
            if (_bullet.IsDisposed || _bullet == null)
            {
                return; // Abortar a execução se o objeto já foi descartado
            }
            try
            {
                if (_bullet.InvokeRequired && !_bullet.IsDisposed)
                {
                    _bullet.Invoke(new MethodInvoker(Move));
                }
                else
                {
                    _bullet.Top -= _speed;
                }
            }
            catch (ObjectDisposedException){ } // Ignora se ele tentar mover um objeto que não existe.
        }

        public override bool IsOutOfBounds()
        {
            return _bullet.Top + _bullet.Height < 0 || _bullet.Top - _bullet.Height > 600;
        }

        public PictureBox GetPictureBox()
        {
            return _bullet;
        }
    }
}
