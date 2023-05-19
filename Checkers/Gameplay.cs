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
                    Console.Write($"{board.fields[j + i * 8].Content}|");
                Console.WriteLine();
            }
        }

        public void SpacingPawns(Player player, Player cpu)
        {
            int playerPawn = 0;
            int cpuPawn = 0;

            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 8; x++)
                    if (board.fields[x + y * 8].IsBlack)
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

        public void CheckIfPawnCanJumpOver(Player turnPlayer, Player opponent)
        {
            turnPlayer.PawnsThatCanJumpOver = new List<Pawn>();

            foreach (var pawn in turnPlayer.pawns)
            {
                pawn.PawnsToJumpOver = new List<Pawn>();
                pawn.FieldsToMove = new List<FieldOnBoard>();

                if (pawn.IsKing)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 1; j < 7; j++)
                        {
                            try
                            {
                                if (board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * j].IsBlack == false
                                    ||
                                    (board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * j].OccupiedBy == opponent.Name
                                    && board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * (j + 1)].OccupiedBy == opponent.Name)
                                    ||
                                    (board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * j].OccupiedBy == turnPlayer.Name))
                                    break;
                                else if (board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * j].OccupiedBy == opponent.Name)
                                {
                                    Pawn opponentPawn = opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == pawn.CurrentPosition + board.crossCheckDictionary[i] * j);
                                    try
                                    {
                                        for (int k = 1; k < 7; k++)
                                        {
                                            if (board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * j + board.crossCheckDictionary[i] * k].IsBlack
                                            &&
                                            board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * j + board.crossCheckDictionary[i] * k].IsEmpty)
                                            {
                                                pawn.FieldsToMove.Add(board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * j + board.crossCheckDictionary[i]*k]);
                                                if (!pawn.PawnsToJumpOver.Contains(opponentPawn))
                                                    pawn.PawnsToJumpOver.Add(opponentPawn);
                                                if (!turnPlayer.PawnsThatCanJumpOver.Contains(pawn))
                                                    turnPlayer.PawnsThatCanJumpOver.Add(pawn);
                                            }
                                        }
                                    }
                                    catch (ArgumentOutOfRangeException)
                                    {
                                        break;
                                    }
                                }
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        try
                        {
                            if (board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * 2].IsBlack
                                && 
                                board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * 2].IsEmpty
                                && 
                                board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i]].OccupiedBy == opponent.Name)
                            {
                                pawn.FieldsToMove.Add(board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * 2]);
                                pawn.PawnsToJumpOver.Add(opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == pawn.CurrentPosition + board.crossCheckDictionary[i]));
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            continue;
                        }
                    }
                    if (pawn.PawnsToJumpOver.Count != 0)
                        turnPlayer.PawnsThatCanJumpOver.Add(pawn);
                }
            }
        }

        public void CheckIfPawnCanMove(Player player)
        {
            player.PawnsThatCanMove = new List<Pawn>();

            foreach (var pawn in player.pawns)
            {
                pawn.FieldsToMove = new List<FieldOnBoard>();
                if (pawn.IsKing)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 1; j < 7; j++)
                        {
                            try
                            {
                                if (!board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * j].IsBlack
                                    ||
                                    !board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i] * j].IsEmpty)
                                    break;
                                else if (board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i]*j].IsBlack
                                    &&
                                    board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i]*j].IsEmpty)
                                {
                                    pawn.FieldsToMove.Add(board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i]*j]);
                                    if (!player.PawnsThatCanMove.Contains(pawn))
                                    {
                                        player.PawnsThatCanMove.Add(pawn);
                                    }
                                }
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                continue;
                            }
                        }   
                    }
                }
                else
                {
                    int counterStart = 0;
                    int counterEnd = 2;
                    if (player.IsCpu)
                    {
                        counterStart = 2;
                        counterEnd = 4;
                    }

                    try
                    {
                        for (int i = counterStart; i < counterEnd; i++)
                        {
                            if (board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i]].IsBlack
                                &&
                                board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i]].IsEmpty)
                            {
                                pawn.FieldsToMove.Add(board.fields[pawn.CurrentPosition + board.crossCheckDictionary[i]]);
                                if (!player.PawnsThatCanMove.Contains(pawn))
                                    player.PawnsThatCanMove.Add(pawn);
                            }

                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        continue;
                    }
                }
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
                    &&
                    player.PawnsThatCanJumpOver.Count != 0
                    &&
                    player.PawnsThatCanJumpOver.Contains(chosenPawn))
                    return chosenPawn;
                else if (chosenPawn != null
                    &&
                    player.PawnsThatCanMove.Count != 0
                    &&
                    player.PawnsThatCanMove.Contains(chosenPawn)
                    &&
                    !jumpOverIsObligatory)
                    return chosenPawn;
                else
                    Console.WriteLine("Niepoprawny pionek, lub ruch pionkiem jest nie możliwy.");
            }
            while (true);
        }

        public void PlayerChoosesField(Pawn chosenPawn, Player player, Player cpu, bool jumpOverIsObligatory)
        {
            bool incorrectChoice = true;

            do
            {
                Console.Write($"Na które pole przestawić pionka {chosenPawn.Name}?: ");
                string playerChoice = Console.ReadLine().ToUpper();
                if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice) && chosenPawn.FieldsToMove.Contains(board.fields[board.fieldsOnBoardDictionary[playerChoice]]))
                {
                    int chosenField = board.fieldsOnBoardDictionary[playerChoice];

                    if (jumpOverIsObligatory)
                    {
                        int jumpDirection=0;
                        int checkJumpDirection = chosenPawn.CurrentPosition - chosenField;
                        if (checkJumpDirection % 7 == 0)
                            jumpDirection = 7;
                        else if (checkJumpDirection % 9 == 0)
                            jumpDirection = 9;

                        Pawn jumpedPawn = null;
                        if (chosenPawn.IsKing)
                        {
                            for (int i = 1; i < Math.Abs(checkJumpDirection) / jumpDirection; i++)
                            {
                                jumpedPawn = chosenPawn.PawnsToJumpOver.FirstOrDefault(pawn => pawn.CurrentPosition == chosenField + (Math.Abs(checkJumpDirection) / (checkJumpDirection / jumpDirection)) * i);
                                if (jumpedPawn != null)
                                    break;
                            }
                        }
                        else
                            jumpedPawn = chosenPawn.PawnsToJumpOver.FirstOrDefault(pawn => pawn.CurrentPosition == chosenField + (Math.Abs(checkJumpDirection) / (checkJumpDirection / jumpDirection)));
                        
                            LeaveField(chosenPawn);
                            PlayerJumpsOver(chosenPawn, jumpedPawn, chosenField, player, cpu);
                            IsPawnKing(player, chosenPawn);
                            incorrectChoice = false;
                    }
                    else if (player.PawnsThatCanMove.Count != 0)
                    {
                        LeaveField(chosenPawn);
                        TakeField(chosenPawn, chosenField, player);
                        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
                        incorrectChoice = false;
                    }
                    else
                        Console.WriteLine($"Nie można przestawć pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenField)}.");
                }
                else
                    Console.WriteLine("Błędne pole.");
            }
            while (incorrectChoice);
        }

        private void PlayerJumpsOver(Pawn chosenPawn, Pawn jumpedPawn, int chosenField, Player player, Player cpu)
        {
            LeaveField(jumpedPawn);
            cpu.pawns.Remove(jumpedPawn);
            TakeField(chosenPawn, chosenField, player);
            Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
            DrawBoard();
            
            if (CheckIfChosenPawnCanJumpOverAgain(chosenPawn, player, cpu).Count != 0)
                PlayerChoosesField(chosenPawn, player, cpu, true);
        }
        public void CpuChoosesPawn(Player cpu, Player player)
        {
            if (cpu.PawnsThatCanJumpOver.Count != 0)
            {
                Random randomCpuPawn = new Random();
                int cpuPawnNumber = randomCpuPawn.Next(0, cpu.PawnsThatCanJumpOver.Count);
                Pawn chosenPawn = cpu.PawnsThatCanJumpOver[cpuPawnNumber];
                CpuJumpsOver(cpu, player, chosenPawn);
                IsPawnKing(cpu, chosenPawn);
            }
            else if (cpu.PawnsThatCanMove.Count != 0)
            {
                Random randomCpuPawn = new Random();
                int cpuPawnNumber = randomCpuPawn.Next(0, cpu.PawnsThatCanMove.Count);
                Pawn chosenPawn = cpu.PawnsThatCanMove[cpuPawnNumber];
                CpuMovesPawn(chosenPawn, cpu, player);
                IsPawnKing(cpu, chosenPawn);
            }
        }

        private void CpuMovesPawn(Pawn chosenPawn, Player cpu, Player player)
        {
            Random random = new Random();
            int cpuPawnToMove = random.Next(0, chosenPawn.FieldsToMove.Count);
            int chosenField = chosenPawn.FieldsToMove[cpuPawnToMove].Number;
            LeaveField(chosenPawn);
            TakeField(chosenPawn, chosenField, cpu);
            Console.WriteLine($"CPU przestawia pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
            IsPawnKing(cpu, chosenPawn);
        }

        private void CpuJumpsOver(Player cpu, Player player, Pawn chosenPawn)
        {
            int jumpDirection = 0;
            Random random = new Random();
            int playerPawnToJumpOver = random.Next(0, chosenPawn.PawnsToJumpOver.Count);
            Pawn jumpedPawn = chosenPawn.PawnsToJumpOver[playerPawnToJumpOver];
            LeaveField(jumpedPawn);
            LeaveField(chosenPawn);
            if ((jumpedPawn.CurrentPosition - chosenPawn.CurrentPosition) % 7 == 0)
                jumpDirection = 7;
            else if ((jumpedPawn.CurrentPosition - chosenPawn.CurrentPosition) % 9 == 0)
                jumpDirection = 9;
            int chosenField = jumpedPawn.CurrentPosition +
                ((jumpedPawn.CurrentPosition - chosenPawn.CurrentPosition) / (Math.Abs(jumpedPawn.CurrentPosition - chosenPawn.CurrentPosition) / jumpDirection)); ;
            chosenPawn.CurrentPosition = chosenField;
            player.pawns.Remove(jumpedPawn);
            TakeField(chosenPawn, chosenField, cpu);
            Console.WriteLine($"CPU przestawia pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
            if (CheckIfChosenPawnCanJumpOverAgain(chosenPawn, cpu, player).Count != 0)
            {
                DrawBoard();
                CpuJumpsOver(cpu, player, chosenPawn);
            }
        }

        private void LeaveField(Pawn chosenPawn)
        {
            board.fields[chosenPawn.CurrentPosition].Content = " ";
            board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
            board.fields[chosenPawn.CurrentPosition].OccupiedBy = null;
        }

        private void TakeField(Pawn chosenPawn, int chosenField, Player player)
        {
            chosenPawn.CurrentPosition = board.fields[chosenField].Number;
            board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
            board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
            board.fields[chosenPawn.CurrentPosition].OccupiedBy = player.Name;
        }

        private void IsPawnKing(Player turnPlayer, Pawn chosenPawn)
        {
            if (turnPlayer.IsCpu)
            {
                if (55 < chosenPawn.CurrentPosition && chosenPawn.CurrentPosition < 63)
                {
                    chosenPawn.Name = chosenPawn.Name.ToUpper();
                    chosenPawn.IsKing = true;
                    board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
                }
            }
            else
            {
                if (0 < chosenPawn.CurrentPosition && chosenPawn.CurrentPosition < 8)
                {
                    chosenPawn.Name = chosenPawn.Name.ToUpper();
                    chosenPawn.IsKing = true;
                    board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
                }
            }
        }

        public List<Pawn> CheckIfChosenPawnCanJumpOverAgain(Pawn chosenPawn, Player turnPlayer, Player opponent)
        {
            chosenPawn.PawnsToJumpOver = new List<Pawn>();
            chosenPawn.FieldsToMove = new List<FieldOnBoard>();

            if (chosenPawn.IsKing)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 1; j < 7; j++)
                    {
                        try
                        {
                            if (board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * j].IsBlack == false
                                ||
                                (board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * j].OccupiedBy == opponent.Name
                                && board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * (j + 1)].OccupiedBy == opponent.Name)
                                ||
                                (board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * j].OccupiedBy == turnPlayer.Name))
                                break;
                            else if (board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * j].OccupiedBy == opponent.Name)
                            {
                                Pawn opponentPawn = opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * j);
                                try
                                {
                                    for (int k = 1; k < 7; k++)
                                    {
                                        if (board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * j + board.crossCheckDictionary[i]*k].IsBlack
                                        &&
                                        board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * j + board.crossCheckDictionary[i]*k].IsEmpty)
                                        {
                                            chosenPawn.FieldsToMove.Add(board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * j + board.crossCheckDictionary[i]*k]);
                                            if (!chosenPawn.PawnsToJumpOver.Contains(opponentPawn))
                                                chosenPawn.PawnsToJumpOver.Add(opponentPawn);
                                            if (!turnPlayer.PawnsThatCanJumpOver.Contains(chosenPawn))
                                                turnPlayer.PawnsThatCanJumpOver.Add(chosenPawn);
                                        }
                                    }
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    break;
                                }
                                chosenPawn.PawnsToJumpOver.Add(opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * j));
                                turnPlayer.PawnsThatCanJumpOver.Add(chosenPawn);
                                break;
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        if (board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * 2].IsBlack
                            && board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * 2].IsEmpty
                            && board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i]].OccupiedBy == opponent.Name)
                        {
                            chosenPawn.FieldsToMove.Add(board.fields[chosenPawn.CurrentPosition + board.crossCheckDictionary[i] * 2]);
                            chosenPawn.PawnsToJumpOver.Add(opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == chosenPawn.CurrentPosition + board.crossCheckDictionary[i]));
                        }
                            
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        continue;
                    }
                }
                if (chosenPawn.PawnsToJumpOver.Count != 0)
                    turnPlayer.PawnsThatCanJumpOver.Add(chosenPawn);
            }
            return chosenPawn.PawnsToJumpOver;
        }
    }
}












