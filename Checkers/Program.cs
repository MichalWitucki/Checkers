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
            //Console.Write("Podaj swoje imię: ");
            Player player = new Player("Gracz",false);
            Player cpu = new Player("CPU", true);
            Console.WriteLine("Pionki gracza oznaczone są małymi literami od a do l.");
            game.SpacingPawns(player, cpu);
            do 
            {
                bool playerJumpOverIsObligatory = false;
                game.DrawBoard();

                game.CheckIfPawnCanJumpOver(player, cpu);
                if (player.PawnsThatCanJumpOver.Count != 0)
                    playerJumpOverIsObligatory = true;
                else
                    game.CheckIfPlayerPawnCanMove(player);
                if (player.PawnsThatCanMove.Count == 0)
                {
                    Console.WriteLine("Wygrał CPU. Player nie ma możliwości ruchu.");
                    break;
                }
                Pawn playerPawn = game.PlayerChoosesPawn(player, playerJumpOverIsObligatory);
                game.PlayerChoosesField(playerPawn, player, cpu, playerJumpOverIsObligatory);
                if (cpu.pawns.Count == 0)
                {
                    Console.WriteLine("Wygrał Player. CPU nie ma pionków.");
                    break;

                }
                //game.DrawBoard();

                game.CheckIfPawnCanJumpOver(cpu, player);
                if (cpu.PawnsThatCanJumpOver.Count == 0)
                    game.CheckIfCpuPawnCanMove(cpu);
                if (cpu.PawnsThatCanMove.Count == 0)
                {
                    Console.WriteLine("Wygrał Player. CPU nie ma możliwości ruchu.");
                    break;
                }
                game.CpuChoosesPawn(cpu, player);
                if (player.pawns.Count == 0)
                {
                    Console.WriteLine("Wygrał CPU. Player nie ma pionków");
                    break;
                }
            }
            while (true);
            
        }
    }
}