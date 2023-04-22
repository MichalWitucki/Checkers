using System;
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
        public static int NumberOfPawns { get; private set; }
       // public bool IsCpu { get; set; }

        public List<Pawn> pawns = new List<Pawn>();

        public Player(string name, bool isCpu)
        {
            Name = name;
            
            for (int i = 1; i <= 12; i++)
            {
                if (isCpu)
                    pawns.Add(new Pawn() { Name = ((char)(i+96)).ToString()});
                else 
                    pawns.Add(new Pawn() { Name = ((char)(i + 64)).ToString() });

                //Color color = Color.FromKnownColor(KnownColor.Red);
                NumberOfPawns++;
            }
        }
    }
}
