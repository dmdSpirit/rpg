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

    [SerializeField] private Image portrait = default;

    [Header("Bars")] [SerializeField] private Image hpBar = default;
    [SerializeField] private Image physicalArmorBar = default;
    [SerializeField] private Image magicalArmorBar = default;

    [Header("Texts")] [SerializeField] private TextMeshProUGUI nameText = default;
    [SerializeField] private TextMeshProUGUI levelText = default;
    [SerializeField] private TextMeshProUGUI hpText = default;
    [SerializeField] private TextMeshProUGUI physicalArmorText = default;
    [SerializeField] private TextMeshProUGUI magicalArmorText = default;

    [Header("Points")] [SerializeField] private Image[] actionPoints = default;
    [SerializeField] private Image[] sourcePoints = default;

    public bool ShowName
    {
        set
        {
            if (value == showName) return;
            if (nameText != null)
                nameText.gameObject.SetActive(value);
            showName = value;
            StatsUpdatedHandler();
        }
    }

    public bool ShowPortrait
    {
        set
        {
            if (value == showPortrait) return;
            if (portrait != null)
                portrait.gameObject.SetActive(value);
            showPortrait = value;
            StatsUpdatedHandler();
        }
    }

    private Unit unit;
    private bool showPortrait;
    private bool showBarImages;
    private bool showBarTexts;
    private bool showName;
    private bool showLevel;
    private bool showActionPoints;
    private bool showSourcePoints;
    private bool isShown;

    private void Awake()
    {
        showPortrait = portrait != null;
        showBarImages = hpBar != null && physicalArmorBar != null && magicalArmorBar != null;
        showBarTexts = hpText != null && physicalArmorText != null && magicalArmorText != null;
        showLevel = levelText != null;
        showName = nameText != null;

        showSourcePoints = sourcePoints != null && sourcePoints.Length != 0;
        showActionPoints = actionPoints != null && actionPoints.Length != 0;
        if (showActionPoints && actionPoints.Length != GameHandler.Instance.maxActionPoints)
            Debug.LogError($"Should be {GameHandler.Instance.maxActionPoints.ToString()} action points on panel.",
                gameObject);
    }

    private void OnEnable()
    {
        isShown = true;
        StatsUpdatedHandler();
    }

    private void OnDisable() => isShown = false;

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
        if (isShown == false || unit == null) return;
        if (showPortrait)
            portrait.sprite = unit.UnitPortraitImage;
        if (showBarImages)
        {
            hpBar.fillAmount = unit.HP / (float) unit.MaxHP;
            magicalArmorBar.fillAmount = unit.MagicalArmor / (float) unit.MaxMagicArmor;
            physicalArmorBar.fillAmount = unit.PhysicalArmor / (float) unit.MaxPhysicalArmor;
        }

        if (showBarTexts)
        {
            hpText.SetText($"{unit.HP}/{unit.MaxHP}");
            magicalArmorText.SetText($"{unit.MagicalArmor}/{unit.MaxMagicArmor}");
            physicalArmorText.SetText($"{unit.PhysicalArmor}/{unit.MaxPhysicalArmor}");
        }

        if (showName)
            nameText.SetText(unit.name);
        if (showLevel)
            levelText.SetText($"Level {unit.Level.ToString()}");

        if (showActionPoints)
            for (var i = 0; i < actionPoints.Length; ++i)
                actionPoints[i].color = i < unit.ActionPoints
                    ? GameHandler.Instance.actionPointsColor
                    : GameHandler.Instance.usedActionPointsColor;
        if (showSourcePoints)
            for (var i = 0; i < sourcePoints.Length; ++i)
                if (i < unit.MaxSourcePoints)
                {
                    sourcePoints[i].gameObject.SetActive(true);
                    sourcePoints[i].color = i < unit.SourcePoints
                        ? GameHandler.Instance.sourcePointsColor
                        : GameHandler.Instance.usedSourcePointsColor;
                }
                else
                    sourcePoints[i].gameObject.SetActive(false);
    }
}