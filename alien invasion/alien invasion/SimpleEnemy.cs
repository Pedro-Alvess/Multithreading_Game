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
    internal class SimpleEnemy
    {
        private static string _assetPath = Fase_1.AssetPath;
        public bool _gameOver = false;

        private PictureBox _enemy;
        private double _life;
        private bool _dead = false;
        private static Random random = new Random();

        private object _locker = new { };
        private EventHandler<BulletEventArgs<SimpleEnemyBullet>> _bulletCreated;
        private List<SimpleEnemyBullet> _bullets = new List<SimpleEnemyBullet>();
        
        private bool _isMovingRight;
        private int _speed = 5;
        private bool _leftBoundary = false;
        private bool _rightBoundary = false;

        public SimpleEnemy(object sender, EventArgs e,Point position)
        {
            Fase_1 form = (Fase_1)sender;

            _enemy = new PictureBox();
            _enemy.Width = 40;
            _enemy.Height = 35;
            _enemy.SizeMode = PictureBoxSizeMode.CenterImage;
            _enemy.BackColor = Color.Transparent;
            _enemy.Image = Image.FromFile(Path.Combine(_assetPath, "alienBasic.png"));

            _enemy.Left = position.X;
            _enemy.Top = position.Y;

            form.Controls.Add(_enemy);
            _enemy.BringToFront();
        }
        public void StartEnemy() 
        {
            Task.Run(() => EnemyThread());
        }
        private void EnemyThread()
        {
            while (!_gameOver)
            {
                //Movimento randomico
                if (random.Next(0, 100) < 5) // 5% de chance dele inverter o movimento
                    _isMovingRight = !_isMovingRight;

                if (_isMovingRight)
                    _enemy.Left += _speed;
                else
                    _enemy.Left -= _speed;

                //Tiro aleatório para baixo
                if(random.Next(0, 100) < 65) //65% de chance de atirar
                {
                    SimpleEnemyBullet bullet = new SimpleEnemyBullet(_enemy.Left + (_enemy.Width / 2), _enemy.Top + _enemy.Height);
                    _bullets.Add(bullet);
                }

                //Implementar movimento das balas

            }
        }

    }
}
