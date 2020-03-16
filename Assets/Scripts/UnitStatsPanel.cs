using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsPanel : UIPanel
{
    [Header("Bars")] [SerializeField] private Image hpBar;
    [SerializeField] private Image physicalArmorBar;
    [SerializeField] private Image magicalArmorBar;

    [Header("Texts")] [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI physicalArmorText;
    [SerializeField] private TextMeshProUGUI magicalArmorText;

    [Header("Points")] [SerializeField] private Image[] actionPoints;
    [SerializeField] private Image[] sourcePoints;

    private Unit unit;

    private void Awake()
    {
        if (actionPoints != null && actionPoints.Length != 0 && actionPoints.Length != GameHandler.Instance.maxActionPoints)
            Debug.LogError($"Should be {GameHandler.Instance.maxActionPoints.ToString()} action points on panel.", gameObject);
    }

    public void ShowPanel(Unit unit)
    {
        this.unit = unit;
        unit.OnStatsUpdated += StatsUpdatedHandler;
        nameText.SetText(unit.name);
        StatsUpdatedHandler();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        if (unit != null)
            unit.OnStatsUpdated -= StatsUpdatedHandler;
        unit = null;
        base.HidePanel();
    }

    // IMPROVE: unit.OnStatsUpdate definitely should be called once a frame and return a list of changed stats
    //                or we'll keep redrawing all UI. On the other side unit stats are not changed very often.
    private void StatsUpdatedHandler()
    {
        if (unit == null) return;
        if (levelText != null)
            levelText.SetText(unit.Level.ToString());
        if (hpBar != null)
            hpBar.fillAmount = unit.HP / (float) unit.MaxHP;
        if (magicalArmorBar != null)
            magicalArmorBar.fillAmount = unit.MagicalArmor / (float) unit.MaxMagicArmor;
        if (physicalArmorBar != null)
            physicalArmorBar.fillAmount = unit.PhysicalArmor / (float) unit.MaxPhysicalArmor;
        if (hpText != null)
            hpText.SetText($"{unit.HP}/{unit.MaxHP}");
        if (magicalArmorText != null)
            magicalArmorText.SetText($"{unit.MagicalArmor}/{unit.MaxMagicArmor}");
        if (physicalArmorText != null)
            physicalArmorText.SetText($"{unit.PhysicalArmor}/{unit.MaxPhysicalArmor}");
        if (actionPoints != null && actionPoints.Length != 0)
            for (var i = 0; i < actionPoints.Length; ++i)
                actionPoints[i].color = i < unit.ActionPoints ? GameHandler.Instance.actionPointsColor : GameHandler.Instance.usedActionPointsColor;
        if (sourcePoints != null && sourcePoints.Length != 0)
            for (var i = 0; i < sourcePoints.Length; ++i)
                if (i < unit.MaxSourcePoints)
                {
                    sourcePoints[i].gameObject.SetActive(true);
                    sourcePoints[i].color = i < unit.SourcePoints ? GameHandler.Instance.sourcePointsColor : GameHandler.Instance.usedSourcePointsColor;
                }
                else
                    sourcePoints[i].gameObject.SetActive(false);
    }
}