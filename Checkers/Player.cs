﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Checkers
{
    public class Player
    {
        public string Name { get; set; }
        public bool  IsCpu { get; set; }
        public List<Pawn> PawnsThatCanMove { get; set; }
        public List<Pawn> PawnsThatCanJumpOver { get; set; }

        public List<Pawn> pawns = new List<Pawn>();

        public Player(string name, bool isCpu)
        {
            Name = name;
            IsCpu = isCpu;

            for (int i = 1; i <= 12; i++)
            {
                if (IsCpu)
                    pawns.Add(new Pawn() { Name = "o" });
                else
                    pawns.Add(new Pawn() { Name = ((char)(i + 96)).ToString() } );
            } 
        }
    }
}
