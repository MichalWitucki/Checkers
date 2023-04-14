using static Checkers.Gameplay;

namespace Checkers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GRA - WARCABY.");
            Console.Write("Podaj swoje imię: ");
            Player player = new Player(Console.ReadLine());
            Player cpu = new Player("CPU");
            Console.WriteLine($"Twój przeciwnik to komputer o imieniu {cpu.Name}");
            DrawBoard();
            Turn(DrawWhoStarts(player, cpu));
        }
    }
}