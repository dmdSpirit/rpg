using UnityEngine;

public enum SkillType
{
    Moving,
    Target,
    AOE
}

public class Skill : ScriptableObject
{
    public string name;
    public SkillType type;
    public Sprite image;
    public string description;
}