﻿using System;
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

        private object _lock = new object();

        private List<SimpleEnemy> _SimpleEnemies = new List<SimpleEnemy>();
        private List<SimpleEnemy> _BufferSEnmies = new List<SimpleEnemy>();
        private List<SimpleEnemy> _BulletSEnmiesDead = new List<SimpleEnemy>();
        private List<Point> _positionSEnemies = new List<Point>();

        private List<MediumEnemy> _MediumEnemies = new List<MediumEnemy>();
        private List<MediumEnemy> _BufferMEnmies = new List<MediumEnemy>();
        private List<MediumEnemy> _BulletMEnmiesDead = new List<MediumEnemy>();
        private List<Point> _positionMEnmies = new List<Point>();

        private List<QueenEnemy> _QueenEnemies = new List<QueenEnemy>();
        private List<QueenEnemy> _BufferQEnmies = new List<QueenEnemy>();
        private List<QueenEnemy> _BulletQEnmiesDead = new List<QueenEnemy>();
        private List<Point> _positionQEnmies = new List<Point>();

        public static Label LblScore;
        public static Label LblShild;
        private static Label _LblGameOver;
        private static Label _LblWin;
        private static PictureBox _pbTrophy;

        public Fase_1()
        {
            //Verifica a integridade dos arquivos;
            AssetPath = new CheckFiles().CheckAssets();

            InitializeComponent();
            LblScore = this.score;
            LblShild = this.shield;
            _LblGameOver = this.lblGameOver;
            _LblWin = this.lblwin;
            _pbTrophy = this.pbTrohpy;
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

                    gameOver = !_player.alive;
                    Thread.Sleep(16);
                }
            });

            Task.Run(() => 
            {
                while (!gameOver)
                {
                    gameOver = !_player.alive;
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
                //Threads para atualiza a posição do inimigo e as balas
                Task.Run( () =>
                {
                    foreach (SimpleEnemy e in _SimpleEnemies)
                    {
                        _positionSEnemies = e.EnemyMovement(_positionSEnemies);
                        e.Update();
                    }
                    _BufferSEnmies.AddRange(_SimpleEnemies.Where(e => e.noBullets && e.isDead));
                    _SimpleEnemies.RemoveAll(enemy => enemy.isDead);
                });
                Task.Delay(25).Wait();
                Task.Run(() =>
                {
                    foreach (MediumEnemy e in _MediumEnemies)
                    {
                       _positionMEnmies = e.EnemyMovement(_positionMEnmies);
                        e.Update();
                    }
                    _BufferMEnmies.AddRange(_MediumEnemies.Where(e => e.noBullets && e.isDead));
                    _MediumEnemies.RemoveAll(enemy => enemy.isDead);
                });
                Task.Run(() =>
                {
                    foreach (QueenEnemy e in _QueenEnemies)
                    {
                       _positionQEnmies = e.EnemyMovement(_positionQEnmies);
                        e.Update();
                    }
                    _BufferQEnmies.AddRange(_QueenEnemies.Where(e => e.noBullets && e.isDead));
                    _QueenEnemies.RemoveAll(enemy => enemy.isDead);
                });

                Task.Delay(100).Wait();

                //Threads que movimenta balas de inimigos destruidos.
                Task.Run(() =>
                {
                    lock (_lock)
                    {
                        _BulletSEnmiesDead.AddRange(_BufferSEnmies);
                        _BufferSEnmies.Clear();
                    }
                    foreach (SimpleEnemy e in _BulletSEnmiesDead)
                    {
                        e.Update();
                    }
                    _BulletSEnmiesDead.RemoveAll(enemy => !enemy.noBullets);
                });
                Task.Run(() =>
                {
                    lock (_lock)
                    {
                        _BulletMEnmiesDead.AddRange(_BufferMEnmies);
                        _BufferMEnmies.Clear();
                    }
                    foreach (MediumEnemy e in _BulletMEnmiesDead)
                    {
                        e.Update();
                    }
                    _BulletMEnmiesDead.RemoveAll(enemy => !enemy.noBullets);
                });
                Task.Run(() =>
                {
                    lock (_lock)
                    {
                        _BulletQEnmiesDead.AddRange(_BufferQEnmies);
                        _BufferQEnmies.Clear();
                    }
                    foreach (QueenEnemy e in _BulletQEnmiesDead)
                    {
                        e.Update();
                    }
                    _BulletQEnmiesDead.RemoveAll(enemy => !enemy.noBullets);
                });
                //Threads para verificar a colisão do player com as balas inimigas;
                Task.Run(() =>
                {
                    _player.CollisionBulletDetection<SimpleEnemy, SimpleEnemyBullet>(
                                    _SimpleEnemies,
                                    _BufferSEnmies,
                                    enemy => enemy.GetBulletList(),
                                    (enemy, bullet) => enemy.RemoveBullet(bullet)
                    );

                });
                Task.Run(() =>
                {
                    _player.CollisionBulletDetection<MediumEnemy, MediumEnemyBullet>(
                                    _MediumEnemies,
                                    _BufferMEnmies,
                                    enemy => enemy.GetBulletList(),
                                    (enemy, bullet) => enemy.RemoveBullet(bullet)
                    );

                });
                Task.Run(() =>
                {
                    _player.CollisionBulletDetection<QueenEnemy, QueenEnemyBullet>(
                                    _QueenEnemies,
                                    _BufferQEnmies,
                                    enemy => enemy.GetBulletList(),
                                    (enemy, bullet) => enemy.RemoveBullet(bullet)
                    );

                });

            }
        }

        public static void GameOver()
        {
            if (_LblGameOver.InvokeRequired)
            {
                //Chama a thread de UI;
                _LblGameOver.Invoke(new MethodInvoker(Win));
            }
            else
            {
                _LblGameOver.Visible = true;
                _LblGameOver.BringToFront();
            }
        }
        public static void Win()
        {
            if (_LblWin.InvokeRequired)
            {
                //Chama a thread de UI;
                _LblWin.Invoke(new MethodInvoker(Win));
            }
            else
            {
                _LblWin.Visible = true;
                _LblWin.BringToFront();
                _pbTrophy.Visible = true;
                _pbTrophy.BringToFront();
            }

        }
    }
}