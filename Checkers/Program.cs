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
            Player player = new Player("gracz",false);
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
                Pawn playerPawn = game.PlayerChoosesPawn(player, playerJumpOverIsObligatory);
                game.PlayerChoosesField(playerPawn, player, cpu, playerJumpOverIsObligatory);

                game.DrawBoard();

                game.CheckIfPawnCanJumpOver(cpu, player);
                if (cpu.PawnsThatCanJumpOver.Count == 0)
                    game.CheckIfCpuPawnCanMove(cpu);
                game.CpuChoosesPawn(cpu, player);

            }
            while (player.pawns.Count > 0 || cpu.pawns.Count > 0 || player.PawnsThatCanMove.Count > 0 || cpu.PawnsThatCanMove.Count > 0) ;
                      
            
        }
    }
}