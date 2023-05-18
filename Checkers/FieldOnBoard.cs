using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class FieldOnBoard
    {
        public bool IsEmpty { get; set; }
        public bool IsBlack { get; set; }
        public string OccupiedBy { get; set; }
        public string Content { get; set; }
        public int Number { get; set; }  
    }
}
