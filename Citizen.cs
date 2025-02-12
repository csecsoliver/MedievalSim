namespace MedievalSim;

public class Citizen
{
    public string Name { get; protected set; }
    public double Wealth { get; protected set; }
    public int Age { get; protected set; }
    public Kingdom Kingdom { get; protected set; }

    public Citizen(string name, double wealth, int age, Kingdom kingdom)
    {
        Name = name;
        Wealth = wealth;
        Age = age;
        Kingdom = kingdom;
    }

    public virtual void Tick()
    {
        Age++;
    }
}