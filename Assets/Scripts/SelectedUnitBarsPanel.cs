using UnityEngine;
using UnityEngine.UI;

public class SelectedUnitBarsPanel : UIPanel
{
    [SerializeField] private Image hpBar;
    [SerializeField] private Image physicalArmorBar;
    [SerializeField] private Image magicalArmorBar;
    [SerializeField] private Image[] sourcePoints;
    [SerializeField] private Image[] actionPoints;
    [SerializeField] private Color usedActionPointColor;
    [SerializeField] private Color usedSourcePointColor;

    private Unit unit;
    private Color actionPointColor;
    private Color sourcePointsColor;

    private void Awake()
    {
        if (actionPoints.Length != GameHandler.Instance.maxActionPoints)
            Debug.LogError($"Should be {GameHandler.Instance.maxActionPoints} action points on panel.", gameObject);
        sourcePointsColor = sourcePoints[0].color;
        actionPointColor = actionPoints[0].color;
    }

    public void ShowPanel(Unit unit)
    {
        this.unit = unit;
        unit.OnStatsUpdated += UpdateBars;
        UpdateBars();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        if (unit != null)
            unit.OnStatsUpdated -= UpdateBars;
        unit = null;
        base.HidePanel();
    }

    private void UpdateBars()
    {
        if (unit == null) return;
        UpdateActionPoints(unit.ActionPoints);
        UpdateSourcePoints(unit.SourcePoints, unit.MaxSourcePoints);
        hpBar.fillAmount = unit.HP / (float) unit.MaxHP;
        magicalArmorBar.fillAmount = unit.MagicalArmor / (float) unit.MaxMagicArmor;
        physicalArmorBar.fillAmount = unit.PhysicalArmor / (float) unit.MaxPhysicalArmor;
    }

    private void UpdateActionPoints(int activePointsCount)
    {
        for (var i = 0; i < actionPoints.Length; ++i)
            actionPoints[i].color = i < activePointsCount ? actionPointColor : usedActionPointColor;
    }

    private void UpdateSourcePoints(int activePointsCount, int maxPoints)
    {
        for (var i = 0; i < sourcePoints.Length; ++i)
        {
            if (i < maxPoints)
            {
                sourcePoints[i].gameObject.SetActive(true);
                sourcePoints[i].color = i < activePointsCount ? sourcePointsColor : usedSourcePointColor;
            }
            else
                sourcePoints[i].gameObject.SetActive(false);
        }
    }
}