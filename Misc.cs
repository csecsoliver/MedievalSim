namespace MedievalSim;

public static class Misc
{
    public static string NonNullInput(string prompt)
    {
        Console.WriteLine(prompt);
        string? input = Console.ReadLine();
        if (input == null)
        {
            return "";
        }

        return input;
    }

    public static int IntInput(string prompt)
    {
        string input = NonNullInput(prompt);
        while (true)
        {
            try
            {
                return int.Parse(input);
            }
            catch (Exception)
            {
                return IntInput(prompt); 

            }

        }

    }

    public static int Menu(List<string> options, string title, string prompt)
    {
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine(prompt);
            Console.WriteLine("***************************************");
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            Console.WriteLine("****************************************");
            Console.WriteLine("Enter a option's number or 0 to go back (or make a year pass).");
            string input = Console.ReadKey().KeyChar.ToString();
            bool isInt = int.TryParse(input, out int result);
            if (result >= 0 && result <= options.Count && isInt)
            {
                return result;
            }
        }
    }
}