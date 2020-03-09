using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Image image;

    private Skill skill;

    public void Initialize(Skill skill)
    {
        this.skill = skill;
    }
}