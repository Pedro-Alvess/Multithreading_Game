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
    public partial class Form1 : Form
    {
        private int speed = 5;
        private bool gameOver = false;

        public Form1()
        {
            InitializeComponent();
            this.TransparencyKey = Color.MidnightBlue;

            Task.Run(() => PlayerMovement());
        }

        private void PlayerMovement()
        {
            while(!gameOver)
            {
                MovePlayer();
                Thread.Sleep(16);
            }
        }

        private void MovePlayer()
        {
            if(InvokeRequired)
            {
                Invoke((MethodInvoker)MovePlayer);
                return;
            }

            if (IsKeyDown(Keys.Left))
            {
                Player.Left -= speed;
            }
            else if (IsKeyDown(Keys.A))
            {
                Player.Left -= speed;
            }
            else if (IsKeyDown(Keys.D))
            {
                Player.Left += speed;
            }
            else if(IsKeyDown(Keys.Right))
            {
                Player.Left += speed;
            }
        }

        private bool IsKeyDown(Keys key)
        {
            return (GetKeyState((int)key) & 0x8000) != 0;
        }

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int key);
        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (keyData == Keys.Left)
        //    {
        //        Player.Left -= velocidade;
        //        return true;
        //    }
        //    else if (keyData == Keys.Right)
        //    {
        //        Player.Left += velocidade;
        //        return true;
        //    }

        //    return base.ProcessCmdKey(ref msg, keyData);
        //}
    }
}
