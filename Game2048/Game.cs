using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game2048
{
    public partial class GameBox : Form
    {
        #region Variables
        public int score = 0;
        public int highscore = 0;
        bool gameover;
        public Label lb = new Label() //Game Over
        {
            Location = new Point(150, 180),
            Text = "Game Over" + "\n" + "Press Enter to Restart",
            Font = new Font("Arial", 28),
            ForeColor = Color.LightBlue,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true,
            Visible = true,
        };
        List<Block> blockset = new List<Block>();
        PictureBox[] pictureset = new PictureBox[2];
        #endregion

        public GameBox()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            score = 0;
            gameover = false;
            blockset.Clear();
            lb.Visible = false;

            //Creating initial blocks
            CreateBlock(2);
            UpdatePictureBox(blockset);
        }

        private void GameBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    BlockMove(Direction.Left);
                    break;
                case Keys.Right:
                    BlockMove(Direction.Right);
                    break;
                case Keys.Up:
                    BlockMove(Direction.Up);
                    break;
                case Keys.Down:
                    BlockMove(Direction.Down);
                    break;
                case Keys.Enter:
                    if (gameover)
                    {
                        StartGame();
                    }
                    break;
            }
            UpdatePictureBox(blockset);
        }

        private void CreateBlock(int k)
        {
            for (int i = 0; i < k; i++)
            {
                bool valid = false;
                do
                {
                    Block bb = Block.GenerateBlock();
                    if (blockset.Contains(bb))
                        valid = false;
                    else
                    {
                        valid = true;
                        blockset.Add(bb);
                    }
                } while (!valid);
            }
        }

        #region BlockMove function
        private void BlockMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    Block.SortDirection = Direction.Left;
                    break;
                case Direction.Right:
                    Block.SortDirection = Direction.Right;
                    break;
                case Direction.Up:
                    Block.SortDirection = Direction.Up;
                    break;
                case Direction.Down:
                    Block.SortDirection = Direction.Down;
                    break;
            }
            bool moved = false;
            List<Block> temp = new List<Block>();
            for (int i = 0; i < 4; i++)
            {
                List<Block> blockrow = (int)direction < 2? Block.TakeRow(blockset, i): Block.TakeCol(blockset, i);
                blockrow.Sort();

                //Merge blocks
                for (int j = 0; j < blockrow.Count; j++)
                {
                    if (blockrow.Count - j < 2)
                        break;
                    if (blockrow[j].Value == blockrow[j + 1].Value)
                    {
                        moved = true;
                        blockrow[j].Value *= 2;
                        score += blockrow[j].Value;
                        blockrow.RemoveAt(j + 1);
                    }
                }
                //Move blocks
                int bound = 0;
                if((int)direction == 0)
                {
                    for (int j = 0; j < blockrow.Count; j++)
                    {
                        if (blockrow[j].PosX != bound)
                        {
                            moved = true;
                            blockrow[j].PosX = bound;
                        }
                        bound += 1;
                        temp.Add(blockrow[j]);
                    }
                }
                else if ((int)direction == 1)
                {
                    for (int j = 0; j < blockrow.Count; j++)
                    {
                        if (blockrow[j].PosX != 3 - bound)
                        {
                            moved = true;
                            blockrow[j].PosX = 3 - bound;
                        }
                        bound += 1;
                        temp.Add(blockrow[j]);
                    }
                }
                else if ((int)direction == 2)
                {
                    for (int j = 0; j < blockrow.Count; j++)
                    {
                        if (blockrow[j].PosY != bound)
                        {
                            moved = true;
                            blockrow[j].PosY = bound;
                        }
                        bound += 1;
                        temp.Add(blockrow[j]);
                    }
                }
                else if((int)direction == 3)
                {
                    for (int j = 0; j < blockrow.Count; j++)
                    {
                        if (blockrow[j].PosY != 3 - bound)
                        {
                            moved = true;
                            blockrow[j].PosY = 3 - bound;
                        }
                        bound += 1;
                        temp.Add(blockrow[j]);
                    }
                }
            }
            blockset = temp;
            if (blockset.Count == 16 && NoMoreMove() == true)
            {
                gameover = true;
            }else if(blockset.Count < 16 && moved == true)
            {
                CreateBlock(1);
            }
        }
        #endregion

        #region To Update PictureBoxes
        private void UpdatePictureBox(List<Block> blocklist)
        {
            if(gameover)
            {
                GameOver();
            }
            //Removing all existing pictureBox
            foreach (PictureBox oldpicture in this.Controls.OfType<PictureBox>().ToArray())
            {
                oldpicture.Dispose();
                this.Controls.Remove(oldpicture);
            }

            //Creating new pictureBoxes according to new position
            PictureBox[] pictureset = new PictureBox[blockset.Count];
            foreach (Block bb in blocklist){
                PictureBox pic = new PictureBox();
                pic.Location = new Point(bb.PosX * 110 + 10, bb.PosY * 110 + 10);
                pic.Size = new Size(bb.Size, bb.Size);
                pic.BackColor = Color.LightGreen;
                pic.Image = Image.FromFile(PicturePath(bb.Value));
                this.Controls.Add(pic);
            }
            label1.Text = score.ToString();
            label4.Text = highscore.ToString();
        }

        private string PicturePath(int value)
        {
            string path = "img\\";
            path += value.ToString() + ".png";
            return path;
        }
        #endregion

        private bool NoMoreMove()
        {
            bool result = true;
            for(int i = 0; i < 4; i++)
            {
                List<Block> blockrow = Block.TakeRow(blockset, i);
                List<Block> blockcol = Block.TakeCol(blockset, i);
                blockrow.Sort();
                blockcol.Sort();
                for(int j = 0; j < blockrow.Count - 1; j++)
                {
                    if(blockrow[j].Value == blockrow[j + 1].Value)
                    {
                        result =  false;
                        break;
                    }
                }
                for (int k = 0; k < blockcol.Count - 1; k++)
                {
                    if (blockcol[k].Value == blockcol[k + 1].Value)
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        public void GameOver()
        {
            this.Controls.Add(lb);
            lb.Visible = true;

            if (score > highscore)
            {
                highscore = score;
            }
        }
    }
}
