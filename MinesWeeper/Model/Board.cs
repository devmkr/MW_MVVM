using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace MinesWeeper.Model
{

    class MinnerBoard
    {
        public Dictionary<FieldAdd, Field> Board { get; private set; }

        public MinnerBoard(int _maxX, int _maxY, int _minesNbr)
        {
            Board = new Dictionary<FieldAdd, Field>();

            if (_maxX == 0 || _maxY == 0 || _minesNbr > _maxX * _maxY)
                throw new ArgumentException();

            //Initize the game board
            for(int x=0; x < _maxX; x++)
                for (int y=0; y < _maxY; y++)                
                    Board.Add(new FieldAdd { X = x, Y = y }, new Field());
                
            

        }
    }



        



    class Field
    {       
        public FieldOption Option { get; private set; }
        public int NumberOfNeighbours { get; private set; } = 0;                            
    }

    internal struct FieldAdd
    {
        public int X { get; set; }
        public int Y { get; set; }

    }


    enum FieldOption
    {
        Safe,
        Minned,
        Quessed,
    }
}
