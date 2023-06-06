using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alien_invasion
{
    internal class BulletEventArgs<T>:EventArgs
    {
        public T Bullet { get;}

        public BulletEventArgs(T bullet)
        {
            Bullet = bullet;
        }
    }
}
