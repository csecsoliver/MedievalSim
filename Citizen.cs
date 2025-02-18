namespace MedievalSim;

public class Citizen
{
    public string Name { get; protected set; }
    public double Wealth { get; protected set; }
    public int Age { get; protected set; }
    
    /// <summary>
    /// Might be redundant, but I do not want to later realize it is not.
    /// </summary>
    public Kingdom Kingdom { get; protected set; }
    public Guid Identifier { get; }

    protected Citizen(string name, double wealth, int age, Kingdom kingdom)
    {
        Name = name;
        Wealth = wealth;
        Age = age;
        Kingdom = kingdom;
        Identifier = Guid.NewGuid();
    }

    protected Citizen(string name, double wealth, int age, Kingdom kingdom, Guid identifier)
    {
        Name = name;
        Wealth = wealth;
        Age = age;
        Kingdom = kingdom;
        Identifier = identifier;
    }

    public virtual void Tick(Random rand)
    {
        Age++;
        if (Age > 40)
        {
            if (rand.NextDouble() < 0.5)
            {
                Die();
                return;
            }
        } else if (Age >= 80)
        {
            Die();
            return;
        }
        Wealth += rand.Next(-20, 100);
        Kingdom.Wealth += Kingdom.WealthTax * Wealth;
        Wealth -= Kingdom.WealthTax * Wealth;
        
        
        
        
    }

    public void Die()
    {
        for (int i = 0; i < Kingdom.Citizens.Count; i++)
        {
            if (Kingdom.Citizens[i].Identifier == Identifier)
            {
                Kingdom.Citizens.RemoveAt(i);
                return;
            }
        }
    }
}