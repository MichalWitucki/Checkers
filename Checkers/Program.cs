using System.ComponentModel;
using System.Drawing;
using static Checkers.Gameplay;

namespace Checkers
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GRA - WARCABY.");
            Gameplay game = new Gameplay();
            Player player = new Player("GRACZ", false);
            Player cpu = new Player("CPU", true);
            Console.WriteLine("Pionki gracza oznaczone są małymi literami od a do l. Damki są oznaczone dużymi literami.");
            game.SpacingPawns(player, cpu);
            do 
            {
                bool playerJumpOverIsObligatory = false;
                game.DrawBoard();
                game.CheckIfPawnCanJumpOver(player, cpu);
                if (player.PawnsThatCanJumpOver.Count != 0)
                    playerJumpOverIsObligatory = true;
                else
                {
                    game.CheckIfPawnCanMove(player);
                    if (player.PawnsThatCanMove.Count == 0)
                    {
                        Console.WriteLine($"Wygrał CPU. {player.Name} nie ma możliwości ruchu.");
                        break;
                    }
                }
                Pawn playerPawn = game.PlayerChoosesPawn(player, playerJumpOverIsObligatory);
                game.PlayerChoosesField(playerPawn, player, cpu, playerJumpOverIsObligatory);
                if (cpu.pawns.Count == 0)
                {
                    Console.WriteLine($"Wygrał {player.Name}. CPU nie ma pionków.");
                    break;
                }

                game.CheckIfPawnCanJumpOver(cpu, player);
                if (cpu.PawnsThatCanJumpOver.Count == 0)
                {
                    game.CheckIfPawnCanMove(cpu);
                    if (cpu.PawnsThatCanMove.Count == 0)
                    {
                        Console.WriteLine($"Wygrał {player.Name}. CPU nie ma możliwości ruchu.");
                        break;
                    }
                } 
                game.CpuChoosesPawn(cpu, player);
                if (player.pawns.Count == 0)
                {
                    Console.WriteLine($"Wygrał CPU. {player.Name} nie ma pionków");
                    break;
                }
            }
            while (true);
        }
    }
}