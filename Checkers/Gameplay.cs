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


        public void PlayerTurn(Player player, Player cpu)
        {
            Pawn chosenPawn = PlayerChoosesPawn(player);
            PlayerMovesPawnOrJumpsOver(chosenPawn, player, cpu);
        }
        public void CpuTurn(Player cpu, Player player)
        {
            Pawn chosenPawn = CpuChoosesPawn(cpu);
            CpuMovesPawnOrJumpsOver(chosenPawn, cpu, player);
        }
        private Pawn PlayerChoosesPawn(Player player)
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

        private Pawn CpuChoosesPawn(Player cpu)
        {
            List<Pawn> cpuPawnsWhichCanJumpOver = new List<Pawn>();
            foreach (var pawn in cpu.pawns)
            if ((board.fields[pawn.CurrentPosition + 14].IsBlack && board.fields[pawn.CurrentPosition + 14].IsEmpty && board.fields[pawn.CurrentPosition + 7].IsPlayer)
                ||
               (board.fields[pawn.CurrentPosition + 18].IsBlack && board.fields[pawn.CurrentPosition + 18].IsEmpty && board.fields[pawn.CurrentPosition + 9].IsPlayer))
               cpuPawnsWhichCanJumpOver.Add(pawn);
            if (cpuPawnsWhichCanJumpOver.Count == 0)
            {
                List<Pawn> cpuPawnsWithMovePossible = new List<Pawn>();
                foreach (var pawn in cpu.pawns)
                if ((board.fields[pawn.CurrentPosition + 7].IsBlack && board.fields[pawn.CurrentPosition + 7].IsEmpty)
                   ||
                   (board.fields[pawn.CurrentPosition + 9].IsBlack && board.fields[pawn.CurrentPosition + 9].IsEmpty))
                   cpuPawnsWithMovePossible.Add(pawn);

                Random randomCpuPawn = new Random();
                int cpuPawnNumber = randomCpuPawn.Next(0, cpuPawnsWithMovePossible.Count);
                Console.WriteLine($"{cpu.Name} wybiera pionka, którym chce ruszyć: {cpuPawnsWithMovePossible[cpuPawnNumber].Name}");
                return cpuPawnsWithMovePossible[cpuPawnNumber];
            }
            else
            {
                Random randomCpuPawn = new Random();
                int cpuPawnNumber = randomCpuPawn.Next(0, cpuPawnsWhichCanJumpOver.Count);
                Console.WriteLine($"{cpu.Name} wybiera pionka, którym chce ruszyć: {cpuPawnsWhichCanJumpOver[cpuPawnNumber].Name}");
                return cpuPawnsWhichCanJumpOver[cpuPawnNumber];
            }
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
                else if (board.fields[chosenField].IsEmpty && board.fields[chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2].IsCpu)
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

        private void PlayerLeavesField(Pawn chosenPawn)
        {
            board.fields[chosenPawn.CurrentPosition].Content = " ";
            board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
            board.fields[chosenPawn.CurrentPosition].IsPlayer = false;
        }
        private void PlayerTakesField(Pawn chosenPawn, int chosenField) 
        {
            chosenPawn.CurrentPosition = board.fields[chosenField].Number;
            board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
            board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
            board.fields[chosenPawn.CurrentPosition].IsPlayer = true;
        }
        private void PlayerJumpsOver(Player cpu, Pawn chosenPawn, int chosenField)
        {
            var jumpedPawn = cpu.pawns
                .FirstOrDefault(pawn => pawn.CurrentPosition == chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2);
            board.fields[jumpedPawn.CurrentPosition].Content = " ";
            cpu.pawns.Remove(jumpedPawn);
        }


        private void CpuMovesPawnOrJumpsOver(Pawn chosenPawn,Player player, Player opponent)
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

        private void CpuLeavesField(Pawn chosenPawn)
        {
            board.fields[chosenPawn.CurrentPosition].Content = " ";
            board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
            board.fields[chosenPawn.CurrentPosition].IsCpu = false;
        }
        private void CpuTakesField(Pawn chosenPawn) 
        {
            board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
            board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
            board.fields[chosenPawn.CurrentPosition].IsCpu = true;
            Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
        }
        private void CpuJumpsOver(Player player, Pawn chosenPawn, int chosenField)
        {
            var jumpedPawn = player.pawns
                .FirstOrDefault(pawn => pawn.CurrentPosition == chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2);
            board.fields[jumpedPawn.CurrentPosition].Content = " ";
            player.pawns.Remove(jumpedPawn);
        }

        
    }
}
