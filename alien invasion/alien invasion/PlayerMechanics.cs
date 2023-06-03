using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.AxHost;
using System.IO;
using System.Reflection;

namespace alien_invasion
{
    internal class PlayerMechanics
    {
        private object _locker = new { };
        public event EventHandler<BulletEventArgs> BulletCreated;

        private PictureBox _player;
        private int _speed = 5;
        private List<Bullet> _bullets = new List<Bullet>();

        private string _assetPath;

        public PlayerMechanics(string assetPath)
        {
            _assetPath = assetPath;
        }
        public void CreatPlayer(object sender, EventArgs e)
        {
            Fase_1 form = (Fase_1)sender;

            _player = new PictureBox();
            _player.Width = 90;
            _player.Height = 98;

            string CurrentPath = AppDomain.CurrentDomain.BaseDirectory;

            string imagePath = Path.Combine(CurrentPath.Remove(CurrentPath.Length - 11),"Assets", "Player.png");            
            _player.Image = Image.FromFile(Path.Combine(_assetPath, "Player.png"));

            _player.BackColor = Color.Transparent;
            _player.Left = 305 + 49;
            _player.Top = 425;


            form.Controls.Add(_player);
            _player.BringToFront();

        }

        public void MoveLeft()
        {
            _player.Left -= _speed;
        }

        public void MoveRight()
        {
            _player.Left += _speed;
        }
        public void ShootRight()
        {
            Bullet bullet = new Bullet(_player.Left + (_player.Width / 2) - 31, _player.Top, _assetPath);
            _bullets.Add(bullet);
            BulletCreated?.Invoke(this, new BulletEventArgs(bullet));
        }
        public void ShootLeft()
        {
            Bullet bullet = new Bullet(_player.Left + (_player.Width / 2) + 31, _player.Top, _assetPath);
            _bullets.Add(bullet);
            BulletCreated?.Invoke(this, new BulletEventArgs(bullet));
        }
        public void Update()
        {
            lock (_locker) // Inicio do bloco de sincronização
            {
                //foreach (Bullet _bullet in _bullets)
                //{
                //    _bullet.Move();
                //}

                for (int i = 0; i < _bullets.Count(); i++)
                {
                    _bullets[i].Move();
                }

                _bullets.RemoveAll(b => b.IsOutOfBounds());
            }// Fim do bloco de sincronização
        }
    }
}