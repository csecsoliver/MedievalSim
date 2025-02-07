namespace MedievalSim;

public class Kingdom
{
    public string Name { get; protected set; }
    public double Wealth { get; protected set; }
    public List<Citizen> Citizens { get; protected set; }
}