namespace MedievalSim;

public class Soldier : Citizen
{
    public Soldier(string name, double wealth, int age, Kingdom kingdom) : base(name, wealth, age, kingdom)
    {
    }
    public Soldier(string name, double wealth, int age, Kingdom kingdom, Guid identifier) : base(name, wealth, age,
        kingdom, identifier)
    {
        
    }
}