using System.Drawing;
using static Checkers.Gameplay;

namespace Checkers
{
    public class Program
    {
        static void Main(string[] args)
        {
            Color.FromKnownColor(KnownColor.White);
            Console.WriteLine("GRA - WARCABY.");
            Gameplay game = new Gameplay();
            //Console.Write("Podaj swoje imię: ");
            Player player = new Player("gracz",false);
            Player cpu = new Player("CPU", true);
            Console.WriteLine("Pionki gracza oznaczone są małymi literami od a do l.");
            game.SpacingPawns(player, cpu);
            do
            {
                game.DrawBoard();

                Pawn chosenPawn = game.PlayerChoosesPawn(player);
                game.PlayerChoosesField(chosenPawn, cpu);

                game.DrawBoard();
                game.CpuTurn(cpu, player);
            }
            while (true);
            while (player.pawns.Count > 0 || cpu.pawns.Count > 0 || player.pawnsThatCanMove.Count > 0 || cpu.pawnsThatCanMove.Count > 0) ;
                      
            
        }
    }
}