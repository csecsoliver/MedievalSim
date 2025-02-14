namespace MedievalSim;

public class Game
{
    // public string SaveName { get; }
    public List<Kingdom> Kingdoms { get; protected set; }

    public Game(int seed, string kingdomName, string kingName)
    {
        Random random = new Random(seed);
        // ReSharper disable once UseCollectionExpression
        Kingdoms = new List<Kingdom>();
        Kingdoms.Add(GenerateKingdom(random, kingdomName, kingName));
        
        
    }

    public Kingdom GenerateKingdom(Random random, string kingdomName, string kingName)
    {
        if (kingdomName == "")
        {
            kingdomName = RData.KingdomNames[random.Next(50)];
            
        }
        if (kingName == "")
        {
            kingName = RData.KingNames[random.Next(100)];
        }
        int[] citizenCount = [random.Next(7,20), random.Next(7,20), random.Next(7,20)]; // első a paraszt, második a kovács, harmadik a katona
        Kingdom kingdom = new Kingdom(kingdomName);
        kingdom.AssignKing(new King(kingName, 1000, random.Next(30,70), kingdom));
        
        for (int i = 0; i < citizenCount[0]; i++)
        {
            kingdom.CreateCitizen(false, 1);
        }
        for (int i = 0; i < citizenCount[1]; i++)
        {
            kingdom.CreateCitizen(false, 2);
            
        }
        for (int i = 0; i < citizenCount[2]; i++)
        {
            kingdom.CreateCitizen(false, 3);
        }
        
        return kingdom;
    }
    
}