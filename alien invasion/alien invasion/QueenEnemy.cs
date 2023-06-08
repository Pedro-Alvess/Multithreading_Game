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
    internal class QueenEnemy
    {
        private static string _assetPath = Fase_1.AssetPath;

        private PictureBox _enemy;
        private double _life;
        private bool _dead = false;
        private static Random random = new Random();

        private object _locker = new { };
        private EventHandler<BulletEventArgs<QueenEnemyBullet>> _bulletCreated;
        private List<QueenEnemyBullet> _bullets = new List<QueenEnemyBullet>();

        private bool _isMovingRight;
        private int _speed = 5;

        public QueenEnemy(object sender, EventArgs e, Point position)
        {
            Fase_1 form = (Fase_1)sender;

            _enemy = new PictureBox();
            _enemy.Width = 60;
            _enemy.Height = 55;
            _enemy.SizeMode = PictureBoxSizeMode.CenterImage;
            _enemy.BackColor = Color.Transparent;
            _enemy.Image = Image.FromFile(Path.Combine(_assetPath, "alienQueennn.png"));

            _enemy.Left = position.X;
            _enemy.Top = position.Y;

            //Sincroniza o objeto com a thread principal
            form.Invoke((MethodInvoker)(() =>
            {
                form.Controls.Add(_enemy);
                _enemy.BringToFront();
            }));
        }
        public void EnemyMovement()
        {
            if (_enemy.Left + _speed >= 750)
            {
                _isMovingRight = !_isMovingRight;
            }
            else if (_enemy.Left - _speed <= 0)
            {
                _isMovingRight = !_isMovingRight;
            }
            else if (random.Next(0, 100) < 10) // 5% de chance dele inverter o movimento
            {
                _isMovingRight = !_isMovingRight;
            }

            //Como os asstes estão na thread principal, só é possivel atualizar a possição na thread form.
            if (_isMovingRight)
            {
                _enemy.Invoke((MethodInvoker)(() => _enemy.Left += _speed));
            }
            else
            {
                _enemy.Invoke((MethodInvoker)(() => _enemy.Left -= _speed));
            }



            //Tiro aleatório para baixo
            if (random.Next(0, 100) < 1) //5% de chance de atirar
            {
                QueenEnemyBullet bullet = new QueenEnemyBullet(_enemy.Left + (_enemy.Width / 2), _enemy.Top + _enemy.Height);
                _bullets.Add(bullet);
                _bulletCreated?.Invoke(this, new BulletEventArgs<QueenEnemyBullet>(bullet));
            }

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
