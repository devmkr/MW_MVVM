﻿using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace MinesWeeper.Model
{

    public class MinnerBoard
    {
        public List<Field> Board { get; set; }        

        public MinnerBoard(int _maxX, int _maxY, int _minesNbr)
        {

            if (_maxX == 0 || _maxY == 0 || _minesNbr > _maxX * _maxY)
                throw new ArgumentException();

            //Init the game board
            Board = new List<Field>();
            //Generate the board
            for (int x = 0; x < _maxX; x++)
                for (int y = 0; y < _maxY; y++)
                    Board.Add(new Field(x, y));

            Random rnd = new Random();
            //randomly to mine on the board        
            foreach (var i in Board.OrderBy(x => rnd.Next()).Take(_minesNbr))
                i.SetMinned();
            //Set number adjacent minned fields
            foreach (var i in Board)
                i.AdjecentMinnedFields = GetAdjacentMinnedsOf(i);           
        }

        private int GetAdjacentMinnedsOf(Field fd)
        {
            return Board.Where(i => i.IsMinned == true && fd.IsAdjacent(i) && i.Position != fd.Position).Count();
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
