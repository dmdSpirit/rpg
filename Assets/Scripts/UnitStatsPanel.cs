using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsPanel : UIPanel
{
    [Flags]
    private enum StatComponents
    {
        None,
        Portrait,
        BarImages,
        BarTexts,
        OtherTexts,
        ActionPoints,
        SourcePoints,
    }

    [SerializeField] private Image portrait;

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
    private StatComponents components = StatComponents.None;

    private void Awake()
    {
        if (portrait != null) components |= StatComponents.Portrait;
        if (hpBar != null && physicalArmorBar != null && magicalArmorBar != null) components |= StatComponents.BarImages;
        if (hpText != null && physicalArmorText != null && magicalArmorText != null) components |= StatComponents.BarTexts;
        if (levelText != null && nameText != null) components |= StatComponents.OtherTexts;
        if (actionPoints != null && actionPoints.Length != 0)
        {
            if (actionPoints.Length != GameHandler.Instance.maxActionPoints)
                Debug.LogError($"Should be {GameHandler.Instance.maxActionPoints.ToString()} action points on panel.", gameObject);
            else
                components |= StatComponents.ActionPoints;
        }

        if (sourcePoints != null && sourcePoints.Length != 0) components |= StatComponents.SourcePoints;
    }

    public void ShowPanel(Unit unit)
    {
        this.unit = unit;
        unit.OnStatsUpdated += StatsUpdatedHandler;
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
        if (components.HasFlag(StatComponents.BarImages))
        {
            hpBar.fillAmount = unit.HP / (float) unit.MaxHP;
            magicalArmorBar.fillAmount = unit.MagicalArmor / (float) unit.MaxMagicArmor;
            physicalArmorBar.fillAmount = unit.PhysicalArmor / (float) unit.MaxPhysicalArmor;
        }

        if (components.HasFlag(StatComponents.BarTexts))
        {
            hpText.SetText($"{unit.HP}/{unit.MaxHP}");
            magicalArmorText.SetText($"{unit.MagicalArmor}/{unit.MaxMagicArmor}");
            physicalArmorText.SetText($"{unit.PhysicalArmor}/{unit.MaxPhysicalArmor}");
        }

        if (components.HasFlag(StatComponents.OtherTexts))
        {
            nameText.SetText(unit.name);
            levelText.SetText($"Level {unit.Level.ToString()}");
        }

        if (components.HasFlag(StatComponents.ActionPoints))
            for (var i = 0; i < actionPoints.Length; ++i)
                actionPoints[i].color = i < unit.ActionPoints ? GameHandler.Instance.actionPointsColor : GameHandler.Instance.usedActionPointsColor;
        if (components.HasFlag(StatComponents.SourcePoints))
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