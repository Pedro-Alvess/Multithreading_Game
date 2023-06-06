using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alien_invasion
{
    internal class SimpleEnemyBullet: Bullet
    {
        private PictureBox _bullet;
        private int _speed = 3;

        public SimpleEnemyBullet(int startX, int startY)
        {
            _bullet = base.creatBullet(13, 13, "enemySimpleBullet.png", startX, startY);
        }

        public override void Move()
        {
            // Se for verdadeiro significa que está sendo chamado por uma thread diferente da thread de UI;
            if(_bullet.InvokeRequired)
            {
                //Chama a thread de UI;
                _bullet.Invoke(new MethodInvoker(Move));
            }
            else
            {
                _bullet.Top += _speed;
            }
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
