namespace MedievalSim;

public class Castle
{
    public string Name { get; protected set; }
    public Citizen Captain { get; protected set; }
    public Castle(string name, Citizen captain){
        Name = name;
        Captain = captain;
    }

}
