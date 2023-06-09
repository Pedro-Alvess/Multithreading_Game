using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace alien_invasion
{
    public partial class Fase_1 : Form
    {
        public bool gameOver = false;
        private PlayerMechanics _player;
        private Stopwatch stopwatch = new Stopwatch();
        public static string AssetPath;

        private bool _shootSide = false;
        private int _timeBetweenShot = 450;

        private List<SimpleEnemy> _SimpleEnemies = new List<SimpleEnemy>();
        private List<Point> _positionSEnemies = new List<Point>();

        private List<MediumEnemy> _MediumEnemies = new List<MediumEnemy>();
        private List<Point> _positionMEnmies = new List<Point>();

        private List<QueenEnemy> _QueenEnemies = new List<QueenEnemy>();
        private List<Point> _positionQEnmies = new List<Point>();

        public Fase_1()
        {
            //Verifica a integridade dos arquivos;
            AssetPath = new CheckFiles().CheckAssets();

            InitializeComponent();
            //this.TransparencyKey = Color.MidnightBlue;
            _player = new PlayerMechanics();

            // Criar o player só após o forms ter iniciado;
            Load += (sender, e) => _player.CreatPlayer(this, e);

            //Cria thread dos inimigos
            Task threadSimpleEnemy = Task.Run(() => CreatSimpleEnemy());
            Task threadMediunEnemy = Task.Run(() => CreatMediumEnemy());
            Task threadQueenEnemy = Task.Run(() => CreatQueenEnemy());

            //Só executa o update dos inimigos quando todos os assets já tiverem sido instanciados.
            Task.WhenAll(threadSimpleEnemy, threadMediunEnemy, threadQueenEnemy).ContinueWith(t => UpdateEnemies());

            //Adiciona um manipulador de eventos
            _player.bulletCreated += Player_BulletCreated;
            Task.Run(() => PlayerUpdate());

            //Garante que o a vareavel gameOver vai ser sincada na thread do Player quando fechar a janela.
            FormClosing += async (sender, e) =>
            {
                await Task.Factory.StartNew(() =>
                {
                    gameOver = true;
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
            };
        }

        private void PlayerUpdate()
        {
            Task.Run(() => 
            {
                while (!gameOver)
                {
                    _player.Update();

                    gameOver = _player.alive;
                    Thread.Sleep(16);
                }
            });

            Task.Run(() => 
            {
                while (!gameOver)
                {
                    gameOver = _player.alive;
                    MovePlayer();
                    Thread.Sleep(16);
                }
            });

        }

        private void MovePlayer()
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)MovePlayer);
                return;
            }

            if (IsKeyDown(Keys.Left))
            {
                _player.MoveLeft();
            }
            else if (IsKeyDown(Keys.A))
            {
                _player.MoveLeft();
            }
            else if (IsKeyDown(Keys.D))
            {
                _player.MoveRight();
            }
            else if (IsKeyDown(Keys.Right))
            {
                _player.MoveRight();
            }
            if (IsKeyDown(Keys.Space) && (stopwatch.ElapsedMilliseconds >= _timeBetweenShot || !stopwatch.IsRunning))
            {
                stopwatch.Restart();
                stopwatch.Start();
                if (_shootSide)
                {
                    _player.ShootRight();
                    _shootSide = false;
                }
                else
                {
                    _player.ShootLeft();
                    _shootSide = true;
                }
                
            }
        }
        private void Player_BulletCreated(object sender, BulletEventArgs<PlayerBullet> e)
        {
            PictureBox bulletEvent = e.Bullet.GetPictureBox();
            Controls.Add(bulletEvent);
        }


        // Verifica quando uma tecla é pressionada
        [DllImport("user32.dll")]
        private static extern short GetKeyState(int key);
        private bool IsKeyDown(Keys key)
        {
            return (GetKeyState((int)key) & 0x8000) != 0;
        }

        //Criação dos inimigos
        private void CreatSimpleEnemy()
        {
            //Primeira fila de inimigos
            for (int i = 50; i <= 610; i += 70)
            {
                Point position = new Point(i, 260);
                SimpleEnemy _enemy = new SimpleEnemy(this, EventArgs.Empty, position);
                _SimpleEnemies.Add(_enemy);
                _positionSEnemies.Add(position);
            }
            //Segunda fila de inimigos
            for (int i = 95; i <= 725; i += 70)
            {
                Point position = new Point(i, 190);
                SimpleEnemy _enemy = new SimpleEnemy(this, EventArgs.Empty, position);
                _SimpleEnemies.Add(_enemy);
                _positionSEnemies.Add(position);
            }

        }
        private void CreatMediumEnemy()
        {
            for (int i = 40; i <= 690; i += 100)
            {
                Point position = new Point(i, 120);
                MediumEnemy _enemy = new MediumEnemy(this, EventArgs.Empty, position);
                _MediumEnemies.Add(_enemy);
                _positionMEnmies.Add(position);
            }
        }
        private void CreatQueenEnemy()
        {
            for (int i = 150; i <= 600; i += 150)
            {
                Point position = new Point(i, 40);
                QueenEnemy _enemy = new QueenEnemy(this, EventArgs.Empty, position);
                _QueenEnemies.Add(_enemy);
                _positionQEnmies.Add(position);
            }
        }
        private void UpdateEnemies()
        {
            while (!gameOver)
            {
                Task.Delay(100).Wait();
                Task.Run( () =>
                {
                    foreach (SimpleEnemy e in _SimpleEnemies)
                    {
                        _positionSEnemies = e.EnemyMovement(_positionSEnemies);
                        e.Update();
                    }
                });
                Task.Delay(25).Wait();
                Task.Run(() =>
                {
                    foreach (MediumEnemy e in _MediumEnemies)
                    {
                       _positionMEnmies = e.EnemyMovement(_positionMEnmies);
                        e.Update();
                    }
                });
                Task.Run(() =>
                {
                    foreach (QueenEnemy e in _QueenEnemies)
                    {
                       _positionQEnmies = e.EnemyMovement(_positionQEnmies);
                        e.Update();
                    }
                });
            }
        }
    }
}