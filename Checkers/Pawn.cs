using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    internal class Pawn
    {
        private char symbol = 'O';
        public string Number { get; set; }

        public int yLocation { get; set; }
        public int xLocation { get; set; }
        public Color Color { get; set; }
        
        public Pawn()
        {
            
        }
    }
}
