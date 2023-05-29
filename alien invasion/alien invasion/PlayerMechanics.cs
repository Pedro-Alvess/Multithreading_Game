using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alien_invasion
{
    internal class PlayerMechanics
    {
        private object _locker = new object();
        public event EventHandler<BulletEventArgs> BulletCreated;

        private PictureBox _player;
        private int _speed = 5;
        private List<Bullet> _bullets = new List<Bullet>();

        public PlayerMechanics(PictureBox playerObject) 
        {
            this._player = playerObject;
        }

        public void MoveLeft()
        {
            _player.Left -= _speed;
        }

        public void MoveRight()
        {
            _player.Left += _speed;
        }
        public void Shoot()
        {
            Bullet bullet = new Bullet(_player.Left + (_player.Width / 2), _player.Top);
            _bullets.Add(bullet);

            BulletCreated?.Invoke(this, new BulletEventArgs(bullet));
        }
        public void Update()
        {
            lock (_locker) // Inicio do bloco de sincronização
            {
                foreach (Bullet _bullet in _bullets)
                {
                    _bullet.Move();
                }

                _bullets.RemoveAll(b => b.IsOutOfBounds());
            }// Fim do bloco de sincronização
        }
    }
}
