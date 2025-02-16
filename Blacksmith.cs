namespace MedievalSim;

public class Blacksmith : Citizen
{
    public Blacksmith(string name, double wealth, int age, Kingdom kingdom) : base(name, wealth, age, kingdom)
    {
    }
    public Blacksmith(string name, double wealth, int age, Kingdom kingdom, Guid identifier) : base(name, wealth, age,
        kingdom, identifier)
    {
        
    }
}