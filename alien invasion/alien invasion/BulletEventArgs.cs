using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alien_invasion
{
    internal class BulletEventArgs:EventArgs
    {
        public PlayerBullet Bullet { get;}

        public BulletEventArgs(PlayerBullet bullet)
        {
            Bullet = bullet;
        }
    }
}
