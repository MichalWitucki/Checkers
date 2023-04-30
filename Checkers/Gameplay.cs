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

        public void DrawBoard()
        {
            Console.Write($"  |");
            foreach (char c in "ABCDEFG")
                Console.Write($"{c}|");
            Console.WriteLine("H|");        
            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{i + 1} |");
                for (int j = 0; j < 8; j++)
                    Console.Write($"{board.fields[j+i*8].Content}|");
                Console.WriteLine(); 
            }
        }

        public void SpacingPawns(Player player, Player cpu)
        {
            int playerPawn = 0;
            int cpuPawn = 0;
            
            for (int y = 0; y < 3; y++) // na 3 zmienic y
                for(int x = 0; x < 8; x++)
                    if (board.fields[x + y * 8].IsBlack) //do metody??
                    {
                        board.fields[x + y * 8].Content = cpu.pawns[cpuPawn].Name;
                        board.fields[x + y * 8].IsEmpty = false;
                        board.fields[x + y * 8].OccupiedBy = cpu.Name;
                        cpu.pawns[cpuPawn].CurrentPosition = x + y * 8;
                        cpuPawn++;
                    }
            for (int y = 5; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    if (board.fields[x + y * 8].IsBlack)
                    {
                        board.fields[x + y * 8].Content = player.pawns[playerPawn].Name;
                        board.fields[x + y * 8].IsEmpty = false;
                        board.fields[x + y * 8].OccupiedBy = player.Name;
                        player.pawns[playerPawn].CurrentPosition = x + y * 8;
                        playerPawn++;
                    }
        }
        //******************************************************************************

        public void CheckIfPawnCanJumpOver(Player turnPlayer, Player opponent)
        {
            turnPlayer.PawnsThatCanJumpOver = new List<Pawn>();
            
            foreach (var pawn in turnPlayer.pawns)
            {
                pawn.PawnsToJumpOver = new List<Pawn>();

                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        if (board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsBlack
                            && board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsEmpty
                            && board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i]].OccupiedBy == opponent.Name)
                            pawn.PawnsToJumpOver.Add(opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == pawn.CurrentPosition + board.CrossCheckDictionary[i]));
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        continue;
                    }
                }
                if (pawn.PawnsToJumpOver.Count != 0)
                    turnPlayer.PawnsThatCanJumpOver.Add(pawn);
             }
        }

        public void CheckIfPlayerPawnCanMove(Player player)
        {

            player.PawnsThatCanMove = new List<Pawn>();
            foreach (var pawn in player.pawns)
                try
                {
                    if ((board.fields[pawn.CurrentPosition - 7].IsBlack
                        && board.fields[pawn.CurrentPosition - 7].IsEmpty)
                        ||
                        (board.fields[pawn.CurrentPosition - 9].IsBlack 
                        && board.fields[pawn.CurrentPosition - 9].IsEmpty))
                        player.PawnsThatCanMove.Add(pawn);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    continue;
                }
        }
        public void CheckIfCpuPawnCanMove(Player cpu)
        {

            cpu.PawnsThatCanMove = new List<Pawn>();
            foreach (var pawn in cpu.pawns)
                try
                {
                    if ((board.fields[pawn.CurrentPosition + 7].IsBlack
                        && board.fields[pawn.CurrentPosition + 7].IsEmpty)
                        ||
                        (board.fields[pawn.CurrentPosition + 9].IsBlack
                        && board.fields[pawn.CurrentPosition + 9].IsEmpty))
                        cpu.PawnsThatCanMove.Add(pawn);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    continue;
                }
        }

        public Pawn PlayerChoosesPawn(Player player, bool jumpOverIsObligatory) 
        {
            do
            {
                Console.Write($"{player.Name} wybiera pionka, którym chce ruszyć: ");
                string playerChoice = Console.ReadLine();
                var chosenPawn = player.pawns.FirstOrDefault(pawn => pawn.Name == playerChoice);
                if (chosenPawn != null 
                    && player.PawnsThatCanJumpOver.Count != 0 
                    && player.PawnsThatCanJumpOver.Contains(chosenPawn))
                    return chosenPawn;
                else if (chosenPawn != null 
                    && player.PawnsThatCanMove.Count != 0 
                    && player.PawnsThatCanMove.Contains(chosenPawn)
                    && !jumpOverIsObligatory)
                    return chosenPawn;
                else
                    Console.WriteLine("Niepoprawny pionek.");
            }
            while (true);
        }

        public void CpuChoosesPawn(Player cpu, Player player)
        {
                if (cpu.PawnsThatCanJumpOver.Count != 0)
                {
                    Random randomCpuPawn = new Random();
                    int cpuPawnNumber = randomCpuPawn.Next(0, cpu.PawnsThatCanJumpOver.Count);
                    Pawn choosenPawn = cpu.PawnsThatCanJumpOver[cpuPawnNumber];
                    CpuJumpsOver(cpu, player, choosenPawn);

                }
                else
                {
                    Random randomCpuPawn = new Random();
                    int cpuPawnNumber = randomCpuPawn.Next(0, cpu.PawnsThatCanMove.Count);
                    Pawn chosenPawn = cpu.PawnsThatCanMove[cpuPawnNumber];
                    CpuMovesPawn(chosenPawn, cpu, player);
                }
        }



            public void PlayerChoosesField(Pawn chosenPawn, Player player, Player cpu, bool jumpOverIsObligatory)
            {
            bool incorrectChoice = true;

            do
            {
                Console.Write($"Na które pole przestawić pionka {chosenPawn.Name}?: ");
                string playerChoice = Console.ReadLine().ToUpper();
                if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice))
                {
                    int chosenField = board.fieldsOnBoardDictionary[playerChoice];
                    if (jumpOverIsObligatory && 
                        (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 18 
                        || board.fields[chosenField].Number == chosenPawn.CurrentPosition - 14 
                        || board.fields[chosenField].Number == chosenPawn.CurrentPosition + 14 
                        || board.fields[chosenField].Number == chosenPawn.CurrentPosition + 18)
                        &&
                        board.fields[chosenField].IsEmpty && board.fields[chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2].OccupiedBy == cpu.Name)
                    {
                        LeaveField(chosenPawn);
                        PlayerJumpsOver(chosenPawn, chosenField, player, cpu);
                        incorrectChoice = false;
                    }
                    else if (player.PawnsThatCanMove.Count != 0 
                        &&
                        ((board.fields[chosenField].IsEmpty 
                        && board.fields[chosenField].IsBlack 
                        && board.fields[chosenField].Number == chosenPawn.CurrentPosition - 7)
                        ||
                        (board.fields[chosenField].IsEmpty 
                        && board.fields[chosenField].IsBlack 
                        && board.fields[chosenField].Number == chosenPawn.CurrentPosition - 9)))
                    {
                        LeaveField(chosenPawn);
                        PlayerTakesField(chosenPawn, chosenField, player);
                        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
                        incorrectChoice = false;
                    }
                    else
                        Console.WriteLine("Błędne pole.");
                }
                else
                    Console.WriteLine("Błędne pole.");
            } 
            while (incorrectChoice);
        }

        private void CpuMovesPawn(Pawn chosenPawn, Player cpu, Player player)
        {
            if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
                &&
                (board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
            {
                Random randomCpuMove = new Random();
                int cpuPawnMove = randomCpuMove.Next(0, 2);
                if (cpuPawnMove == 0)
                {
                    LeaveField(chosenPawn);
                    chosenPawn.CurrentPosition += 7;
                    CpuTakesField(chosenPawn, cpu); 
                }
                else
                {
                    LeaveField(chosenPawn);
                    chosenPawn.CurrentPosition += 9;
                    CpuTakesField(chosenPawn, cpu);
                }
            }
            else if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
                    && (!board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
            {
                LeaveField(chosenPawn);
                chosenPawn.CurrentPosition += 7;
                CpuTakesField(chosenPawn, cpu);
            }
            else if ((board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
                    && (!board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack))
            {
                LeaveField(chosenPawn);
                chosenPawn.CurrentPosition += 9;
                CpuTakesField(chosenPawn, cpu);
            }
            else if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && !board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
                    && (board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
            {
                LeaveField(chosenPawn);
                chosenPawn.CurrentPosition += 9;
                CpuTakesField(chosenPawn, cpu);
            }
            else if ((board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && !board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
                && (board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack))
            {
                LeaveField(chosenPawn);
                chosenPawn.CurrentPosition += 7;
                CpuTakesField(chosenPawn, cpu);
            }
        }

        private void PlayerJumpsOver(Pawn chosenPawn, int chosenField, Player player, Player cpu)
        {
            Pawn jumpedPawn = cpu.pawns
                .FirstOrDefault(pawn => pawn.CurrentPosition == chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2);
            LeaveField(jumpedPawn);
            cpu.pawns.Remove(jumpedPawn);
            PlayerTakesField(chosenPawn, chosenField, player);
            Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
            if (CheckIfChosenPawnCanJumpOverAgain(chosenPawn, player, cpu).Count != 0)
            {
                DrawBoard();
                PlayerChoosesField(chosenPawn, player, cpu, true);
            }
        }

        private void CpuJumpsOver(Player cpu, Player player, Pawn chosenPawn)
        {
            if (chosenPawn.PawnsToJumpOver.Count > 1)
            {
                //logika gdy jest wiecej
            }
            else
            {
                var jumpedPawn = chosenPawn.PawnsToJumpOver[0];
                board.fields[jumpedPawn.CurrentPosition].Content = " ";
                board.fields[jumpedPawn.CurrentPosition].IsEmpty = true;
                board.fields[jumpedPawn.CurrentPosition].OccupiedBy = null;
                LeaveField(chosenPawn);
                chosenPawn.CurrentPosition = chosenPawn.CurrentPosition - 2 * (chosenPawn.CurrentPosition - jumpedPawn.CurrentPosition);
                player.pawns.Remove(jumpedPawn);
                CpuTakesField(chosenPawn, cpu);
            }
        }

            private void LeaveField(Pawn chosenPawn)
        {
            board.fields[chosenPawn.CurrentPosition].Content = " ";
            board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
            board.fields[chosenPawn.CurrentPosition].OccupiedBy = null;

        }
        private void PlayerTakesField(Pawn chosenPawn, int chosenField, Player player) //**//
        {
         
            chosenPawn.CurrentPosition = board.fields[chosenField].Number;
            IsPawnKing(player, chosenPawn);
            board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
            board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
            board.fields[chosenPawn.CurrentPosition].OccupiedBy = player.Name;
        }
        private void CpuTakesField(Pawn chosenPawn, Player cpu)
        {
            IsPawnKing(cpu, chosenPawn);
            board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
            board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
            board.fields[chosenPawn.CurrentPosition].OccupiedBy = cpu.Name;
            Console.WriteLine($"CPU Przestawia pionka na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
        }

        private void IsPawnKing(Player turnPlayer, Pawn chosenPawn) 
        {
            if (turnPlayer.IsCpu)
            {
                if (55 < chosenPawn.CurrentPosition && chosenPawn.CurrentPosition < 63)
                {
                    chosenPawn.Name = chosenPawn.Name.ToUpper();
                    chosenPawn.IsKing = true;
                }
            }
            else 
            {
                if (0 < chosenPawn.CurrentPosition && chosenPawn.CurrentPosition < 8)
                {
                    chosenPawn.Name = chosenPawn.Name.ToUpper();
                    chosenPawn.IsKing = true;
                }
                
            }
        }


        public List<Pawn> CheckIfChosenPawnCanJumpOverAgain(Pawn chosenPawn, Player turnPlayer, Player opponent)
        {
            chosenPawn.PawnsToJumpOver = new List<Pawn>();
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsBlack
                        && board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsEmpty
                        && board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i]].OccupiedBy == opponent.Name)
                        chosenPawn.PawnsToJumpOver.Add(opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == chosenPawn.CurrentPosition + board.CrossCheckDictionary[i]));
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    continue;
                }
            }
            return chosenPawn.PawnsToJumpOver;
        }


        //************************************************************************************************************************
        //public void CpuTurn(Player cpu, Player player)
        //{
        //    //CpuChoosesPawn(cpu, player);

        //}
        //private void CpuChoosesPawn(Player cpu, Player player)
        //{
        //    List<Pawn> cpuPawnsWhichCanJumpOver = new List<Pawn>();

        //    foreach (var pawn in cpu.pawns)
        //    {
        //        //pawn.PlayerPawnsToJumpOverByCpuPawn = new List<Pawn>();
        //        for (int i = 0; i < 4; i++)
        //        {
        //            try
        //            {
        //                if (board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsBlack
        //                    && board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsEmpty
        //                    && board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i]].IsPlayer) ;
        //                //pawn.PlayerPawnsToJumpOverByCpuPawn.Add(player.pawns.FirstOrDefault(playerPawn => playerPawn.CurrentPosition == pawn.CurrentPosition + board.CrossCheckDictionary[i]));
        //            }
        //            catch (ArgumentOutOfRangeException ex)
        //            {
        //                continue;
        //            }
        //        }

        //        //if (pawn.PlayerPawnsToJumpOverByCpuPawn.Count != 0)
        //        //cpuPawnsWhichCanJumpOver.Add(pawn);
        //        //}


        //        if (cpuPawnsWhichCanJumpOver.Count != 0)
        //        {
        //            Random randomCpuPawn = new Random();
        //            int cpuPawnNumber = randomCpuPawn.Next(0, cpuPawnsWhichCanJumpOver.Count);
        //            Pawn choosenPawn = cpuPawnsWhichCanJumpOver[cpuPawnNumber];
        //            CpuJumpsOver(player, choosenPawn);

        //        }
        //        else
        //        {
        //            List<Pawn> cpuPawnsWithMovePossible = new List<Pawn>();
        //            //foreach (var pawn in cpu.pawns)
        //            try
        //            {
        //                if ((board.fields[pawn.CurrentPosition + 7].IsBlack && board.fields[pawn.CurrentPosition + 7].IsEmpty)
        //               ||
        //               (board.fields[pawn.CurrentPosition + 9].IsBlack && board.fields[pawn.CurrentPosition + 9].IsEmpty))
        //                    cpuPawnsWithMovePossible.Add(pawn);
        //            }
        //            catch (ArgumentOutOfRangeException ex)
        //            {
        //                continue;
        //            }

        //            Random randomCpuPawn = new Random();
        //            int cpuPawnNumber = randomCpuPawn.Next(0, cpuPawnsWithMovePossible.Count);
        //            Pawn chosenPawn = cpuPawnsWithMovePossible[cpuPawnNumber];
        //            CpuMovesPawn(chosenPawn, cpu, player);
        //        }
        //    }
        //    private void CpuMovesPawnoLD(Pawn chosenPawn,Player player, Player opponent)
        //{
        //    if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack) 
        //        &&
        //        (board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
        //    {
        //        Random randomCpuMove = new Random();
        //        int cpuPawnMove = randomCpuMove.Next(0, 2);
        //        if (cpuPawnMove == 0)
        //        {
        //            CpuLeavesField(chosenPawn);
        //            chosenPawn.CurrentPosition += 7;
        //            CpuTakesField(chosenPawn);
        //        }
        //        else
        //        {
        //            CpuLeavesField(chosenPawn);
        //            chosenPawn.CurrentPosition += 9;
        //            CpuTakesField(chosenPawn);
        //        }
        //    }
        //    else if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
        //            && (!board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
        //    {
        //        CpuLeavesField(chosenPawn); 
        //        chosenPawn.CurrentPosition += 7;
        //        CpuTakesField(chosenPawn);
        //    }
        //    else if ((board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
        //            && (!board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack))
        //    {
        //        CpuLeavesField(chosenPawn);
        //        chosenPawn.CurrentPosition += 9;
        //        CpuTakesField(chosenPawn);
        //    }
        //    else if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && !board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
        //            && (board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
        //    {
        //        CpuLeavesField(chosenPawn);
        //        chosenPawn.CurrentPosition += 9;
        //        CpuTakesField(chosenPawn);
        //    }
        //    else if ((board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && !board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
        //        && (board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack))
        //    {
        //        CpuLeavesField(chosenPawn);
        //        chosenPawn.CurrentPosition += 7;
        //        CpuTakesField(chosenPawn);
        //    }
        //}
        //private void CpuJumpsOverOld(Player player, Pawn chosenPawn)
        //{
        //    if (chosenPawn.PlayerPawnsToJumpOverByCpuPawn.Count > 1)
        //    {
        //        //logika gdy jest wiecej
        //    }
        //    else
        //    {
        //        var jumpedPawn = chosenPawn.PlayerPawnsToJumpOverByCpuPawn[0];
        //        board.fields[jumpedPawn.CurrentPosition].Content = " ";
        //        board.fields[jumpedPawn.CurrentPosition].IsEmpty = true;
        //        board.fields[jumpedPawn.CurrentPosition].IsPlayer = false;
        //        CpuLeavesField(chosenPawn);
        //        chosenPawn.CurrentPosition = chosenPawn.CurrentPosition - 2 * (chosenPawn.CurrentPosition - jumpedPawn.CurrentPosition);
        //        player.pawns.Remove(jumpedPawn);
        //        CpuTakesField(chosenPawn);
        //    }



            ////}
            //private void CpuLeavesFieldold(Pawn chosenPawn)
            //{
            //    board.fields[chosenPawn.CurrentPosition].Content = " ";
            //    board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
            //    board.fields[chosenPawn.CurrentPosition].IsCpu = false;
            //}
            //private void CpuTakesFieldOld(Pawn chosenPawn) 
            //{
            //    IsCpuPawnKing(chosenPawn); 
            //    board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
            //    board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
            //    board.fields[chosenPawn.CurrentPosition].IsCpu = true;
            //    Console.WriteLine($"CPU Przestawia pionka na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
            //}
            //private void IsCpuPawnKing(Pawn chosenPawn)
            //{
            //    if (55 < chosenPawn.CurrentPosition && chosenPawn.CurrentPosition < 63)
            //    {
            //        chosenPawn.Name = chosenPawn.Name.ToUpper();
            //        chosenPawn.IsKing = true;
            //    }
            //}


        }
}

//private void PlayerMovesPawnOrJumpsOver(Pawn chosenPawn, Player player, Player cpu)
//{
//    Console.Write($"Na które pole przestawić pionka {chosenPawn.Name}?: "); // dodac petle
//    string playerChoice = Console.ReadLine().ToUpper();
//    if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice))
//    {
//        int chosenField = board.fieldsOnBoardDictionary[playerChoice];
//        if ((board.fields[chosenField].IsEmpty && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 7 && board.fields[chosenField].IsBlack))
//            ||
//            (board.fields[chosenField].IsEmpty && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 9 && board.fields[chosenField].IsBlack)))
//        {
//            PlayerLeavesField(chosenPawn);
//            PlayerTakesField(chosenPawn, chosenField);
//            Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
//        }
//        else if ((board.fields[chosenField].Number == chosenPawn.CurrentPosition -18 || board.fields[chosenField].Number == chosenPawn.CurrentPosition - 14 ||
//            board.fields[chosenField].Number == chosenPawn.CurrentPosition + 14 || board.fields[chosenField].Number == chosenPawn.CurrentPosition + 18) &&
//            board.fields[chosenField].IsEmpty && board.fields[chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2].IsCpu)
//        {
//            PlayerLeavesField(chosenPawn);
//            PlayerJumpsOver(cpu, chosenPawn, chosenField);
//            PlayerTakesField(chosenPawn, chosenField);
//            Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
//            if (IsNextJumpPossible(chosenPawn))
//            {

//            }

//        }
//        else
//            Console.WriteLine("Ruch niemożliwy.");
//    }
//    else
//        Console.WriteLine("Błędne pole.");
//}
//private void PlayerMovesKingOrJumpsOver(Pawn chosenPawn, Player player, Player cpu)
//{
//Console.Write($"Na które pole przestawić pionka {chosenPawn.Name}?: "); // dodac petle
//string playerChoice = Console.ReadLine().ToUpper();
//if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice))
//{
//    int chosenField = board.fieldsOnBoardDictionary[playerChoice];
//    if ((board.fields[chosenField].IsEmpty && board.fields[chosenField].IsBlack && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 7 ))
//        ||
//        (board.fields[chosenField].IsEmpty && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 9 && board.fields[chosenField].IsBlack)))
//    {
//        PlayerLeavesField(chosenPawn);
//        PlayerTakesField(chosenPawn, chosenField);
//        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
//    }
//    else if ((board.fields[chosenField].Number == chosenPawn.CurrentPosition -18 || board.fields[chosenField].Number == chosenPawn.CurrentPosition - 14 ||
//        board.fields[chosenField].Number == chosenPawn.CurrentPosition + 14 || board.fields[chosenField].Number == chosenPawn.CurrentPosition + 18) &&
//        board.fields[chosenField].IsEmpty && board.fields[chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2].IsCpu)
//    {
//        PlayerLeavesField(chosenPawn);
//        PlayerJumpsOver(cpu, chosenPawn, chosenField);
//        PlayerTakesField(chosenPawn, chosenField);
//        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
//    }
//    else
//        Console.WriteLine("Ruch niemożliwy.");
//}
//else
//    Console.WriteLine("Błędne pole.");
//}




//private bool IsPlayerNextJumpPossibleOld(Pawn chosenPawn)
//{
//    bool IsNextJumpPossible = false;
//    for (int i = 0; i < 4; i++)
//    {
//        try
//        {
//            if (board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsBlack
//                && board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsEmpty
//                && board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i]].IsCpu)
//            {
//                IsNextJumpPossible = true;
//                return IsNextJumpPossible;
//            }
//        }
//        catch (ArgumentOutOfRangeException ex)
//        {
//            continue;
//        }
//    }
//    return IsNextJumpPossible;
//}

//private void PlayerJumpsOverOld(Pawn chosenPawn, int chosenField, Player cpu)
//{
//    PlayerLeavesField(chosenPawn);
//    var jumpedPawn = cpu.pawns
//        .FirstOrDefault(pawn => pawn.CurrentPosition == chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2);
//    board.fields[jumpedPawn.CurrentPosition].Content = " ";
//    board.fields[jumpedPawn.CurrentPosition].IsEmpty = true;
//    board.fields[jumpedPawn.CurrentPosition].IsCpu = false;
//    cpu.pawns.Remove(jumpedPawn);
//    PlayerTakesField(chosenPawn, chosenField);
//    Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
//    if (IsPlayerNextJumpPossible(chosenPawn))
//    {
//        DrawBoard();
//        PlayerChoosesField(chosenPawn, cpu, true);
//    }
//}

//public void PlayerChoosesFieldOld(Pawn chosenPawn, Player cpu, bool checkOnlyJump = false) 
//{
//    bool incorrectChoice = true;
//    do
//    {
//        Console.Write($"Na które pole przestawić pionka {chosenPawn.Name}?: ");
//        string playerChoice = Console.ReadLine().ToUpper();
//        if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice))
//        {
//            int chosenField = board.fieldsOnBoardDictionary[playerChoice];
//            if (CheckIfMoveIsPossible(chosenPawn, chosenField) && checkOnlyJump == false)
//            {
//                PlayerMoves(chosenPawn, chosenField);
//                Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {playerChoice}.");
//                incorrectChoice = false;
//            }
//            else if (CheckIfJumpIsPossible(chosenPawn,chosenField))
//            {
//                PlayerJumpsOver(chosenPawn, chosenField, cpu);

//                incorrectChoice = false;  
//            }
//            else
//                Console.WriteLine("Ruch niemożliwy.");
//        }
//        else
//            Console.WriteLine("Błędne pole.");
//    } while (incorrectChoice);
//}

//private bool CheckIfMoveIsPossible(Pawn chosenPawn, int chosenField)
//{
//    if ((board.fields[chosenField].IsEmpty && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 7 && board.fields[chosenField].IsBlack))
//       ||
//       (board.fields[chosenField].IsEmpty && (board.fields[chosenField].Number == chosenPawn.CurrentPosition - 9 && board.fields[chosenField].IsBlack)))
//        return true;
//    else return false;
//}        

//private bool CheckIfJumpIsPossible(Pawn chosenPawn, int chosenField)
//{
//    if ((board.fields[chosenField].Number == chosenPawn.CurrentPosition - 18 || board.fields[chosenField].Number == chosenPawn.CurrentPosition - 14 ||
//                board.fields[chosenField].Number == chosenPawn.CurrentPosition + 14 || board.fields[chosenField].Number == chosenPawn.CurrentPosition + 18) &&
//                board.fields[chosenField].IsEmpty && board.fields[chosenPawn.CurrentPosition - (chosenPawn.CurrentPosition - chosenField) / 2].IsCpu)
//        return true;
//    else return false;
//}

//private void PlayerMoves(Pawn chosenPawn, int chosenField)
//{
//    if (chosenPawn.IsKing)
//    {
//        // PlayerMovesKing();
//    }
//    else
//        PlayerMovesPawn(chosenPawn, chosenField);
//}










