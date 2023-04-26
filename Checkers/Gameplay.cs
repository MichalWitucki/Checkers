using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        //public Player DrawWhoStarts(Player player, Player cpu) 
        //{
        //    Random random = new Random();
             
        //    switch (random.Next(0, 2))
        //    {
        //        case 0:
        //            return player;
        //        default:
        //           return cpu;
        //    }
        //}
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
            }
        }

        public void SpacingPawns(Player player, Player cpu)
        {
            int playerPawn = 0;
            int cpuPawn = 0;
            
            for (int y = 0; y < 1; y++) // na 3 zmienic y
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

        public void PlayerTurn(Player player, Player cpu)
        {
            Pawn chosenPawn = PlayerChoosesPawn(player);
            if (chosenPawn.IsKing) 
            {
                PlayerMovesKingOrJumpsOver(chosenPawn, player, cpu);
            }
            else
                PlayerMovesPawnOrJumpsOver(chosenPawn, player, cpu);
        }



        private Pawn PlayerChoosesPawn(Player player)
        {
            do
            {
                Console.Write($"{player.Name} wybiera pionka, którym chce ruszyć: ");
                string playerChoice = Console.ReadLine();
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
        private void PlayerMovesPawnOrJumpsOver(Pawn chosenPawn, Player player, Player cpu)
        {
            Console.Write($"Na które pole przestawić pionka {chosenPawn.Name}?: ");
            string playerChoice = Console.ReadLine().ToUpper();
            if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice))
            {
                int chosenField = board.fieldsOnBoardDictionary[playerChoice];
                if ((board.fields[chosenField].IsEmpty && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 7 && board.fields[chosenField].IsBlack))
                    ||
                    (board.fields[chosenField].IsEmpty && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 9 && board.fields[chosenField].IsBlack)))
                {
                    PlayerLeavesField(chosenPawn);
                    PlayerTakesField(chosenPawn, chosenField);
                    Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
                }
                else if ((board.fields[chosenField].Number == chosenPawn.CurrentPosition -18 || board.fields[chosenField].Number == chosenPawn.CurrentPosition - 14 ||
                    board.fields[chosenField].Number == chosenPawn.CurrentPosition + 14 || board.fields[chosenField].Number == chosenPawn.CurrentPosition + 18) &&
                    board.fields[chosenField].IsEmpty && board.fields[chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2].IsCpu)
                    
            
                {
                    PlayerLeavesField(chosenPawn);
                    PlayerJumpsOver(cpu, chosenPawn, chosenField);
                    PlayerTakesField(chosenPawn, chosenField);
                    Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
                }
                else
                    Console.WriteLine("Ruch niemożliwy.");
            }
            else
                Console.WriteLine("Błędne pole.");
        }
        private void PlayerMovesKingOrJumpsOver(Pawn chosenPawn, Player player, Player cpu)
        {
            
        }
        private void PlayerLeavesField(Pawn chosenPawn)
        {
            board.fields[chosenPawn.CurrentPosition].Content = " ";
            board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
            board.fields[chosenPawn.CurrentPosition].IsPlayer = false;
        }
        private void PlayerTakesField(Pawn chosenPawn, int chosenField) 
        {
            chosenPawn.CurrentPosition = board.fields[chosenField].Number;
            IsPlayerPawnKing(chosenPawn);
            board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
            board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
            board.fields[chosenPawn.CurrentPosition].IsPlayer = true;

        }
        private void PlayerJumpsOver(Player cpu, Pawn chosenPawn, int chosenField)
        {
            var jumpedPawn = cpu.pawns
                .FirstOrDefault(pawn => pawn.CurrentPosition == chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2);
            board.fields[jumpedPawn.CurrentPosition].Content = " ";
            board.fields[jumpedPawn.CurrentPosition].IsEmpty = true;
            board.fields[jumpedPawn.CurrentPosition].IsCpu = false;
            cpu.pawns.Remove(jumpedPawn);
        }
        private void IsPlayerPawnKing(Pawn chosenPawn)
        {
            if (0 < chosenPawn.CurrentPosition && chosenPawn.CurrentPosition < 8)
            {
                chosenPawn.Name = chosenPawn.Name.ToUpper();
                chosenPawn.IsKing = true;
            }
                          
        }
        //************************************************************************************************************************
        public void CpuTurn(Player cpu, Player player)
        {
            CpuChoosesPawn(cpu, player);

        }
        private void CpuChoosesPawn(Player cpu, Player player)
       {
            List<Pawn> cpuPawnsWhichCanJumpOver = new List<Pawn>();
            
            foreach (var pawn in cpu.pawns)
            {
                pawn.PlayerPawnsToJumpOverByCpuPawn = new List<Pawn>();
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        if (board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsBlack 
                            && board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsEmpty 
                            && board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i]].IsPlayer)
                            pawn.PlayerPawnsToJumpOverByCpuPawn.Add(player.pawns.FirstOrDefault(playerPawn => playerPawn.CurrentPosition == pawn.CurrentPosition + board.CrossCheckDictionary[i]));
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        continue;
                    }
                }

                if (pawn.PlayerPawnsToJumpOverByCpuPawn.Count != 0)
                    cpuPawnsWhichCanJumpOver.Add(pawn);
            }


            if (cpuPawnsWhichCanJumpOver.Count != 0)
            {
                Random randomCpuPawn = new Random();
                int cpuPawnNumber = randomCpuPawn.Next(0, cpuPawnsWhichCanJumpOver.Count);
                Pawn choosenPawn = cpuPawnsWhichCanJumpOver[cpuPawnNumber];
                CpuJumpsOver(player, choosenPawn);

            }
            else
            {
                List<Pawn> cpuPawnsWithMovePossible = new List<Pawn>();
                foreach (var pawn in cpu.pawns)
                    try
                    {
                        if ((board.fields[pawn.CurrentPosition + 7].IsBlack && board.fields[pawn.CurrentPosition + 7].IsEmpty)
                       ||
                       (board.fields[pawn.CurrentPosition + 9].IsBlack && board.fields[pawn.CurrentPosition + 9].IsEmpty))
                            cpuPawnsWithMovePossible.Add(pawn);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        continue;
                    }

                Random randomCpuPawn = new Random();
                int cpuPawnNumber = randomCpuPawn.Next(0, cpuPawnsWithMovePossible.Count);
                Pawn chosenPawn = cpuPawnsWithMovePossible[cpuPawnNumber];
                CpuMovesPawn(chosenPawn, cpu, player);
            }
        }
        private void CpuMovesPawn(Pawn chosenPawn,Player player, Player opponent)
        {
            if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack) 
                &&
                (board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
            {
                Random randomCpuMove = new Random();
                int cpuPawnMove = randomCpuMove.Next(0, 2);
                if (cpuPawnMove == 0)
                {
                    CpuLeavesField(chosenPawn);
                    chosenPawn.CurrentPosition += 7;
                    CpuTakesField(chosenPawn);
                }
                else
                {
                    CpuLeavesField(chosenPawn);
                    chosenPawn.CurrentPosition += 9;
                    CpuTakesField(chosenPawn);
                }
            }
            else if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
                    && (!board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
            {
                CpuLeavesField(chosenPawn); 
                chosenPawn.CurrentPosition += 7;
                CpuTakesField(chosenPawn);
            }
            else if ((board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
                    && (!board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack))
            {
                CpuLeavesField(chosenPawn);
                chosenPawn.CurrentPosition += 9;
                CpuTakesField(chosenPawn);
            }
            else if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && !board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
                    && (board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
            {
                CpuLeavesField(chosenPawn);
                chosenPawn.CurrentPosition += 9;
                CpuTakesField(chosenPawn);
            }
            else if ((board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && !board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
                && (board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack))
            {
                CpuLeavesField(chosenPawn);
                chosenPawn.CurrentPosition += 7;
                CpuTakesField(chosenPawn);
            }
        }
        private void CpuJumpsOver(Player player, Pawn chosenPawn)
        {
            if (chosenPawn.PlayerPawnsToJumpOverByCpuPawn.Count > 1)
            {
                //logika gdy jest wiecej
            }
            else
            {
                var jumpedPawn = chosenPawn.PlayerPawnsToJumpOverByCpuPawn[0];
                board.fields[jumpedPawn.CurrentPosition].Content = " ";
                board.fields[jumpedPawn.CurrentPosition].IsEmpty = true;
                board.fields[jumpedPawn.CurrentPosition].IsPlayer = false;
                CpuLeavesField(chosenPawn);
                chosenPawn.CurrentPosition = chosenPawn.CurrentPosition - 2*(chosenPawn.CurrentPosition - jumpedPawn.CurrentPosition);
                player.pawns.Remove(jumpedPawn);
                CpuTakesField(chosenPawn);
            }
                
            

        }
        private void CpuLeavesField(Pawn chosenPawn)
        {
            board.fields[chosenPawn.CurrentPosition].Content = " ";
            board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
            board.fields[chosenPawn.CurrentPosition].IsCpu = false;
        }
        private void CpuTakesField(Pawn chosenPawn) 
        {
            IsCpuPawnKing(chosenPawn); 
            board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
            board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
            board.fields[chosenPawn.CurrentPosition].IsCpu = true;
            Console.WriteLine($"CPU Przestawia pionka na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
        }
        private void IsCpuPawnKing(Pawn chosenPawn)
        {
            if (55 < chosenPawn.CurrentPosition && chosenPawn.CurrentPosition < 63)
            {
                chosenPawn.Name = chosenPawn.Name.ToUpper();
                chosenPawn.IsKing = true;
            }
        }


    }
}
