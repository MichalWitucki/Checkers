using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Pawn
    {
        public string Name { get; set; }
        public int CurrentPosition { get; set; }
        public bool IsKing { get; set; }
        public List<Pawn> PlayerPawnsToJumpOverByCpuPawn { get; set; }
        
        

        public Pawn()
        {
           
        }
    }
}
