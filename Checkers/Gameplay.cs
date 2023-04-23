using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
               
        public Player DrawWhoStarts(Player player, Player cpu) 
        {
            Random random = new Random();
             
            switch (random.Next(0, 2))
            {
                case 0:
                    return player;
                default:
                   return cpu;
            }
        }

        public void DrawBoard()
        {
            Console.Write($"  |");
            foreach (char c in "ABCDEFG")
            {
                Console.Write($"{c}|");
            }
            Console.WriteLine("H|");
            Console.WriteLine("  -----------------"); 
            
            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{i + 1} |");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write($"{board.fields[j+i*8].Content}|");
                }
                Console.WriteLine(); 
                //Console.WriteLine("  -----------------");
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
                    if (board.fields[x + y * 8].IsBlack) //do metody
                    {
                        board.fields[x + y * 8].Content = cpu.pawns[cpuPawn].Name;
                        board.fields[x + y * 8].IsEmpty = false;
                        board.fields[x + y * 8].IsCpu = true;
                        cpu.pawns[cpuPawn].CurrentPosition = x + y * 8;
                        cpuPawn++;
                    }
                    
                        
                }
            }
            
            for (int y = 5; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (board.fields[x + y * 8].IsBlack)
                    {
                        board.fields[x + y * 8].Content = player.pawns[playerPawn].Name;
                        board.fields[x + y * 8].IsEmpty = false;
                        board.fields[x + y * 8].IsPlayer = true;
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

        public void Turn(Player player, Player opponent)
        {
            Pawn chosenPawn = ChoosePawn(player);
            MovePawnOrJumpOver(chosenPawn,player, opponent);
        }
        
        private Pawn ChoosePawn(Player player)
        {
            if (player.Name == "CPU")
            {
                List<Pawn> cpuPawnsWithMovePossible = new List<Pawn>();
                foreach (var pawn in player.pawns)
                    if ((board.fields[pawn.CurrentPosition + 7].IsBlack && board.fields[pawn.CurrentPosition + 7].IsEmpty) ||
                        (board.fields[pawn.CurrentPosition + 9].IsBlack && board.fields[pawn.CurrentPosition + 9].IsEmpty))
                        cpuPawnsWithMovePossible.Add(pawn);

                Random randomCpuPawn = new Random();
                int cpuPawnNumber = randomCpuPawn.Next(0, cpuPawnsWithMovePossible.Count);
                Console.WriteLine($"{player.Name} wybiera pionka, którym chce ruszyć: {cpuPawnsWithMovePossible[cpuPawnNumber].Name}");
                return cpuPawnsWithMovePossible[cpuPawnNumber];

                

            }
            else
            {
                do
                {
                    Console.Write($"{player.Name} wybiera pionka, którym chce ruszyć: ");
                    string playerChoice = Console.ReadLine().ToUpper();
                    var chosenPawn = player.pawns.FirstOrDefault(pawn => pawn.Name == playerChoice);
                    if (chosenPawn != null)
                    {

                        return chosenPawn;
                    }
                    else
                        Console.WriteLine("Niepoprawny pionek.");
                }
                while (true);
            }
            
        }

        private void MovePawnOrJumpOver(Pawn chosenPawn,Player player, Player opponent)
        {
            if (player.Name == "CPU")
            {
                if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack) 
                    && (board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
                {
                    Random randomCpuMove = new Random();
                    int cpuPawnMove = randomCpuMove.Next(0, 2);
                    if (cpuPawnMove == 0)
                    {
                        board.fields[chosenPawn.CurrentPosition].Content = " ";
                        board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
                        board.fields[chosenPawn.CurrentPosition].IsCpu = false;

                        chosenPawn.CurrentPosition += 7;

                        board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
                        board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
                        board.fields[chosenPawn.CurrentPosition].IsCpu = true;

                        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
                    }
                    else
                    {
                        board.fields[chosenPawn.CurrentPosition].Content = " ";
                        board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
                        board.fields[chosenPawn.CurrentPosition].IsCpu = false;

                        chosenPawn.CurrentPosition += 9;

                        board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
                        board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
                        board.fields[chosenPawn.CurrentPosition].IsCpu = true;

                        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
                    }
                }
                else if ((!board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack) 
                    && board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
                {
                    board.fields[chosenPawn.CurrentPosition].Content = " ";
                    board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
                    board.fields[chosenPawn.CurrentPosition].IsCpu = false;

                    chosenPawn.CurrentPosition += 9;

                    board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
                    board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
                    board.fields[chosenPawn.CurrentPosition].IsCpu = true;

                    Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
                }
                else if ((!board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
                    && board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
                {
                    board.fields[chosenPawn.CurrentPosition].Content = " ";
                    board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
                    board.fields[chosenPawn.CurrentPosition].IsCpu = false;

                    chosenPawn.CurrentPosition += 7;

                    board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
                    board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
                    board.fields[chosenPawn.CurrentPosition].IsCpu = true;

                    Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
                }
            }
            else
            {
                Console.Write($"Na które pole przestawić pionka {chosenPawn.Name}?: ");
                string playerChoice = Console.ReadLine().ToUpper();
                if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice))
                {
                    int chosenField = board.fieldsOnBoardDictionary[playerChoice];
                    if (
                        (board.fields[chosenField].IsEmpty && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 7 && board.fields[chosenField].IsBlack))
                        ||
                        (board.fields[chosenField].IsEmpty && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 9 && board.fields[chosenField].IsBlack)))
                                               
                    {
                        board.fields[chosenPawn.CurrentPosition].Content = " ";
                        board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
                        board.fields[chosenPawn.CurrentPosition].IsPlayer = false;

                        chosenPawn.CurrentPosition = board.fields[chosenField].Number;
                        board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
                        board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
                        board.fields[chosenPawn.CurrentPosition].IsPlayer = true;

                        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
                    }
                    else if (board.fields[chosenField].IsEmpty && board.fields[chosenField - (chosenField - chosenPawn.CurrentPosition) / 2].IsCpu)
                    {
                        board.fields[chosenPawn.CurrentPosition].Content = " ";
                        board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
                        board.fields[chosenPawn.CurrentPosition].IsPlayer = false;

                        chosenPawn.CurrentPosition = board.fields[chosenField].Number;

                        board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
                        board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
                        board.fields[chosenPawn.CurrentPosition].IsPlayer = true;

                        //!!!!!!!!opponent.pawns.RemoveAt(chosenField - (chosenField - chosenPawn.CurrentPosition) / 2);

                        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
                    }
                    else
                    {
                        Console.WriteLine("Ruch niemożliwy.");
                    }
                }
                else
                {
                    Console.WriteLine("Błędne pole.");
                };
            }

            
        }
    }
}
