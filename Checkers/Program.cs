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
            Console.WriteLine("Pionki gracza oznaczone są dużymi literami od A do L, \npionki komputera oznaczone są małymi literami od a do l.");
            game.SpacingPawns(player, cpu);
            do
            {
                game.DrawBoard();
                game.PlayerTurn(player, cpu);
                game.DrawBoard();
                game.CpuTurn(cpu, player);
            }
            while (true);
            
                      
            
        }
    }
}