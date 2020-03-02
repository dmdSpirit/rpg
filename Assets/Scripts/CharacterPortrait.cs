using UnityEngine;
using UnityEngine.UI;

public class CharacterPortrait : MonoBehaviour
{
    [SerializeField] private Unit unit = default;

    private Button button;
    private Image portraitBorder;
    private Color selectedColor = Color.green;
    private Color unselectedColor;

    private void Awake()
    {
        portraitBorder = GetComponent<Image>();
        unselectedColor = portraitBorder.color;
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectUnit);
    }

    private void Start()
    {
        if (unit != null)
        {
            var selection = unit.GetComponent<UnitSelection>();
            if (selection != null)
                selection.OnSelectionChanged += OnSelectedChangedHandler;
        }
    }

    private void OnSelectedChangedHandler(bool isSelected)
    {
        portraitBorder.color = isSelected ? selectedColor : unselectedColor;
    }

    private void SelectUnit()
    {
        if (unit == null) return;
        SelectionController.Instance.Select(unit);
    }
}