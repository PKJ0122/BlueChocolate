using System;

[Serializable]
public class ReRollProbabilitys
{
    public int Level;
    public ReRollProbability[] reRollProbabilities;
}

[Serializable]
public class ReRollProbability : IComparable<ReRollProbability>
{
    public Cost Cost;
    public int Probability;

    public int CompareTo(ReRollProbability other)
    {
        if (other == null) return 1;

        return Cost.CompareTo(other.Cost);
    }
}

public enum Cost
{
    Cost1 = 1,
    Cost2 = 2,
    Cost3 = 3,
    Cost4 = 4,
    Cost5 = 5
}