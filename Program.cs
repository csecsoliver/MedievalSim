using System.ComponentModel.Design;

namespace MedievalSim;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello !");
        Console.WriteLine("Welcome to Medieval Simulator !");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Title:
        Console.Clear();
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Start a new game");
        Console.WriteLine("2. Exit");
        ConsoleKeyInfo choice = Console.ReadKey();
        
        switch (choice.KeyChar)
        {
            case '1':
                Console.Clear();
                
                int seed = Misc.NonNullInput("Enter a seed to use or leave empty for a random one:").GetHashCode();
                string kingdomName = Misc.NonNullInput("Enter your kingdom's name:");
                string kingName = Misc.NonNullInput("Enter your king's name:");
                Console.WriteLine("Starting a new game...");
                Game thegame = new Game(seed, kingdomName, kingName);
                Thread.Sleep(1000);
                
                Console.Clear();
                Console.WriteLine($"Hello {thegame.Kingdoms[0].King.Name}!");
                Console.WriteLine($"Your kingdom, {thegame.Kingdoms[0].Name}, is in danger. Not all emerging kingdoms nearby are friendly towards you it seem.");
                Console.WriteLine("You need to manage yours well to to survive!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                while (true)
                {
                    thegame.GameLoop();
                }
                
                break;
            case '2':
                Console.WriteLine("Exiting...");
                Thread.Sleep(1000);
                return;
            default:
                goto Title;
        }
        
        
    }
}