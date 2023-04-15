using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        
        public static int DrawWhoStarts()
        {
            Random random = new Random();
            return random.Next(0, 2);
        }

        public static void DrawBoard()
        {

            Console.Write($"   ");
            foreach (char c in "ABCDEFG")
            {
                Console.Write($"{c} ");
            }
            Console.WriteLine("H");
            Console.WriteLine("   ----------------");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Console.Write($"{i+1} |");
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write($"{board[i,j]} ".Substring(0,2));
                }
                Console.WriteLine();
            }
        }


        public static void SpacingPawns(Player firstPlayer, Player secondPlayer)
        {
           int firstPlayerPawn = 0;
           int secondPlayerPawn = 0;
           
                   
           for (int y = 0; y < 3; y++)
            {
                for(int x = 0; x < 8; x++)
                {
                    if ((y % 2 == 0 && x % 2 !=0) || (y % 2 != 0 && x % 2 == 0))
                    {
                        SetPawnLocation(firstPlayer, firstPlayerPawn, x, y);
                        firstPlayerPawn++;

                    }
                }
            }
            for (int y = 5; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if ((y % 2 == 0 && x % 2 != 0) || (y % 2 != 0 && x % 2 == 0))
                    {
                        SetPawnLocation(secondPlayer, secondPlayerPawn, x, y);
                        secondPlayerPawn++;

                    }
                
                }

            }
        }
        private static void SetPawnLocation(Player player, int pawn, int x, int y)
        {
            player.pawns[pawn].yLocation = y;
            player.pawns[pawn].xLocation = x;
            board[y, x] = player.pawns[pawn].Number;
            
        }

        public static void Turn(Player firstPlayer)
        {
            //ustawic kolor
            Console.WriteLine($"Zaczyna {firstPlayer.Name}.");
        }
    }
}
