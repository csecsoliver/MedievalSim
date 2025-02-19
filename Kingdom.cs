namespace MedievalSim;

public class Kingdom
{
    public string Name { get; }

    private double _wealth;
    public double Wealth
    {
        get => _wealth;
        set
        {
            if (value <= 0)
            {
                _wealth = 0;
                Console.WriteLine("You lost the kingdom. Exiting...");
                Environment.Exit(0);
            }
            else
            {
                _wealth = value;
            }
            
        }
    }
    public List<Citizen> Citizens { get; protected set; }
    public double WealthTax { get; set; }
    private double _wellbeing;

    public double Wellbeing
    {
        get => _wellbeing;
        set
        {
            switch (value)
            {
                case < 0.1:
                    _wellbeing = 0.1;
                    break;
                case > 1:
                    _wellbeing = 1;
                    break;
                default:
                    _wellbeing = value;
                    break;
            } 
            
        }
    }

    public double WellbeingSpend { get; set; }
    // public double _armyState;
    //
    // public double ArmyState
    // {
    //     get
    //     {
    //         return _armyState;
    //     }
    //     set
    //     {
    //         if (value < 0 )
    //         {
    //             _wellbeing = 0;
    //         } else if (value > 1)
    //         {
    //             _wellbeing = 1;
    //         }
    //         _wellbeing = value;
    //     }
    // }

    // public List<Castle> Castles { get; protected set; }

    /// <summary>
    /// A slightly flawed implementation of this property,
    /// because the Kingdom needs to exist to be able to create a King (which inherits Citizen) for it.
    /// </summary>
    public King King { get; set; }

    public Guid Identifier { get; }

    public Kingdom(string name)
    {
        Name = name;
        Wealth = 1000;
        Citizens = new List<Citizen>();
        Identifier = Guid.NewGuid();
        _wellbeing = 1;
        WellbeingSpend = 100;
        WealthTax = 0;
    }

    /// <summary>
    /// This method creates a randomly generated Citizen of a given profession.
    /// </summary>
    /// <param name="random">A Random class to be able to use a global seed.</param>
    /// <param name="born">Did the citizen newly come to life, or is it already an adult.</param>
    /// <param name="profession">{1=Peasant, 2=Blacksmith, 3=Soldier} any other input will result in an exception</param>
    public void CreateCitizen(Random random, bool born, int profession)
    {
        int age;
        double wealth;
        if (born)
        {
            age = 0;
            wealth = 0;
        }
        else
        {
            age = random.Next(20, 60);
            wealth = random.Next(50, 300);
        }

        Citizen x = profession switch
        {
            1 => new Peasant(RData.Names[random.Next(200)], wealth, age, this),
            2 => new Blacksmith(RData.Names[random.Next(200)], wealth, age, this),
            3 => new Soldier(RData.Names[random.Next(200)], wealth, age, this),
            _ => throw new ArgumentException("Invalid profession")
        };
        Citizens.Add(x);
    }

    public void Tick(Random rand)
    {
        List<Citizen> citizens = new List<Citizen>();
        citizens.Clear();
        citizens.AddRange(Citizens);
        foreach (var c in citizens)
        {
            c.Tick(rand);
            if (rand.NextDouble() < 0.1 && c.Age > 20)
            {
                CreateCitizen(rand, true, rand.Next(1, 4));
            }
            
        }
        
        

        
        SpendWellbeing();
    }

    public void SpendWellbeing()
    {
        Wealth -= WellbeingSpend;
    }

    public void RandomTick(Random rand)
    {
        Wellbeing = rand.NextDouble();
        WealthTax = rand.NextDouble();
        WellbeingSpend = rand.Next(50, 200);
        Wellbeing = (1 - WealthTax) + rand.Next(-20, 20) * 0.01;
    }
}