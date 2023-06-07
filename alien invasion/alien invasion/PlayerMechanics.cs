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
        private PictureBox _player;
        private double _live;
        public bool alive = false;

        private object _locker = new { };
        public event EventHandler<BulletEventArgs<PlayerBullet>> bulletCreated;
        private List<PlayerBullet> _bullets = new List<PlayerBullet>();

        private int _speed = 5;
        private bool _leftBoundary = false;
        private bool _rightBoundary = false;
        
        private static string _assetPath = Fase_1.AssetPath;

        public void CreatPlayer(object sender, EventArgs e)
        {
            Fase_1 form = (Fase_1)sender;

            _player = new PictureBox();
            _player.Width = 90;
            _player.Height = 98;
            _player.SizeMode = PictureBoxSizeMode.CenterImage;          
            _player.Image = Image.FromFile(Path.Combine(_assetPath, "Player.png"));
            _player.BackColor = Color.Transparent;
            _player.Left = 305 + 49;
            _player.Top = 425;


            form.Controls.Add(_player);
            _player.BringToFront();

        }

        public void MoveLeft()
        {
            if (_player.Left - _speed <= -7)
            {
                _leftBoundary = true;
            }
            else
            {
                _leftBoundary = false;
            }

            if (!_leftBoundary)
            {
                _player.Left -= _speed;
            }
            
        }

        public void MoveRight()
        {
            if(_player.Left + _speed >= 700)
            {
                _rightBoundary = true;
            }
            else
            {
                _rightBoundary = false;
            }
            if (!_rightBoundary)
            {
                _player.Left += _speed;
            }
        }
        public void ShootRight()
        {
            PlayerBullet bullet = new PlayerBullet(_player.Left + (_player.Width / 2) - 31, _player.Top);
            _bullets.Add(bullet);
            bulletCreated?.Invoke(this, new BulletEventArgs<PlayerBullet>(bullet));
        }
        public void ShootLeft()
        {
            PlayerBullet bullet = new PlayerBullet(_player.Left + (_player.Width / 2) + 31, _player.Top);
            _bullets.Add(bullet);
            bulletCreated?.Invoke(this, new BulletEventArgs<PlayerBullet>(bullet));
        }
        public void Update()
        {
            lock (_locker) // Inicio do bloco de sincronização
            {
                for (int i = 0; i < _bullets.Count(); i++)
                {
                    _bullets[i].Move();
                }
                _bullets.RemoveAll(b =>
                {
                    bool isOutOfBounds = b.IsOutOfBounds();
                    if (isOutOfBounds)
                    {
                        // Remover o objeto da interface do usuário
                        Fase_1.ActiveForm.Invoke((MethodInvoker)(() =>
                        {
                            Fase_1.ActiveForm.Controls.Remove(b.GetPictureBox());
                            b.GetPictureBox().Dispose();
                        }));
                    }
                    return isOutOfBounds;
                });
            }// Fim do bloco de sincronização
        }
    }
}