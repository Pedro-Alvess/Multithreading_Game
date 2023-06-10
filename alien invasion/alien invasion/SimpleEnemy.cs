using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alien_invasion
{
    internal class SimpleEnemy
    {
        private static string _assetPath = Fase_1.AssetPath;

        private PictureBox _enemy;
        private int _life = 3;
        private bool _dead = false;
        private static Random random = new Random();

        private object _locker = new { };
        private EventHandler<BulletEventArgs<SimpleEnemyBullet>> _bulletCreated;
        private List<SimpleEnemyBullet> _bullets = new List<SimpleEnemyBullet>();

        private bool _isMovingRight;
        private int _speed = 5;
        private int _X;
        private int _Y;
        private int _offset = 30;
        private bool _boundary;

        public bool isDead
        {
            get { return _dead; }
        }
        public bool noBullets
        {
            get { return _bullets.Any(); }
        }
        public SimpleEnemy(object sender, EventArgs e, Point position)
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

            if(!(_enemy.IsDisposed || _enemy == null)) // Verifica se o inimigo ainda existe
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



                //Tiro aleatório para baixo
                if (random.Next(0, 100) < 2) //2% de chance de atirar
                {
                    SimpleEnemyBullet bullet = new SimpleEnemyBullet(_enemy.Left + (_enemy.Width / 2), _enemy.Top + _enemy.Height);
                    _bullets.Add(bullet);
                    _bulletCreated?.Invoke(this, new BulletEventArgs<SimpleEnemyBullet>(bullet));
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
                    if(_life <= 0)
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

    }
}
