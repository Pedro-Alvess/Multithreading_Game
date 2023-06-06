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

        public Fase_1()
        {
            //Verifica a integridade dos arquivos;
            AssetPath = new CheckFiles().CheckAssets();

            InitializeComponent();
            //this.TransparencyKey = Color.MidnightBlue;
            _player = new PlayerMechanics();

            // Criar o player só após o forms ter iniciado;
            Load += (sender, e) => _player.CreatPlayer(this, e);

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
            while (!gameOver)
            {
                //IsShoot();
                MovePlayer();
                _player.Update();
                gameOver = _player.alive;
                Thread.Sleep(1);
            }
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
            if (IsKeyDown(Keys.Space) && (stopwatch.ElapsedMilliseconds >= 200 || !stopwatch.IsRunning))
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

        private bool IsKeyDown(Keys key)
        {
            return (GetKeyState((int)key) & 0x8000) != 0;
        }

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int key);
    }
}