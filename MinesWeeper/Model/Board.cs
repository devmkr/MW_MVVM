using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Math;

namespace MinesWeeper.Model
{

    public class MinnerBoard
    {
        public List<Field> Board { get; set; }
  

        public static MinnerBoard Create(int _maxX, int _maxY, int _minesNbr)
        {
            if (_maxX == 0 || _maxY == 0 || _minesNbr > _maxX * _maxY)
                throw new ArgumentException();

            var rr = new MinnerBoard();
            //Init the game board
            rr.Board = new List<Field>();
            //Generate the board
            for (int x = 0; x < _maxX; x++)
                for (int y = 0; y < _maxY; y++)
                    rr.Board.Add(new Field(x, y));

           
            rr.MineBoard(_minesNbr);
            rr.SetNumberOfAdjacentMinnedField();   

            return rr;
        }

        async public static Task<MinnerBoard> CreateAsync(int _maxX, int _maxY, int _minesNbr)
        {
            var t = await Task.FromResult<MinnerBoard>(Create(_maxX, _maxY, _minesNbr));
            Thread.Sleep(4000);
           
            return t;
        }

       


        private MinnerBoard()
        {
            //Factory Pattern
        }

        private void MineBoard(int minesNbr)
        {
            Random rnd = new Random();
            //randomly to mine on the board   
            foreach (var i in Board.OrderBy(x => rnd.Next()).Take(minesNbr))
                i.SetMinned();
        }

        private void SetNumberOfAdjacentMinnedField()
        {
            foreach (var i in Board)
                i.AdjecentMinnedFields = GetAdjacentMinnedsOf(i);
        }

        private int GetAdjacentMinnedsOf(Field fd)
        {
            return Board.Where(i => i.IsMinned && fd.IsAdjacent(i)).Count();
        }      
   
    }

    public class Field 
    {
        public bool IsMinned { get; private set; } = false;
        public int AdjecentMinnedFields { get; set; } = 0;
        public FieldAdd Position { get; private set; }

        public Field(int _x, int _y)
        {
            Position = new FieldAdd { X = _x, Y = _y };
        }

        public Field(Field fd)
        {
            IsMinned = fd.IsMinned;
            AdjecentMinnedFields = fd.AdjecentMinnedFields;
            Position = fd.Position;
        }

        public void SetMinned() => IsMinned = true;
        

        internal bool IsAdjacent(Field f)
        {
            return Abs(Position.X - f.Position.X) <= 1 && Abs(Position.Y - f.Position.Y) <= 1;
        }

        #pragma warning disable 
        public struct FieldAdd
        {
            public int X { get; set; }
            public int Y { get; set; }

            public static bool operator ==(FieldAdd f1, FieldAdd f2)
            {
                return !(f1 != f2) ? true : false;
            }

            public static bool operator !=(FieldAdd f1, FieldAdd f2)
            {
                return (f1.X != f2.Y || f1.Y != f2.Y) ? true : false;
            }


        }
    }

   
}
