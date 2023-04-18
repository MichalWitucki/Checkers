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
            
            Player player = new Player("player",false);
            Player cpu = new Player("CPU", true);
            Console.WriteLine($"Twój przeciwnik to komputer {cpu.Name}. Pionki gracza oznaczone są dużymi literami od A do L, \npionki komputera oznaczone są małymi literami od a do l.");
            // zaimplementować zasady
            int whoStarts = DrawWhoStarts();
            game.SpacingPawns(player, cpu);
            switch (whoStarts)
            {
                case 0:
                    game.DrawBoard();
                    game.PlayerTurn(player);
                    game.CpuTurn(cpu);
                    break;
                case 1:
                    game.DrawBoard();
                    game.CpuTurn(cpu);
                    game.PlayerTurn(player);
                    break;
            }
            
            
        }
    }
}