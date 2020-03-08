using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCInfoPanel : UIPanel
{
    [SerializeField] private TextMeshProUGUI characterName = default;
    [SerializeField] private Image image = default;
    [SerializeField] private TextMeshProUGUI strength = default;
    [SerializeField] private TextMeshProUGUI dexterity = default;
    [SerializeField] private TextMeshProUGUI intelligence = default;
    [SerializeField] private TextMeshProUGUI constitution = default;
    [SerializeField] private TextMeshProUGUI memory = default;
    [SerializeField] private TextMeshProUGUI wisdom = default;
    [SerializeField] private Button closeButton = default;

    private void Awake()
    {
        closeButton.onClick.AddListener(HidePanel);
    }

    public void ShowPanel(Unit unit)
    {
        base.ShowPanel();
        characterName.SetText(unit.name);
        image.sprite = unit.UnitPortraitImage;
        var stats = unit.Stats;
        strength.SetText(stats.strength.ToString());
        dexterity.SetText(stats.dexterity.ToString());
        intelligence.SetText(stats.intelligence.ToString());
        constitution.SetText(stats.constitution.ToString());
        memory.SetText(stats.memory.ToString());
        wisdom.SetText(stats.wisdom.ToString());
    }
}