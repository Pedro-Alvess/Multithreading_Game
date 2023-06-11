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
        public bool alive = true;

        public static int Score = 0;
        private int _lastScore = 0;

        public static int Shild = 100;
        private int _lastShild = 100;

        private object _locker = new { };
        public event EventHandler<BulletEventArgs<PlayerBullet>> bulletCreated;
        public static List<PlayerBullet> bullets = new List<PlayerBullet>();
        private List<PlayerBullet> _bufferBullets = new List<PlayerBullet>();

        private int _speed = 7;
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
            _bufferBullets.Add(bullet);
            bulletCreated?.Invoke(this, new BulletEventArgs<PlayerBullet>(bullet));
        }
        public void ShootLeft()
        {
            PlayerBullet bullet = new PlayerBullet(_player.Left + (_player.Width / 2) + 31, _player.Top);
            _bufferBullets.Add(bullet);
            bulletCreated?.Invoke(this, new BulletEventArgs<PlayerBullet>(bullet));
        }

        public void CollisionBulletDetection<TEnemy, TBullet>(List<TEnemy> bullets, List<TEnemy> deadBullets, Func<TEnemy, List<TBullet>> getBulletList, Action<TEnemy, TBullet> removeBullet)
        {
            List<TEnemy> allBullets = new List<TEnemy>();
            allBullets.AddRange(bullets);
            allBullets.AddRange(deadBullets);

            for(int i = allBullets.Count - 1; i >= 0; i--)
            {
                List<TBullet> bulletList = getBulletList(allBullets[i]);

                for (int c = bulletList.Count - 1; c>= 0; c--)
                {
                    TBullet bullet = bulletList[c]; ;

                    if (((dynamic)bullet).GetPictureBox().Bounds.IntersectsWith(_player.Bounds))
                    {
                        new MiniExplosion(((dynamic)bullet).GetPictureBox().Location);

                        removeBullet(allBullets[i], bullet);
                    }
                }
                
            }
        }

        public void Update()
        {
            if(_lastScore != Score)
            {
                _lastScore= Score;
                Fase_1.ActiveForm.Invoke((MethodInvoker)(() => { Fase_1.LblScore.Text = Score.ToString(); }));

                if(Score > 1405)
                {
                    Fase_1.Win();
                }
            }
            if(_lastShild != Shild)
            {
                _lastShild = Shild;

                if (Shild <= 0)
                {
                    _DestroyPlayer();
                    Shild = 0;
                }

                Fase_1.ActiveForm.Invoke((MethodInvoker)(() => { Fase_1.LblShild.Text = ((double)Shild/100).ToString("00%"); }));

                if(Shild <= 0)
                {
                    _DestroyPlayer();
                }
            }          
           
            lock (_locker) // Inicio do bloco de sincronização
            {
                bullets.AddRange(_bufferBullets);
                _bufferBullets.Clear();
            }
            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Move();
            }
            bullets.RemoveAll(b =>
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
        }

        private void _DestroyPlayer()
        {
            new Explosion(_player.Location,500);
            alive = false;

            Fase_1.ActiveForm.Invoke((MethodInvoker)(() =>
            {
                Fase_1.ActiveForm.Controls.Remove(_player);
                _player.Dispose();
                Fase_1.GameOver();
            }));

        }
    }
}