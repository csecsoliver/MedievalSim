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
        for (int i = 0; i < random.Next(1, 10); i++)
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
            random.Next(7, 27), random.Next(7, 20), random.Next(7, 20)
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
        List<Kingdom> kingdoms = new List<Kingdom>();
        kingdoms.AddRange(Kingdoms);
        foreach (Kingdom k in kingdoms)
        {
            k.Tick(_random);
            if (k != Kingdoms[0])
            {
                k.RandomTick(_random);
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
        Console.WriteLine($"Wellbeing: {Kingdoms[0].Wellbeing}");
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
            string prompt =
                $"Current taxes: {Kingdoms[0].WealthTax * 100}% and {Kingdoms[0].Wealth} gold in storage.\n" +
                $"Citizen wellbeing/happiness: {Kingdoms[0].Wellbeing}\n" + $"Average citizen wealth: {avgW} gold\n" +
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
            Thread.Sleep(1000);
            
            Console.WriteLine("No.. I lied, it's now set to zero");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
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
        Kingdoms[0].Wellbeing = Kingdoms[0].Wellbeing * (Kingdoms[0].WellbeingSpend / (pastSpend + 1) +
                                _random.Next(-10, 10) * 0.01);
        Kingdoms[0].Wealth -= input;
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

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Kingdoms[0].Wealth -= cost;
    }

    public void Population()
    {
        Console.Clear();
        List<string> popmenu = ["Invest in blacksmithing", "Invest in farming", "List citizen"];
        int peasantNum = 0;
        int soldierNum = 0;
        int blacksmithNum = 0;
        foreach (Citizen k in Kingdoms[0].Citizens)
        {
            if (k.GetType() == typeof(Blacksmith))
            {
                blacksmithNum++;
            }
            else if (k.GetType() == typeof(Soldier))
            {
                soldierNum++;
            }
            else if (k.GetType() == typeof(Peasant))
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
                case 3:
                    ListCitizen();
                    break;
            }
        }
    }

    public void ListCitizen()
    {
        Console.Clear();
        foreach (Citizen c in Kingdoms[0].Citizens)
        {
            Console.WriteLine($"{c.Name}, Wealth: {c.Wealth}, Age: {c.Age}, Profession: {c.GetType().Name}");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        
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
                    Kingdoms[0].Citizens[i].Age, Kingdoms[0].Citizens[i].Kingdom, Kingdoms[0].Citizens[i].Identifier);
            }
        }

        Console.WriteLine($"It cost {cost} gold to motivate career change.");
        if (train > 0)
        {
            Console.WriteLine($"Weren't able to motivate {train} blacksmiths, ran out of peasants.");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
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
                    Kingdoms[0].Citizens[i].Age, Kingdoms[0].Citizens[i].Kingdom, Kingdoms[0].Citizens[i].Identifier);
            }
        }

        Console.WriteLine($"It cost {cost} gold to motivate career change.");
        if (train > 0)
        {
            Console.WriteLine($"Weren't able to motivate {train} peasants, ran out of blacksmiths.");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public void Relations()
    {
        Console.Clear();
        List<string> popmenu = ["List kingdoms", "Start War", "Friendly stuff"];
        

        bool cont = false;
        while (!cont)
        {
            int soldierNum = 0;
            int blacksmithNum = 0;
            double blacksmithAvgAge = 0;
            double soldierAvgAge = 0;
            foreach (Citizen k in Kingdoms[0].Citizens)
            {
                if (k.GetType() == typeof(Blacksmith) && k.Age>20)
                {
                    
                    blacksmithNum++;
                    blacksmithAvgAge += k.Age;
                }
                else if (k.GetType() == typeof(Soldier) && k.Age>20)
                {
                    soldierNum++;
                    soldierAvgAge += k.Age;
                }
            }

            if (blacksmithNum == 0)
            {
                blacksmithAvgAge = 0;
            }
            else
            {
                blacksmithAvgAge /= blacksmithNum;
            }

            if (soldierNum == 0)
            {
                soldierAvgAge = 0;
            }
            else
            {
                soldierAvgAge /= soldierNum;
            }
            string prompt =
                $"Blacksmiths: {blacksmithNum} Avg age: {blacksmithAvgAge}\nSoldiers: {soldierNum} Avg age: {soldierAvgAge}";
            int popchoice = Misc.Menu(popmenu, "Relations", prompt);
            switch (popchoice)
            {
                case 0:
                    cont = true;
                    break;
                case 1:
                    ListKingdoms();
                    break;
                case 2:
                    StartWar();
                    break;
                case 3:
                    Frend();
                    break;
            }
        }
    }

    public void ListKingdoms()
    {
        foreach (Kingdom k in Kingdoms)
        {
            Console.Clear();
            if (k == Kingdoms[0])
            {
                continue;
            }

            Thread.Sleep(100);
            Console.WriteLine($"Kingdom Name: {k.Name}");
            Thread.Sleep(100);
            Console.WriteLine($"Kingdom's wealth: {k.Wealth}");
            Thread.Sleep(100);
            Console.WriteLine($"King Name: {k.King.Name}");
            Thread.Sleep(100);
            Console.WriteLine($"King's wealth: {k.King.Wealth}");
            Thread.Sleep(100);
            Console.WriteLine($"King's age: {k.King.Age}");
            Thread.Sleep(100);
            Console.WriteLine($"Number of citizen: {k.Citizens.Count}");
            Thread.Sleep(100);
            Console.WriteLine($"Wellbeing: {k.Wellbeing}");
            Thread.Sleep(100);
            int soldierNum = 0;
            int blacksmithNum = 0;
            double blacksmithAvgAge = 0;
            double soldierAvgAge = 0;
            foreach (Citizen c in k.Citizens)
            {
                if (c.GetType() == typeof(Blacksmith)&& c.Age>20)
                {
                    blacksmithNum++;
                    blacksmithAvgAge += c.Age;
                }
                else if (c.GetType() == typeof(Soldier)&& c.Age>20)
                {
                    soldierNum++;
                    soldierAvgAge += c.Age;
                }
            }

            if (blacksmithNum == 0)
            {
                blacksmithAvgAge = 0;
                Console.WriteLine("No blacksmoths");
            }
            else
            {
                blacksmithAvgAge /= blacksmithNum;
                Console.WriteLine($"Blacksmiths: {blacksmithNum} Avg age: {blacksmithAvgAge}");
                Thread.Sleep(100);
            }

            if (soldierNum == 0)
            {
                soldierAvgAge = 0;
                Console.WriteLine("No soldiers were found, kingdom defeated.");
            }
            else
            {
                soldierAvgAge /= soldierNum;
                Console.WriteLine($"Soldiers: {soldierNum} Avg age: {soldierAvgAge}");
                Thread.Sleep(100);
            }
            
            
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    public void StartWar()
    {
        Console.Clear();
        string input = Misc.NonNullInput("Which kingdom do you want to start a war with? (enter the name):");
        if (input == Kingdoms[0].Name)
        {
            Console.WriteLine("You can't start a war with yourself, you're not that bored.");
            Thread.Sleep(1000);
            return;
        }
        
        foreach (Kingdom k in Kingdoms)
        {
            if (k.Name == input)
            {
                War war = new War(Kingdoms[0], k);
                Kingdom winner = war.GetWinner(_random);
                if (winner == Kingdoms[0])
                {
                    Console.WriteLine("You won the war!");
                    Kingdoms[0].Wealth += k.Wealth*0.3;
                    k.Wealth -= k.Wealth*0.3;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("You lost the war!");
                k.Wealth += Kingdoms[0].Wealth*0.3;
                Kingdoms[0].Wealth -= Kingdoms[0].Wealth*0.3;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
        }
    }
    
    public void Frend()
    {
        Console.Clear();
        string input = Misc.NonNullInput("Which kingdom do you want to befriend? (enter the name):");
        if (input == Kingdoms[0].Name)
        {
            Console.WriteLine("You can't befriend yourself, you're not that lonely.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }
        
        foreach (Kingdom k in Kingdoms)
        {
            if (k.Name == input)
            {
                Console.WriteLine("You are now friends with " + k.Name);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
        }
    }
}