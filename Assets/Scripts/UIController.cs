using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoSingleton<UIController>, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnMouseEnterUI;
    public event Action OnMouseExitUI;

    [SerializeField] private NPCInfoPanel npcInfoPanel = default;
    [SerializeField] private InitiativePanel initiativePanel = default;

    private void Start()
    {
        npcInfoPanel.HidePanel();
        initiativePanel.HidePanel();
    }

    public void ShowInitiativePanel(InitiativeOrder initiativeOrder) => initiativePanel.ShowPanel(initiativeOrder);
    public void ShowNPCInfoPanel(Unit unit) => npcInfoPanel.ShowPanel(unit);

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterUI?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitUI?.Invoke();
    }
}