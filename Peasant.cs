namespace MedievalSim;

public class Peasant : Citizen
{
    public Peasant(string name, double wealth, int age, Kingdom kingdom) : base(name, wealth, age, kingdom)
    {
    }

    public Peasant(string name, double wealth, int age, Kingdom kingdom, Guid identifier) : base(name, wealth, age,
        kingdom, identifier)
    {
        
    }
}