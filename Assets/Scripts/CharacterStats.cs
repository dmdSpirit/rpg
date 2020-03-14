using System;

[Serializable]
public class CharacterStats
{
    public int strength;
    public int dexterity;
    public int intelligence;
    public int constitution;
    public int memory;
    public int wits;

    // FIXME: Calculate stats.
    public int hp;
    public int physicalArmor;
    public int magicalArmor;
}
