﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Board
    {
        public List<FieldOnBoard> fields = new List<FieldOnBoard>();

        public Dictionary<string, int> fieldsOnBoardDictionary = new Dictionary<string, int>();
       

        public Board()
        {
            fields.Add(new FieldOnBoard() { IsEmpty = true, IsBlack = false, Number = 0, Content = "█" });

            for (int i = 1; i < 64; i++)
            {
                if (i % 8 == 0)
                {
                    fields.Add(new FieldOnBoard() { IsEmpty = true, IsBlack = fields[i - 1].IsBlack, Number = i});
                    if (fields[i].IsBlack)
                        fields[i].Content = " ";
                    else
                        fields[i].Content = "█";
                }
                else if (fields[i - 1].IsBlack == false)
                    fields.Add(new FieldOnBoard() { IsEmpty = true, IsBlack = true, Number = i, Content = " " });
                else
                    fields.Add(new FieldOnBoard() { IsEmpty = true, IsBlack = false, Number = i, Content = "█" });
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
