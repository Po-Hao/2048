using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048
{
    public enum Direction {Left, Right, Up, Down}

    public class Block : IComparable<Block>, IEquatable<Block>
    {
        #region 欄位
        private static Random random;
        private int value;
        private int size = 100;
        private int posx;
        private int posy;
        public static int blockCount;
        private static Direction sortDirection;
        #endregion

        #region 屬性
        public int Value { get => value; set => this.value = value; }
        public int Size { get => size; set => size = value; }
        public int PosX { get => posx; set => posx = value; }
        public int PosY { get => posy; set => posy = value; }
        public static Direction SortDirection { get => sortDirection; set => sortDirection = value; }

        #endregion

        #region 建構式
        public Block(int posx, int posy, int value)
        {
            PosX = posx;
            PosY = posy;
            Value = value;
        }

        #endregion

        #region 靜態方法
        public static Block GenerateBlock()
        {
            float rateOfFour = 0.2f;
            if (Block.random == null)
                Block.random = new Random();
            int x = random.Next(0, 4);
            int y = random.Next(0, 4);
            int value = random.NextDouble() > rateOfFour ? 2 : 4;
            return new Block(x, y, value);
        }

        public static string PrintAllBlock(List<Block> blocks)
        {
            string blockdigit = "";
            foreach (Block b in blocks)
                blockdigit += "(" + b.PosX.ToString() + "," + b.PosY.ToString() + "," + b.Value.ToString() + ")";
            return blockdigit;
        }

        public static List<Block> TakeRow(List<Block> blocks, int r)
        {
            List<Block> row = new List<Block>();
            foreach (Block block in blocks)
            {
                if (block.PosY == r)
                    row.Add(block);
            }
            return row;
        }

        public static List<Block> TakeCol(List<Block> blocks, int c)
        {
            List<Block> col = new List<Block>();
            foreach (Block block in blocks)
            {
                if (block.PosX == c)
                    col.Add(block);
            }
            return col;
        }
        #endregion

        #region 實作介面
        public int CompareTo(Block other) //IComparable
        {
            switch (SortDirection)
            {
                case Direction.Left:
                    if (this.PosX > other.PosX)
                        return 1;
                    else if (this.PosX < other.PosX)
                        return -1;
                    else
                        return 0;
                case Direction.Right:
                    if (this.PosX > other.PosX)
                        return -1;
                    else if (this.PosX < other.PosX)
                        return 1;
                    else
                        return 0;
                case Direction.Up:
                    if (this.PosY > other.PosY)
                        return 1;
                    else if (this.PosY < other.PosY)
                        return -1;
                    else
                        return 0;
                case Direction.Down:
                    if (this.PosY > other.PosY)
                        return -1;
                    else if (this.PosY < other.PosY)
                        return 1;
                    else
                        return 0;
                default:
                    if (this.PosX > other.PosX)
                        return 1;
                    else if (this.PosX < other.PosX)
                        return -1;
                    else
                        return 0;
            }
        }

        public bool Equals(Block other) //IEquatable
        {
            if (this.PosX == other.PosX && this.PosY == other.PosY)
                return true;
            else
                return false;
        }
        #endregion


    }
}
