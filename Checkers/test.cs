//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;

//namespace Checkers
//{
//    internal class test
//    {
//        public Board board = new Board();
//        public void DrawBoard()
//        {
//            Console.Write($"  |");
//            foreach (char c in "ABCDEFG")
//                Console.Write($"{c}|");
//            Console.WriteLine("H|");
//            for (int i = 0; i < 8; i++)
//            {
//                Console.Write($"{i + 1} |");
//                for (int j = 0; j < 8; j++)
//                    Console.Write($"{board.fields[j + i * 8].Content}|");
//                Console.WriteLine();
//            }
//        }

//        public void SpacingPawns(Player player, Player cpu)
//        {
//            int playerPawn = 0;
//            int cpuPawn = 0;

//            for (int y = 0; y < 3; y++)
//                for (int x = 0; x < 8; x++)
//                    if (board.fields[x + y * 8].IsBlack)
//                    {
//                        board.fields[x + y * 8].Content = cpu.pawns[cpuPawn].Name;
//                        board.fields[x + y * 8].IsEmpty = false;
//                        board.fields[x + y * 8].OccupiedBy = cpu.Name;
//                        cpu.pawns[cpuPawn].CurrentPosition = x + y * 8;
//                        cpuPawn++;
//                    }
//            for (int y = 5; y < 8; y++)
//                for (int x = 0; x < 8; x++)
//                    if (board.fields[x + y * 8].IsBlack)
//                    {
//                        board.fields[x + y * 8].Content = player.pawns[playerPawn].Name;
//                        board.fields[x + y * 8].IsEmpty = false;
//                        board.fields[x + y * 8].OccupiedBy = player.Name;
//                        player.pawns[playerPawn].CurrentPosition = x + y * 8;
//                        playerPawn++;
//                    }
//        }

//        public void CheckIfPawnCanJumpOver(Player turnPlayer, Player opponent)
//        {
//            turnPlayer.PawnsThatCanJumpOver = new List<Pawn>();

//            foreach (var pawn in turnPlayer.pawns)
//            {
//                pawn.PawnsToJumpOver = new List<Pawn>();
//                if (pawn.IsKing)
//                {
//                    for (int i = 0; i < 4; i++)
//                    {
//                        for (int j = 1; j < 7; j++)
//                        {
//                            try
//                            {
//                                if (board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * j].IsBlack == false
//                                    ||
//                                    (board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * j].OccupiedBy == opponent.Name
//                                    && board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * (j + 1)].OccupiedBy == opponent.Name))
//                                    break;
//                                else if (board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * j].OccupiedBy == opponent.Name
//                                &&
//                                board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * j + board.CrossCheckDictionary[i]].IsBlack
//                                &&
//                                board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * j + board.CrossCheckDictionary[i]].IsEmpty)
//                                {
//                                    pawn.PawnsToJumpOver.Add(opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == pawn.CurrentPosition + board.CrossCheckDictionary[i] * j));
//                                    turnPlayer.PawnsThatCanJumpOver.Add(pawn);
//                                    break;
//                                }
//                            }
//                            catch (ArgumentOutOfRangeException)
//                            {
//                                break;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    for (int i = 0; i < 4; i++)
//                    {
//                        try
//                        {
//                            if (board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsBlack
//                                &&
//                                board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsEmpty
//                                &&
//                                board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i]].OccupiedBy == opponent.Name)
//                                pawn.PawnsToJumpOver.Add(opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == pawn.CurrentPosition + board.CrossCheckDictionary[i]));
//                        }
//                        catch (ArgumentOutOfRangeException)
//                        {
//                            continue;
//                        }
//                    }
//                    if (pawn.PawnsToJumpOver.Count != 0)
//                        turnPlayer.PawnsThatCanJumpOver.Add(pawn);
//                }
//            }
//        }

//        public void CheckIfPlayerPawnCanMove(Player player)
//        {
//            player.PawnsThatCanMove = new List<Pawn>();
//            foreach (var pawn in player.pawns)
//                if (pawn.IsKing)
//                {
//                    for (int i = 0; i < 4; i++)
//                    {
//                        try
//                        {
//                            if (board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i]].IsBlack
//                            &&
//                            board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i]].IsEmpty)
//                            {
//                                player.PawnsThatCanMove.Add(pawn);
//                                break;
//                            }
//                        }
//                        catch (ArgumentOutOfRangeException)
//                        {
//                            continue;
//                        }
//                    }
//                }
//                else
//                {
//                    try
//                    {
//                        if ((board.fields[pawn.CurrentPosition - 7].IsBlack && board.fields[pawn.CurrentPosition - 7].IsEmpty)
//                            ||
//                            (board.fields[pawn.CurrentPosition - 9].IsBlack && board.fields[pawn.CurrentPosition - 9].IsEmpty))
//                            player.PawnsThatCanMove.Add(pawn);
//                    }
//                    catch (ArgumentOutOfRangeException)
//                    {
//                        continue;
//                    }
//                }
//        }

//        public Pawn PlayerChoosesPawn(Player player, bool jumpOverIsObligatory)
//        {
//            do
//            {
//                Console.Write($"{player.Name} wybiera pionka, którym chce ruszyć: ");
//                string playerChoice = Console.ReadLine();
//                var chosenPawn = player.pawns.FirstOrDefault(pawn => pawn.Name == playerChoice);
//                if (chosenPawn != null
//                    &&
//                    player.PawnsThatCanJumpOver.Count != 0
//                    &&
//                    player.PawnsThatCanJumpOver.Contains(chosenPawn))
//                    return chosenPawn;
//                else if (chosenPawn != null
//                    &&
//                    player.PawnsThatCanMove.Count != 0
//                    &&
//                    player.PawnsThatCanMove.Contains(chosenPawn)
//                    &&
//                    !jumpOverIsObligatory)
//                    return chosenPawn;
//                else
//                    Console.WriteLine("Niepoprawny pionek, lub ruch pionkiem jest nie możliwy.");
//            }
//            while (true);
//        }

//        public void PlayerChoosesField(Pawn chosenPawn, Player player, Player cpu, bool jumpOverIsObligatory)
//        {
//            bool incorrectChoice = true;

//            do
//            {
//                Console.Write($"Na które pole przestawić pionka {chosenPawn.Name}?: ");
//                string playerChoice = Console.ReadLine().ToUpper();
//                if (board.fieldsOnBoardDictionary.ContainsKey(playerChoice))
//                {
//                    int chosenField = board.fieldsOnBoardDictionary[playerChoice];

//                    if (jumpOverIsObligatory)
//                    {
//                        int jumpDirection;
//                        int checkJumpDirection = chosenPawn.CurrentPosition - chosenField;
//                        if (checkJumpDirection % 7 == 0)
//                            jumpDirection = 7;
//                        else if (checkJumpDirection % 9 == 0)
//                            jumpDirection = 9;
//                        else
//                        {
//                            Console.WriteLine($"Nie można przestawć pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenField)}.");
//                            continue;
//                        }
//                        Pawn jumpedPawn = null;
//                        if (chosenPawn.IsKing)
//                        {
//                            for (int i = 1; i < Math.Abs(checkJumpDirection) / jumpDirection; i++)
//                            {
//                                jumpedPawn = chosenPawn.PawnsToJumpOver.FirstOrDefault(pawn => pawn.CurrentPosition == chosenField + (Math.Abs(checkJumpDirection) / (checkJumpDirection / jumpDirection)) * i);
//                                if (jumpedPawn != null)
//                                    break;
//                            }
//                        }
//                        else
//                            jumpedPawn = chosenPawn.PawnsToJumpOver.FirstOrDefault(pawn => pawn.CurrentPosition == chosenField + (Math.Abs(checkJumpDirection) / (checkJumpDirection / jumpDirection)));
//                        if (board.fields[chosenField].IsEmpty
//                            &&
//                            board.fields[chosenField].IsBlack
//                            &&
//                            jumpedPawn != null)
//                        {
//                            LeaveField(chosenPawn);
//                            PlayerJumpsOver(chosenPawn, jumpedPawn, chosenField, player, cpu);
//                            IsPawnKing(player, chosenPawn);
//                            incorrectChoice = false;
//                        }
//                        else
//                            Console.WriteLine($"Nie można przestawć pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenField)}.");
//                    }
//                    else if (player.PawnsThatCanMove.Count != 0
//                            &&
//                            jumpOverIsObligatory == false
//                            &&
//                            chosenPawn.IsKing
//                            &&
//                            board.fields[chosenField].IsEmpty
//                            &&
//                            board.fields[chosenField].IsBlack
//                            &&
//                            ((board.fields[chosenField].Number - chosenPawn.CurrentPosition) % 7 == 0 || (board.fields[chosenField].Number - chosenPawn.CurrentPosition) % 9 == 0))
//                    {
//                        LeaveField(chosenPawn);
//                        PlayerTakesField(chosenPawn, chosenField, player);
//                        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
//                        incorrectChoice = false;
//                    }
//                    else if (player.PawnsThatCanMove.Count != 0
//                            &&
//                            jumpOverIsObligatory == false
//                            &&
//                            ((board.fields[chosenField].IsEmpty
//                            && board.fields[chosenField].IsBlack
//                            && board.fields[chosenField].Number == chosenPawn.CurrentPosition - 7)
//                            ||
//                            (board.fields[chosenField].IsEmpty
//                            && board.fields[chosenField].IsBlack
//                            && board.fields[chosenField].Number == chosenPawn.CurrentPosition - 9)))
//                    {
//                        LeaveField(chosenPawn);
//                        PlayerTakesField(chosenPawn, chosenField, player);
//                        Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
//                        IsPawnKing(player, chosenPawn);
//                        incorrectChoice = false;
//                    }
//                    else
//                        Console.WriteLine($"Nie można przestawć pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenField)}.");
//                }
//                else
//                    Console.WriteLine("Błędne pole.");
//            }
//            while (incorrectChoice);
//        }

//        private void PlayerJumpsOver(Pawn chosenPawn, Pawn jumpedPawn, int chosenField, Player player, Player cpu)
//        {
//            LeaveField(jumpedPawn);
//            cpu.pawns.Remove(jumpedPawn);
//            PlayerTakesField(chosenPawn, chosenField, player);
//            Console.WriteLine($"Przestawiono pionka {chosenPawn.Name} na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
//            DrawBoard();
//            if (CheckIfChosenPawnCanJumpOverAgain(chosenPawn, player, cpu).Count != 0)
//                PlayerChoosesField(chosenPawn, player, cpu, true);
//        }

//        public void CheckIfCpuPawnCanMove(Player cpu)
//        {
//            cpu.PawnsThatCanMove = new List<Pawn>();
//            foreach (var pawn in cpu.pawns)
//                if (pawn.IsKing)
//                {
//                    for (int i = 0; i < 4; i++)
//                    {
//                        try
//                        {
//                            if (board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i]].IsBlack
//                            &&
//                            board.fields[pawn.CurrentPosition + board.CrossCheckDictionary[i]].IsEmpty)
//                            {
//                                cpu.PawnsThatCanMove.Add(pawn);
//                                break;
//                            }
//                        }
//                        catch (ArgumentOutOfRangeException)
//                        {
//                            continue;
//                        }
//                    }
//                }
//                else
//                {
//                    try
//                    {
//                        if ((board.fields[pawn.CurrentPosition + 7].IsBlack && board.fields[pawn.CurrentPosition + 7].IsEmpty)
//                            ||
//                            (board.fields[pawn.CurrentPosition + 9].IsBlack && board.fields[pawn.CurrentPosition + 9].IsEmpty))
//                            cpu.PawnsThatCanMove.Add(pawn);
//                    }
//                    catch (ArgumentOutOfRangeException)
//                    {
//                        continue;
//                    }
//                }
//        }

//        public void CpuChoosesPawn(Player cpu, Player player)
//        {
//            if (cpu.PawnsThatCanJumpOver.Count != 0)
//            {
//                Random randomCpuPawn = new Random();
//                int cpuPawnNumber = randomCpuPawn.Next(0, cpu.PawnsThatCanJumpOver.Count);
//                Pawn chosenPawn = cpu.PawnsThatCanJumpOver[cpuPawnNumber];
//                CpuJumpsOver(cpu, player, chosenPawn);
//                IsPawnKing(cpu, chosenPawn);
//            }
//            else
//            {
//                Random randomCpuPawn = new Random();
//                int cpuPawnNumber = randomCpuPawn.Next(0, cpu.PawnsThatCanMove.Count);
//                Pawn chosenPawn = cpu.PawnsThatCanMove[cpuPawnNumber];
//                CpuMovesPawn(chosenPawn, cpu, player);
//                IsPawnKing(cpu, chosenPawn);
//            }
//        }

//        private void CpuMovesPawn(Pawn chosenPawn, Player cpu, Player player)
//        {
//            if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
//                &&
//                (board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
//            {
//                Random randomCpuMove = new Random();
//                int cpuPawnMove = randomCpuMove.Next(0, 2);
//                if (cpuPawnMove == 0)
//                {
//                    LeaveField(chosenPawn);
//                    chosenPawn.CurrentPosition += 7;
//                    CpuTakesField(chosenPawn, cpu);
//                    IsPawnKing(cpu, chosenPawn);
//                }
//                else
//                {
//                    LeaveField(chosenPawn);
//                    chosenPawn.CurrentPosition += 9;
//                    CpuTakesField(chosenPawn, cpu);
//                    IsPawnKing(cpu, chosenPawn);
//                }
//            }
//            else if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
//                    && (!board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
//            {
//                LeaveField(chosenPawn);
//                chosenPawn.CurrentPosition += 7;
//                CpuTakesField(chosenPawn, cpu);
//                IsPawnKing(cpu, chosenPawn);
//            }
//            else if ((board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
//                    && (!board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack))
//            {
//                LeaveField(chosenPawn);
//                chosenPawn.CurrentPosition += 9;
//                CpuTakesField(chosenPawn, cpu);
//                IsPawnKing(cpu, chosenPawn);
//            }
//            else if ((board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && !board.fields[chosenPawn.CurrentPosition + 7].IsBlack)
//                    && (board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && board.fields[chosenPawn.CurrentPosition + 9].IsBlack))
//            {
//                LeaveField(chosenPawn);
//                chosenPawn.CurrentPosition += 9;
//                CpuTakesField(chosenPawn, cpu);
//                IsPawnKing(cpu, chosenPawn);
//            }
//            else if ((board.fields[chosenPawn.CurrentPosition + 9].IsEmpty && !board.fields[chosenPawn.CurrentPosition + 9].IsBlack)
//                && (board.fields[chosenPawn.CurrentPosition + 7].IsEmpty && board.fields[chosenPawn.CurrentPosition + 7].IsBlack))
//            {
//                LeaveField(chosenPawn);
//                chosenPawn.CurrentPosition += 7;
//                CpuTakesField(chosenPawn, cpu);
//                IsPawnKing(cpu, chosenPawn);
//            }
//        }

//        private void CpuJumpsOver(Player cpu, Player player, Pawn chosenPawn)
//        {
//            int jumpDirection = 0;
//            Random random = new Random();
//            int playerPawnNumber = random.Next(0, chosenPawn.PawnsToJumpOver.Count);
//            Pawn jumpedPawn = chosenPawn.PawnsToJumpOver[playerPawnNumber];
//            board.fields[jumpedPawn.CurrentPosition].Content = " ";
//            board.fields[jumpedPawn.CurrentPosition].IsEmpty = true;
//            board.fields[jumpedPawn.CurrentPosition].OccupiedBy = null;
//            LeaveField(chosenPawn);
//            if ((jumpedPawn.CurrentPosition - chosenPawn.CurrentPosition) % 7 == 0)
//                jumpDirection = 7;
//            else if ((jumpedPawn.CurrentPosition - chosenPawn.CurrentPosition) % 9 == 0)
//                jumpDirection = 9;
//            chosenPawn.CurrentPosition = jumpedPawn.CurrentPosition +
//                ((jumpedPawn.CurrentPosition - chosenPawn.CurrentPosition) / (Math.Abs(jumpedPawn.CurrentPosition - chosenPawn.CurrentPosition) / jumpDirection));
//            player.pawns.Remove(jumpedPawn);
//            CpuTakesField(chosenPawn, cpu);
//            if (CheckIfChosenPawnCanJumpOverAgain(chosenPawn, cpu, player).Count != 0)
//            {
//                DrawBoard();
//                CpuJumpsOver(cpu, player, chosenPawn);
//            }
//        }

//        private void LeaveField(Pawn chosenPawn)
//        {
//            board.fields[chosenPawn.CurrentPosition].Content = " ";
//            board.fields[chosenPawn.CurrentPosition].IsEmpty = true;
//            board.fields[chosenPawn.CurrentPosition].OccupiedBy = null;
//        }

//        private void PlayerTakesField(Pawn chosenPawn, int chosenField, Player player)
//        {
//            chosenPawn.CurrentPosition = board.fields[chosenField].Number;
//            board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
//            board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
//            board.fields[chosenPawn.CurrentPosition].OccupiedBy = player.Name;
//        }

//        private void CpuTakesField(Pawn chosenPawn, Player cpu)
//        {
//            board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
//            board.fields[chosenPawn.CurrentPosition].IsEmpty = false;
//            board.fields[chosenPawn.CurrentPosition].OccupiedBy = cpu.Name;
//            Console.WriteLine($"CPU Przestawia pionka na pole {board.fieldsOnBoardDictionary.ElementAt(chosenPawn.CurrentPosition)}.");
//        }

//        private void IsPawnKing(Player turnPlayer, Pawn chosenPawn)
//        {
//            if (turnPlayer.IsCpu)
//            {
//                if (55 < chosenPawn.CurrentPosition && chosenPawn.CurrentPosition < 63)
//                {
//                    chosenPawn.Name = chosenPawn.Name.ToUpper();
//                    chosenPawn.IsKing = true;
//                    board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
//                }
//            }
//            else
//            {
//                if (0 < chosenPawn.CurrentPosition && chosenPawn.CurrentPosition < 8)
//                {
//                    chosenPawn.Name = chosenPawn.Name.ToUpper();
//                    chosenPawn.IsKing = true;
//                    board.fields[chosenPawn.CurrentPosition].Content = chosenPawn.Name;
//                }
//            }
//        }

//        public List<Pawn> CheckIfChosenPawnCanJumpOverAgain(Pawn chosenPawn, Player turnPlayer, Player opponent)
//        {
//            chosenPawn.PawnsToJumpOver = new List<Pawn>();

//            if (chosenPawn.IsKing)
//            {
//                for (int i = 0; i < 4; i++)
//                {
//                    for (int j = 1; j < 7; j++)
//                    {
//                        try
//                        {
//                            if (board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * j].IsBlack == false)
//                                break;
//                            else if (board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * j].OccupiedBy == opponent.Name
//                            &&
//                            board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * j + board.CrossCheckDictionary[i]].IsBlack
//                            &&
//                            board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * j + board.CrossCheckDictionary[i]].IsEmpty)
//                            {
//                                chosenPawn.PawnsToJumpOver.Add(opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * j));
//                                turnPlayer.PawnsThatCanJumpOver.Add(chosenPawn);
//                                break;
//                            }
//                        }
//                        catch (ArgumentOutOfRangeException)
//                        {
//                            break;
//                        }
//                    }
//                }
//            }
//            else
//            {
//                for (int i = 0; i < 4; i++)
//                {
//                    try
//                    {
//                        if (board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsBlack
//                            && board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i] * 2].IsEmpty
//                            && board.fields[chosenPawn.CurrentPosition + board.CrossCheckDictionary[i]].OccupiedBy == opponent.Name)
//                            chosenPawn.PawnsToJumpOver.Add(opponent.pawns.FirstOrDefault(opponentPawn => opponentPawn.CurrentPosition == chosenPawn.CurrentPosition + board.CrossCheckDictionary[i]));
//                    }
//                    catch (ArgumentOutOfRangeException)
//                    {
//                        continue;
//                    }
//                }
//                if (chosenPawn.PawnsToJumpOver.Count != 0)
//                    turnPlayer.PawnsThatCanJumpOver.Add(chosenPawn);
//            }

//            return chosenPawn.PawnsToJumpOver;
//        }
//    }
//}

//    }
//}
