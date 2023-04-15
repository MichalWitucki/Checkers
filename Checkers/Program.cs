using static Checkers.Gameplay;

namespace Checkers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GRA - WARCABY.");
            Console.Write("Podaj swoje imię: ");
            Player player = new Player("player");
            Player cpu = new Player("CPU");
            Console.WriteLine($"Twój przeciwnik to komputer o imieniu {cpu.Name}");
            switch (DrawWhoStarts())
            {
                case 0:
                    SpacingPawns(player, cpu);
                    break;
                case 1:
                    SpacingPawns(cpu, player);
                    break;
            }
                    
            DrawBoard();
            //Turn(firstPlayer);
        }
    }
}