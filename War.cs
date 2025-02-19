namespace MedievalSim;

public class War
{
    public Guid Identifier { get; }
    public Kingdom Side1 { get; }
    public Kingdom Side2 { get; }

    public War(Kingdom side1, Kingdom side2)
    {
        Identifier = Guid.NewGuid();
        Side1 = side1;
        Side2 = side2;
    }

    public Kingdom GetWinner(Random rand)
    {
        Kingdom winner;
        
        double score1 = 0.0;
        double score2 = 0.0;
        List<Citizen> citizens = new List<Citizen>();
        citizens.Clear();
        citizens.AddRange(Side1.Citizens);
        foreach (Citizen c in citizens)
        {
            if (c.GetType() == typeof(Soldier))
            {
                score1 += 1 - ((c.Age - 20.0)/60.0)*0.2;
                score1 += rand.NextDouble()-0.5;
                if (rand.NextDouble() > 0.60)
                {
                    c.Die();
                }
            } else if (c.GetType() == typeof(Blacksmith))
            {
                score1 += 0.5 - ((c.Age - 20.0) / 60.0) * 0.15;
                
            }
        }
        
        citizens.Clear();
        citizens.AddRange(Side2.Citizens);
        foreach (Citizen c in citizens)
        {
            if (c.GetType() == typeof(Soldier))
            {
                if (c.Age<20)
                {
                    continue;
                }
                score2 += 1 - ((c.Age - 20.0)/60.0)*0.2;
                score2 += rand.NextDouble()-0.5;
                if (rand.NextDouble() > 0.60)
                {
                    c.Die();
                }
            } else if (c.GetType() == typeof(Blacksmith))
            {
                if (c.Age<20)
                {
                    continue;
                }
                score2 += 0.5 - ((c.Age - 20.0) / 60.0) * 0.15;
                
            }
        }

        if (score1 > score2)
        {
            winner = Side1;
        }
        else if (score1 < score2)
        {
            winner = Side2;
        }
        else
        {
            
            winner = new List<Kingdom>([Side1,Side2]).ElementAt(rand.Next(0,2));
        }
        
        
        
        
        return winner;
    }
}