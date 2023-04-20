using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Board
    {
        public List<FieldOnBoard> fieldsOnBoard = new List<FieldOnBoard>();

        public Dictionary<string, int> fieldsOnBoardDictionary = new Dictionary<string, int>();
       

        public Board()
        {
            fieldsOnBoard.Add(new FieldOnBoard() { IsEmpty = true, IsBlack = false, Number = 0, Content = " " });

            for (int i = 1; i < 64; i++)
            {
                if (i % 8 == 0)
                    fieldsOnBoard.Add(new FieldOnBoard() { IsEmpty = true, IsBlack = fieldsOnBoard[i - 1].IsBlack, Number = i, Content = " " });
                else if (fieldsOnBoard[i - 1].IsBlack == false)
                    fieldsOnBoard.Add(new FieldOnBoard() { IsEmpty = true, IsBlack = true, Number = i, Content = " " });
                else
                    fieldsOnBoard.Add(new FieldOnBoard() { IsEmpty = true, IsBlack = false, Number = i, Content = " " });
                //Console.WriteLine($"{i} {fieldsOnBoard[i].IsEmpty} {fieldsOnBoard[i].IsBlack} ");
            }
            SetFieldsOnBoardDictionary();

        }

        private void SetFieldsOnBoardDictionary()
        {
            int fieldNumber = 0;
            foreach (char r in "12345678")
            {
                foreach(char c in "ABCDEFGH")
                {
                    fieldsOnBoardDictionary[c.ToString()+r] = fieldNumber;
                    fieldNumber++;
                }
            }
        }
    }
}
