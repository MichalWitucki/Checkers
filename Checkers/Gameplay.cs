using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    internal static class Gameplay
    {
        private static string[,] board = new string[8, 8] {
            {" "," "," "," "," "," "," "," " },
            {" "," "," "," "," "," "," "," " },
            {" "," "," "," "," "," "," "," " },
            {" "," "," "," "," "," "," "," " },
            {" "," "," "," "," "," "," "," " },
            {" "," "," "," "," "," "," "," " },
            {" "," "," "," "," "," "," "," " },
            {" "," "," "," "," "," "," "," " }
        };
        
        public static Player DrawWhoStarts(Player player, Player cpu)
        {
            Random random = new Random();
            int whoStarts = random.Next(0, 2);
            switch (whoStarts)
            {
                case 0:
                    return player;
                default:
                    return cpu;
            }      
        }

        public static void DrawBoard()
        {
            foreach (char c in "ABCDEFG")
            {
                Console.Write($"{c} ");
            }
            Console.WriteLine("H");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Console.Write(i+1);
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(board[i,j]);
                }
                Console.WriteLine(i+1);
            }
        }

        public static void Turn(Player firstPlayer)
        {
            Console.WriteLine($"Zaczyna {firstPlayer.Name}");
        }
    }
}
