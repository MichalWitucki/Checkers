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

        public Pawn Pawn { get; private set; }

        public Player(string name)
        {
            Name = name;
            NumberOfPawns = 12;
            List<Pawn> pawns = new List<Pawn>();
            for (int i = 1; i <= NumberOfPawns; i++)
            {
                pawns.Add(new Pawn() {Name = i.ToString(),});
            }
        }
    }
}
