using UnityEngine;
using UnityEngine.UI;

public class BarsPanel : UIPanel
{
    [SerializeField] private Image hpBar = default;
    [SerializeField] private Image physicalArmorBar = default;
    [SerializeField] private Image magicalArmorBar = default;

    private Unit unit;

    public void ShowPanel(Unit unit)
    {
        this.unit = unit;
        unit.OnStatsUpdated += UpdateBars;
        UpdateBars();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        if (unit == null) return;
        unit.OnStatsUpdated -= UpdateBars;
        unit = null;
        base.HidePanel();
    }

    // IMPROVE: Create separate calls or better collect all updates for 1 frame.
    private void UpdateBars()
    {
        hpBar.fillAmount = unit.HP / (float) unit.MaxHP;
        magicalArmorBar.fillAmount = unit.MagicalArmor / (float) unit.MaxMagicArmor;
        physicalArmorBar.fillAmount = unit.PhysicalArmor / (float) unit.MaxPhysicalArmor;
    }
}