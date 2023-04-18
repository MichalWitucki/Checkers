using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Gameplay
    {
        
        public Board board = new Board();
               
        public static int DrawWhoStarts() 
        {
            Random random = new Random();
            return random.Next(0, 2);
        }

        public void DrawBoard()
        {
            Console.Write($"   ");
            foreach (char c in "ABCDEFG")
            {
                Console.Write($"{c} ");
            }
            Console.WriteLine("H");
            Console.WriteLine("  -----------------"); 
            
            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{i + 1} |");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write($"{board.fieldsOnBoard[j+i*8].Content} ");
                }
                Console.WriteLine();
            }
        }


        public void SpacingPawns(Player player, Player cpu)
        {
            int playerPawn = 0;
            int cpuPawn = 0;
            
            for (int y = 0; y < 3; y++)
            {
                for(int x = 0; x < 8; x++)
                {
                    if (board.fieldsOnBoard[x + y * 8].IsBlack) //do metody
                    {
                        board.fieldsOnBoard[x + y * 8].Content = cpu.pawns[cpuPawn].Name;
                        board.fieldsOnBoard[x + y * 8].IsEmpty = false;
                        cpu.pawns[cpuPawn].CurrentPosition = x + y * 8;
                        cpuPawn++;
                    }
                    
                        
                }
            }
            
            for (int y = 5; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (board.fieldsOnBoard[x + y * 8].IsBlack)
                    {
                        board.fieldsOnBoard[x + y * 8].Content = player.pawns[playerPawn].Name;
                        board.fieldsOnBoard[x + y * 8].IsEmpty = false;
                        player.pawns[playerPawn].CurrentPosition = x + y * 8;
                        playerPawn++;
                    }
                
                }
            }
        }
        private void SetPawnLocation(Player player, int pawn)
        {
            // do metody 
        }

        public void PlayerTurn(Player player)
        {
            
            Pawn chosenPawn = ChoosePawn(player);
           
            MovePawn(chosenPawn);
        }

    

        public void CpuTurn(Player cpu)
        {
            Console.Write("CPU wybiera pionka, którym chce ruszyć: ");

        }

        private Pawn ChoosePawn(Player player)
        {
            

            do
            {
                Console.Write($"{player.Name} wybiera pionka, którym chce ruszyć: ");
                string playerChoice = Console.ReadLine().ToUpper();
                var chosenPawn = player.pawns.FirstOrDefault(x => x.Name == playerChoice);
                if (chosenPawn != null && IsPawnMovePossible(player, chosenPawn))
                {
                    
                    return chosenPawn;
                }
                else
                    Console.WriteLine("Niepoprawny wybór.");      
            }
            while (true);
        }
        private bool IsPawnMovePossible(Player player, Pawn chosenPawn)
        {
            if ((board.fieldsOnBoard[chosenPawn.CurrentPosition - 7].IsEmpty == true && 
                board.fieldsOnBoard[chosenPawn.CurrentPosition - 7].IsBlack == true) 
                || (board.fieldsOnBoard[chosenPawn.CurrentPosition - 9].IsEmpty == true && 
                board.fieldsOnBoard[chosenPawn.CurrentPosition - 9].IsBlack == true))
                return true;
            else
                return false;
        }

        private void MovePawn(Pawn pawn)
        {
            Console.Write($"Na które pole przestawić pionka {pawn.Name}?: ");
            string playerChoice = Console.ReadLine().ToUpper();
            if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice))
            {

            }
            else
            {

            };
            
        }
    }
}
