namespace MedievalSim;

public class Kingdom
{
    public string Name { get; protected set; }
    public double Wealth { get; protected set; }
    public List<Citizen> Citizens { get; protected set; }

    // public List<Castle> Castles { get; protected set; }
    public King King { get; protected set; }
    
    public Guid Identifier { get; }

    public Kingdom(string name)
    {
        Name = name;
        Wealth = 1000;
        Citizens = new List<Citizen>();
        Identifier = Guid.NewGuid();
    }

    public void AssignKing(King king)
    {
        King = king;
    }

    /// <summary>
    /// This method creates a randomly generated Citizen of a given profession.
    /// </summary>
    /// <param name="born">Did the citizen newly come to life, or is it already an adult.</param>
    /// <param name="profession">{1=Peasant, 2=Blacksmith, 3=Soldier} any other input will result in an exception</param>
    public void CreateCitizen(bool born, int profession)
    {
        int age;
        double wealth;
        Random random = new Random();
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

        Citizen x;
        switch (profession)
        {
            case 1:
                x = new Peasant(RData.Names[random.Next(20)], wealth, age,this);
                break;
            case 2:
                x = new Blacksmith(RData.Names[random.Next(20)], wealth, age,this);
                break;
            case 3:
                x = new Soldier(RData.Names[random.Next(20)], wealth, age,this);
                break;
            default:
                throw new ArgumentException("Invalid profession");
        }
        Citizens.Add(x);
        
    }
}