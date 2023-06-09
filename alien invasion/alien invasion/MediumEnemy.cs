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
    internal class MediumEnemy
    {
        private static string _assetPath = Fase_1.AssetPath;

        private PictureBox _enemy;
        private double _life;
        private bool _dead = false;
        private static Random random = new Random();

        private object _locker = new { };
        private EventHandler<BulletEventArgs<MediumEnemyBullet>> _bulletCreated;
        private List<MediumEnemyBullet> _bullets = new List<MediumEnemyBullet>();

        private bool _isMovingRight;
        private int _speed = 10;
        private int _X;
        private int _Y;
        private int _offset = 40;
        private bool _boundary;

        public MediumEnemy(object sender, EventArgs e, Point position)
        {
            Fase_1 form = (Fase_1)sender;

            _enemy = new PictureBox();
            _enemy.Width = 45;
            _enemy.Height = 40;
            _enemy.SizeMode = PictureBoxSizeMode.CenterImage;
            _enemy.BackColor = Color.Transparent;
            _enemy.Image = Image.FromFile(Path.Combine(_assetPath, "alienMedium.png"));

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

            bool goRight = positions.Contains(new Point(_X + _speed + _offset, _Y));
            bool goLeft = positions.Contains(new Point(_X - _speed - _offset, _Y));

            positions.Remove(_enemy.Location);

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
            else if (random.Next(0, 100) < 10) // 10% de chance dele inverter o movimento
            {
                _isMovingRight = !_isMovingRight;
            }
            else if (!_boundary && goRight || goLeft) // Evita que os inimigos se interpolem
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
            if (random.Next(0, 100) < 1) //1% de chance de atirar
            {
                MediumEnemyBullet bullet = new MediumEnemyBullet(_enemy.Left + (_enemy.Width / 2), _enemy.Top + _enemy.Height);
                _bullets.Add(bullet);
                _bulletCreated?.Invoke(this, new BulletEventArgs<MediumEnemyBullet>(bullet));
            }

            positions.Add(_enemy.Location);
            return positions;
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
