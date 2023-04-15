using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Checkers
{
    internal class Player
    {
        public string Name { get; set; }
        public static int NumberOfPawns { get; private set; }

        //public Pawn Pawn { get; private set; }

        public List<Pawn> pawns = new List<Pawn>();

        public Player(string name)
        {
            Name = name;
            
            for (int i = 1; i <= 12; i++)
            {
                pawns.Add(new Pawn() {Number = i.ToString(),});
                NumberOfPawns++;

            }
        }
    }
}
