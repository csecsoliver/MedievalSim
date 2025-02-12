namespace MedievalSim;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello !");
        Console.WriteLine("Welcome to Medieval Simulator !");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Start a new game");
        Console.WriteLine("2. Load a saved game");
        Console.WriteLine("3. Options");
        Console.WriteLine("4. Exit");
        ConsoleKeyInfo choice = Console.ReadKey();
        switch (choice.KeyChar)
        {
            case '1':
                Console.WriteLine("Starting a new game...");
                Thread.Sleep(1000);

                break;
            case '2':
                Console.WriteLine("Loading saved games...");
                Thread.Sleep(1000);

                break;
        }
    }
}