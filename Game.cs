namespace MedievalSim;

public class Game
{
    // public string SaveName { get; }
    public List<Kingdom> Kingdoms { get; }
    public int Year { get; protected set; }
    private readonly Random _random;

    public Game(int seed, string kingdomName, string kingName)
    {
        Random random = new Random(seed);
        _random = random;
        // ReSharper disable once UseCollectionExpression
        Kingdoms = new List<Kingdom>();
        Kingdoms.Add(GenerateKingdom(random, kingdomName, kingName));
        for (int i = 0; i < random.Next(1, 4); i++)
        {
            Kingdoms.Add(GenerateKingdom(random, "", ""));
        }
    }

    public Kingdom GenerateKingdom(Random random, string kingdomName, string kingName)
    {
        if (kingdomName == "")
        {
            Found:
            kingdomName = RData.KingdomNames[random.Next(50)];
            foreach (Kingdom k in Kingdoms)
            {
                if (k.Name == kingdomName)
                {
                    goto Found; //This is inelegant, but replaces a while (true) and an extra boolean
                }
            }
        }

        if (kingName == "")
        {
            Found:
            kingName = RData.KingNames[random.Next(100)];
            foreach (Kingdom k in Kingdoms)
            {
                if (k.King.Name == kingName)
                {
                    goto Found; //This is inelegant, but replaces a while (true) and an extra boolean
                }
            }
        }

        int[] citizenCount =
        [
            random.Next(7, 20), random.Next(7, 20), random.Next(7, 20)
        ]; // első a paraszt, második a kovács, harmadik a katona
        Kingdom kingdom = new Kingdom(kingdomName);
        kingdom.King = new King(kingName, 1000, random.Next(30, 70), kingdom);
        for (int i = 0; i < citizenCount[0]; i++)
        {
            kingdom.CreateCitizen(random, false, 1);
        }

        for (int i = 0; i < citizenCount[1]; i++)
        {
            kingdom.CreateCitizen(random, false, 2);
        }

        for (int i = 0; i < citizenCount[2]; i++)
        {
            kingdom.CreateCitizen(random, false, 3);
        }

        return kingdom;
    }

    public void GameLoop()
    {
        
        
        Console.Clear();



        Year++;
        foreach (Kingdom k in Kingdoms)
        {
            foreach (var c in k.Citizens)
            {
                c.Tick(_random);
            }
        }



        List<string> mainmenu = ["Stats", "Taxes", "Population", "Relations"];
        string prompt = $"A year has passed. Current year: {Year}";
        bool cont = false;
        while (!cont)
        {
            int mainchoice = Misc.Menu(mainmenu, "Main Menu", prompt);
            switch (mainchoice)
            {
                case 0:
                    cont = true;
                    break;
                case 1:
                    Stats();
                    break;
                case 2:
                    Taxes();
                    break;
                case 3:
                    Population();
                    break;
                case 4:
                    Relations();
                    break;
            }
        }
        // todo: make time pass
    }

    public void Stats()
    {
        Console.Clear();
        Console.WriteLine("Your Kingdom's stats:");
        Console.WriteLine($"Kingdom Name: {Kingdoms[0].Name}");
        Console.WriteLine($"Kingdom's wealth: {Kingdoms[0].Wealth}");
        Console.WriteLine($"King Name: {Kingdoms[0].King.Name}");
        Console.WriteLine($"King's wealth: {Kingdoms[0].King.Wealth}");
        Console.WriteLine($"King's age: {Kingdoms[0].King.Age}");
        Console.WriteLine($"Number of citizen: {Kingdoms[0].Citizens.Count}");
        Console.WriteLine($"Priority profession: {Kingdoms[0].Wellbeing}");
        
        Console.Write("Press any key to return...");
        Console.ReadKey();
    }

    public void Taxes()
    {
        Console.Clear();
        List<string> taxesmenu = ["Change taxes", "Wellbeing", "Army"];
        double avgW = 0;
        foreach (Citizen k in Kingdoms[0].Citizens)
        {
            avgW += k.Wealth;
        }

        avgW /= Kingdoms[0].Citizens.Count;
        bool cont = false;
        while (!cont)
        {
            string prompt = $"Current taxes: {Kingdoms[0].WealthTax * 100}% and {Kingdoms[0].Wealth} gold in storage.\n" +
                                    $"Citizen wellbeing/happiness: {Kingdoms[0].Wellbeing}\n" +
                                    $"Average citizen wealth: {avgW} gold\n" +
                                    $"Wellbeing spend: {Kingdoms[0].WellbeingSpend} gold";
            int taxeschoice = Misc.Menu(taxesmenu, "Manage taxes", prompt);
            switch (taxeschoice)
            {
                case 0:
                    cont = true;
                    break;
                case 1:
                    ChangeTaxes();
                    break;
                case 2:
                    Wellbeing();
                    break;
                case 3:
                    ArmyState();
                    break;
            }
        }
    }

    public void ChangeTaxes()
    {
        Console.Clear();
        Console.WriteLine("What do you want to change the wealth tax to? Input a percentage as a number.");
        Kingdoms[0].WealthTax = Misc.IntInput("") * 0.01;
        if (Kingdoms[0].WealthTax < 0)
        {
            Console.WriteLine("Well, now you're giving out money (happiness soars)");
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("No.. I lied, it's now set to zero");
            Thread.Sleep(1000);
        }

        Kingdoms[0].Wellbeing = (1 - Kingdoms[0].WealthTax) + _random.Next(-20, 20) * 0.01;
    }

    public void Wellbeing()
    {
        Console.Clear();
        Console.WriteLine(
            "What do you want to change the wellbeing spend to? Input a positive gold amount as a number (negative is zero).");
        double pastSpend = Kingdoms[0].WellbeingSpend;
        double input = Misc.IntInput("");
        if (input < 0)
        {
            input = 0;
        }

        Kingdoms[0].WellbeingSpend = input;
        Kingdoms[0].Wellbeing= Kingdoms[0].Wellbeing * Kingdoms[0].WellbeingSpend / (pastSpend + 1) + _random.Next(-10, 10) * 0.01;
    }

    public void ArmyState()
    {
        Console.Clear();
        Console.WriteLine("How many soldiers do you want to train? (negative means decommission)");
        double train = Misc.IntInput("");
        double cost = 0;
        if (train < 0)
        {
            for (int i = 0; i < Kingdoms[0].Citizens.Count; i++)
            {
                if (Kingdoms[0].Citizens[i].GetType() == typeof(Soldier) && train < 0)
                {
                    train++;
                    
                    Kingdoms[0].Citizens[i] = new Peasant(Kingdoms[0].Citizens[i].Name, Kingdoms[0].Citizens[i].Wealth,
                        Kingdoms[0].Citizens[i].Age, Kingdoms[0].Citizens[i].Kingdom,
                        Kingdoms[0].Citizens[i].Identifier);
                }
            }
        }
        else
        {
            for (int i = 0; i < Kingdoms[0].Citizens.Count; i++)
            {
                if (Kingdoms[0].Citizens[i].GetType() == typeof(Peasant) && train > 0)
                {
                    train--;
                    cost += 100;
                    Kingdoms[0].Citizens[i] = new Soldier(Kingdoms[0].Citizens[i].Name, Kingdoms[0].Citizens[i].Wealth,
                        Kingdoms[0].Citizens[i].Age, Kingdoms[0].Citizens[i].Kingdom,
                        Kingdoms[0].Citizens[i].Identifier);
                }
            }

            Console.WriteLine($"It cost {cost} gold to train these soldiers.");
        }
        
        if (train > 0)
        {
            Console.WriteLine($"Weren't able to train {train} soldiers, ran out of Peasants to train.");
        }

        if (train < 0)
        {
            Console.WriteLine($"Weren't able to decommission {-train} soldiers, ran out of Soldiers.");
        }
        Kingdoms[0].Wealth -= cost;
    }
    
    public void Population()
    {
        Console.Clear();
        List<string> popmenu = ["Invest in blacksmithing", "Invest in farming"];

        int peasantNum = 0;
        int soldierNum = 0;
        int blacksmithNum = 0;
        
        
        foreach (Citizen k in Kingdoms[0].Citizens)
        {
            if (k.GetType() == typeof(Blacksmith))
            {
                blacksmithNum++;
            } else if (k.GetType() == typeof(Soldier))
            {
                soldierNum++;
            } else if (k.GetType() == typeof(Peasant))
            {
                peasantNum++;
            }
        }
        
        bool cont = false;
        while (!cont)
        {
            string prompt = $"Blacksmiths: {blacksmithNum}\nSoldiers: {soldierNum}\nPeasants: {peasantNum}";
            int popchoice = Misc.Menu(popmenu, "Population", prompt);
            switch (popchoice)
            {
                case 0:
                    cont = true;
                    break;
                case 1:
                    TrainBlacksmith();
                    break;
                case 2:
                    TrainPeasant();
                    break;
            }
        }
    }
    
    public void TrainBlacksmith()
    {
        Console.Clear();
        Console.WriteLine("How many peasants do you want to make blacksmiths? (negative means zero)");
        double train = Misc.IntInput("");
        if (train < 0) train = 0;
        double cost = 0;
        for (int i = 0; i < Kingdoms[0].Citizens.Count; i++)
        {
            if (Kingdoms[0].Citizens[i].GetType() == typeof(Peasant) && train > 0)
            {
                train--;
                cost += 100;
                Kingdoms[0].Citizens[i] = new Blacksmith(Kingdoms[0].Citizens[i].Name, Kingdoms[0].Citizens[i].Wealth,
                    Kingdoms[0].Citizens[i].Age, Kingdoms[0].Citizens[i].Kingdom,
                    Kingdoms[0].Citizens[i].Identifier);
            }
        }

        Console.WriteLine($"It cost {cost} gold to motivate career change.");
        if (train > 0)
        {
            Console.WriteLine($"Weren't able to motivate {train} blacksmiths, ran out of peasants.");
        }
    }
    
    public void TrainPeasant()
    {
        Console.Clear();
        Console.WriteLine("How many blacksmiths do you want to make peasants? (negative means zero)");
        double train = Misc.IntInput("");
        if (train < 0) train = 0;
        double cost = 0;
        for (int i = 0; i < Kingdoms[0].Citizens.Count; i++)
        {
            if (Kingdoms[0].Citizens[i].GetType() == typeof(Blacksmith) && train > 0)
            {
                train--;
                cost += 100;
                Kingdoms[0].Citizens[i] = new Peasant(Kingdoms[0].Citizens[i].Name, Kingdoms[0].Citizens[i].Wealth,
                    Kingdoms[0].Citizens[i].Age, Kingdoms[0].Citizens[i].Kingdom,
                    Kingdoms[0].Citizens[i].Identifier);
            }
        }

        Console.WriteLine($"It cost {cost} gold to motivate career change.");
        if (train > 0)
        {
            Console.WriteLine($"Weren't able to motivate {train} peasants, ran out of blacksmiths.");
        }
    }

    public void Relations()
    {
        Console.Clear();
        
    }
}