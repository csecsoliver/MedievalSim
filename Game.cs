namespace MedievalSim;

public class Game
{
    public string SaveName { get; }
    public List<Kingdom> Kingdoms { get; }

    public Game(string text)
    {
    }

    public string ToText()
    {
        string output = "";
        output += $"{SaveName}\n";
        foreach (Kingdom kingdom in Kingdoms)
        {
            output += "#kingdomStart\n";
            output += $"{kingdom.Name}\n";
            output += $"{kingdom.Wealth}\n";
            output += $"{kingdom.King.Name}\n";
            output += $"{kingdom.King.Wealth}\n";
            output += $"{kingdom.King.Age}\n";
            output += $"{kingdom.Citizens.Count}\n";
            foreach (Citizen citizen in kingdom.Citizens)
            {
                output += "#citizenStart";
                output += $"{citizen.Name}\n";
                output += $"{citizen.Wealth}\n";
                output += $"{citizen.Age}\n";
                output += "#citizenEnd";
            }

            output += $"{kingdom.Castles.Count}\n";
            foreach (Castle castle in kingdom.Castles)
            {
                output += "#castleStart\n";
                output += $"{castle.Name}\n";
                output += $"{castle.Army.Count}\n";
                foreach (Soldier citizen in castle.Army)
                {
                    output += "soldierStart\n";
                    output += $"{citizen.Name}\n";
                    output += $"{citizen.Wealth}\n";
                    output += $"{citizen.Age}\n";
                    output += "soldierEnd\n";
                }
            }

            output += "#kingdomEnd\n";
        }

        return output;
    }
}