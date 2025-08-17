public class StatModifier
{
    public readonly float Value;
    public readonly StatModType ModType;

    public StatModifier(float value, StatModType type)
    {
        Value = value;
        ModType = type;
    }
}
