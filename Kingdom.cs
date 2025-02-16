namespace MedievalSim;

public class Kingdom
{
    public string Name { get; protected set; }
    public double Wealth { get; protected set; }
    public List<Citizen> Citizens { get; protected set; }
    
    public double WealthTax { get; set; }
    private double _wellbeing;
    public double Wellbeing
    {
        get
        {
            return _wellbeing;
        }
        set
        {
            if (value < 0 )
            {
                _wellbeing = 0;
            } else if (value > 1)
            {
                _wellbeing = 1;
            }
            _wellbeing = value;
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
            wealth = random.Next(50,300);
        }

        Citizen x = profession switch
        {
            1 => new Peasant(RData.Names[random.Next(20)], wealth, age, this),
            2 => new Blacksmith(RData.Names[random.Next(20)], wealth, age, this),
            3 => new Soldier(RData.Names[random.Next(20)], wealth, age, this),
            _ => throw new ArgumentException("Invalid profession")
        };
        Citizens.Add(x);
        
    }
}