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

namespace alien_invasion
{
    public partial class Fase_1 : Form
    {
        private bool gameOver = false;
        private PlayerMechanics _player;

        public Fase_1()
        {
            //Verifica a integridade dos arquivos;
            //new CheckFiles().CheckAssets();

            InitializeComponent();
            this.TransparencyKey = Color.MidnightBlue;
            _player = new PlayerMechanics(Player);
            _player.BulletCreated += Player_BulletCreated;

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
            while(!gameOver)
            {
                MovePlayer();
                _player.Update();
                Thread.Sleep(16);
            }
        }

        private void MovePlayer()
        {
            if(IsDisposed) return;

            if(InvokeRequired)
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
            else if(IsKeyDown(Keys.Right))
            {
                _player.MoveRight();
            }

            if (IsKeyDown(Keys.Space))
            {
                _player.Shoot();
            }
        }
        private void Player_BulletCreated(object sender, BulletEventArgs e)
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
