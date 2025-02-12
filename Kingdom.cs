namespace MedievalSim;

public class Kingdom
{
    public string Name { get; protected set; }
    public double Wealth { get; protected set; }
    public List<Citizen> Citizens { get; protected set; }

    public List<Castle> Castles { get; protected set; }
    public King King { get; protected set; }

    public Kingdom(string name, King king)
    {
        Name = name;
        Wealth = 1000;
        Citizens = new List<Citizen>();
        King = king;
    }
}