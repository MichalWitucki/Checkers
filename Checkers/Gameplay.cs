﻿using System;
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
            //Console.WriteLine("  -----------"); 
            
            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{i + 1} |");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write($"{board.fieldsOnBoard[j+i*8].Content}|");
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
                        board.fieldsOnBoard[x + y * 8].IsCPU = true;
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
                        board.fieldsOnBoard[x + y * 8].IsPlayer = true;
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

        public void Turn(Player player)
        {
            if (player.Name == "CPU")
                CpuTurn();
            else
            {
                Pawn chosenPawn = ChoosePawn(player);
                MovePawnOrJumpOver(chosenPawn);
            }
                          
            
        }

    

        public void CpuTurn()
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
                if (chosenPawn != null)
                {
                    
                    return chosenPawn;
                }
                else
                    Console.WriteLine("Niepoprawny pionek.");      
            }
            while (true);
        }


        private void MovePawnOrJumpOver(Pawn chosenPawn)
        {
            Console.Write($"Na które pole przestawić pionka {chosenPawn.Name}?: ");
            string playerChoice = Console.ReadLine().ToUpper();


            if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice))
            { 
                if ((board.fieldsOnBoard[board.fieldsOnBoardDictionary[playerChoice]].IsEmpty && 
                    (board.fieldsOnBoard[board.fieldsOnBoardDictionary[playerChoice]].Number == chosenPawn.CurrentPosition -7 && board.fieldsOnBoard[board.fieldsOnBoardDictionary[playerChoice]].IsBlack)) ||
                    (board.fieldsOnBoard[board.fieldsOnBoardDictionary[playerChoice]].IsEmpty &&
                    (board.fieldsOnBoard[board.fieldsOnBoardDictionary[playerChoice]].Number == chosenPawn.CurrentPosition - 9 && board.fieldsOnBoard[board.fieldsOnBoardDictionary[playerChoice]].IsBlack))
                    )
                {
                    board.fieldsOnBoard[chosenPawn.CurrentPosition].Content = " "; 
                    board.fieldsOnBoard[chosenPawn.CurrentPosition].IsEmpty = true;
                    board.fieldsOnBoard[chosenPawn.CurrentPosition].IsPlayer = false;
                    
                    chosenPawn.CurrentPosition = board.fieldsOnBoardDictionary[playerChoice];
                    board.fieldsOnBoard[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
                    board.fieldsOnBoard[chosenPawn.CurrentPosition].IsEmpty = false;
                    board.fieldsOnBoard[chosenPawn.CurrentPosition].IsPlayer = true;

                    Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
                }
                //else if (true)
                //{
                    //bicie
                //}
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
