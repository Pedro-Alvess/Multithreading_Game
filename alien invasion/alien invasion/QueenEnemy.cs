using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alien_invasion
{
    internal class QueenEnemy
    {
        private static string _assetPath = Fase_1.AssetPath;

        private PictureBox _enemy;
        private double _life = 10;
        private bool _dead = false;
        private static Random random = new Random();

        private object _locker = new { };
        private EventHandler<BulletEventArgs<QueenEnemyBullet>> _bulletCreated;
        private List<QueenEnemyBullet> _bullets = new List<QueenEnemyBullet>();
        private Stopwatch stopwatch = new Stopwatch();
        private int _timeBetweenShot = 1000;

        private bool _isMovingRight;
        private int _speed = 5;
        private int _X;
        private int _Y;
        private int _offset = 55;
        private bool _boundary;

        private int _shotPerc = 1;

        public bool isDead
        {
            get { return _dead; }
        }
        public bool noBullets
        {
            get { return _bullets.Any(); }
        }
        public QueenEnemy(object sender, EventArgs e, Point position)
        {
            Fase_1 form = (Fase_1)sender;

            _enemy = new PictureBox();
            _enemy.Width = 60;
            _enemy.Height = 55;
            _enemy.SizeMode = PictureBoxSizeMode.CenterImage;
            _enemy.BackColor = Color.Transparent;
            _enemy.Image = Image.FromFile(Path.Combine(_assetPath, "alienQueen.png"));

            _enemy.Left = position.X;
            _enemy.Top = position.Y;

            //Sincroniza o objeto com a thread principal
            form.Invoke((MethodInvoker)(() =>
            {
                form.Controls.Add(_enemy);
                _enemy.BringToFront();
            }));
        }
        public List<Point> EnemyMovement(List<Point> positions)
        {
            _boundary = false;
            _Y = _enemy.Location.Y;
            _X = _enemy.Location.X;

            positions.Remove(_enemy.Location);

            CollisionBulletDetection(positions);

            if (!(_enemy.IsDisposed || _enemy == null))
            {
                bool goRight = positions.Contains(new Point(_X + _speed + _offset, _Y));
                bool goLeft = positions.Contains(new Point(_X - _speed - _offset, _Y));

                if (_enemy.Left + _speed >= 750)
                {
                    _isMovingRight = !_isMovingRight;
                    _boundary = true;
                }
                else if (_enemy.Left - _speed <= 0)
                {
                    _isMovingRight = !_isMovingRight;
                    _boundary = true;
                }
                else if (random.Next(0, 100) < 14) // 14% de chance dele inverter o movimento
                {
                    _isMovingRight = !_isMovingRight;
                }
                else if (!_boundary && goRight || goLeft)// Evita que os inimigos se interpolem
                {
                    _isMovingRight = !_isMovingRight;
                }

                //Como os asstes estão na thread principal, só é possivel atualizar a possição na thread form.
                if (_isMovingRight && !goRight)
                {
                    _enemy.Invoke((MethodInvoker)(() => _enemy.Left += _speed));
                }
                else if (!_isMovingRight && !goLeft)
                {
                    _enemy.Invoke((MethodInvoker)(() => _enemy.Left -= _speed));
                }

                if (PlayerMechanics.Score >= 1000)
                {
                    _shotPerc = 13;
                }
                else if (PlayerMechanics.Score >= 750)
                {
                    _shotPerc = 5;
                }
                else if (PlayerMechanics.Score >= 500)
                {
                    _shotPerc = 3;
                }

                //Tiro aleatório para baixo
                if (random.Next(0, 100) < _shotPerc && (stopwatch.ElapsedMilliseconds >= _timeBetweenShot || !stopwatch.IsRunning)) //Chance de atirar
                {
                    stopwatch.Restart();
                    stopwatch.Start();
                    QueenEnemyBullet bullet = new QueenEnemyBullet(_enemy.Left + (_enemy.Width / 2), _enemy.Top + _enemy.Height);
                    _bullets.Add(bullet);
                    _bulletCreated?.Invoke(this, new BulletEventArgs<QueenEnemyBullet>(bullet));
                }

                positions.Add(_enemy.Location);
            }


            return positions;
        }
        private void CollisionBulletDetection(List<Point> position)
        {
            List<PlayerBullet> playerBullets = PlayerMechanics.bullets;

            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                PlayerBullet b = playerBullets[i];

                if (b.GetPictureBox().Bounds.IntersectsWith(_enemy.Bounds))
                {
                    new MiniExplosion(b.GetPictureBox().Location);

                    lock (_locker)
                    {
                        PlayerMechanics.bullets.Remove(b);
                    }

                    Fase_1.ActiveForm.Invoke((MethodInvoker)(() =>
                    {
                        Fase_1.ActiveForm.Controls.Remove(b.GetPictureBox());
                        b.GetPictureBox().Dispose();
                    }));

                    _life--;
                    if (_life <= 0)
                    {
                        _dead = true;
                        _DestroyEnemy();
                        return;
                    }
                }
            }
        }
        private void _DestroyEnemy()
        {
            new Explosion(_enemy.Location);
            PlayerMechanics.Score += 150;

            Fase_1.ActiveForm.Invoke((MethodInvoker)(() =>
            {
                Fase_1.ActiveForm.Controls.Remove(_enemy);
                _enemy.Dispose();
            }));
        }

        public void Update()
        {
            lock (_locker)//Inicio do bloco de sincronização
            {
                for (int i = 0; i < _bullets.Count; i++)
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
                }); // Usa uma função Lambda para remover as bullets
            }
        }
        public List<QueenEnemyBullet> GetBulletList()
        {
            return new List<QueenEnemyBullet>(_bullets);
        }
        public void RemoveBullet(QueenEnemyBullet bullet)
        {
            PlayerMechanics.Shild -= 35;

            QueenEnemyBullet bulletToRemove = _bullets.Find(b => b == bullet);
            if (bulletToRemove != null)
            {
                _bullets.Remove(bulletToRemove);
                Fase_1.ActiveForm.Invoke((MethodInvoker)(() =>
                {
                    Fase_1.ActiveForm.Controls.Remove(bulletToRemove.GetPictureBox());
                    bulletToRemove.GetPictureBox().Dispose();
                }));
            }
        }


    }
}
